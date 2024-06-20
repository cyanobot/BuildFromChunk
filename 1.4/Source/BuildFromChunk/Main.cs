using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace BuildFromChunk
{
    public class Main : Mod
    {
        public Main(ModContentPack mcp) : base(mcp)
        {
            //GetSettings<Settings>();
            var harmony = new Harmony("cyanobot.buildfromchunk");

            harmony.PatchAll();
        }

        /*
        public override string SettingsCategory()
        {
            return "name";
        }

        static public void ApplySettings()
        {

        }

        public override void DoSettingsWindowContents(Rect inRect) => Settings.DoSettingsWindowContents(inRect);
        */
    }

}
