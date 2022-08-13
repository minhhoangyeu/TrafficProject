using System;
using System.Collections.Generic;
using System.Linq;

namespace Traffic.Application.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable, Func<T, T, bool> comparer)
        {
            return enumerable.Distinct(new LambdaComparer<T>(comparer));
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> comparer)
        {
            var dictionary = new HashSet<TKey>();
            foreach (T item in source)
            {
                TKey key = comparer(item);
                if (dictionary.Add(key))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> Except<T, TKey>(
            this IEnumerable<T> items,
            IEnumerable<T> other,
            Func<T, TKey> lambda)
        {
            return from item in items
                   join otherItem in other on lambda(item) equals lambda(otherItem) into tempItems
                   from temp in tempItems.DefaultIfEmpty()
                   where ReferenceEquals(null, temp) || temp.Equals(default(T))
                   select item;
        }

        public static IEnumerable<T> ExceptExtension<T>(
            this IEnumerable<T> source,
            IEnumerable<T> target,
            Func<T, T, bool> lambda)
        {
            return source.Except(target, new LambdaComparer<T>(lambda));
        }

        public static IEnumerable<T> Intersect<T>(
            this IEnumerable<T> listA,
            IEnumerable<T> listB,
            Func<T, T, bool> lambda)
        {
            return listA.Intersect(listB, new LambdaComparer<T>(lambda));
        }

        private class LambdaComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _expression;

            public LambdaComparer(Func<T, T, bool> lambda)
            {
                this._expression = lambda;
            }
            public bool Equals(T x, T y)
            {
                return this._expression(x, y);
            }

            public int GetHashCode(T obj)
            {
                /*
                 If you just return 0 for the hash the Equals comparer will kick in. 
                 The underlying evaluation checks the hash and then short circuits the evaluation if it is false.
                 Otherwise, it checks the Equals. If you force the hash to be true (by assuming 0 for both objects), 
                 you will always fall through to the Equals check which is what we are always going for.
                */
                return 0;
            }
        }
    }
}
