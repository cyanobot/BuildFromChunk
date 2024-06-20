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
        static FieldInfo fld_Qual_awful = AccessTools.Field(typeof(StatModifierQuality), "awful");
        static FieldInfo fld_Qual_poor = AccessTools.Field(typeof(StatModifierQuality), "poor");
        static FieldInfo fld_Qual_normal = AccessTools.Field(typeof(StatModifierQuality), "normal");
        static FieldInfo fld_Qual_good = AccessTools.Field(typeof(StatModifierQuality), "good");
        static FieldInfo fld_Qual_excellent = AccessTools.Field(typeof(StatModifierQuality), "excellent");
        static FieldInfo fld_Qual_masterwork = AccessTools.Field(typeof(StatModifierQuality), "masterwork");
        static FieldInfo fld_Qual_legendary = AccessTools.Field(typeof(StatModifierQuality), "legendary");
        static List<FieldInfo> flds_Qual = new List<FieldInfo>
        {
            fld_Qual_awful,
            fld_Qual_poor,
            fld_Qual_normal,
            fld_Qual_good,
            fld_Qual_excellent,
            fld_Qual_masterwork,
            fld_Qual_legendary
        };

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
                //also remove category 
                if (block == null || block.stuffProps == null)
                {
                    //Log.Message("old stuffProps.categories: " + __instance.stuffProps.categories.ToStringSafeEnumerable());
                    __instance.stuffProps.categories.RemoveAll(scd => scd == BuildFromChunksDefOf.CYB_StoneChunks);
                    //Log.Message("new stuffProps.categories: " + __instance.stuffProps.categories.ToStringSafeEnumerable());
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
                if (block.stuffProps.statOffsetsQuality != null)
                {
                    __instance.stuffProps.statOffsetsQuality = new List<StatModifierQuality>();
                    foreach (StatModifierQuality statModQual in block.stuffProps.statOffsetsQuality)
                    {
                        StatModifierQuality newStatModQual = new StatModifierQuality();
                        newStatModQual.stat = statModQual.stat;
                        foreach (FieldInfo field in flds_Qual)
                        {
                            field.SetValue(newStatModQual, field.GetValue(statModQual));
                        }
                        __instance.stuffProps.statOffsetsQuality.Add(newStatModQual);
                    }
                }
                if (block.stuffProps.statFactorsQuality != null)
                {
                    __instance.stuffProps.statFactorsQuality = new List<StatModifierQuality>();
                    foreach (StatModifierQuality statModQual in block.stuffProps.statFactorsQuality)
                    {
                        StatModifierQuality newStatModQual = new StatModifierQuality();
                        newStatModQual.stat = statModQual.stat;
                        foreach (FieldInfo field in flds_Qual)
                        {
                            field.SetValue(newStatModQual, field.GetValue(statModQual));
                        }
                        __instance.stuffProps.statFactorsQuality.Add(newStatModQual);
                    }
                }
                __instance.stuffProps.color = block.stuffProps.color;
                __instance.stuffProps.constructEffect = block.stuffProps.constructEffect;
                __instance.stuffProps.appearance = block.stuffProps.appearance ?? StuffAppearanceDefOf.Smooth;
                __instance.stuffProps.allowColorGenerators = block.stuffProps.allowColorGenerators;
                __instance.stuffProps.canSuggestUseDefaultStuff = block.stuffProps.canSuggestUseDefaultStuff;
                __instance.stuffProps.soundImpactBullet = block.stuffProps.soundImpactBullet;
                __instance.stuffProps.soundImpactMelee = block.stuffProps.soundImpactMelee;
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
