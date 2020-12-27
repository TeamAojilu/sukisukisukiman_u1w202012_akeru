using System.Collections.Generic;

namespace Unity1Week202012
{
    public static class Constants
    {
        private static readonly string[] m_colors = new string[] { "red", "blue", "green", "yellow" };
        private static readonly string[] m_shapes = new string[] { "0", "1", "2", "3" };
        public static IReadOnlyList<string> ColorNames => m_colors;
        public static IReadOnlyList<string> ShapeNames => m_shapes;

        // 累計の保存は大雑把でいいかなぁと思ったので、100分の1で足し算する.
        // 0.01だときりのいい数値になってしまうので、中途半端にする.
        public const float TotalScoreRate = 0.017f;
    }
}
