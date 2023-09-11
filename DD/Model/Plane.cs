using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DD
{
    internal struct MyPlane
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Vector Xaxis { get; set; }
        public Vector Yaxis { get; set; }

        //构造函数
        public MyPlane(double x, double y, Vector xaxis, Vector yaxis)
        {
            X = x;
            Y = y;
            Xaxis = xaxis;
            Yaxis = yaxis;
        }
    }
}
