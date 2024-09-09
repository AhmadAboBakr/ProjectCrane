using System;

namespace Kandooz.ScriptableSystem
{
    public static class Extensions
    {
        public static Type GetDeclaredType<T>(this T obj )
        {
            return typeof( T );
        }
    }
}