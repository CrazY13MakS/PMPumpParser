using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace PMPumpParser
{
    class CustomTextParser
    {
        private readonly String _rawText;
        /// <summary>
        ///  Upper bound of occurrences of symbol
        /// </summary>
        private const int MaxCharsCount = 10;

        private readonly int _collectionCapacity;
        public CustomTextParser([DisallowNull]String text)
        {
            _rawText = text;
            _collectionCapacity = _rawText.Length / 64;
        }

        public String ParseUsingSpanAndHashset()
        {
            ReadOnlySpan<char> span = _rawText.AsSpan();
            HashSet<CustomTuple> hashSet = new HashSet<CustomTuple>(_collectionCapacity, new CustomTupleEqualityComparer());
            for (int i = 0; i < span.Length; i++)
            {
                var tuple = new CustomTuple() { Char = span[i], Count = 1 };
                if (hashSet.TryGetValue(tuple, out CustomTuple value))
                {
                    value.Count++;
                    if (value.Count < MaxCharsCount)
                    {
                        value.Indexes.Add(i);
                    }
                }
                else
                {
                    tuple.Indexes = new List<int>(MaxCharsCount) { i };
                    hashSet.Add(tuple);
                }
            }
            var indexesList = hashSet.Where(x => x.Count < MaxCharsCount).SelectMany(x => x.Indexes).OrderBy(x => x).ToList();
            StringBuilder stringBuilder = new StringBuilder(indexesList.Count);
            for (int i = 0; i < indexesList.Count; i++)
            {

                stringBuilder.Append(span[indexesList[i]]);
            }
            return stringBuilder.ToString();
        }

        public String ParseUsingHashset()
        {
            HashSet<CustomTuple> hashSet = new HashSet<CustomTuple>(_collectionCapacity, new CustomTupleEqualityComparer());
            for (int i = 0; i < _rawText.Length; i++)
            {
                var tuple = new CustomTuple() { Char = _rawText[i], Count = 1 };
                if (hashSet.TryGetValue(tuple, out CustomTuple value))
                {
                    value.Count++;
                    if (value.Count < MaxCharsCount)
                    {
                        value.Indexes.Add(i);
                    }
                }
                else
                {
                    tuple.Indexes = new List<int>(MaxCharsCount) { i };//max
                    hashSet.Add(tuple);
                }
            }
            var indexesList = hashSet.Where(x => x.Count < MaxCharsCount).SelectMany(x => x.Indexes).OrderBy(x => x).ToList();
            StringBuilder stringBuilder = new StringBuilder(indexesList.Count);
            for (int i = 0; i < indexesList.Count; i++)
            {

                stringBuilder.Append(_rawText[indexesList[i]]);
            }
            return stringBuilder.ToString();
        }

        public String ParseUsingDictionary()
        {
            Dictionary<char, CustomTuple> dictionary = new Dictionary<char, CustomTuple>(_collectionCapacity);
            for (int i = 0; i < _rawText.Length; i++)
            {
                var currentChar = _rawText[i];
                if (dictionary.TryGetValue(currentChar, out CustomTuple value))
                {
                    value.Count++;
                    if (value.Count < MaxCharsCount)
                    {
                        value.Indexes.Add(i);
                    }
                }
                else
                {
                    dictionary[currentChar] = new CustomTuple() { Count = 1, Char = currentChar, Indexes = new List<int>(MaxCharsCount) { i } };

                }
            }
            var indexesList = dictionary.Where(x => x.Value.Count < 10).SelectMany(x => x.Value.Indexes).OrderBy(x => x).ToList();
            StringBuilder stringBuilder = new StringBuilder(indexesList.Count);
            for (int i = 0; i < indexesList.Count; i++)
            {

                stringBuilder.Append(_rawText[indexesList[i]]);
            }
            return stringBuilder.ToString();
        }

        public String ParseUsingDictionaryAndSpan()
        {
            ReadOnlySpan<char> span = _rawText.AsSpan();
            Dictionary<char, CustomTuple> dictionary = new Dictionary<char, CustomTuple>(_collectionCapacity);
            for (int i = 0; i < span.Length; i++)
            {
                var currentChar = span[i];
                if (dictionary.TryGetValue(currentChar, out CustomTuple value))
                {
                    value.Count++;
                    if (value.Count < MaxCharsCount)
                    {
                        value.Indexes.Add(i);
                    }
                }
                else
                {
                    dictionary[currentChar] = new CustomTuple() { Count = 1, Char = currentChar, Indexes = new List<int>(MaxCharsCount) { i } };
                }
            }
            var indexesList = dictionary.Where(x => x.Value.Count < MaxCharsCount).SelectMany(x => x.Value.Indexes).OrderBy(x => x).ToList();
            StringBuilder stringBuilder = new StringBuilder(indexesList.Count);
            for (int i = 0; i < indexesList.Count; i++)
            {
                stringBuilder.Append(span[indexesList[i]]);
            }
            return stringBuilder.ToString();
        }

        private class CustomTuple
        {
            public int Count { get; set; }
            public char Char { get; set; }

            public List<int> Indexes { get; set; }

        }

        private class CustomTupleEqualityComparer : IEqualityComparer<CustomTuple>
        {
            public bool Equals([AllowNull] CustomTuple x, [AllowNull] CustomTuple y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                return x.Char == y.Char;
            }

            public int GetHashCode([DisallowNull] CustomTuple obj)
            {
                return obj.Char.GetHashCode();
            }
        }
    }
}
