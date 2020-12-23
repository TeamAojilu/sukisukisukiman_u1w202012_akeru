using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Aojilu
{
    /// <summary>
    /// テキストを正規表現で読み込んでpositionDataを生成するもの
    /// </summary>
    public class TextReadPieceSource : MonoBehaviour,IPieceDataList
    {
        [SerializeField]private TextAsset m_textData;

        public List<Vector2Int[]> m_PositionDataList => ReadText();

        private List<Vector2Int[]> ReadText()
        {
            var result = new List<Vector2Int[]>();
            var text = m_textData.text;
            var mc = System.Text.RegularExpressions.Regex.Matches(
                text, @"{(.*?)}\((.*?)\)", System.Text.RegularExpressions.RegexOptions.Singleline
                );
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                //Debug.Log($"{m.Groups[1].Value},{m.Groups[2].Value}");
                result.Add(ConvertText2Cells(m.Groups[1].Value, m.Groups[2].Value).ToArray());
            }

            return result;
        }

        private List<Vector2Int> ConvertText2Cells(string value, string index)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            var indexdata = index.Trim().Split(',');
            int center_x = int.Parse(indexdata[0]);
            int center_y = int.Parse(indexdata[1]);
            var texts = value.Trim().Split('\n');
            for (int y = 0; y < texts.Length; y++)
            {
                for (int x = 0; x < texts[y].Length; x++)
                {
                    switch (texts[y][x])
                    {
                        case '_': break;
                        case 'o': result.Add(new Vector2Int(x, y) - new Vector2Int(center_x, center_y)); break;

                    }
                }
            }
            return result;
        }
    }
}