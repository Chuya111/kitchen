using DD.Data;
using System.Collections.Generic;

namespace DD.Model
{
    public class LayOutObj
    {
        public int Id { set; get; }
        public string? Name { get; set; }
        public double X_coordinate { get; set; } = 0;
        public double Y_coordinate { get; set; } = 0;
        public double Width { get; set; }
        public double Height { get; set; }
        public string Color { get; set; }
        public double AxisWidth { get; set; }
        public double AxisHeight { get; set; }
        public List<MyModule> Modules { set; get; }
        public string ImagePath { get; set; } // 新添加的属性，用于指定截图位置
    }
}
