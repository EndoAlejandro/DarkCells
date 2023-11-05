namespace DarkHavoc.Senses
{
    public readonly struct WallResult
    {
        public bool TopCheck { get; }
        public bool MidCheck { get; }
        public bool BottomCheck { get; }
        public bool FacingWall => TopCheck || MidCheck || BottomCheck;

        public WallResult(bool topCheck, bool midCheck, bool bottomCheck)
        {
            TopCheck = topCheck;
            MidCheck = midCheck;
            BottomCheck = bottomCheck;
        }
    }
}