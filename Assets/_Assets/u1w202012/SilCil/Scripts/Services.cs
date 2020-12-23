namespace Unity1Week202012
{
    public static class Services
    {
        public static IBoard Board { get; set; } = new SampleBoard();
        public static IPiecePosition PiecePosition { get; set; } = new SamplePiecePosition();
        public static IPieceConnection PieceConnection { get; set; } = new PieceConnection();
        public static IPointerInput PointerInput { get; set; } = new SamplePointerInput();
        public static IBonusSpaceChecker BonusSpaceChecker { get; set; } = new SampleBonusSpaceChecker();
        public static IBonusSpaceInfo BonusSpaceInfo { get; set; } = new SampleBonusSpaceInfo();
        public static IPieceObjectFactory PieceObjectFactory { get; set; }
        public static IInitialPieceGenerator InitialPieceGenerator { get; set; } = new SampleInitialPieceGenerator();
    }
}