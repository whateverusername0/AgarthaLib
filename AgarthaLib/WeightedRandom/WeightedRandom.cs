using System;
using System.Collections.Generic;
using System.Linq;

namespace Agartha.WeightedRandom
{
    // my implementation of weighted random pick from TIDESIM.
    public class WeightedRandom
    {
        private readonly Random _rng;

        public WeightedRandom(Random rng) : this()
            => _rng = rng;

        public WeightedRandom()
            => _rng = new Random();

        /// <summary>
        ///     Picks a random item using the weighted random algorithm.
        /// </summary>
        /// <typeparam name="T"> Item's type. </typeparam>
        /// <param name="items"> A weighted list of items. </param>
        /// <param name="rng"> Your own random if necessary. </param>
        /// <returns> The picked item. </returns>
        /// <exception cref="ArgumentNullException"> Gets thrown if the collection is null. </exception>
        /// <exception cref="Exception"> Gets thrown if the random pick somehow fails. </exception>
        public WeightedItem<T> Pick<T>(List<WeightedItem<T>> items, Random rng = null)
        {
            if (items.Count == 0 || items == null)
                throw new ArgumentNullException("Items were null!");

            rng ??= _rng;
            var totalWeight = items.Sum(item => item.Weight);
            var rand = (float)rng.NextDouble() * totalWeight;

            foreach (var item in items)
            {
                rand -= item.Weight;
                if (rand <= 0) return item;
            }

            throw new Exception("Weighted random failed! This should never happen!");
        }

        public T Pick<T>(List<WeightedItem<T>> items)
            => Pick(items, _rng).Value;

        public T PityPick<T>(ref List<WeightedItem<T>> items)
        {
            var @return = Pick(items, _rng);

            items.ForEach(q => q.Weight += 1);
            items[items.IndexOf(@return)].Weight = 1;

            return @return.Value;
        }
    }
}
