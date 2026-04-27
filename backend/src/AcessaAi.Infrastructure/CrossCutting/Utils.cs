using System;
using System.Collections.Generic;
using System.Text;

namespace AcessaAi.Infrastructure.CrossCutting
{
    public static class Utils
    {
        public static bool IsNull<T>(this T? obj) where T : class
        {
            return obj is null;
        }

        public static bool IsNotNull<T>(this T? obj) where T : class
        {
            return obj is not null;
        }
    }
}
