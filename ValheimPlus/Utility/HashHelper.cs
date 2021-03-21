using System;
namespace CustomSeed
{
    public static class HashHelper
    {
        public static int GetStableHashCode(this string str)
        {
            int num = 5381;
            int num2 = num;
            int num3 = 0;
            while (num3 < str.Length && str[num3] != '\0')
            {
                num = ((num << 5) + num ^ (int)str[num3]);
                if (num3 == str.Length - 1 || str[num3 + 1] == '\0')
                {
                    break;
                }
                num2 = ((num2 << 5) + num2 ^ (int)str[num3 + 1]);
                num3 += 2;
            }
            return num + num2 * 1566083941;
        }

    }
}
