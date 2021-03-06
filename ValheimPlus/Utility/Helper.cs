using System;
using System.Reflection;

namespace ValheimPlus
{
    static class Helper
    {
        public static int TryGetBoolMethod(PropertyInfo prop, object target, string name)
        {
            var method = prop.PropertyType.GetMethod(name, BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

            if (method != null)
            {
                try
                {
                    var instance = prop.GetValue(target, null);
                    bool result = (bool)method.Invoke(instance, new object[] { });
                    return result ? 1 : 0;
                } catch { }
            }
            return -1;
        }

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
    }
}
