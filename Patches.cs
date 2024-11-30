using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace SortColonistBar.Patches
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            new Harmony("rimworld.mod.sortcolonistbar").PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(PlayerPawnsDisplayOrderUtility))]
    [HarmonyPatch("Sort")]
    internal class PlayerPawnsDisplayOrderUtility_Sort_Patch
    {
        [HarmonyPrefix]
        public static bool Sort_Prefix(ref Func<Pawn, int> ___displayOrderGetter, ref List<Pawn> pawns)
        {
            if (Tools.Sort == Tools.SortChoice.Name)
                return false;

            ___displayOrderGetter = Tools.DisplayOrderGetter;
            
            return true;
        }

        [HarmonyPostfix]
        public static void Sort_PostFix(ref Func<Pawn, int> ___displayOrderGetter, ref List<Pawn> pawns)
        {
            if (Tools.Sort == Tools.SortChoice.Manual)
                return;
            if (pawns.NullOrEmpty())
                return;
            if (Tools.Sort == Tools.SortChoice.Name)
            {
                pawns.SortBy(x => x?.LabelCap);
            }
            if (Tools.Reverse)
            {
                pawns.Reverse();
            }
        }
    }

    [HarmonyPatch(typeof(ColonistBarColonistDrawer))]
    [HarmonyPatch("HandleClicks")]
    internal class ColonistBarColonistDrawer_HandleClicks_Patches
    {
        private static bool mousePressed = false;

        [HarmonyPrefix]
        public static bool Prefix(Rect rect, Pawn colonist, int reorderableGroup)
        {

#if DEBUG
            if (Event.current.type != EventType.Layout
                && Event.current.button == 0
                && Event.current.type == EventType.MouseDown
                && Event.current.clickCount == 2
                && Mouse.IsOver(rect))
            {
                // Debugging purposes to get some stats
                Log.Message(nameof(colonist.LabelCap) + ": " + colonist.LabelCap);
                Log.Message(nameof(colonist.ThingID) + ": " + colonist.thingIDNumber);
                Log.Message(nameof(StatDefOf.TradePriceImprovement) + ": " + colonist.GetStatValue(StatDefOf.TradePriceImprovement));
                Log.Message(nameof(StatDefOf.MedicalTendSpeed) + ": " + colonist.GetStatValue(StatDefOf.MedicalTendSpeed));
                Log.Message(nameof(StatDefOf.MoveSpeed) + ": " + colonist.GetStatValue(StatDefOf.MoveSpeed));
                Log.Message(nameof(StatDefOf.MarketValue) + ": " + colonist.GetStatValue(StatDefOf.MarketValue));
            }
#endif
            if (Event.current.type != EventType.MouseDrag
             && Event.current.button == 1
             && Event.current.clickCount == 1)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    mousePressed = true;
                }
                if (Event.current.type == EventType.MouseUp
                    && mousePressed
                    && Mouse.IsOver(rect))
                {
                    Find.WindowStack.Add(Tools.LabelMenu);
                    return false;
                }
            }
            else
            {
                mousePressed = false;
            }
            return true;
        }
    }
}
