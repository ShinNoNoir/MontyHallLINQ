using System;
using System.Collections.Generic;
using System.Linq;

namespace MontyHallLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            /*================================================================
             * The purpose of this program is to compute the probability of  
             * winning a prize in the Monty Hall problem.
             * 
             * The aim is to express this program in such a way that it 
             * closely resembles a natural language description of the problem.
             */

            // Sketch of intended solution using dummy implementations:
            var doors = new List<int>() { 1, 2, 3 };
            
            var switchingStrategyWinProbability
                = from doorWithPrize in OneOf(doors)
                  from doorPickedByContestant in OneOf(doors)
                  let doorsQuizMasterCouldOpen = doors.Without(doorWithPrize, doorPickedByContestant)
                  from doorOpenedByQuizMaster in OneOf(doorsQuizMasterCouldOpen)
                  let doorContestantCouldSwithTo = doors.Without(doorPickedByContestant, doorOpenedByQuizMaster).First()
                  select doorContestantCouldSwithTo == doorWithPrize;
        }
        
        // This is a dummy implementation with a type signature to satisfy the compiler for now:
        static IEnumerable<T> OneOf<T>(IEnumerable<T> items) => items;
    }

    static class IEnumerableExtensions
    {
        public static IEnumerable<T> Without<T>(this IEnumerable<T> list, params T[] items)
            => list.Where(item => !items.Contains(item));
    }
}
