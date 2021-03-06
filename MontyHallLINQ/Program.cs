﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VectorSpace;

namespace MontyHallLINQ
{
    class Program
    {
        /// <summary>
        /// Helper function to ensure a dot is used when printing doubles.
        /// </summary>
        private static void ConfigureOutputFormatting()
        {
            var ci = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        static void Main(string[] args)
        {
            ConfigureOutputFormatting();

            /*=================================================================
             * The purpose of this program is to compute the probability of  
             * winning a prize in the Monty Hall problem.
             * 
             * The aim is to express this program in such a way that it 
             * closely resembles a natural language description of the problem.
             *=================================================================
             */
            var doors = new List<int>() { 1, 2, 3 };

            var switchingStrategyWinProbability
                = from doorWithPrize in OneOf(doors)
                  from doorPickedByContestant in OneOf(doors)
                  let doorsQuizMasterCouldOpen = doors.Without(doorWithPrize, doorPickedByContestant)
                  from doorOpenedByQuizMaster in OneOf(doorsQuizMasterCouldOpen)
                  let doorContestantCouldSwitchTo = doors.Without(doorPickedByContestant, doorOpenedByQuizMaster).First()
                  select doorContestantCouldSwitchTo == doorWithPrize;

            Console.WriteLine($"{nameof(switchingStrategyWinProbability)}:\n\t{switchingStrategyWinProbability}");

            // The above computation will result in a winning probability
            // of 2/3 (0.667) being printed to screen.


            // Now, some people may believe the probability of winning is 1/2,
            // reasoning that in the end the contestant is presented with 
            // only 2 choices:
            //   * the door initially picked by the contestant, or
            //   * the other remaining door.
            //
            // Now, if the contestant would randomly decide on the spot 
            // whether to switch doors or stick with the door initially 
            // picked, then indeed the probability of winning becomes 
            // fifty-fifty:
            var winProbabilityWhenDecidingOnTheSpot
                = from doorWithPrize in OneOf(doors)
                  from doorPickedByContestant in OneOf(doors)
                  let doorsQuizMasterCouldOpen = doors.Without(doorWithPrize, doorPickedByContestant)
                  from doorOpenedByQuizMaster in OneOf(doorsQuizMasterCouldOpen)
                  let doorContestantCouldSwitchTo = doors.Without(doorPickedByContestant, doorOpenedByQuizMaster).First()

                  from contestantIsGoingToSwitch in OneOf(new List<bool> { false, true })
                  
                  select (contestantIsGoingToSwitch 
                            ? doorContestantCouldSwitchTo 
                            : doorPickedByContestant
                         ) == doorWithPrize;

            Console.WriteLine($"{nameof(winProbabilityWhenDecidingOnTheSpot)}:\n\t{winProbabilityWhenDecidingOnTheSpot}");
        }

        /// <summary>
        /// Creates a linear combination of N basis vectors (<paramref name="items"/>)
        /// where each basis vector is scaled by 1/N.
        /// The result represents a probability vector where each outcome has equal probability
        /// of occurrence (uniform distribution).
        /// </summary>
        /// <typeparam name="T">The basis of the vector space.</typeparam>
        /// <param name="items">A finite number of basis vectors.</param>
        /// <returns>A probability vector for a uniform distribution.</returns>
        static Vector<T> OneOf<T>(IEnumerable<T> items)
        {
            var res = new Vector<T>();
            var list = items.ToList();
            var probability = 1.0 / list.Count;
            foreach (var element in list)
            {
                res[element] = probability;
            }
            return res;
        }
    }

    /// <summary>
    /// Provides convenient extension methods for <see cref="IEnumerable{T}"/>s.
    /// </summary>
    static class IEnumerableExtensions
    {
        /// <summary>
        /// Filters a sequence of values based on a <paramref name="items"/> to exclude.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="items">Items to omit from <paramref name="source"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements which do not occur in <paramref name="items"/>.</returns>
        public static IEnumerable<T> Without<T>(this IEnumerable<T> source, params T[] items)
            => source.Where(item => !items.Contains(item));
    }
}
