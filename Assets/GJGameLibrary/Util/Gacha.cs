using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GJGameLibrary
{
    public class Gacha<T>
    {
        public GachaDataElement<T>[] datas;

        public Gacha(GachaDataElement<T>[] datas)
        {
            datas = datas.OrderByDescending(x=>x.chance).ToArray();
            float chance = 0f;
            for(int i =0;i < datas.Length; i++)
            {
                chance += datas[i].chance;
                datas[i].chance = chance;
            }
            this.datas = datas;
        }

        public T[] GetRandomItem(int count)
        {
            if (datas.Length < count)
                return datas.Select(x => x.data).ToArray();
            else
            {
                return GetRandomItem(count, new List<GachaDataElement<T>>());
            }
        }
        public T[] GetRandomItemDistinct(int count)
        {
            if (count < datas.Length)
                return GetRandomItemDistinct(count, new List<GachaDataElement<T>>());
            else
                return datas.Select(x => x.data).ToArray();
        }

        private T[] GetRandomItem(int count, List<GachaDataElement<T>> selected)
        {
            selected.Add(GetRandomItem(datas.ToList()));
            if (selected.Count < count)
                return GetRandomItem(count, selected);
            else
                return selected.Select(x => x.data).ToArray();
        }
        private T[] GetRandomItemDistinct(int count, List<GachaDataElement<T>> selected)
        {
            var currentDatas = datas.Where(x => !selected.Select(y => y.key).Contains(x.key)).ToList();
            if (selected.Count == count || currentDatas.Count == 0)
                return selected.Select(x=>x.data).ToArray();

            selected.Add(GetRandomItem(currentDatas));

            return GetRandomItemDistinct(count, selected);
        }

        private GachaDataElement<T> GetRandomItem(List<GachaDataElement<T>> data)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);
            var filted = data.Where(x => x.chance >= chance);
            if (filted.Count() == 0)
                return data.First();
            else
                return data.Where(x => x.chance >= chance).First();
        }
    }
    public class GachaDataElement<T>
    {
        public T data;
        public float chance;
        public string key { get; private set; }
        public GachaDataElement(T data, float chance)
        {
            this.data = data;
            this.chance = chance;
            key = Guid.NewGuid().ToString();
        }
    }
}