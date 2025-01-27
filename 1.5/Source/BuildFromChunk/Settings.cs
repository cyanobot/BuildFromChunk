using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace BuildFromChunk
{
    
    public class Settings : ModSettings
    {
        public static bool restrictBlocks;
        public static bool anyPrimitiveStonecutting;

        public static List<string> stonecuttingSpotDefNames = new List<string>
        {
            "VBY_PrimitiveStoneCuttingSpot",
            "DankPyon_StonecuttingSpot"
        };
        public static List<ThingDef> stonecuttingSpots = new List<ThingDef>();
        public static List<RecipeDef> stonecuttingRecipes = new List<RecipeDef>();

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref restrictBlocks, "restrictBlocks", restrictBlocks, true);
        }

        public static void DoSettingsWindowContents(Rect rect)
        {
            Listing_Standard l = new Listing_Standard(GameFont.Small)
            {
                ColumnWidth = rect.width
            };

            l.Begin(rect);

            if (anyPrimitiveStonecutting)
            {
                l.CheckboxLabeled("No stone blocks at stonecutting spot: ", ref restrictBlocks, "When this setting is ON, pawns cannot cut stone blocks at a stonecutting spot but require a workbench"); ;
            }
            else
            {
                Color orig = GUI.color;
                GUI.color = new Color(1f, 1f, 1f, 0.2f);

                l.CheckboxLabeled("No stone blocks at stonecutting spot: ", ref restrictBlocks, "No effect because no mod identified that adds a stonecutting spot");

                GUI.color = orig;
            }

            l.End();

            ApplySettings();
        }

        public static void ApplySettings()
        {
            if (anyPrimitiveStonecutting 
                && !stonecuttingSpots.NullOrEmpty() 
                && !stonecuttingRecipes.NullOrEmpty()
                )
            {
                foreach (ThingDef spotDef in stonecuttingSpots)
                {
                    foreach(RecipeDef recipeDef in stonecuttingRecipes)
                    {
                        if (restrictBlocks)
                        {
                            recipeDef.recipeUsers.RemoveAll(u => u == spotDef);
                        }
                        else
                        {
                            recipeDef.recipeUsers.AddDistinct(spotDef);
                        }
                    }
                }
            }
        }
    }
    
}
