using NCMB;

namespace Unity1Week202012
{
    //NCMBObjectとMyGamePlayDataのデータの変換処理を行うinterface
    public interface INCMB_PlayerDataConverter
    {
        MyGameSaveData ConvertNCMB2PlayerData(NCMBObject ncmbObject);
        NCMBObject ConvertPlayerData2NCMB(MyGameSaveData data, NCMBObject ncmbObject);
    }
}