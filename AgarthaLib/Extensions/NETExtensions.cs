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
        public static float Reverse(this float @object, float max)
            => max - @object;

        public static int Reverse(this int @object, int max)
            => max - @object;

        public static bool IsValid<T>(this List<T> @object)
            => @object != null && @object.Count > 0;

        public static List<T> Reverse<T>(this List<T> @object)
        {
            var list = new List<T>(@object);
            list.Reverse();
            return list;
        }

        public static float Normalize(this float @object, float min, float max)
            => (@object - min) / (max - min);
        public static float Normalize(this float @object, ValueRange<float> thresholds)
            => Normalize(@object, thresholds.Min, thresholds.Max);

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
    }
}