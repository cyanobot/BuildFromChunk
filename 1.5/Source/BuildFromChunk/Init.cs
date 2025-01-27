using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using static BuildFromChunk.Settings;

namespace BuildFromChunk
{
    [StaticConstructorOnStartup]
    class Init
    {
        static Init()
        {
            Log.Message("BuildFromChunk.Init");

            if (LoadedModManager.RunningModsListForReading.Any(mcp =>
                mcp.Name == "Medieval Overhaul"
                || mcp.Name == "Primitive Core"
            ))
            {
                Log.Message("found mod with primitive stonecutting spot");
                anyPrimitiveStonecutting = true;

                foreach(string defName in stonecuttingSpotDefNames)
                {
                    ThingDef def = DefDatabase<ThingDef>.AllDefsListForReading.Find(d => d.defName == defName);
                    if (def != null) stonecuttingSpots.Add(def);
                }

                foreach(RecipeDef recipe in DefDatabase<RecipeDef>.AllDefsListForReading)
                {
                    if (recipe.AllRecipeUsers == null || recipe.AllRecipeUsers.Count() == 0) continue;
                    if (recipe.AllRecipeUsers.Any(u => stonecuttingSpots.Contains(u))
                        //&& (recipe.products.Any(p => p.thingDef.IsWithinCategory(ThingCategoryDefOf.StoneBlocks))
                            //|| (!recipe.specialProducts.NullOrEmpty() 
                                //&& recipe.specialProducts.Contains(SpecialProductType.Butchery)
                                //&& recipe.ingredients.Any(i => i.filter.AllowedThingDefs.All(d => d.IsWithinCategory(ThingCategoryDefOf.StoneChunks)))
                        )//))
                    {
                        stonecuttingRecipes.Add(recipe);
                    }
                }
            }
            else
            {
                anyPrimitiveStonecutting = false;
            }

            Log.Message("stonecuttingSpots: " + stonecuttingSpots.ToStringSafeEnumerable());
            Log.Message("stonecuttingRecipes: " + stonecuttingRecipes.ToStringSafeEnumerable());
            ApplySettings();
        }
    }
}
