using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012.Aojilu.Title
{
    public class SimpelAchivementDataSuplier : MonoBehaviour,IAchivementTextSupplier
    {
        private void Start()
        {
            AojiluService_Title.AchivementTextSupplier = this;
        }

        public Dictionary<string, bool> GetAchivementDataList()
        {
            return AojiluService.DataSaver.PlaySaveData.AchivementDatas;
        }

    }
}