using System;

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

            if (value >= 0)
            {
                targetValue = targetValue + ((targetValue / 100) * value);
            }
            else
            {
                targetValue = targetValue - ((targetValue / 100) * (value * -1));
            }
            return targetValue;
        }
    }
}
