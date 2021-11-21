using System;
using System.Collections.Generic;

namespace ImmutableArray
{
    public class Array
    {
        public int Size;
        public int Depth;

        private static Dictionary<int, int> powers = new Dictionary<int, int>();

        private const int _maxArrSize = 3;
        private readonly Array[] refs;
        private readonly int[] values;

        public int this[uint i]
        {
            get { return FindElemAt(i); }
        }
        
        public Array(int size)
        {
            Size = size;
            Depth = (int) Math.Ceiling(Math.Log(size, _maxArrSize));
            if (Depth > 1)
            {
                refs = new Array[_maxArrSize];
                for (int i = 0; i < _maxArrSize; ++i)
                {
                    refs[i] = new Array(Pow(_maxArrSize, Depth - 1));
                }    
            }
            else
            {
                values = new int[_maxArrSize];
            }
        }

        private Array(Array other, uint idx, int newVal)
        {
            Depth = other.Depth;
            Size = other.Size;
            if (Depth > 1)
            {
                var levelIdx = idx / Pow(_maxArrSize, Depth - 1);
                refs = new Array[_maxArrSize];
                for (int i = 0; i < _maxArrSize; ++i)
                {
                    refs[i] = other.refs[i];
                    if (levelIdx == i)
                    {
                        refs[i] = new Array(other.refs[i], idx, newVal);
                    }
                }
            }
            else
            {
                values = new int[_maxArrSize];
                for (int i = 0; i < _maxArrSize; ++i)
                {
                    values[i] = other.values[i];
                    if (idx % _maxArrSize == i)
                    {
                        values[i] = newVal;
                    }
                }
            }
        }

        private Array(Array a)
        {
            Size = a.Size;
            Depth = a.Depth;
        }

        public int FindElemAt(uint idx)
        {
            if (Depth > 1)
            {
                var p = Pow(_maxArrSize, Depth - 1);
                var actualIdx = idx / p;
                return refs[actualIdx].FindElemAt(idx);
            }
            else
            {
                return values[idx % _maxArrSize];
            }
        }

        private int Pow(int @base, int power)
        {
            if (powers.ContainsKey(power))
            {
                return powers[power];
            }
            
            var val = @base;
            for (int i = 0; i < (power - 1); ++i)
            {
                val *= val;
            }
            powers.Add(power, val);
            return val;
        }

        public Array Update(uint idx, int val)
        {
            return new Array(this, idx, val);
        }
    }
}