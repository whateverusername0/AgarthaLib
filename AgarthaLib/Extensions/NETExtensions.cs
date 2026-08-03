using AgarthaLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class NETExtensions
    {
        public static float Loop(this float f, float loop)
            => f >= loop ? f - loop : (f <= -loop ? f + loop : f);

        public static float RecursiveLoop(this float f, float loop)
        {
            var result = f.Loop(loop);
            if (f > loop || f < -loop)
                return result.RecursiveLoop(loop);
            return result;
        }

        public static float Reverse(this float f, float max)
            => max - f;

        public static int Reverse(this int i, int max)
            => max - i;

        public static bool IsValid<T>(this List<T> l)
            => l != null && l.Count > 0;

        public static List<T> Reverse<T>(this List<T> l)
        {
            var list = new List<T>(l);
            list.Reverse();
            return list;
        }

        public static float Normalize(this float f, float min, float max)
            => (f - min) / (max - min);
        public static float Normalize(this float f, ValueRange<float> thresholds)
            => Normalize(f, thresholds.Min, thresholds.Max);

        public static bool Compare(this IEnumerator a, IEnumerator b)
        {
            var referenceEquals = ReferenceEquals(a, b);
            var notNull = a != null && b != null;
            var typesEqual = a.GetType() == b.GetType();

            var invalidNames = new string[] { "__state", "obj" };
            var fields = a.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => !invalidNames.Any(q => f.Name.Contains(q)));

            var fieldsEqual = false;

            try
            {
                fieldsEqual = fields.All(q => object.Equals(q.GetValue(a), q.GetValue(b)));
            }
            catch(Exception e) { Debug.LogWarning(e); }

            return referenceEquals || (notNull && typesEqual && fieldsEqual);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
            => source.MinBy(selector, null);

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (selector == null)
                throw new ArgumentNullException("selector");

            comparer ??= Comparer<TKey>.Default;

            using var sourceIterator = source.GetEnumerator();

            if (!sourceIterator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");

            var min = sourceIterator.Current;
            var minKey = selector(min);

            while (sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }
            return min;
        }

        public static TEnum[] GetEnumValues<TEnum>() where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .OrderBy(q => (int)(object)q);
            return values.ToArray();
        }
    }
}