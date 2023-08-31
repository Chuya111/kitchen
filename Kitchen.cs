using DD.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace DD
{
    public partial class ToolKit
    {
        //static double kitchenshaft_width = 500;type1用不上
        static double kitchenshaft_length = 600;

        static double kitchendoor_size = 1100;
        /*static int decorate_thickness = 25;*/

        static int kitchendoorPos = 2;//单侧开门的长边开门和短边开门
        static int opendoorPos = 3;//单侧开门,对侧开门,转折开门,三侧开门
        /*static bool shaftDir = true;*///镜像旋转怎么写

        //static Vector x_axis = new Vector(1, 0);
        //static Vector y_axis = new Vector(0, -1); //这两个应该是room的属性

        /*static string kitchenPath = "Kitchen/"; *///还没做
        static Rectangle kitchenshaft { get; set; }
        static Point origin;


        //static int standard_width = 600;//type1用不上
        static int washer_length = 800;
        static int cooker_length = 800;
        static int basket_length = 350;
        //static int electrical_length = 600;//type1用不上
        static int dishwasher_length = 600;
        //家具固定的标准尺寸

        public static void CheckKitchen(RoomTest room)
        {
            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Remove(x)); //这个写法有点蠢,需要优化一下 ?
            room.furnitures.Clear();

            SetKitchenKit(room);
            //SetKitchenShaft(room, 18, 42);

            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Add(x));
        }
        //???

        public static void SetKitchenShaft(RoomTest room, double width, double height)
        {
            if (kitchenshaft == null)
            {
                kitchenshaft = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Orange,
                    StrokeThickness = 4,
                };
                Canvas.SetZIndex(kitchenshaft, -1);
                MainWindow.Instance.canvas2.Children.Add(kitchenshaft);
                Canvas.SetLeft(kitchenshaft, Canvas.GetLeft(room));
                Canvas.SetTop(kitchenshaft, Canvas.GetTop(room));
            }
            else
            {
                kitchenshaft.Width = width;
                kitchenshaft.Height = height;
                Canvas.SetLeft(kitchenshaft, Canvas.GetLeft(room));
                Canvas.SetTop(kitchenshaft, Canvas.GetTop(room));
            }

        }
        //管井镜像

        public static void SetKitchenKit(RoomTest room)
        {
            var origin_x = (Canvas.GetLeft(room) + room.StrokeThickness * 0.5) * scale_rate;
            var origin_y = (Canvas.GetTop(room) + room.ActualHeight - room.StrokeThickness * 0.5) * scale_rate;
            var width = (room.ActualWidth - room.StrokeThickness) * scale_rate;
            var height = (room.ActualHeight - room.StrokeThickness) * scale_rate;

            const int maxkitchendepth = 4750;
            const int minkitchendepth = 1950;
            const int maxkitchenbay = 2750;
            const int minkitchenbay = 1650;

            //2450是怎么算出来的,需要2450写成家具加管井

            Action<RoomTest,double, double> function = (opendoorPos, kitchendoorPos, shaftDir) switch//怎么写镜像,shaftDir是什么？
            {
                (0, 0, true) when (maxkitchenbay >= width && width >= 2450 && maxkitchendepth >= height && height >= 3100) => KitchenType2,
                (0, 0, true) when (maxkitchenbay >= width && width >= 2450 && 3100 > height && height >= minkitchendepth) => KitchenType5,///忘记了高度的最小值
                (0, 0, true) when (2450 >= width && width >= 1850 && maxkitchendepth >= height && height >= 2750) => KitchenType2,
                (0, 0, true) when (2450 >= width && width >= 1850 && 2750 >= height && height >= 2550) => KitchenType3,
                //单侧开门,短边开门

                (0, 1, true) when (maxkitchenbay >= width && width >= 1850 && maxkitchendepth >= height && height >= 3900) => KitchenType3,
                (0, 1, true) when (maxkitchenbay >= width && width >= minkitchenbay && 3900 > height && height >= 2650) => KitchenType6,
                (0, 1, true) when (maxkitchenbay >= width && width >= 1850 && 2650 > height && height >= 2550) => KitchenType3,
                //单侧开门,长边边开门

                (1, 0, true) when (2450 >= width && width >= 1850 && maxkitchendepth >= height && height >= 3350) => KitchenType1,
                (1, 0, true) when (maxkitchenbay >= width && width >= 2450 && 3350 > height && height >= 2150) => KitchenType4,
                (1, 1, true) when (2450 >= width && width >= 1850 && maxkitchendepth >= height && height >= 3350) => KitchenType1,
                (1, 1, true) when (maxkitchenbay >= width && width >= 2450 && 3350 > height && height >= 2150) => KitchenType4,
                //对侧开门 此时无所谓0,1是否可以写成>=0

                (2, 0, true) when (maxkitchenbay >= width && width >= 1850 && maxkitchendepth >= height && height >= 2750) => KitchenType2,
                (2, 0, true) when (maxkitchenbay >= width && width >= 1850 && 2750 > height && height >= 2550) => KitchenType3,
                (2, 1, true) when (maxkitchenbay >= width && width >= 1850 && maxkitchendepth >= height && height >= 2750) => KitchenType2,
                (2, 1, true) when (maxkitchenbay >= width && width >= 1850 && 2750 > height && height >= 2550) => KitchenType3,
                //转折开门

                (3, 0, true) when (2450 >= width && width >= 1850 && maxkitchendepth >= height && height >= 3350) => KitchenType1,
                (3, 1, true) when (2450 >= width && width >= 1850 && maxkitchendepth >= height && height >= 3350) => KitchenType1,
                //三侧开门

                _ => null,
                //default被_替代
            }; ;

            function?.Invoke(room, /*new Point(origin_x, origin_y), */width, height);
        }
        //梳理逻辑：shower:剩余长宽判断家具类型,确定基点位置(紧邻着墙(用算法写？)和次一级的家具）生成方向：四个维度具体是哪一个维度(也可以用算法写？）
        //fridge,storage:剩余长宽判断家具类型，确定基点位置,生成方向
        //washer,cooker,basket,electrical,dishwasher::确定基点位置,生成方向，维度，图片
        
        //方向维度基点位置：1.右边墙 (width - thickness) * xAxis(下，上方）(0.5 * Fridge[FridgeType].Item1 + thickness) * yAxis + (width - thickness) * xAxis;
        //3.上边左  (height - thickness) * yAxis + (0 + thickness + 0.5 * Fridge[FridgeType].Item1 ) * xAxis
        //4.左下(0.5 * Fridge[FridgeType].Item1 + thickness) * yAxis + (0 + thickness) * xAxis;
        //5.下左(0 + thickness) * yAxis + (0.5 * Fridge[FridgeType].Item1 + thickness) * xAxis;
        //同一种布局里面找到基点然后去生成其他的
        //值A第一种情况左墙A= -1 * yAxis, xAxis；第二种情况下墙A=   xAxis, yAxis;右墙 yAxis, -1 * xAxis；上墙 -1 * xAxis, -1 * yAxis;(这是什么？怎么判断是上下左右？？？）明
        public void SetFridge(double restLength, double restWidth, double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            
            fridgeType = MatchFurniture(restWidth, restLength, FurnitureSize.Fridge);

            var fridge_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var fridge = new Furniture(FurnitureType.Fridge, fridge_pos.X / scale_rate, fridge_pos.Y / scale_rate,
            FurnitureSize.Fridge[fridgeType].Item1 / scale_rate, FurnitureSize.Fridge[fridgeType].Item2 / scale_rate, X_Axis, Y_Axis);
        }
        public void SetStorage(double restLength, double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            
            double restWidth = 600;
            storageType = MatchFurniture(restWidth, restLength, FurnitureSize.Storage);

            var storage_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var storage = new Furniture(FurnitureType.Storage, storage_pos.X / scale_rate, storage_pos.Y / scale_rate,
            FurnitureSize.Storage[storageType].Item1 / scale_rate, FurnitureSize.Storage[storageType].Item2 / scale_rate, X_Axis, Y_Axis);
        }

        public void SetWasher(double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            var washer_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var washer = new Furniture(FurnitureType.Washer, washer_pos.X / scale_rate, washer_pos.Y / scale_rate, 800 / scale_rate, 600 / scale_rate, X_Axis, Y_Axis);
        }
        public void SetCooker(double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            var cooker_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var cooker = new Furniture(FurnitureType.Cooker, cooker_pos.X / scale_rate, cooker_pos.Y / scale_rate, 800 / scale_rate, 600 / scale_rate, X_Axis, Y_Axis);
        }
        public void SetBasket(double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            var basket_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var basket = new Furniture(FurnitureType.Basket, basket_pos.X / scale_rate, basket_pos.Y / scale_rate, 350 / scale_rate, 600 / scale_rate, X_Axis, Y_Axis);
        }
        public void SetElectrical(double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            var electrical_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var electrical = new Furniture(FurnitureType.Electrical, electrical_pos.X / scale_rate, electrical_pos.Y / scale_rate, 300 / scale_rate, 600 / scale_rate, X_Axis, Y_Axis);
        }
        public void SetDishwasher(double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            var dishwasher_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var dishwasher = new Furniture(FurnitureType.Dishwasher, dishwasher_pos.X / scale_rate, dishwasher_pos.Y / scale_rate, 300 / scale_rate, 600 / scale_rate, X_Axis, Y_Axis);
        }

        //public void SetFillBlock(double restLength, double XLocation, double YLocation,
        //        Vector X_Axis, Vector Y_Axis)
        //{
        //    var storage_pos = origin + x_axis * XLocation + y_axis * YLocation;
        //    var storage = new Furniture(storage_pos.X / scale_rate, storage_pos.Y / scale_rate,
        //    restLength / scale_rate, 600 / scale_rate, X_Axis, Y_Axis);
        //}？？只是想画一个方框，没有部品库,extrastorage:填充块:判断有家具的维度哪里还有空缺，有空缺使用方法，维度有空缺分别去调用方法


        public void KitchenType1(RoomTest room, double width, double height)
        {
            double minrestLength = height - decorate_thickness * 2 - kitchenshaft_length - washer_length - 450;
            double fridge_restWidth = width - decorate_thickness * 2 - kitchendoor_size;//前面加一个冰箱的修饰
            double firstyleftlocation = 0 + decorate_thickness/* Fridge[FridgeType].Item1 type1里面固定*/ ;
            double xleftlocation = width - decorate_thickness;
            Vector leftx_axis = y_axis;
            Vector lefty_axis = -1 * x_axis;//布局里面声明静态的字段/属性

            //public void SetCooker(double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis)
            //{
            //    var cooker_pos = origin + x_axis * XLocation + y_axis * YLocation;
            //    var cooker = new Furniture(FurnitureType.Cooker, cooker_pos.X / scale_rate, cooker_pos.Y / scale_rate, 800 / scale_rate, 600 / scale_rate, X_Axis, Y_Axis);
            //}

            //SetWasher(xleftlocation, height - decorate_thickness - shaft_length - washer_length, Vector leftx_axis, Vector lefty_axis );
            (double restlength, double YLocation) SetCookerAndStorage(double restlength, double YLocation)
            {
                SetCooker(xleftlocation, YLocation, leftx_axis, lefty_axis);
                restlength -= Fridge[fridgeType].Item1;
                restlength += 450;
                YLocation += cooker_length;
                SetStorage(restlength, xleftlocation, YLocation,
                                                leftx_axis, lefty_axis);
                YLocation += Storage[storageType].Item1;
                return (restlength, YLocation);//？？
            }
            void SetWasherAndFillBlock(double restlength, double YLocation) 
            {
                SetWasher(xleftlocation, YLocation, leftx_axis, lefty_axis);
                restlength -= 450;
                YLocation += washer_length;
                //SetFillBlock(restLength, xleftlocation, YLocation,
                //                        leftx_axis, lefty_axis);//填充块怎么做？
            }
            if (height >= 3700)
            {
                if (height >= 4300)
                {//把共同的东西提取出来
                    double restlength = minrestLength - cooker_length - basket_length - dishwasher_length;
                    SetFridge(restlength, fridge_restWidth, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis);
                    double YLocation = firstyleftlocation + Fridge[fridgeType].Item1;
                    SetBasket(xleftlocation, YLocation, leftx_axis, lefty_axis);
                    YLocation += basket_length;

                    var result = SetCookerAndStorage(restlength, YLocation);
                    restlength = result.restlength;
                    YLocation = result.YLocation;

                    SetDishwasher(xleftlocation, YLocation, leftx_axis, lefty_axis);
                    YLocation += dishwasher_length;
                    SetWasherAndFillBlock(restlength, YLocation);
                }
                else
                {
                    double restlength = minrestLength - cooker_length - basket_length - dishwasher_length;
                    SetFridge(restlength, fridge_restWidth, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis);
                    double YLocation = firstyleftlocation + Fridge[fridgeType].Item1;
                    SetBasket(xleftlocation, YLocation, leftx_axis, lefty_axis);
                    YLocation += basket_length;

                    var result = SetCookerAndStorage(restlength, YLocation);
                    restlength = result.restlength;
                    YLocation = result.YLocation;

                    SetDishwasher(xleftlocation, YLocation, leftx_axis, lefty_axis);
                    YLocation += dishwasher_length;
                    SetWasherAndFillBlock(restlength, YLocation);
                }
            }
            else
            {
                double restlength = minrestLength - cooker_length - basket_length - dishwasher_length;
                SetFridge(restlength, fridge_restWidth, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis);
                double YLocation = firstyleftlocation + Fridge[fridgeType].Item1;
                SetBasket(xleftlocation, YLocation, leftx_axis, lefty_axis);
                YLocation += basket_length;

                var result = SetCookerAndStorage(restlength, YLocation);
                restlength = result.restlength;
                YLocation = result.YLocation;

                SetDishwasher(xleftlocation, YLocation, leftx_axis, lefty_axis);
                YLocation += dishwasher_length;
                SetWasherAndFillBlock(restlength, YLocation);
            }
        }
        public void KitchenType2(RoomTest room,  double width, double height) { }
        public void KitchenType3(RoomTest room,  double width, double height) { }
        public void KitchenType4(RoomTest room,  double width, double height) { }
        public void KitchenType5(RoomTest room,  double width, double height) { }
        public void KitchenType6(RoomTest room,  double width, double height) { }
    }




}
//    }
//}
