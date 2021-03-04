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


	/*
	[HarmonyPatch(typeof(DropTable), "GetDropList", new Type[] { typeof(int) })]
    public static class DropTable_GetDropList_Patch
	{
        private static Boolean Prefix(ref DropTable __instance, ref List<GameObject> __result, int amount)
        {
			List<GameObject> list = new List<GameObject>();

			// If object has no drops, exit
			if (__instance.m_drops.Count == 0)
			{
				__result = list;
				return false;
			}

			__instance.m_dropChance = 1;

			// if drop chance is not met, exit
			if (UnityEngine.Random.value > __instance.m_dropChance)
			{
				__result = list;
				return false;
			}

			List<DropTable.DropData> list2 = new List<DropTable.DropData>(__instance.m_drops);

			float num = 0f; // Drop weight chance

			int amountWithMultiplierApplied = 0;
			foreach (DropTable.DropData dropData in list2)
			{
				if(amountWithMultiplierApplied == 0) // be sure to add the multiplier only once
                {
					switch (dropData.m_item.name)
					{
						case "Wood": // Basic wood
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, Configuration.Current.Gathering.wood);
							break;
						case "Stone": // Stone
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, 200);
							break;
						case "IronScrap": // Iron from muddy scrap piles
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, Configuration.Current.Gathering.ironScrap);
							break;
						case "FineWood": // Fine wood
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, Configuration.Current.Gathering.fineWood);
							break;
						case "ElderBark": // Elder bark
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, Configuration.Current.Gathering.elderBark);
							break;
						case "RoundLog": // Core wood, untested
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, 200);
							break;
						case "TinOre": // Tin ore
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, Configuration.Current.Gathering.tinOre);
							break;
						case "CopperOre": // Copper ore, untested
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, 200);
							break;
						case "SilverOre": // Silver ore, untested
							amountWithMultiplierApplied += (int)Helper.applyModifierValue(amount, Configuration.Current.Gathering.silverOre);
							break;
						default:
							break;
					}
				}

				num += dropData.m_weight;
			}

			// dont reduce below 0
			if (amountWithMultiplierApplied < 1)
				amountWithMultiplierApplied = 1;

			// Overwrite the original with the multiplied amount
			amount = amountWithMultiplierApplied;


			// amount is the amount of total rolls per destroyed object
			for (int i = 0; i < amount; i++)
			{
				float num2 = UnityEngine.Random.Range(0f, num); // get random int up to weight
				bool flag = false;
				float num3 = 0f;
				foreach (DropTable.DropData dropData2 in list2)
				{
					num3 += dropData2.m_weight;
					if (num2 <= num3)
					{
						flag = true;

						// Default min/max
						int num4 = UnityEngine.Random.Range(dropData2.m_stackMin, dropData2.m_stackMax);

						Debug.Log(num4);
						for (int j = 0; j < num4 ; j++)
						{
							list.Add(dropData2.m_item);
						}
						if (__instance.m_oneOfEach)
						{
							list2.Remove(dropData2);
							num -= dropData2.m_weight;
							break;
						}
						break;
					}
				}
				if (!flag && list2.Count > 0)
				{
					list.Add(list2[0].m_item);
				}
			}

			__result = list;
			return false;
		}
    }
	*/
}
