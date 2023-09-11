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
        static double shaft_width = 350;
        static double shaft_length = 900;
        static double doorSize = 800;

        static int doorPos = 2;
        static bool shaftDir = true;

        static Vector x_axis = new Vector(1, 0);
        static Vector y_axis = new Vector(0, -1); //这两个应该是room的属性

        static string restroomPath = "Restroom/";

        static Rectangle shaft { get; set; }
        public static void CheckRestroom(RoomTest room)
        {
            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Remove(x)); //这个写法有点蠢，需要优化一下 ?
            room.furnitures.Clear();

            SetRestroomKit(room);
            //SetShaft(room, 18, 42);

            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Add(x));
        }

        public static void SetShaft(RoomTest room,double width,double height)
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

        public static void SetRestroomKit(RoomTest room)
        {
            var origin_x = (Canvas.GetLeft(room) + room.StrokeThickness * 0.5) * scale_rate;
            var origin_y = (Canvas.GetTop(room) + room.ActualHeight - room.StrokeThickness * 0.5) * scale_rate;
            var width = (room.ActualWidth - room.StrokeThickness) * scale_rate;
            var height = (room.ActualHeight - room.StrokeThickness) * scale_rate;
            var rate = height / width;

            const double thres1 = 2200.0 / 1500.0;
            const double thres2 = 1.0;
            const double thres3 = 0.0;

            Action<RoomTest, Point, double, double> function = (rate, doorPos, shaftDir) switch
            { 
                //( >= thres1, 0, false) when (width - decorate_thickness - 900 > 750) => RestroomType3,
                ( >= thres1, 1, false) => null,
                //( >= thres1, 2, false) => RestroomType4,
                //( >= thres1, 0, true) => RestroomType2,
                ( >= thres1, 1, true)  => null,
                ( >= thres1, 2, true) when (height >= 2450 && width >= 1500) => RestroomType1,
                ( >= thres1, 2, true) when (height >= 2350 && width >= 1600) => RestroomType6,

                ( >= thres2 , 0, false) => null,
                ( >= thres2, 1, false) => null,
                ( >= thres2, 2, false) => null,
                ( >= thres2, 0, true) => null,
                ( >= thres2, 1, true) => null,
                ( >= thres2, 2, true) when (height >= 2350 && width >= 1600) => RestroomType6,

                ( >= thres3, 0, false) => null,
                ( >= thres3, 1, false) => null,
                ( >= thres3, 2, false) => null,
                ( >= thres3, 0, true) => null,
                ( >= thres3, 1, true) => null,
                ( >= thres3, 2, true) when (height - 900 - decorate_thickness * 2 >= 900 && width >= 2200) => RestroomType7,
                ( >= thres3, 2, true) when (height - 900 - decorate_thickness * 2 >= 750 && width >= 1900) => RestroomType8,

                _ => null,

            }; ;

            function?.Invoke(room, new Point(origin_x, origin_y), width, height);             

        }

        public static void RestroomType1(RoomTest room,Point origin,double width,double height)
        {

            double restLength = width - decorate_thickness * 2 - shaft_width;
            double restWidth = shaft_length;
            var showerType = MatchFurniture(restWidth, restLength, FurnitureSize.Shower);

            var shower_pos = origin
                + x_axis * (shaft_width + decorate_thickness)
                + y_axis * (height - shaft_length * 0.5 - decorate_thickness);

            var shower = new Furniture(FurnitureType.Shower, shower_pos.X / scale_rate, shower_pos.Y / scale_rate,
                FurnitureSize.Shower[showerType].Item2 / scale_rate, FurnitureSize.Shower[showerType].Item1 / scale_rate, -1 * y_axis, x_axis);

            shower.ImagePath = restroomPath + showerType.ToString() + "_Shower.png";
            shower.Transform = GetTransform(shower.X_Axis, shower.Y_Axis);
            room.furnitures.Add(shower);




            restLength = height - decorate_thickness * 2 - 900 - 900;
            restWidth = width - decorate_thickness * 2 - doorSize - 150;
            var basinType = MatchFurniture(restWidth, restLength, FurnitureSize.Basin);

            var basin_pos = origin
                + x_axis * (0 + decorate_thickness)
                + y_axis * (FurnitureSize.Basin[basinType].Item1 * 0.5 + decorate_thickness);
            var basin = new Furniture(FurnitureType.Basin, basin_pos.X / scale_rate, basin_pos.Y / scale_rate,
                FurnitureSize.Basin[basinType].Item1 / scale_rate, FurnitureSize.Basin[basinType].Item2 / scale_rate, -1 * y_axis, x_axis);
            basin.ImagePath = restroomPath + basinType.ToString() + "_Basin.png";
            basin.Transform = GetTransform(basin.X_Axis, basin.Y_Axis);
            room.furnitures.Add(basin);




            var toilet_pos = origin
                + x_axis * (0 + decorate_thickness)
                + y_axis * (height + FurnitureSize.Basin[basinType].Item1 - 900 + decorate_thickness) * 0.5;
            var toilet = new Furniture(FurnitureType.Toilet, toilet_pos.X / scale_rate, toilet_pos.Y / scale_rate,
                 900 / scale_rate, 1200 / scale_rate, -1 * y_axis, x_axis);
            toilet.ImagePath = restroomPath + "Standard_Size_Toilet.png";
            toilet.Transform = GetTransform(toilet.X_Axis, toilet.Y_Axis);
            room.furnitures.Add(toilet);//????


          
        }

        //public void RestroomType2(Plane basePl, double width, double height)
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

        //public void RestroomType3(Plane basePl, double width, double height)
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

        //public void RestroomType4(Plane basePl, double width, double height)
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

        //public void RestroomType5(Plane basePl, double width, double height)
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

        public static void RestroomType6(RoomTest room, Point origin, double width, double height)
        {

            //var wmPt = basePl.Origin + (height - decorate_thickness) * yAxis + (width + 350 + Shower[showerType].Item1) * 0.5 * xAxis;
            //var wmPl = new Plane(wmPt, -1 * xAxis, -1 * yAxis);
            //SetWashingMachine(wmPl, width - 350 - Shower[showerType].Item1 - decorate_thickness * 2, height - 800);

            double restLength = width - decorate_thickness * 2 - shaft_width;
            double restWidth = shaft_length;
            var showerType = MatchFurniture(restWidth, restLength, FurnitureSize.Shower);

            var shower_pos = origin
                + x_axis * (shaft_width + decorate_thickness)
                + y_axis * (height - shaft_length * 0.5 - decorate_thickness);
            var shower = new Furniture(FurnitureType.Shower, shower_pos.X / scale_rate, shower_pos.Y / scale_rate,
                FurnitureSize.Shower[showerType].Item2 / scale_rate, FurnitureSize.Shower[showerType].Item1 / scale_rate, -1 * y_axis, x_axis);

            restLength = width - decorate_thickness * 2 - doorSize - 150;
            restWidth = height - decorate_thickness * 2 - 900 - 900;
            var basinType = MatchFurniture(restWidth, restLength, FurnitureSize.Basin);

            var basin_pos = origin
                + x_axis * (FurnitureSize.Basin[basinType].Item1 * 0.5 + decorate_thickness)
                + y_axis * (0 + decorate_thickness);
            var basin = new Furniture(FurnitureType.Basin, basin_pos.X / scale_rate, basin_pos.Y / scale_rate,
                FurnitureSize.Basin[basinType].Item1 / scale_rate, FurnitureSize.Basin[basinType].Item2 / scale_rate, x_axis, y_axis);

            var toilet_pos = origin
                + x_axis * (0 + decorate_thickness)
                + y_axis * (height + FurnitureSize.Basin[basinType].Item2 - 900 + decorate_thickness) * 0.5;
            var toilet = new Furniture(FurnitureType.Toilet, toilet_pos.X / scale_rate, toilet_pos.Y / scale_rate,
                900 / scale_rate, 1200 / scale_rate, -1 * y_axis, x_axis);

            shower.ImagePath = restroomPath + showerType.ToString() + "_Shower.png";
            basin.ImagePath = restroomPath + basinType.ToString() + "_Basin.png";
            toilet.ImagePath = restroomPath + "Standard_Size_Toilet.png";

            shower.Transform = GetTransform(shower.X_Axis, shower.Y_Axis);
            basin.Transform = GetTransform(basin.X_Axis, basin.Y_Axis);
            toilet.Transform = GetTransform(toilet.X_Axis, toilet.Y_Axis);
            room.furnitures.Add(shower);
            room.furnitures.Add(basin);
            room.furnitures.Add(toilet);
        }

        public static void RestroomType7(RoomTest room, Point origin, double width, double height)
        {

            double restLength = width - doorSize - 150 - decorate_thickness * 2;
            double restWidth = height - 900 - decorate_thickness * 2;
            //var showerType = MatchFurniture(restWidth, restLength, FurnitureSize.Shower);
            var showerType = ShowerType.Diamond_Shape_Type;

            var shower_pos = origin
                + x_axis * (0 + decorate_thickness)
                + y_axis * (decorate_thickness + FurnitureSize.Shower[showerType].Item2 * 0.5);
            var shower = new Furniture(FurnitureType.Shower, shower_pos.X / scale_rate, shower_pos.Y / scale_rate,
                FurnitureSize.Shower[showerType].Item2 / scale_rate, FurnitureSize.Shower[showerType].Item1 / scale_rate, y_axis, x_axis);

            restLength = width - 1200 - shaft_width - decorate_thickness * 2;
            restWidth = height - decorate_thickness * 2 - doorSize; ;
            var basinType = MatchFurniture(restWidth, restLength, FurnitureSize.Basin);

            var basin_pos = origin
                + x_axis * (width - FurnitureSize.Basin[basinType].Item1 * 0.5 - decorate_thickness)
                + y_axis * (height - decorate_thickness);
            var basin = new Furniture(FurnitureType.Basin, basin_pos.X / scale_rate, basin_pos.Y / scale_rate,
                FurnitureSize.Basin[basinType].Item1 / scale_rate, FurnitureSize.Basin[basinType].Item2 / scale_rate, -1 * x_axis, -1 * y_axis);

            var toilet_pos = origin
                + x_axis * (shaft_width + decorate_thickness)
                + y_axis * (height - shaft_length * 0.5 - decorate_thickness);
            var toilet = new Furniture(FurnitureType.Toilet, toilet_pos.X / scale_rate, toilet_pos.Y / scale_rate,
                 900 / scale_rate, 1200 / scale_rate, -1 * y_axis, x_axis);

            shower.ImagePath = restroomPath + showerType.ToString() + "_Shower.png";
            basin.ImagePath = restroomPath + basinType.ToString() + "_Basin.png";
            toilet.ImagePath = restroomPath + "Standard_Size_Toilet.png";

            shower.Transform = GetTransform(shower.X_Axis, shower.Y_Axis);
            basin.Transform = GetTransform(basin.X_Axis, basin.Y_Axis);
            toilet.Transform = GetTransform(toilet.X_Axis, toilet.Y_Axis);
            room.furnitures.Add(shower);
            room.furnitures.Add(basin);
            room.furnitures.Add(toilet);

        }

        public static void RestroomType8(RoomTest room, Point origin, double width, double height)
        {
            double restLength = width - doorSize - 150 - decorate_thickness * 2;
            double restWidth = height - 900 - decorate_thickness * 2;
            //var showerType = MatchFurniture(restWidth, restLength, FurnitureSize.Shower);
            var showerType = ShowerType.Diamond_Shape_Type;

            var shower_pos = origin
                + x_axis * (shaft_width + decorate_thickness)
                + y_axis * (height - shaft_length * 0.5 - decorate_thickness);
            var shower = new Furniture(FurnitureType.Shower, shower_pos.X / scale_rate, shower_pos.Y / scale_rate,
                FurnitureSize.Shower[showerType].Item2 / scale_rate, FurnitureSize.Shower[showerType].Item1 / scale_rate, -1 * y_axis, x_axis);

            restLength = width - FurnitureSize.Shower[showerType].Item1 - shaft_width - decorate_thickness * 2; 
            restWidth = height - decorate_thickness * 2 - doorSize; ;
            var basinType = MatchFurniture(restWidth, restLength, FurnitureSize.Basin);

            var basin_pos = origin
                + x_axis * (width - FurnitureSize.Basin[basinType].Item1 * 0.5 - decorate_thickness)
                + y_axis * (height - decorate_thickness);
            var basin = new Furniture(FurnitureType.Basin, basin_pos.X / scale_rate, basin_pos.Y / scale_rate,
                FurnitureSize.Basin[basinType].Item1 / scale_rate, FurnitureSize.Basin[basinType].Item2 / scale_rate, -1 * x_axis, -1 * y_axis);

            var toilet_pos = origin
                + x_axis * (0 + decorate_thickness)
                + y_axis * (height - shaft_length) * 0.5;
            var toilet = new Furniture(FurnitureType.Toilet, toilet_pos.X / scale_rate, toilet_pos.Y / scale_rate,
                 900 / scale_rate, 1200 / scale_rate, -1 * y_axis, x_axis);

            shower.ImagePath = restroomPath + showerType.ToString() + "_Shower.png";
            basin.ImagePath = restroomPath + basinType.ToString() + "_Basin.png";
            toilet.ImagePath = restroomPath + "Standard_Size_Toilet.png";

            shower.Transform = GetTransform(shower.X_Axis, shower.Y_Axis);
            basin.Transform = GetTransform(basin.X_Axis, basin.Y_Axis);
            toilet.Transform = GetTransform(toilet.X_Axis, toilet.Y_Axis);

            room.furnitures.Add(shower);
            room.furnitures.Add(basin);
            room.furnitures.Add(toilet);
        }


        //public void SetWashingMachine(Plane basePl, double restWidth, double restHeight)
        //{
        //    if (restWidth >= 750)
        //    {
        //        washingMachine = new Rectangle3d(basePl, new Interval(-350, 350), new Interval(0, 650));
        //    }
        //}

    }
}
