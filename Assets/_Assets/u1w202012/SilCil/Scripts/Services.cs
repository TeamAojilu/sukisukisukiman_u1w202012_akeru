﻿namespace Unity1Week202012
{
    public static class Services
    {
        public static IBoard Board { get; set; } = new SampleBoard();
        public static IPointerInput PointerInput { get; set; } = new SamplePointerInput();
    }
}