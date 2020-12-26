using System.Collections.Generic;

namespace Unity1Week202012.Aojilu.Title
{
    public class SampleAchivementTextSupplier : IAchivementTextSupplier
    {
        public Dictionary<string, bool> GetAchivementDataList()
        {
            return new Dictionary<string,bool> {

                { "testAchievement_1" ,true},
                { "testAchievement_2" ,true},
                { "testAchievement_3" ,true},
                { "testAchievement_false" ,false},
            };
        }
    }
}