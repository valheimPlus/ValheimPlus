using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus.GameClasses
{


	[HarmonyPatch(typeof(DropTable), "GetDropList", new Type[] { typeof(int) })]
	public static class DropTable_GetDropList_Patch
	{
		private static void Postfix(ref DropTable __instance, ref List<GameObject> __result, int amount)
		{
			if (!Configuration.Current.Gathering.IsEnabled)
				return;

			int wood = 0;
			GameObject woodObject = null;

			int coreWood = 0;
			GameObject coreWoodObject = null;

			int stone = 0;
			GameObject stoneObject = null;

			int scrapIron = 0;
			GameObject scrapIronObject = null;

			int tinOre = 0;
			GameObject tinOreObject = null;

			int copperOre = 0;
			GameObject copperOreObject = null;

			int silverOre = 0;
			GameObject silverOreObject = null;

			int elderBark = 0;
			GameObject elderBarkObject = null;

			int fineWood = 0;
			GameObject fineWoodObject = null;

			int chitin = 0;
			GameObject chitinObject = null;

			List<GameObject> defaultDrops = new List<GameObject>();
			foreach (GameObject toDrop in __result)
            {
				switch (toDrop.name)
				{
					case "Wood": // Wood
						wood += 1;
						woodObject = toDrop;
						break;
					case "RoundLog": // Corewood
						coreWood += 1;
						coreWoodObject = toDrop;
						break;
					case "Stone": // Stone
						stone += 1;
						stoneObject = toDrop;
						break;
					case "ScrapIron": // Iron
						scrapIron += 1;
						scrapIronObject = toDrop;
						break;
					case "TinOre": // Tin
						tinOre += 1;
						tinOreObject = toDrop;
						break;
					case "CopperOre": // Copper
						copperOre += 1;
						copperOreObject = toDrop;
						break;
					case "SilverOre": // Silver
						silverOre += 1;
						silverOreObject = toDrop;
						break;
					case "ElderBark": // ElderBark
						elderBark += 1;
						elderBarkObject = toDrop;
						break;
					case "FineWood": // Finewood
						fineWood += 1;
						fineWoodObject = toDrop;
						break;
					case "Chitin": // Chitin
						chitin += 1;
						chitinObject = toDrop;
						break;

					default:
						defaultDrops.Add(toDrop);
						break;
				}
			}

			// Add Wood
			for (int i = 0; i < Helper.applyModifierValue(wood, Configuration.Current.Gathering.wood); i++)
			{
				defaultDrops.Add(woodObject);
			}

			// Add CoreWood
			for (int i = 0; i < Helper.applyModifierValue(coreWood, Configuration.Current.Gathering.coreWood); i++)
			{
				defaultDrops.Add(coreWoodObject);
			}

			// Add Stone
			for (int i = 0; i < Helper.applyModifierValue(stone, Configuration.Current.Gathering.stone); i++)
			{
				defaultDrops.Add(stoneObject);
			}

			// ScrapIron
			for (int i = 0; i < Helper.applyModifierValue(scrapIron, Configuration.Current.Gathering.ironScrap); i++)
			{
				defaultDrops.Add(scrapIronObject);
			}

			// TinOre
			for (int i = 0; i < Helper.applyModifierValue(tinOre, Configuration.Current.Gathering.tinOre); i++)
			{
				defaultDrops.Add(tinOreObject);
			}

			// CopperOre
			for (int i = 0; i < Helper.applyModifierValue(copperOre, Configuration.Current.Gathering.copperOre); i++)
			{
				defaultDrops.Add(copperOreObject);
			}

			// silverOre
			for (int i = 0; i < Helper.applyModifierValue(silverOre, Configuration.Current.Gathering.silverOre); i++)
			{
				defaultDrops.Add(silverOreObject);
			}

			// ElderBark
			for (int i = 0; i < Helper.applyModifierValue(elderBark, Configuration.Current.Gathering.elderBark); i++)
			{
				defaultDrops.Add(elderBarkObject);
			}

			// FineWood
			for (int i = 0; i < Helper.applyModifierValue(fineWood, Configuration.Current.Gathering.fineWood); i++)
			{
				defaultDrops.Add(fineWoodObject);
			}

			// Chitin
			for (int i = 0; i < Helper.applyModifierValue(chitin, Configuration.Current.Gathering.chitin); i++)
			{
				defaultDrops.Add(chitinObject);
			}

			__result = defaultDrops;
		}
	}


}
