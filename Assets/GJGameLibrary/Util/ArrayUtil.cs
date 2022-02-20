using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GJGameLibrary
{
    public class ArrayUtil
    {
        public static T[][] GetSplitedList<T>(T[] array, int maxCount)
        {
            List<T[]> output = new List<T[]>();
            for (int i = 0; i < array.Length / maxCount + 1; i++)
            {
                List<T> tmp = new List<T>();
                for (int j = 0; j < maxCount; j++)
                {
                    int index = i * maxCount + j;
                    if (index >= array.Length)
                        break;

                    tmp.Add(array[index]);
                }
                if(tmp.Count>0)
                    output.Add(tmp.ToArray());
            }
            return output.ToArray();
        }
        public static T[] Range<T>(T[] array, int start, int count)
        {
            if (array.Length < start)
            {
                return new T[] { };
            }

            var list = array.ToList();

            if (array.Length < start + count)
            {
                count = array.Length % count;
            }
            var result = list.ToList().GetRange(start, count).ToArray();
            return result;
        }
    }
}
