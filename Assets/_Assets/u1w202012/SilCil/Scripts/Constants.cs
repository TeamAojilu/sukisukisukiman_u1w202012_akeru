using System.Collections.Generic;

namespace Unity1Week202012
{
    public static class Constants
    {
        private static readonly string[] m_colors = new string[] { "red", "blue", "green", "black" };
        private static readonly string[] m_shapes = new string[] { "0", "1", "2", "3" };
        public static IReadOnlyList<string> ColorNames => m_colors;
        public static IReadOnlyList<string> ShapeNames => m_shapes;
    }
}
