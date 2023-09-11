using System.Collections.Generic;

namespace DD.Model
{
    public class WallDTO
    {
        public int parentModuleID { set; get; }
        public int rightOffset { set; get; }
        public int leftOffset { set; get; }
        public bool visible { set; get; }
        /// 位置 上0右1下2左3
        public int position { set; get; }
        /// 墙长度
        public double lineLength { set; get; }
        /// 墙厚度
        public double strokeThickness { set; get; }

        public List<WindowDTO> lstWindows { set; get; }
        public List<DoorDTO> lstDoors { set; get; }

    }
}
