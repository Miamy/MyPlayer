using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyPlayer.CommonClasses
{
    public static class ExtensionMethods
    {
        public static T Previous<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item) - 1;
            return index > -1 ? list[index] : default;
        }
        public static T Next<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item) + 1;
            return index < list.Count() ? list[index] : default;
        }
        public static T Previous<T>(this IList<T> list, Func<T, bool> lookup)
        {
            var item = list.SingleOrDefault(lookup);
            var index = list.IndexOf(item) - 1;
            return index > -1 ? list[index] : default;
        }
        public static T Next<T>(this IList<T> list, Func<T, bool> lookup)
        {
            var item = list.SingleOrDefault(lookup);
            var index = list.IndexOf(item) + 1;
            return index < list.Count() ? list[index] : default;
        }
        public static T PreviousOrFirst<T>(this IList<T> list, T item)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");

            var previous = list.Previous(item);
            return previous == null ? list.First() : previous;
        }
        public static T NextOrLast<T>(this IList<T> list, T item)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");
            var next = list.Next(item);
            return next == null ? list.Last() : next;
        }
        public static T PreviousOrFirst<T>(this IList<T> list, Func<T, bool> lookup)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");
            var previous = list.Previous(lookup);
            return previous == null ? list.First() : previous;
        }
        public static T NextOrLast<T>(this IList<T> list, Func<T, bool> lookup)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");
            var next = list.Next(lookup);
            return next == null ? list.Last() : next;
        }


        private static readonly char[] Splitters = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
        public static string GetOneLevelUp(this string path)
        {
            return path.GetLevelUp(1);
        }

        public static string GetLevelUp(this string path, int count)
        {
            var parts = path.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < count + 1)
            {
                return "";
            }
            Array.Resize(ref parts, parts.Length - count);
            return "/" + string.Concat(parts.Select(s => s + "/"));
        }


        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> sequence, Func<T, IEnumerable<T>> childFetcher)
        {
            var itemsToYield = new Queue<T>(sequence);
            while (itemsToYield.Count > 0)
            {
                var item = itemsToYield.Dequeue();
                yield return item;

                var children = childFetcher(item);
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        itemsToYield.Enqueue(child);
                    }
                }
            }
        }
    }
}
