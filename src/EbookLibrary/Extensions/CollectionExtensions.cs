using System.Collections.Generic;

namespace EbookLibrary
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this List<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
    }
}
