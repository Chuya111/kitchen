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
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace DD
{
    public partial class ToolKit
    {
        static double shaft_width = 500;
        static double shaft_length = 600;

        static double door_size = 1100;
        static int decorate_thickness = 25;//找不到

        static int doorPos = 2;//单侧开门的长边开门和短边开门
        static int opendoorPos = 3;//单侧开门,对侧开门，转折开门，三侧开门
        static bool shaftDir = true;//镜像旋转怎么写

        static Vector x_axis = new Vector(1, 0);
        static Vector y_axis = new Vector(0, -1); //这两个应该是room的属性

        static string kitchenPath = "Kitchen/"; //还没做
        static Rectangle shaft { get; set; }


        static int standard_width = 600;
        static int washer_length = 800;
        static int cooker_length = 800;
        static int basket_length = 350;
        static int electrical_length = 600;
        static int dishwasher_length = 600;
        //家具固定的标准尺寸




        public static void CheckKitchen(RoomTest room)
        {
            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Remove(x)); //这个写法有点蠢，需要优化一下 ?
            room.furnitures.Clear();

            SetKitchenKit(room);
            //SetShaft(room, 18, 42);

            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Add(x));
        }
        //???

        public static void SetShaft(RoomTest room, double width, double height)
        {
            if (shaft == null)
            {
                shaft = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Orange,
                    StrokeThickness = 4,
                };
                Canvas.SetZIndex(shaft, -1);
                MainWindow.Instance.canvas2.Children.Add(shaft);
                Canvas.SetLeft(shaft, Canvas.GetLeft(room));
                Canvas.SetTop(shaft, Canvas.GetTop(room));
            }
            else
            {
                shaft.Width = width;
                shaft.Height = height;
                Canvas.SetLeft(shaft, Canvas.GetLeft(room));
                Canvas.SetTop(shaft, Canvas.GetTop(room));
            }

        }
        //管井镜像

        public static void SetKitchenKit(RoomTest room)
        {
            var origin_x = (Canvas.GetLeft(room) + room.StrokeThickness * 0.5) * scale_rate;
            var origin_y = (Canvas.GetTop(room) + room.ActualHeight - room.StrokeThickness * 0.5) * scale_rate;
            var width = (room.ActualWidth - room.StrokeThickness) * scale_rate;
            var height = (room.ActualHeight - room.StrokeThickness) * scale_rate;
            var rate = height / width;

            const int maxkitchendepth = 4750;
            const int minkitchendepth = 1950;
            const int maxkitchenbay = 2750;
            const int minkitchenbay = 1650;

            //2450是怎么算出来的，需要2450写成家具加管井

            Action<RoomTest, Point, double, double> function = (opendoorPos, doorPos, shaftDir) switch//怎么写镜像，shaftDir是什么？
            {
                (0, 0, true) when (maxkitchenbay >= width >= 2450 && maxkitchendepth >= height >= 3100) => KitchenType2,
                (0, 0, true) when (maxkitchenbay >= width >= 2450 && height < 3100) => KitchenType5,
                (0, 0, true) when (2450 >= width >= 1850 && maxkitchendepth >= height >= 2750) => KitchenType2,
                (0, 0, true) when (2450 >= width >= 1850 && 2750 >= height >= 2550) => KitchenType3,
                //单侧开门，短边开门

                (0, 1, true) when (maxkitchenbay >= width >= 1850 && maxkitchendepth >= height >= 3900) => KitchenType3,
                (0, 1, true) when (maxkitchenbay >= width >= minkitchenbay && 3900 > height >= 2650) => KitchenType6,
                (0, 1, true) when (maxkitchenbay >= width >= 1850 && 2650 > height >= 2550) => KitchenType3,
                //单侧开门，长边边开门

                (1, >= 0, true) when (2450 >= width >= 1850 && maxkitchendepth >= height >= 3350) => KitchenType1,
                (1, >= 0, true) when (maxkitchenbay >= width >= 2450 && 3350 > height >= 2150) => KitchenType4,
                //对侧开门 此时无所谓0，1是否可以写成>=0

                (2, >= 0, true) when (maxkitchenbay >= width >= 1850 && maxkitchendepth >= height >= 2750) => KitchenType2,
                (2, >= 0, true) when (maxkitchenbay >= width >= 1850 && 2750 > height >= 2550) => KitchenType3,
                //转折开门

                (3, >= 0, true) when (2450 >= width >= 1850 && maxkitchendepth >= height >= 3350) => KitchenType1,
                //三侧开门

                _ => null,
                //default被_替代
            }; ;

            function?.Invoke(room, new Point(origin_x, origin_y), width, height);
        }

        // 重要
        //梳理逻辑：shower:剩余长宽判断家具类型，确定基点位置（紧邻着墙（用算法写？）和次一级的家具）生成方向：四个维度具体是哪一个维度（也可以用算法写？）

        //对于厨房来说：
        //每一个布局需要一个基点的函数——6个函数（对于这个函数是通用的）全部方法里需要一个固定的位置？四个角——4/1个函数，生成方向——4个函数，填充块判断的函数
        //fridge,storage:剩余长宽判断家具类型，确定基点位置,生成方向
        //        public void SetFridge（double restLength，double restWidth，double XCoordinate, double YCoordinate,
        //        Vector X_Axis, Vector Y_Axis）
        //                {;}
        //    public void SetStorage（double restLength，double restWidth，double XCoordinate, double YCoordinate,
        //    Vector X_Axis, Vector Y_Axis）
        //                {;
        //                }
        //washer,cooker,basket,electrical,dishwasher::确定基点位置,生成方向，维度，图片
        //public void SetWasher（double XCoordinate, double YCoordinate,Vector X_Axis, Vector Y_Axis）{;}
        //public void SetCooker（double XCoordinate, double YCoordinate,Vector X_Axis, Vector Y_Axis）{;}

        //extrastorage:填充块:判断有家具的维度哪里还有空缺，有空缺使用方法，维度有空缺分别去调用方法
        //
        //方向维度基点位置：1.右边墙 (width - thickness) * xAxis（下，上方）(0.5 * Fridge[FridgeType].Item1 + thickness) * yAxis + (width - thickness) * xAxis;
        //3.上边左  (height - thickness) * yAxis + (0 + thickness + 0.5 * Fridge[FridgeType].Item1 ) * xAxis
        //4.左下(0.5 * Fridge[FridgeType].Item1 + thickness) * yAxis + (0 + thickness) * xAxis;
        //5.下左(0 + thickness) * yAxis + (0.5 * Fridge[FridgeType].Item1 + thickness) * xAxis;
        //同一种布局里面找到基点然后去生成其他的

        //值A第一种情况左墙A= -1 * yAxis, xAxis；第二种情况下墙A=   xAxis, yAxis;右墙 yAxis, -1 * xAxis；上墙 -1 * xAxis, -1 * yAxis;（这是什么？怎么判断是上下左右？？？）

        public class SetFurniture
        {
            //分类，类去调用方法和很多方法直接调用有什么区别？类里面方法的变量是不是要先在类里面声明？
        }
        public void SetFridge（double restLength，double restWidth，double XLocation, double YLocation,
                Vector X_Axis, Vector Y_Axis）
        {
            var fridgeType = MatchFurniture(restWidth, restLength, FurnitureSize.Fridge);
            var fridge_pos = origin
            + x_axis * (XLocation)
            + y_axis * (YLocation);
            var fridge = new Furniture(FurnitureType.Fridge, fridge_pos.X / scale_rate, fridge_pos.Y / scale_rate,
            FurnitureSize.Fridge[fridgeType].Item1 / scale_rate, FurnitureSize.Fridge[fridgeType].Item2 / scale_rate, Vector X_Axis, Vector Y_Axis);
        }
        public void SetStorage（double restLength，double XLocation, double YLocation,
                Vector X_Axis, Vector Y_Axis）
        {
            double restWidth = 600;
            var storageType = MatchFurniture(restWidth, restLength, FurnitureSize.Storage);
            var storage_pos = origin
            + x_axis * (XLocation)
            + y_axis * (YLocation);
            var storage = new Furniture(FurnitureType.Storage, storage_pos.X / scale_rate, storage_pos.Y / scale_rate,
            FurnitureSize.Storage[storageType].Item1 / scale_rate, FurnitureSize.Storage[storageType].Item2 / scale_rate, Vector X_Axis, Vector Y_Axis);
        }
        
        public void SetWasher（double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis）
        {
            var washer_pos = origin + (YLocation) *y_axis + (XLocation) * x_axis;
            var washer = new Furniture(washer_pos.X / scale_rate, washer_pos.Y / scale_rate, 800 / scale_rate, 600 / scale_rate, Vector X_Axis, Vector Y_Axis); 
        }
        public void SetCooker（double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis）
        {
            var cooker_pos = origin + (YLocation) *y_axis + (XLocation) * x_axis;
            var cooker = new Furniture(cooker_pos.X / scale_rate, cooker_pos.Y / scale_rate, 800 / scale_rate, 600 / scale_rate, Vector X_Axis, Vector Y_Axis); 
        }
        public void SetBasket（double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis）
        {
            var basket_pos = origin + (YLocation) *y_axis + (XLocation) * x_axis;
            var basket = new Furniture(basket_pos.X / scale_rate, basket_pos.Y / scale_rate, 350 / scale_rate, 600 / scale_rate, Vector X_Axis, Vector Y_Axis); 
        }
        public void SetElectrical（double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis）
        {
            var electrical_pos = origin + (YLocation) *y_axis + (XLocation) * x_axis;
            var electrical = new Furniture(electrical_pos.X / scale_rate, electrical_pos.Y / scale_rate, 300 / scale_rate, 600 / scale_rate, Vector X_Axis, Vector Y_Axis); 
        }
        public void SetDishwasher（double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis）
        {
            var dishwasher_pos = origin + (YLocation) *y_axis + (XLocation) * x_axis;
            var dishwasher = new Furniture(dishwasher_pos.X / scale_rate, dishwasher_pos.Y / scale_rate, 300 / scale_rate, 600 / scale_rate, Vector X_Axis, Vector Y_Axis); 
        }

        public void SetFillBlock（double restLength，double XLocation, double YLocation,
                Vector X_Axis, Vector Y_Axis）
        { 
            var storage_pos = origin
            + x_axis * (XLocation)
            + y_axis * (YLocation);
            var storage = new Furniture(storage_pos.X / scale_rate, storage_pos.Y / scale_rate,
            restLength / scale_rate, 600 / scale_rate, Vector X_Axis, Vector Y_Axis);
        }
    public void KitchenType1(Plane basePl, double width, double height)
        {
        double minrestLength = height - decorate_thickness * 2 - shaft_length - washerlength - 450;
        double fridge_restWidth = width - decorate_thickness * 2 - door_size;//前面加一个冰箱的修饰
        double firstyleftlocation = 0 + decorate_thickness/* Fridge[FridgeType].Item1 type1里面固定*/ ;
        double xleftlocation = width - decorate_thickness;
        Vector leftx_axis = y_axis;
        Vector lefty_axis = -1 * x_axis;//布局里面声明静态的字段/属性
        SetWasher（xleftlocation, height - decorate_thickness - shaft_length - washer_length, Vector leftx_axis, Vector lefty_axis ）;
    public double SetCookerAndStorage(restLength, YLocation)
    {
        SetCooker（xleftlocation, YLocation,leftx_axis, lefty_axis）;
        restlength -= Fridge[fridgeType].Item1;
        restlength += 450;
        YLocation += cooker_length;
        SetStorage（restLength，xleftlocation, YLocation,
                                        leftx_axis, lefty_axis）;
        YLocation += Storage[storageType].Item1;
        return restLength;
        return YLocation;
    }
    public void SetWasherAndFillBlock(restLength, YLocation) //泛型？？
    {
        SetWasher（xleftlocation, YLocation,leftx_axis, lefty_axis）;
        restlength -= 450；
        YLocation += washer_length;
        SetFillBlock（restLength，xleftlocation, YLocation,
                                leftx_axis, lefty_axis）;
    }
            if (height >= 3700)
            {

                if (height >= 4300)
                {//把共同的东西提取出来
            double restlength= minrestLength - cooker_length - basket_length - dishwasher_length
                    Setfridge(restlength, fridge_restWidth, xleftlocation , firstyleftlocation, leftx_axis, lefty_axis);
                    double YLocation = firstyleftlocation + Fridge[fridgeType].Item1;
                    SetBasket（xleftlocation,YLocation ,leftx_axis, lefty_axis）;
                    YLocation += basket_length;
                    SetCookerAndStorage(restLength, YLocation);
                    restlength, YLocation= SetCookerAndStorage(restLength, YLocation);//这样写可以吗
                    SetDishwasher（xleftlocation, YLocation,leftx_axis, lefty_axis）;
                    YLocation += dishwasher_length;
                    SetWasherAndFillBlock(restLength, YLocation);
                }
                else
                {
                    double restlength = minrestLength - cooker_length - basket_length - dishwasher_length
                            Setfridge(restlength, fridge_restWidth, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis);
                    double YLocation = firstyleftlocation + Fridge[fridgeType].Item1;
                    SetBasket（xleftlocation,YLocation ,leftx_axis, lefty_axis）;
                    YLocation += basket_length;
                    SetCookerAndStorage(restLength, YLocation);
                    restlength, YLocation = SetCookerAndStorage(restLength, YLocation);
                    SetDishwasher（xleftlocation, YLocation,leftx_axis, lefty_axis）;
                    YLocation += dishwasher_length;
                    SetWasherAndFillBlock(restLength, YLocation);
        }

            }
            else
            {
                    double restlength = minrestLength - cooker_length - basket_length - dishwasher_length
                    Setfridge(restlength, fridge_restWidth, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis);
                    double YLocation = firstyleftlocation + Fridge[fridgeType].Item1;
                    SetBasket（xleftlocation,YLocation ,leftx_axis, lefty_axis）;
                    YLocation += basket_length;
                    SetCookerAndStorage(restLength, YLocation);
                    restlength, YLocation = SetCookerAndStorage(restLength, YLocation);
                    SetDishwasher（xleftlocation, YLocation,leftx_axis, lefty_axis）;
                    YLocation += dishwasher_length;
                    SetWasherAndFillBlock(restLength, YLocation);
    }
        }


        //public static void RestroomType1(RoomTest room, Point origin, double width, double height)
        //{

        //    double restLength = /*width - decorate_thickness * 2 - shaft_width;*/
        //    double restWidth = /*shaft_length;*/
        //    var showerType = MatchFurniture(restWidth, restLength, FurnitureSize.Shower);

        //    var shower_pos = origin
        //        + x_axis * (/*shaft_width + decorate_thickness*/)
        //        + y_axis * (/*height - shaft_length * 0.5 - decorate_thickness*/);

        //    var shower = new Furniture(FurnitureType.Shower, shower_pos.X / scale_rate, shower_pos.Y / scale_rate,
        //        FurnitureSize.Shower[showerType].Item2 / scale_rate, FurnitureSize.Shower[showerType].Item1 / scale_rate,/* -1 * y_axis, x_axis*/);

        //    //shower.ImagePath = restroomPath + showerType.ToString() + "_Shower.png";
        //    //shower.Transform = GetTransform(shower.X_Axis, shower.Y_Axis);
        //    //room.furnitures.Add(shower);

        //    restLength = height - decorate_thickness * 2 - 900 - 900;
        //    restWidth = width - decorate_thickness * 2 - doorSize - 150;
        //    var basinType = MatchFurniture(restWidth, restLength, FurnitureSize.Basin);

        //    var basin_pos = origin
        //        + x_axis * (0 + decorate_thickness)
        //        + y_axis * (FurnitureSize.Basin[basinType].Item1 * 0.5 + decorate_thickness);
        //    var basin = new Furniture(FurnitureType.Basin, basin_pos.X / scale_rate, basin_pos.Y / scale_rate,
        //        FurnitureSize.Basin[basinType].Item1 / scale_rate, FurnitureSize.Basin[basinType].Item2 / scale_rate, -1 * y_axis, x_axis);
        //    basin.ImagePath = restroomPath + basinType.ToString() + "_Basin.png";
        //    basin.Transform = GetTransform(basin.X_Axis, basin.Y_Axis);
        //    room.furnitures.Add(basin);




        //    var toilet_pos = origin
        //        + x_axis * (0 + decorate_thickness)
        //        + y_axis * (height + FurnitureSize.Basin[basinType].Item1 - 900 + decorate_thickness) * 0.5;
        //    var toilet = new Furniture(FurnitureType.Toilet, toilet_pos.X / scale_rate, toilet_pos.Y / scale_rate,
        //         900 / scale_rate, 1200 / scale_rate, -1 * y_axis, x_axis);
        //    toilet.ImagePath = restroomPath + "Standard_Size_Toilet.png";
        //    toilet.Transform = GetTransform(toilet.X_Axis, toilet.Y_Axis);
        //    room.furnitures.Add(toilet);//????



        }

        

        //public void KitchenType2(Plane basePl, double width, double height)
        //{

        //    double restLength = width - decorate_thickness * 2 - 350;
        //    double restWidth = 900;
        //    var showerType = MatchFurniture(restWidth, restLength, Shower);

        //    var showerPt = basePl.Origin + (height - 900 * 0.5 - decorate_thickness) * yAxis + (350 + decorate_thickness) * xAxis;
        //    var showerPl = new Plane(showerPt, -1 * yAxis, xAxis);
        //    shower = new Rectangle3d(showerPl,
        //      new Interval(-0.5 * Shower[showerType].Item2, 0.5 * Shower[showerType].Item2),
        //      new Interval(0, Shower[showerType].Item1));

        //    restLength = height - decorate_thickness * 2 - 900 - 900;
        //    restWidth = width - decorate_thickness * 2;
        //    var basinType = MatchFurniture(restWidth, restLength, Basin);

        //    var basinPt = basePl.Origin + (Basin[basinType].Item1 * 0.5 + decorate_thickness) * yAxis + (width - decorate_thickness) * xAxis;
        //    var basinPl = new Plane(basinPt, yAxis, -1 * xAxis);
        //    basin = new Rectangle3d(basinPl, new Interval(Basin[basinType].Item1 * -0.5, Basin[basinType].Item1 * 0.5), new Interval(0, Basin[basinType].Item2));

        //    var toiletPt = basePl.Origin + (height + Basin[basinType].Item1 - 900 + decorate_thickness) * 0.5 * yAxis + (width - decorate_thickness) * xAxis;
        //    var toiletPl = new Plane(toiletPt, yAxis, -1 * xAxis);
        //    toilet = new Rectangle3d(toiletPl, new Interval(-225, 225), new Interval(0, 750));

        //}

        //public void KitchenType3(Plane basePl, double width, double height)
        //{

        //    var showerPt = basePl.Origin + (height - 350 - decorate_thickness) * yAxis + (900 * 0.5 + decorate_thickness) * xAxis;
        //    var showerPl = new Plane(showerPt, -1 * xAxis, -1 * yAxis);
        //    shower = new Rectangle3d(showerPl, new Interval(-450, 450), new Interval(0, 900));

        //    var restLength = height - decorate_thickness * 2 - 1200;
        //    var restWidth = width - decorate_thickness * 2;
        //    var basinType = MatchFurniture(restWidth, restLength, Basin);

        //    var basinPt = basePl.Origin + (Basin[basinType].Item1 * 0.5 + decorate_thickness) * yAxis + (width - decorate_thickness) * xAxis;
        //    var basinPl = new Plane(basinPt, yAxis, -1 * xAxis);
        //    basin = new Rectangle3d(basinPl, new Interval(Basin[basinType].Item1 * -0.5, Basin[basinType].Item1 * 0.5), new Interval(0, Basin[basinType].Item2));

        //    var toiletPt = basePl.Origin + (height - decorate_thickness) * yAxis + (width + 900) * 0.5 * xAxis;
        //    var toiletPl = new Plane(toiletPt, -1 * xAxis, -1 * yAxis);
        //    toilet = new Rectangle3d(toiletPl, new Interval(-225, 225), new Interval(0, 750));

        //}

        //public void KitchenType4(Plane basePl, double width, double height)
        //{
        //    double restLength = height - decorate_thickness * 2 - 350 - 900;
        //    double restWidth = 900;
        //    var showerType = MatchFurniture(restWidth, restLength, Shower);

        //    var showerPt = basePl.Origin + (height - 350 - decorate_thickness) * yAxis + (900 * 0.5 + decorate_thickness) * xAxis;
        //    var showerPl = new Plane(showerPt, -1 * xAxis, -1 * yAxis);
        //    shower = new Rectangle3d(showerPl,
        //      new Interval(-0.5 * Shower[showerType].Item2, 0.5 * Shower[showerType].Item2),
        //      new Interval(0, Shower[showerType].Item1));

        //    restLength = width - decorate_thickness * 2 - 900;
        //    restWidth = height;
        //    var basinType = MatchFurniture(restWidth, restLength, Basin);

        //    var basinPt = basePl.Origin + (height - decorate_thickness) * yAxis + (width + 900) * 0.5 * xAxis;
        //    var basinPl = new Plane(basinPt, -1 * xAxis, -1 * yAxis);
        //    basin = new Rectangle3d(basinPl, new Interval(Basin[basinType].Item1 * -0.5, Basin[basinType].Item1 * 0.5), new Interval(0, Basin[basinType].Item2));

        //    var toiletPt = basePl.Origin + (height - 350 - Shower[showerType].Item1 + decorate_thickness) * 0.5 * yAxis + (0 + decorate_thickness) * xAxis;
        //    var toiletPl = new Plane(toiletPt, -1 * yAxis, xAxis);
        //    toilet = new Rectangle3d(toiletPl, new Interval(-225, 225), new Interval(0, 750));

        //}

        //public void KitchenType5(Plane basePl, double width, double height)
        //{
        //    double restLength = height - 350 - decorate_thickness * 2;
        //    double restWidth = 900;

        //    var showerType = MatchFurniture(restWidth, restLength, Shower);
        //    var showerPt = basePl.Origin + (height - 350 - decorate_thickness) * yAxis + (900 * 0.5 + decorate_thickness) * xAxis;
        //    var showerPl = new Plane(showerPt, -1 * xAxis, -1 * yAxis);

        //    shower = new Rectangle3d(showerPl,
        //      new Interval(-0.5 * Shower[showerType].Item2, 0.5 * Shower[showerType].Item2),
        //      new Interval(0, Shower[showerType].Item1));
        //    ///
        //    restLength = width - 900 - 900 - decorate_thickness * 2;
        //    restWidth = height;

        //    var basinType = MatchFurniture(restWidth, restLength, Basin);
        //    var basinPt = basePl.Origin + (height - decorate_thickness) * yAxis + (width - decorate_thickness - Basin[basinType].Item1 * 0.5) * xAxis;
        //    var basinPl = new Plane(basinPt, -1 * xAxis, -1 * yAxis);

        //    basin = new Rectangle3d(basinPl, new Interval(Basin[basinType].Item1 * -0.5, Basin[basinType].Item1 * 0.5), new Interval(0, Basin[basinType].Item2));
        //    ///
        //    var toiletPt = basePl.Origin + (width + 900 - Basin[basinType].Item1) * 0.5 * xAxis + (height - decorate_thickness) * yAxis;
        //    var toiletPl = new Plane(toiletPt, -1 * xAxis, -1 * yAxis);
        //    toilet = new Rectangle3d(toiletPl, new Interval(-225, 225), new Interval(0, 750));

        //}

        //public static void KitchenType6(RoomTest room, Point origin, double width, double height)
        //{

        //    //var wmPt = basePl.Origin + (height - decorate_thickness) * yAxis + (width + 350 + Shower[showerType].Item1) * 0.5 * xAxis;
        //    //var wmPl = new Plane(wmPt, -1 * xAxis, -1 * yAxis);
        //    //SetWashingMachine(wmPl, width - 350 - Shower[showerType].Item1 - decorate_thickness * 2, height - 800);

        //    double restLength = width - decorate_thickness * 2 - shaft_width;
        //    double restWidth = shaft_length;
        //    var showerType = MatchFurniture(restWidth, restLength, FurnitureSize.Shower);

        //    var shower_pos = origin
        //        + x_axis * (shaft_width + decorate_thickness)
        //        + y_axis * (height - shaft_length * 0.5 - decorate_thickness);
        //    var shower = new Furniture(FurnitureType.Shower, shower_pos.X / scale_rate, shower_pos.Y / scale_rate,
        //        FurnitureSize.Shower[showerType].Item2 / scale_rate, FurnitureSize.Shower[showerType].Item1 / scale_rate, -1 * y_axis, x_axis);

        //    restLength = width - decorate_thickness * 2 - doorSize - 150;
        //    restWidth = height - decorate_thickness * 2 - 900 - 900;
        //    var basinType = MatchFurniture(restWidth, restLength, FurnitureSize.Basin);

        //    var basin_pos = origin
        //        + x_axis * (FurnitureSize.Basin[basinType].Item1 * 0.5 + decorate_thickness)
        //        + y_axis * (0 + decorate_thickness);
        //    var basin = new Furniture(FurnitureType.Basin, basin_pos.X / scale_rate, basin_pos.Y / scale_rate,
        //        FurnitureSize.Basin[basinType].Item1 / scale_rate, FurnitureSize.Basin[basinType].Item2 / scale_rate, x_axis, y_axis);

        //    var toilet_pos = origin
        //        + x_axis * (0 + decorate_thickness)
        //        + y_axis * (height + FurnitureSize.Basin[basinType].Item2 - 900 + decorate_thickness) * 0.5;
        //    var toilet = new Furniture(FurnitureType.Toilet, toilet_pos.X / scale_rate, toilet_pos.Y / scale_rate,
        //        900 / scale_rate, 1200 / scale_rate, -1 * y_axis, x_axis);

        //    shower.ImagePath = restroomPath + showerType.ToString() + "_Shower.png";
        //    basin.ImagePath = restroomPath + basinType.ToString() + "_Basin.png";
        //    toilet.ImagePath = restroomPath + "Standard_Size_Toilet.png";

        //    shower.Transform = GetTransform(shower.X_Axis, shower.Y_Axis);
        //    basin.Transform = GetTransform(basin.X_Axis, basin.Y_Axis);
        //    toilet.Transform = GetTransform(toilet.X_Axis, toilet.Y_Axis);
        //    room.furnitures.Add(shower);
        //    room.furnitures.Add(basin);
        //    room.furnitures.Add(toilet);
        }

        

    }
}
