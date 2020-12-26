using System.Collections.Generic;

namespace Unity1Week202012
{
    public static class Services
    {
        public static IBoard Board { get; set; } = new SampleBoard();
        public static ISpaceChecker SpaceChecker { get; set; } = new SampleSpaceChecker();
        public static IPiecePosition PiecePosition { get; set; } = new SamplePiecePosition();
        public static IPieceConnection PieceConnection { get; set; } = new PieceConnection();
        public static IPointerInput PointerInput { get; set; } = new SamplePointerInput();

        public static IPieceObjectFactory PieceObjectFactory { get; set; }
        public static IInitialPieceGenerator InitialPieceGenerator { get; set; } = new SampleInitialPieceGenerator();
        public static IPoppedPieceGenerator PoppedPieceGenerator { get; set; } = new SamplePoppedPieceGenerator();

        public static List<ICombination> Combinations { get; set; } = new List<ICombination>() { new SameColorCombination() };
        public static ICombinationsViewer CombinationsViewer { get; set; } = new SampleCombinationsViewer();

        public static IStartEffect StartEffect { get; set; }
        public static IBonusEffect BonusEffect { get; set; }

        public static IBonusSpaceChecker BonusSpaceChecker { get; set; } = new SampleBonusSpaceChecker();
        public static IBonusSpaceInfo BonusSpaceInfo { get; set; } = new SampleBonusSpaceInfo();

        public static void Reset()
        {
            Board = new SampleBoard();
            SpaceChecker = new SampleSpaceChecker();
            PiecePosition = new SamplePiecePosition();
            PieceConnection = new PieceConnection();
            PointerInput = new SamplePointerInput();

            PieceObjectFactory = null;
            InitialPieceGenerator = new SampleInitialPieceGenerator();
            PoppedPieceGenerator = new SamplePoppedPieceGenerator();

            Combinations.Clear();
            Combinations.Add(new SameColorCombination());
            CombinationsViewer = new SampleCombinationsViewer();

            StartEffect = null;
            BonusEffect = null;

            BonusSpaceChecker = new SampleBonusSpaceChecker();
            BonusSpaceInfo = new SampleBonusSpaceInfo();
        }
    }
}
