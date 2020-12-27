using System;
using System.Linq;
using System.Collections.Generic;

namespace Unity1Week202012
{
    public class BanParameter
    {
        public readonly string m_id;
        public readonly int m_count;
        private readonly bool[] m_colors;
        private readonly bool[] m_shapes;
        private readonly bool[] m_sizes;

        public IReadOnlyList<bool> Colors => m_colors;
        public IReadOnlyList<bool> Shapes => m_shapes;
        public IReadOnlyList<bool> Sizes => m_sizes;

        public BanParameter(string id, int count, bool[] colors, bool[] shapes, bool[] sizes)
        {
            m_id = id;
            m_count = count;
            m_colors = colors;
            m_shapes = shapes;
            m_sizes = sizes;
        }
    }

    public class BanParameterReader
    {
        private const string Comment = "#";
        private static readonly char[] m_separators = new char[] { ' ', '\t' };

        private Dictionary<string, BanParameter> m_parameters = new Dictionary<string, BanParameter>();

        public IReadOnlyDictionary<string, BanParameter> Parameters => m_parameters;

        public void ParseParameters(string text)
        {
            foreach(var line in text.Split(new char[] { '\r', '\n' }))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith(Comment)) continue;

                string[] words = line.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length < 5) continue;

                string id = words[0];
                if (!int.TryParse(words[1], out int count)) continue;
                if (!TryConvertBoolArray(words[2], out bool[] colors) || colors.Length < Constants.ColorNames.Count) continue;
                if (!TryConvertBoolArray(words[3], out bool[] shapes) || shapes.Length < Constants.ShapeNames.Count) continue;
                if (!TryConvertBoolArray(words[4], out bool[] sizes)) continue;

                m_parameters.Add(id, new BanParameter(id, count, colors, shapes, sizes));
            }
        }

        private bool TryConvertBoolArray(string s, out bool[] result)
        {
            result = null;
            if (s.Any(c => c != '1' && c != '0')) return false;
            result = Convert(s).ToArray();
            return true;
        }

        private IEnumerable<bool> Convert(string s)
        {
            foreach(var c in s)
            {
                switch (c)
                {
                    case '0':
                        yield return false;
                        break;
                    case '1':
                        yield return true;
                        break;
                }
            }
        }
    }
}