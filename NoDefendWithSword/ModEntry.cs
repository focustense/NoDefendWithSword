using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using Harmony;

namespace NoDefendWithSword
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            MeleeWeaponPatches.Initialize(this.Monitor);

            var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Tools.MeleeWeapon), nameof(StardewValley.Tools.MeleeWeapon.animateSpecialMove)),
                prefix: new HarmonyMethod(typeof(MeleeWeaponPatches), nameof(MeleeWeaponPatches.animateSpecialMove_Prefix))
            );
        }
    }

    public class MeleeWeaponPatches
    {
        private static IMonitor Monitor;

        /*********
        ** Static methods
        *********/

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        /// 
        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        /// 
        public static bool animateSpecialMove_Prefix(StardewValley.Tools.MeleeWeapon __instance, StardewValley.Farmer who)
        {
            try
            {
                if (__instance.type != StardewValley.Tools.MeleeWeapon.defenseSword || __instance.parentSheetIndex == StardewValley.Tools.MeleeWeapon.scythe || __instance.Name.Contains("Scythe"))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(animateSpecialMove_Prefix)}:\n{ex}", LogLevel.Error);
                return true;
            }
        }
    }
}