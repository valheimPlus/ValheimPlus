using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Diagnostics;

namespace ValheimPlus
{
    static class GameObjectAssistant
    {
        private static ConcurrentDictionary<float, Stopwatch> stopwatches = new ConcurrentDictionary<float, Stopwatch>();

        public static Stopwatch GetStopwatch(GameObject o)
        {
            float hash = GetGameObjectPosHash(o);
            Stopwatch stopwatch = null;

            if (!stopwatches.TryGetValue(hash, out stopwatch))
            {
                stopwatch = new Stopwatch();
                stopwatches.TryAdd(hash, stopwatch);
            }

            return stopwatch;
        }

        private static float GetGameObjectPosHash(GameObject o)
        {
            return (1000f * o.transform.position.x) + o.transform.position.y + (.001f * o.transform.position.z);
        }

        public static T GetChildComponentByName<T>(string name, GameObject objected) where T : Component
        {
            foreach (T component in objected.GetComponentsInChildren<T>(true))
            {
                if (component.gameObject.name == name)
                {
                    return component;
                }
            }
            return null;
        }
    }
}
