namespace Unity1Week202012
{
    public static class Services
    {
        public static IBoard Board { get; set; } = new SampleBoard();
        public static IPiecePosition PiecePosition { get; set; } = new SamplePiecePosition();
        public static IPointerInput PointerInput { get; set; } = new SamplePointerInput();
    }
}