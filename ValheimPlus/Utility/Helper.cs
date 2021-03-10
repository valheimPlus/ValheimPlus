using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace ValheimPlus
{
    static class Helper
    {
		public static Character getPlayerCharacter(Player __instance)
		{
			return (Character)__instance;
		}

        public static float tFloat(this float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }
         
        public static float applyModifierValue(float targetValue, float value)
        {
            if (value == 50) value = 51; // Decimal issue
            if (value == -50) value = -51; // Decimal issue

            if (value <= -100)
                value = -100;

            float newValue = targetValue;

            if (value >= 0)
            {
                newValue = targetValue + ((targetValue / 100) * value);
            }
            else
            {
                newValue = targetValue - ((targetValue / 100) * (value * -1));
            }

            return newValue;
        }

        // Resize child EffectArea's collision that matches the specified type(s).
        public static void ResizeChildEffectArea(MonoBehaviour parent, EffectArea.Type includedTypes, float newRadius)
        {
            if (parent != null)
            {
                EffectArea effectArea = parent.GetComponentInChildren<EffectArea>();
                if (effectArea != null)
                {
                    if ((effectArea.m_type & includedTypes) != 0)
                    {
                        SphereCollider collision = effectArea.GetComponent<SphereCollider>();
                        if (collision != null)
                        {
                            collision.radius = newRadius;
                        }
                    }
                }
            }
        }


        public static List<Container> GetNearbyChests(GameObject target, float range = 10)
        {
            Collider[] hitColliders = Physics.OverlapSphere(target.transform.localPosition, range, LayerMask.GetMask(new string[] { "piece" }));

            // Order the found objects to select the nearest first instead of the farthest inventory.
            IOrderedEnumerable<Collider> orderedColliders = hitColliders.OrderBy(x => Vector3.Distance(target.transform.localPosition, x.transform.position));

            List<Container> validContainers = new List<Container>();
            foreach (var hitCollider in hitColliders)
            {
                try
                {
                    Container foundContainer = hitCollider.GetComponentInParent<Container>();
                    if (foundContainer.m_name.Contains("piece_chest") && foundContainer.GetInventory() != null)
                    {
                        validContainers.Add(foundContainer);
                    }
                }
                catch{ }
            }

            return validContainers;
        }



    }
}
