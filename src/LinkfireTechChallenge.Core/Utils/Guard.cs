using System;

namespace LinkfireTechChallenge.Core.Utils
{
    public static class Guard
    {
        public static void NotNull<T>([ValidatedNotNull] this T value, string name) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        public static void NotNull<T>([ValidatedNotNull] this T value, string name, string msg) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(name, msg);
        }

        public static void NotNullOrWhiteSpace([ValidatedIsNullOrWhiteSpace] this string value, string name, string msg)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(name, msg);
        }
    }
}
