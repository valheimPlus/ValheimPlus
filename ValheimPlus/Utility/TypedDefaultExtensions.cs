using System;

namespace ValheimPlus.Utility
{
    public static class TypedDefaultExtensions
    {
        public static object ToDefault(this Type targetType)
        {

            if (targetType == null)
                throw new NullReferenceException();

            var mi = typeof(TypedDefaultExtensions)
                .GetMethod("_ToDefaultHelper", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            var generic = mi.MakeGenericMethod(targetType);

            var returnValue = generic.Invoke(null, new object[0]);
            return returnValue;
        }

        static T _ToDefaultHelper<T>()
        {
            return default(T);
        }
    }
}
