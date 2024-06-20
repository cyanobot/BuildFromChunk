using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using static BuildFromChunk.Main;
using System.Reflection.Emit;

namespace BuildFromChunk
{
    [HarmonyPatch(typeof(ThingDef),nameof(ThingDef.ResolveReferences))]
    class Patch_ThingDef_ResolveReferences
    {
        static void Postfix(ThingDef __instance)
        {
            //Log.Message("Patch_ThingDef_ResolveReferences.Postfix for ThingDef: " + __instance);
            if (__instance.stuffProps?.categories?.Contains(BuildFromChunksDefOf.CYB_StoneChunks) ?? false)
            {
                //Log.Message("Patch_ThingDef_ResolveReferences.Postfix for chunk: " + __instance);
                
                //get the stone block associated with the chunk
                ThingDef block = __instance.butcherProducts.Find(tdcc => tdcc.thingDef.thingCategories.Contains(ThingCategoryDefOf.StoneBlocks))?.thingDef;
                //Log.Message("block: " + block);
                //if  we can't find the block or the block isn't a stuff, remove category and do not proceed
                if (block == null || block.stuffProps == null)
                {
                    __instance.stuffProps.categories.RemoveAll(scd => scd == BuildFromChunksDefOf.CYB_StoneChunks);
                    return;
                }


                //copy fields from the stuff properties of the block
                //using fallbacks where fields might not be defined, if required
                __instance.stuffProps.stuffAdjective = block.stuffProps.stuffAdjective ?? __instance.label;
                __instance.stuffProps.commonality = block.stuffProps.commonality;
                if (block.stuffProps.statOffsets != null)
                {
                    __instance.stuffProps.statOffsets = new List<StatModifier>();
                    foreach (StatModifier statMod in block.stuffProps.statOffsets)
                    {
                        StatModifier newStatMod = new StatModifier();
                        newStatMod.stat = statMod.stat;
                        newStatMod.value = statMod.value;
                        __instance.stuffProps.statOffsets.Add(newStatMod);
                    }
                }
                if (block.stuffProps.statFactors != null)
                {
                    __instance.stuffProps.statFactors = new List<StatModifier>();
                    foreach (StatModifier statMod in block.stuffProps.statFactors)
                    {
                        StatModifier newStatMod = new StatModifier();
                        newStatMod.stat = statMod.stat;
                        newStatMod.value = statMod.value;
                        __instance.stuffProps.statFactors.Add(newStatMod);
                    }
                }
                __instance.stuffProps.color = block.stuffProps.color;
                __instance.stuffProps.constructEffect = block.stuffProps.constructEffect;
                __instance.stuffProps.appearance = block.stuffProps.appearance ?? StuffAppearanceDefOf.Smooth;
                __instance.stuffProps.allowColorGenerators = block.stuffProps.allowColorGenerators;
                __instance.stuffProps.canSuggestUseDefaultStuff = block.stuffProps.canSuggestUseDefaultStuff;
                __instance.stuffProps.soundImpactStuff = block.stuffProps.soundImpactStuff;
                __instance.stuffProps.soundMeleeHitSharp = block.stuffProps.soundMeleeHitSharp;
                __instance.stuffProps.soundMeleeHitBlunt = block.stuffProps.soundMeleeHitBlunt;
            }
        }
    }

    /*
    [HarmonyPatch(typeof(ThingDef))]
    [HarmonyPatch(nameof(ThingDef.VolumePerUnit),MethodType.Getter)]
    class Patch_VolumePerUnit
    {
        public const float CHUNK_MULTIPLIER = 10f;

        static float Postfix(float __result, ThingDef __instance)
        {
            float value = __result;

            if (__instance.stuffProps?.categories?.Contains(BuildFromChunksDefOf.StoneChunks) ?? false)
            {
                value *= CHUNK_MULTIPLIER;
            }

            return value;
        }
    }
    */
}
