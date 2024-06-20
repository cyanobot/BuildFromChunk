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

namespace BuildFromChunk
{
    [DefOf]
    public static class BuildFromChunksDefOf
    {
        public static StuffCategoryDef CYB_StoneChunks;
    }
}
