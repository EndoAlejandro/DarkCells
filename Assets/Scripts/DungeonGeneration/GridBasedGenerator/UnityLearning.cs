using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class UnityLearning
    {
        // n%a[i]
        // b[i]%n
        public static int getTotalX(List<int> a, List<int> b)
        {
            int maxA = a.Max();
            int minB = b.Min();

            int count = 0;
            for (int i = maxA; i <= minB; i++)
            {
                if (!ValidationA(i, a)) continue;
                if (!ValidationB(i, b)) continue;
                count++;
            }

            return count;
        }

        private static bool ValidationA(int n, List<int> numbers)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                if (n % numbers[i] != 0) return false;
            }

            return true;
        }

        private static bool ValidationB(int n, List<int> numbers)
        {
            for (int i = numbers.Count - 1; i >= 0; i--)
            {
                if (numbers[i] % n != 0) return false;
            }

            return true;
        }
    }
}