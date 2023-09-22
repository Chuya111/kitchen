using DD.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DD
{
    public partial class ToolKit
    {

        static int decorate_thickness = 25;
        static int scale_rate = 25;

        public static void CheckFurnitures(RoomTest room)
        {

            Canvas canvas = MainWindow.Instance.canvas2;
            Action<RoomTest> function = room.Text switch
            {
                "Bedroom" => CheckBedroom,
                "Restroom" => CheckRestroom,
                "Kitchen" => CheckKitchen,
                _ => null,
            };

            function?.Invoke(room);
        }
        
        public static T MatchFurniture<T>(double restWidth, double restLength, Dictionary<T, Tuple<int, int, int>> FurType)
        {
            try
            {
                T furType = FurType.Keys
              .Where(x => FurType[x].Item1 <= restLength && FurType[x].Item2 <= restWidth)
              .OrderBy(x => FurType[x].Item1)
              .Last();
            
            return furType;
            }
            catch (InvalidOperationException)
            {
                return default(T);
            }

        }
            
        static private Transform GetTransform(Vector xAxis, Vector yAxis)
        {
            yAxis *= -1; //默认户型的y轴向上，而WPF的y轴向下，所以需要翻转（默认户型的xy轴是正常Rhino的xy，每个房间都需要遵守）
            var matrix = new Matrix(xAxis.X, xAxis.Y,
                                    yAxis.X, yAxis.Y,
                                    0, 0);

            Transform transform = new MatrixTransform(matrix);
            return transform;
        }
    }
}
