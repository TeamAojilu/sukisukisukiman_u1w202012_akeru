using System;
using UnityEngine;

namespace Unity1Week202012
{
    /// <summary>
    /// ゲームのセーブデータ
    /// </summary>
    [Serializable]
    public class MyGameSaveData
    {
        [SerializeField] public string m_playerName;
        [SerializeField] public int m_maxScore;
    }
}