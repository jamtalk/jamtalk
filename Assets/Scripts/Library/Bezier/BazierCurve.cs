using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJGameLibrary.Util.Bezier
{
    public class Bezier
    {
        public static Vector3[] Curve(int count, params Vector3[] points)
        {
            var result = new Vector3[count + 1];
            if (points == null || points.Length < 2) return new Vector3[] { };

            result = new Vector3[count + 1];
            float unit = 1.0f / count;

            int n = points.Length - 1;
            int[] C = GetCombinationValues(n); 
            float[] T = new float[n + 1];      
            float[] U = new float[n + 1];      

            int k = 0; float t = 0f;
            for (; k < count + 1; k++, t += unit)
            {
                result[k] = Vector3.zero;

                T[0] = 1f;
                U[0] = 1f;
                T[1] = t;
                U[1] = 1f - t;

                for (int i = 2; i <= n; i++)
                {
                    T[i] = T[i - 1] * T[1];
                    U[i] = U[i - 1] * U[1];
                }

                for (int i = 0; i <= n; i++)
                {
                    result[k] += C[i] * T[i] * U[n - i] * points[i];
                }
            }

            return result;
        }

        #region Methods
        private static int[] GetCombinationValues(int n)
        {
            int[] arr = new int[n + 1];

            for (int r = 0; r <= n; r++)
            {
                arr[r] = Combination(n, r);
            }
            return arr;
        }

        private static int Factorial(int n)
        {
            if (n == 0 || n == 1) return 1;
            if (n == 2) return 2;

            int result = n;
            for (int i = n - 1; i > 1; i--)
            {
                result *= i;
            }
            return result;
        }

        private static int Permutation(int n, int r)
        {
            if (r == 0) return 1;
            if (r == 1) return n;

            int result = n;
            int end = n - r + 1;
            for (int i = n - 1; i >= end; i--)
            {
                result *= i;
            }
            return result;
        }

        private static int Combination(int n, int r)
        {
            if (n == r) return 1;
            if (r == 0) return 1;

            if (n - r < r)
                r = n - r;

            return Permutation(n, r) / Factorial(r);
        }
        #endregion
    }
}
