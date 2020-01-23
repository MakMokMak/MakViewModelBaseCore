using System.Collections.Generic;

namespace MakViewModelBaseCoreTest
{
    class TestHelper
    {
        internal static bool IsSameCollection<T>(IList<T> expected, IList<T> actual)
        {
            if (expected.Count != actual.Count) return false;

            for (int i = 0; i < expected.Count; ++i)
            {
                if (!expected[i].Equals(actual[i])) return false;
            }

            return true;
        }
    }
}
