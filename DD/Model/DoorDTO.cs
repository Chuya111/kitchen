namespace DD.Model
{
    public class DoorDTO
    {
        public int parentModuleID { set; get; }
        public int parentModuleWallID { set; get; }
        public bool isMainDoor { set; get; }
        public bool isInside { set; get; }
        public bool isLeft { set; get; }
        public int rightOffset { set; get; }
        public int leftOffset { set; get; }
        public bool visible { set; get; }
        public int position { set; get; }
        public int doorSize { set; get; }
        public double strokeThickness { set; get; }
        public bool isStartPosition { set; get; }

    }
}
