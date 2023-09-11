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
        static double kitchenshaft_width = 600;
        static double kitchenshaft_length = 600;
        static double kitchendoor_size = 1100;
        static double standard_width = 600;

        static string kitchenPath = "Kitchen/"; 
        static Rectangle kitchenshaft { get; set; }

        //static double room_width;
        //static double room_depth;
        //static double room_restlength = room_depth - decorate_thickness * 2 - kitchenshaft_length;
        //static double room_restwidth = room_width - decorate_thickness * 2 - kitchenshaft_width;
        static double firstyrightlocation = 0 + decorate_thickness;
        //static double xrightlocation = room_width - decorate_thickness;//right维度x方向固定
        static double firstyleftlocation = 0 + decorate_thickness;
        static double xleftlocation = 0 + decorate_thickness;//left维度x方向固定
        //static double ytoplocation = room_depth - decorate_thickness;
        static double firstxtoplocation = 0 + decorate_thickness;//top维度y方向固定
        static double ybottomlocation = 0 + decorate_thickness;
        static double firstxbottomlocation = 0 + decorate_thickness;//bott维度y方向固定
        //不同布局家具布置的四个维度——测试对不对

        static Vector rightx_axis = new Vector(0, -1);
        static Vector righty_axis = new Vector(-1, 0);//家具right方向
        static Vector leftx_axis = new Vector(0, 1);
        static Vector lefty_axis = new Vector(1, 0);//家具left方向
        static Vector topx_axis = new Vector(-1, 0);
        static Vector topy_axis = new Vector(0, 1);//家具top方向
        static Vector bottomx_axis = new Vector(1, 0);
        static Vector bottomy_axis = new Vector(0, -1);//家具bottom方向

        public static void CheckKitchen(RoomTest room)
        {
            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Remove(x)); //这个写法有点蠢,需要优化一下 ?
            room.furnitures.Clear();
            SetKitchenKit(room);
            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Add(x));
        }//???
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
        public enum opendoorPos
        {
            Single_SidedShort,
            Single_SidedLong,
            Double_Sided,
            Opposite_Sided,
            Three_Sided,
        }

        public static void SetKitchenKit(RoomTest room)
        {
            
            var origin_x = (Canvas.GetLeft(room) + room.StrokeThickness * 0.5) * scale_rate;
            var origin_y = (Canvas.GetTop(room) + room.ActualHeight - room.StrokeThickness * 0.5) * scale_rate;
            var width = (room.ActualWidth - room.StrokeThickness) * scale_rate;
            var depth = (room.ActualHeight - room.StrokeThickness) * scale_rate;

            double maxkitchendepth = kitchenshaft_length + FurnitureSize.Washer[WasherType.Standard_Size].Item1 + FurnitureSize.Fridge[FridgeType.L488_Type].Item1
                                        + FurnitureSize.Cooker[CookerType.Standard_Size].Item1 + FurnitureSize.Basket[BasketType.Standard_Size].Item1
                                        + FurnitureSize.Storage[StorageType.Double_Type].Item1 + FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1
                                        + decorate_thickness * 2;
            double minkitchendepth = kitchenshaft_length + FurnitureSize.Storage[StorageType.Drawers_Type].Item1 + FurnitureSize.Cooker[CookerType.Standard_Size].Item1
                                        + decorate_thickness * 2;
            double maxkitchenwidth = standard_width + FurnitureSize.Washer[WasherType.Standard_Size].Item1 
                                    + FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1 + FurnitureSize.Fridge[FridgeType.L488_Type].Item2 + decorate_thickness * 2;
            double minkitchenwidth = kitchendoor_size + decorate_thickness * 2;
            double minkitchenwidth2450 = FurnitureSize.Fridge[FridgeType.L333_Type].Item1+kitchendoor_size + decorate_thickness * 2 + standard_width;

            var doorPosition = opendoorPos.Single_SidedLong;//只能满足Single_SidedShort的条件
            //3100转换成家具的形式，周一做
            Action<RoomTest, Point, double, double> function = doorPosition switch
            {
                opendoorPos.Single_SidedShort when (maxkitchenwidth >= width && width >= minkitchenwidth2450 && maxkitchendepth >= depth && depth >= 3100) => KitchenType2,
                opendoorPos.Single_SidedShort when (maxkitchenwidth >= width && width >= minkitchenwidth2450 && 3100 > depth && depth >= minkitchendepth) => KitchenType5,
                opendoorPos.Single_SidedShort when (minkitchenwidth2450 >= width && width >= 1850 && maxkitchendepth >= depth && depth >= 2750) => KitchenType2,
                opendoorPos.Single_SidedShort when (minkitchenwidth2450 >= width && width >= 1850 && 2750 >= depth && depth >= 2550) => KitchenType3,
                //单侧开门,短边开门

                opendoorPos.Single_SidedLong when (maxkitchenwidth >= width && width >= 1850 && maxkitchendepth >= depth && depth >= 3900) => KitchenType3,
                opendoorPos.Single_SidedLong when (maxkitchenwidth >= width && width >= minkitchenwidth && 3900 > depth && depth >= 2650) => KitchenType6,
                opendoorPos.Single_SidedLong when (maxkitchenwidth >= width && width >= 1850 && 2650 > depth && depth >= 2550) => KitchenType3,
                //单侧开门,长边边开门

                opendoorPos.Double_Sided when (minkitchenwidth2450 >= width && width >= 1850 && maxkitchendepth >= depth && depth >= 3350) => KitchenType1,
                opendoorPos.Double_Sided when (maxkitchenwidth >= width && width >= minkitchenwidth2450 && 3350 > depth && depth >= 2150) => KitchenType4,
                //对侧开门 

                opendoorPos.Opposite_Sided when (maxkitchenwidth >= width && width >= 1850 && maxkitchendepth >= depth && depth >= 2750) => KitchenType2,
                opendoorPos.Opposite_Sided when (maxkitchenwidth >= width && width >= 1850 && 2750 > depth && depth >= 2550) => KitchenType3,
                //转折开门

                opendoorPos.Three_Sided when (minkitchenwidth2450 >= width && width >= 1850 && maxkitchendepth >= depth && depth >= 3350) => KitchenType1,
                //三侧开门

                _ => null,
                //default被_替代

            };
            function?.Invoke(room, new Point(origin_x, origin_y), width, depth);

        }

            //放固定尺寸家具的方法，还可以优化
            public static void SetkitchenFurniture<T>(RoomTest room, FurnitureType FurType, Point origin, double XLocation, double YLocation, Vector X_Axis, Vector Y_Axis,
        T furType, Dictionary<T, Tuple<int, int, int>> FurDictionary)
        {
            double yLocation;
            double xLocation;
            if (Y_Axis * x_axis != 0)
            {
                yLocation = YLocation + FurDictionary[furType].Item1 * 0.5;
                xLocation = XLocation;
            }
            else
            {
                yLocation = YLocation;
                xLocation = XLocation + FurDictionary[furType].Item1 * 0.5;
            }
            var furniture_pos = origin + x_axis * xLocation + y_axis * yLocation;
            var furniture = new Furniture(
                FurType,
                furniture_pos.X / scale_rate,
                furniture_pos.Y / scale_rate,
               FurDictionary[furType].Item1 / scale_rate,
               FurDictionary[furType].Item2 / scale_rate,
                X_Axis,
                Y_Axis);
            furniture.ImagePath = kitchenPath + $"Standard_Size_{FurType}.png";
            furniture.Transform = GetTransform(furniture.X_Axis, furniture.Y_Axis);
            room.furnitures.Add(furniture);
        }
        //放可变家具的方法,
        public static void SetkitchenFurniture<T>(RoomTest room, FurnitureType FurType, Point origin, double restLength, double restWidth, double XLocation,
                    double YLocation, Vector X_Axis, Vector Y_Axis,
                    Dictionary<T, Tuple<int, int, int>> FurDictionary, out T furType
                    )
        {
            furType = MatchFurniture(restWidth, restLength, FurDictionary);

            double yLocation;
            double xLocation;
            if (Y_Axis * x_axis != 0)
            {
                yLocation = YLocation + FurDictionary[furType].Item1 * 0.5;
                xLocation = XLocation;
            }
            else
            {
                yLocation = YLocation;
                xLocation = XLocation + FurDictionary[furType].Item1 * 0.5;
            }
            var furniture_pos = origin + x_axis * xLocation + y_axis * yLocation;
            var furniture = new Furniture(
              FurType,
              furniture_pos.X / scale_rate,
              furniture_pos.Y / scale_rate,
              FurDictionary[furType].Item1 / scale_rate,
              FurDictionary[furType].Item2 / scale_rate,
              X_Axis,
              Y_Axis);
            furniture.ImagePath = kitchenPath + furType.ToString() + $"_{FurType}.png";
            furniture.Transform = GetTransform(furniture.X_Axis, furniture.Y_Axis);
                room.furnitures.Add(furniture);
        }

        //暂定KitchenType1灶台和柜子放一起的方法
        public static void SetKitchenType1CookerStorage(RoomTest room, Point origin, double restlength, double XLocation, double YLocation,
                                                    Vector X_Axis, Vector Y_Axis, out StorageType storageType, out double rest_Length, out double Y_Location)
        {
            SetkitchenFurniture(room, FurnitureType.Cooker, origin, XLocation, YLocation, X_Axis, Y_Axis,
                                           CookerType.Standard_Size, FurnitureSize.Cooker);//放Cooker
            YLocation += FurnitureSize.Cooker[CookerType.Standard_Size].Item1 ;
            restlength += FurnitureSize.Storage[StorageType.Drawers_Type].Item1;
            SetkitchenFurniture(room, FurnitureType.Storage, origin, restlength, standard_width, XLocation, YLocation, X_Axis, Y_Axis,
                                FurnitureSize.Storage, out storageType);//放Storage
            YLocation += FurnitureSize.Storage[storageType].Item1;
            Y_Location = YLocation;
            rest_Length = restlength;
        }
        //放填充块（目前定义为水槽的填充块）
        public static void SetkitchenFurniture(RoomTest room, FurnitureType FurType, Point origin, double restLength, double restWidth, double XLocation,
                    double YLocation, Vector X_Axis, Vector Y_Axis)
        {
            var furniture_pos = origin + x_axis * XLocation + y_axis * YLocation;
            var furniture = new Furniture(
                FurType,
                furniture_pos.X / scale_rate,
                furniture_pos.Y / scale_rate,
                restLength / scale_rate,
                restWidth / scale_rate,
                X_Axis,
                Y_Axis);
        }      
        //布局1，家具放在right方向
        public static void KitchenType1(RoomTest room, Point origin, double width, double depth)//有问题
        {
            double room_restlength = depth - decorate_thickness * 2 - kitchenshaft_length;
            double minrestLength = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1
                                 - FurnitureSize.Storage[StorageType.Drawers_Type].Item1 - FurnitureSize.Cooker[CookerType.Standard_Size].Item1;
            double minroom_restlength = FurnitureSize.Storage[StorageType.Drawers_Type].Item1 + FurnitureSize.Cooker[CookerType.Standard_Size].Item1
                + FurnitureSize.Washer[WasherType.Standard_Size].Item1 + FurnitureSize.Fridge[FridgeType.L333_Type].Item1;
            double fridge_restWidth = width - decorate_thickness * 2 - kitchendoor_size;
            //double firstyrightlocation = 0 + decorate_thickness;
            double xrightlocation = width - decorate_thickness;
            double YLocation = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1 /** 0.5*/;
            SetkitchenFurniture(room, FurnitureType.Washer, origin, xrightlocation,
                                YLocation, rightx_axis, righty_axis, WasherType.Standard_Size, FurnitureSize.Washer);//放固定位置的水槽
            //3
            double restlength = minrestLength;
            FridgeType fridgeType;
            if (room_restlength >= minroom_restlength + FurnitureSize.Basket[BasketType.Standard_Size].Item1)//添加调味拉篮2
            {
                if (room_restlength >= minroom_restlength + FurnitureSize.Basket[BasketType.Standard_Size].Item1
                            + FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1)//开始添加电器柜
                {
                    if (room_restlength >= minroom_restlength + FurnitureSize.Basket[BasketType.Standard_Size].Item1
                                        + FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1 + FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1)//添加洗碗机最大尺寸
                    {
                        YLocation -= FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1 /** 0.5*/;
                        SetkitchenFurniture(room, FurnitureType.Dishwasher, origin, xrightlocation,
                                    YLocation, rightx_axis, righty_axis, DishwasherType.Standard_Size, FurnitureSize.Dishwasher);//添加洗碗机
                        restlength = minrestLength - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1 - FurnitureSize.Basket[BasketType.Standard_Size].Item1
                                        - FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;
                        SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlength, fridge_restWidth, xrightlocation, firstyrightlocation, rightx_axis, righty_axis,
                                            FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                        YLocation = firstyrightlocation + FurnitureSize.Fridge[fridgeType].Item1;
                    }
                    else//3
                    {
                        restlength = minrestLength - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1 - FurnitureSize.Basket[BasketType.Standard_Size].Item1
                                        /*- FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1*/;
                        SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlength, fridge_restWidth, xrightlocation, firstyrightlocation, rightx_axis, righty_axis,
                                            FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                        YLocation = firstyrightlocation + FurnitureSize.Fridge[fridgeType].Item1;
                    }
                    SetkitchenFurniture(room, FurnitureType.Electrical, origin, xrightlocation, YLocation, rightx_axis, righty_axis,
                                        ElectricalType.Standard_Size, FurnitureSize.Electrical);//放Electrical
                    YLocation += FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1 /** 0.5*/;
                }
                else//2
                {
                    restlength = minrestLength - FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                    SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlength, fridge_restWidth, xrightlocation, firstyrightlocation, rightx_axis, righty_axis,
                                        FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                    YLocation = firstyrightlocation + FurnitureSize.Fridge[fridgeType].Item1;
                }
                SetkitchenFurniture(room, FurnitureType.Basket, origin, xrightlocation, YLocation, rightx_axis, righty_axis,
                                    BasketType.Standard_Size, FurnitureSize.Basket);//放Basket
                YLocation += FurnitureSize.Basket[BasketType.Standard_Size].Item1 /** 0.5*/;
            }
            else//1
            {
                SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlength, fridge_restWidth, xrightlocation, firstyrightlocation, rightx_axis, righty_axis,
                                FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                YLocation = firstyrightlocation + FurnitureSize.Fridge[fridgeType].Item1;
            }
            StorageType storageType;
            double rest_Length;
            double Y_Location;
            restlength -= FurnitureSize.Fridge[fridgeType].Item1;
            SetKitchenType1CookerStorage( room, origin, restlength, xrightlocation, YLocation,
                                    rightx_axis, righty_axis, out storageType, out rest_Length, out Y_Location);//柜子好像还是有问题
        }

        public static void KitchenType2(RoomTest room, Point origin, double width, double depth) //KitchenType2有2个维度
        {
            double room_restlength = depth - decorate_thickness * 2 - kitchenshaft_length;
            double room_restwidth = width - decorate_thickness * 2 - kitchenshaft_width;
            double minrestLength = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1
                                 - FurnitureSize.Storage[StorageType.Drawers_Type].Item1 - FurnitureSize.Cooker[CookerType.Standard_Size].Item1;
            double fridge_restWidth = depth - decorate_thickness * 2;
            double minfurniture_length = FurnitureSize.Storage[StorageType.Drawers_Type].Item1 + FurnitureSize.Cooker[CookerType.Standard_Size].Item1
                + FurnitureSize.Washer[WasherType.Standard_Size].Item1 /*+ FurnitureSize.Fridge[FridgeType.L333_Type].Item1*/;
            double xrightlocation = width - decorate_thickness;
            double ytoplocation = depth - decorate_thickness;
            double YLocation = firstyrightlocation;
            double restlength = minrestLength + FurnitureSize.Storage[StorageType.Drawers_Type].Item1;
            if (room_restlength >= minfurniture_length + FurnitureSize.Basket[BasketType.Standard_Size].Item1)
            {
                if (room_restlength >= minfurniture_length + FurnitureSize.Basket[BasketType.Standard_Size].Item1
                              + FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1)//3
                {
                    double YdishLocation = room_restlength - FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;
                    SetkitchenFurniture(room, FurnitureType.Dishwasher, origin, xrightlocation,
                    YdishLocation, rightx_axis, righty_axis, DishwasherType.Standard_Size, FurnitureSize.Dishwasher);//放洗碗机，互不干涉从上面放
                    restlength -= FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;
                }
                SetkitchenFurniture(room, FurnitureType.Basket, origin, xrightlocation, YLocation, rightx_axis, righty_axis,
                                        BasketType.Standard_Size, FurnitureSize.Basket);//放Basket
                YLocation += FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                restlength -= FurnitureSize.Basket[BasketType.Standard_Size].Item1;
            }
            StorageType storageType;
            double rest_Length;
            double Y_Location;
            SetKitchenType1CookerStorage(room, origin, restlength, xrightlocation, YLocation,
                                    rightx_axis, righty_axis, out storageType, out rest_Length, out Y_Location);//放柜子和灶台


            SetkitchenFurniture(room, FurnitureType.Washer, origin, xrightlocation,
                                Y_Location, rightx_axis, righty_axis, WasherType.Standard_Size, FurnitureSize.Washer);//放固定位置的水槽
            rest_Length -= FurnitureSize.Storage[storageType].Item1;
            YLocation = room_restlength - rest_Length * 0.5;
            SetkitchenFurniture(room, FurnitureType.FillBlock, origin, rest_Length,
                standard_width, xrightlocation, YLocation, rightx_axis, righty_axis);//放FillBlock是依附水槽的，从下面放
            //top方向
            FridgeType fridgeType;
            double restwidth = room_restwidth - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
            firstxtoplocation = 0 + decorate_thickness;
            SetkitchenFurniture(room, FurnitureType.Fridge, origin, restwidth, fridge_restWidth,
                            firstxtoplocation, ytoplocation, topx_axis, topy_axis,
                                FurnitureSize.Fridge, out fridgeType);//放冰箱
            firstxtoplocation = room_restwidth - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
            SetkitchenFurniture(room, FurnitureType.Electrical, origin, firstxtoplocation, ytoplocation, topx_axis, topy_axis,
                                ElectricalType.Standard_Size, FurnitureSize.Electrical);//放Electrical
        }

        public static void KitchenType3(RoomTest room, Point origin, double width, double depth) 
        {
            firstyrightlocation = 0 + decorate_thickness;
            double room_restlength = depth - decorate_thickness * 2 - kitchenshaft_length;
            double room_restwidth = width - decorate_thickness * 2 - kitchenshaft_width;
            double minrestLength = room_restlength /*- FurnitureSize.Washer[WasherType.Standard_Size].Item1*/
                                 - FurnitureSize.Storage[StorageType.Drawers_Type].Item1 - FurnitureSize.Cooker[CookerType.Standard_Size].Item1;
            double fridge_restWidth = width - decorate_thickness * 2 - kitchendoor_size;
            double minroom_restlength = FurnitureSize.Storage[StorageType.Drawers_Type].Item1 + FurnitureSize.Cooker[CookerType.Standard_Size].Item1
                /*+ FurnitureSize.Washer[WasherType.Standard_Size].Item1*/+ FurnitureSize.Fridge[FridgeType.L333_Type].Item1;
            double xrightlocation = width - decorate_thickness;//rightx方向固定
            double ytoplocation = depth - decorate_thickness;
            firstxtoplocation = 0 + decorate_thickness;
            double YLocation = ytoplocation;
            SetkitchenFurniture(room, FurnitureType.Washer, origin, firstxtoplocation,
                                YLocation, topx_axis, topy_axis, WasherType.Standard_Size, FurnitureSize.Washer);//放固定位置的水槽
            firstxtoplocation += FurnitureSize.Washer[WasherType.Standard_Size].Item1;
            //3
            if (room_restlength >= minroom_restlength)
            {
                double restlength = minrestLength;
                FridgeType fridgeType;
                if (room_restlength >= minroom_restlength + FurnitureSize.Basket[BasketType.Standard_Size].Item1)//添加调味拉篮2
                {
                    if (room_restlength >= minroom_restlength + FurnitureSize.Basket[BasketType.Standard_Size].Item1
                              + FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1)//添加电器柜
                    {
                        restlength = minrestLength - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1 - FurnitureSize.Basket[BasketType.Standard_Size].Item1
                                      /*- FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1*/;
                        SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlength, fridge_restWidth, xrightlocation, firstyrightlocation, rightx_axis, righty_axis,
                                            FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                        restlength -= FurnitureSize.Fridge[fridgeType].Item1;
                        YLocation = firstyrightlocation + FurnitureSize.Fridge[fridgeType].Item1;
                        SetkitchenFurniture(room, FurnitureType.Electrical, origin, xrightlocation, YLocation, rightx_axis, righty_axis,
                                            ElectricalType.Standard_Size, FurnitureSize.Electrical);//放Electrical
                        YLocation += FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1/* * 0.5*/;
                        restlength -= FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
                    }
                    else
                    {
                        restlength = minrestLength - FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                        SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlength, fridge_restWidth, xrightlocation, firstyrightlocation, rightx_axis, righty_axis,
                                            FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                        restlength -= FurnitureSize.Fridge[fridgeType].Item1;
                        YLocation = firstyrightlocation + FurnitureSize.Fridge[fridgeType].Item1;
                    }
                    SetkitchenFurniture(room, FurnitureType.Basket, origin, xrightlocation, YLocation, rightx_axis, righty_axis,
                                        BasketType.Standard_Size, FurnitureSize.Basket);//放Basket
                    YLocation += FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                    restlength -= FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                }
                else
                {
                    SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlength, fridge_restWidth, xrightlocation, firstyrightlocation, rightx_axis, righty_axis,
                                    FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                    YLocation = firstyrightlocation + FurnitureSize.Fridge[fridgeType].Item1;
                    restlength -= FurnitureSize.Fridge[fridgeType].Item1;
                }
                StorageType storageType;
                double rest_Length;
                double Y_Location;
                SetKitchenType1CookerStorage(room, origin, restlength, xrightlocation, YLocation,
                                       rightx_axis, righty_axis, out storageType, out rest_Length, out Y_Location);//柜子好像还是有问题
            }
            //top方向
            if (room_restwidth >= FurnitureSize.Washer[WasherType.Standard_Size].Item1 + FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1)
            {
                double rest_Length = room_restwidth - FurnitureSize.Washer[WasherType.Standard_Size].Item1 - FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;
                SetkitchenFurniture(room, FurnitureType.FillBlock, origin, rest_Length,
                    standard_width, firstxtoplocation, ytoplocation, topx_axis, topy_axis);//放FillBlock
                firstxtoplocation += rest_Length * 0.5;
                SetkitchenFurniture(room, FurnitureType.Dishwasher, origin, firstxtoplocation,
                            ytoplocation, topx_axis, topy_axis, DishwasherType.Standard_Size, FurnitureSize.Dishwasher);  //放置Dishwasher
            }
            else
            {
                double rest_Length = room_restwidth - FurnitureSize.Washer[WasherType.Standard_Size].Item1;
                firstxtoplocation += rest_Length * 0.5;
                SetkitchenFurniture(room, FurnitureType.FillBlock, origin, rest_Length,
                    standard_width, firstxtoplocation, ytoplocation, topx_axis, topy_axis);//放FillBlock
            }

        }
        public static void KitchenType4(RoomTest room, Point origin, double width, double depth) //3个方向
        {
            double room_restlength = depth - decorate_thickness * 2 - kitchenshaft_length;
            double minfurniturerestLengthleft = FurnitureSize.Storage[StorageType.Drawers_Type].Item1 + FurnitureSize.Cooker[CookerType.Standard_Size].Item1;
            double minrestLength = depth - decorate_thickness * 2 - minfurniturerestLengthleft;
            double minfurnitureleftLength = FurnitureSize.Fridge[FridgeType.L333_Type].Item1 + minfurniturerestLengthleft;
            double minroom_restlength = FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1+ FurnitureSize.Washer[WasherType.Standard_Size].Item1;
            double fridge_restWidth = width - decorate_thickness * 2 - kitchendoor_size - standard_width;
            double firstyrightlocation = 0 + decorate_thickness;
            double xrightlocation = width - decorate_thickness;//right维度x方向固定
            double firstyleftlocation = 0 + decorate_thickness;
            double xleftlocation = 0 + decorate_thickness;//left维度x方向固定

            double YLocation = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1 ;
            SetkitchenFurniture(room, FurnitureType.Washer, origin, xrightlocation,
                                YLocation, rightx_axis, righty_axis, WasherType.Standard_Size, FurnitureSize.Washer);//放固定位置的水槽right
            YLocation = firstyrightlocation;
            SetkitchenFurniture(room, FurnitureType.Dishwasher, origin, xrightlocation,
                        YLocation, rightx_axis, righty_axis, DishwasherType.Standard_Size, FurnitureSize.Dishwasher);//添加洗碗机
            double restlengthright = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1
                            - FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;
            YLocation += FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;

            //3
            if (room_restlength >= minroom_restlength + FurnitureSize.Storage[StorageType.Drawers_Type].Item1)
            {
                StorageType storageTyperight;
                restlengthright = room_restlength - minroom_restlength;
                SetkitchenFurniture(room, FurnitureType.Storage, origin, restlengthright, standard_width,
                     xrightlocation,YLocation, rightx_axis, righty_axis, FurnitureSize.Storage, out storageTyperight);//放右边的柜子
                restlengthright -= FurnitureSize.Storage[storageTyperight].Item1;
            }

            double fillylocation = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1 - restlengthright * 0.5;
            SetkitchenFurniture(room, FurnitureType.FillBlock, origin, restlengthright,
                standard_width, xrightlocation, fillylocation, rightx_axis, righty_axis);//放FillBlock
            double restlengthleft = minrestLength;

            FridgeType fridgeType;
            if (depth >= decorate_thickness * 2 + minfurnitureleftLength + FurnitureSize.Basket[BasketType.Standard_Size].Item1)
            {
                restlengthleft -= FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                if (depth >= decorate_thickness * 2 + minfurnitureleftLength + FurnitureSize.Basket[BasketType.Standard_Size].Item1
                                    + FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1)//3
                {
                    restlengthleft -= FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
                    SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlengthleft, fridge_restWidth,
                        xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                        FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                    restlengthleft -= FurnitureSize.Fridge[fridgeType].Item1;
                    firstyleftlocation += FurnitureSize.Fridge[fridgeType].Item1;
                    SetkitchenFurniture(room, FurnitureType.Electrical, origin, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                    ElectricalType.Standard_Size, FurnitureSize.Electrical);//放Electrical
                    firstyleftlocation += FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
                    restlengthleft -= FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
                }
                else//2
                {
                    SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlengthleft, fridge_restWidth,
                        xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                        FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                    restlengthleft -= FurnitureSize.Fridge[fridgeType].Item1;
                    firstyleftlocation += FurnitureSize.Fridge[fridgeType].Item1;
                }

                SetkitchenFurniture(room, FurnitureType.Basket, origin,
                                xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                BasketType.Standard_Size, FurnitureSize.Basket);//放Basket
                firstyleftlocation += FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                
            }
            else//1
            {
                SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlengthleft, fridge_restWidth,
                            xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                            FurnitureSize.Fridge, out fridgeType);//放最下面的冰箱
                firstyleftlocation += FurnitureSize.Fridge[fridgeType].Item1;
                restlengthleft -= FurnitureSize.Fridge[fridgeType].Item1;
            }
            StorageType storageType;
            double rest_Length;
            double Y_Location;
            //restlengthleft -= 600;
            SetKitchenType1CookerStorage(room, origin, restlengthleft,
               xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                out storageType, out rest_Length, out Y_Location);

        }
        public static void KitchenType5(RoomTest room, Point origin, double width, double depth)
        {
            double room_restlength = depth - decorate_thickness * 2 - kitchenshaft_length;
            double room_restwidth = width - decorate_thickness * 2 - kitchenshaft_width;
            double minrestLength = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1
                                 - FurnitureSize.Storage[StorageType.Drawers_Type].Item1 - FurnitureSize.Cooker[CookerType.Standard_Size].Item1;
            double fridge_restWidth = width - decorate_thickness * 2 - kitchendoor_size - standard_width;
            double minfurniture_length = FurnitureSize.Storage[StorageType.Drawers_Type].Item1 + FurnitureSize.Cooker[CookerType.Standard_Size].Item1
                                        + FurnitureSize.Washer[WasherType.Standard_Size].Item1 /*+ FurnitureSize.Fridge[FridgeType.L333_Type].Item1*/;
            double firstyrightlocation = 0 + decorate_thickness;
            double xrightlocation = width - decorate_thickness;//right维度x方向固定
            double firstyleftlocation = 0 + decorate_thickness;
            double xleftlocation = 0 + decorate_thickness;//left维度x方向固定
            double ytoplocation = depth - decorate_thickness;
            double xtoplocation = 0 + decorate_thickness;//top维度y方向固定
            double YLocation = firstyrightlocation;
            double restlength = minrestLength + FurnitureSize.Storage[StorageType.Drawers_Type].Item1;

            //left
            FridgeType fridgeType;
            double restlengthleft = room_restlength - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
            SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlengthleft, fridge_restWidth,
                            xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                FurnitureSize.Fridge, out fridgeType);//放冰箱
            firstyleftlocation += FurnitureSize.Fridge[fridgeType].Item1;
            SetkitchenFurniture(room, FurnitureType.Electrical, origin, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                ElectricalType.Standard_Size, FurnitureSize.Electrical);//放Electrical
            firstyleftlocation += FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;

            //right
            if (room_restlength >= minfurniture_length + FurnitureSize.Basket[BasketType.Standard_Size].Item1)
            {
                SetkitchenFurniture(room, FurnitureType.Basket, origin, xrightlocation, YLocation, rightx_axis, righty_axis,
                                        BasketType.Standard_Size, FurnitureSize.Basket);//放Basket
                YLocation += FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                restlength -= FurnitureSize.Basket[BasketType.Standard_Size].Item1;

                StorageType storageTypeleft;
                restlengthleft = depth - decorate_thickness * 2 - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1 - FurnitureSize.Fridge[fridgeType].Item1;
                SetkitchenFurniture(room, FurnitureType.Storage, origin, restlengthleft, standard_width, xleftlocation, firstyleftlocation, leftx_axis, lefty_axis,
                                    FurnitureSize.Storage, out storageTypeleft);//放左边的柜子
            }
            StorageType storageType;
            double rest_Length;
            double Y_Location;
            SetKitchenType1CookerStorage(room, origin, restlength, xrightlocation, YLocation,
                                    rightx_axis, righty_axis, out storageType, out rest_Length, out Y_Location);//放柜子和灶台(右边的）
            //top
            xtoplocation += standard_width;
            SetkitchenFurniture(room, FurnitureType.Washer, origin, xtoplocation,
                                ytoplocation, topx_axis, topy_axis, WasherType.Standard_Size, FurnitureSize.Washer);//放固定位置的水槽
            xtoplocation += FurnitureSize.Washer[WasherType.Standard_Size].Item1;

            if (room_restwidth >= FurnitureSize.Washer[WasherType.Standard_Size].Item1 + FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1 + standard_width)
            {
                //double xdishLocation = room_restwidth - FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;
                SetkitchenFurniture(room, FurnitureType.Dishwasher, origin, xtoplocation,
                                ytoplocation, topx_axis, topy_axis, DishwasherType.Standard_Size, FurnitureSize.Dishwasher);
            }
            rest_Length = room_restwidth - FurnitureSize.Washer[WasherType.Standard_Size].Item1;
            xtoplocation += rest_Length * 0.5;
            SetkitchenFurniture(room, FurnitureType.FillBlock, origin, rest_Length,
                standard_width, xtoplocation, ytoplocation, topx_axis, topy_axis);//放FillBlock
        }
        public static void KitchenType6(RoomTest room, Point origin, double width, double depth)
        {

            double room_restlength = depth - decorate_thickness * 2 - kitchenshaft_length;
            double room_restwidth = width - decorate_thickness * 2 - kitchenshaft_width;
            double minrestLength = room_restlength - FurnitureSize.Washer[WasherType.Standard_Size].Item1
                                 - FurnitureSize.Storage[StorageType.Drawers_Type].Item1 - FurnitureSize.Cooker[CookerType.Standard_Size].Item1;
            double fridge_restWidth = width - decorate_thickness * 2 - kitchendoor_size - standard_width;
            double minfurniture_length = FurnitureSize.Storage[StorageType.Drawers_Type].Item1 + FurnitureSize.Cooker[CookerType.Standard_Size].Item1
                                        + FurnitureSize.Washer[WasherType.Standard_Size].Item1 /*+ FurnitureSize.Fridge[FridgeType.L333_Type].Item1*/;
            double firstyrightlocation = 0 + decorate_thickness;
            double xrightlocation = width - decorate_thickness;//right维度x方向固定
            double ytoplocation = depth - decorate_thickness;
            double xtoplocation = 0 + decorate_thickness;//top维度y方向固定
            double ybottomlocation = 0 + decorate_thickness;
            double firstxbottomlocation = 0 + decorate_thickness;//bott维度y方向固定

            double YLocation = firstyrightlocation;
            YLocation += standard_width;
            double restlength = minrestLength + FurnitureSize.Storage[StorageType.Drawers_Type].Item1;



            //right
            if (room_restlength >= minfurniture_length + FurnitureSize.Basket[BasketType.Standard_Size].Item1)
            {
                SetkitchenFurniture(room, FurnitureType.Basket, origin, xrightlocation, YLocation, rightx_axis, righty_axis,
                                        BasketType.Standard_Size, FurnitureSize.Basket);//放Basket
                YLocation += FurnitureSize.Basket[BasketType.Standard_Size].Item1;
                restlength -= FurnitureSize.Basket[BasketType.Standard_Size].Item1;
            }
            StorageType storageType;
            double rest_Length;
            double Y_Location;
            restlength -= standard_width;
            SetKitchenType1CookerStorage(room, origin, restlength, xrightlocation, YLocation,
                                    rightx_axis, righty_axis, out storageType, out rest_Length, out Y_Location);//放柜子和灶台(右边的）
            //bottom
            FridgeType fridgeType;
            double restlengthleft = room_restlength - FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1;
            SetkitchenFurniture(room, FurnitureType.Fridge, origin, restlengthleft, fridge_restWidth,
                            firstxbottomlocation, ybottomlocation, bottomx_axis, bottomy_axis,
                                FurnitureSize.Fridge, out fridgeType);//放冰箱
            firstxbottomlocation += FurnitureSize.Fridge[fridgeType].Item1;
            //top
            SetkitchenFurniture(room, FurnitureType.Washer, origin, xtoplocation,
                                ytoplocation, topx_axis, topy_axis, WasherType.Standard_Size, FurnitureSize.Washer);//放固定位置的水槽
            xtoplocation += FurnitureSize.Washer[WasherType.Standard_Size].Item1;

            if (room_restwidth >= FurnitureSize.Fridge[FridgeType.L333_Type].Item1 + FurnitureSize.Electrical[ElectricalType.Standard_Size].Item1)
            {
                if (room_restwidth >= FurnitureSize.Washer[WasherType.Standard_Size].Item1 + FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1)
                {
                    //double xdishLocation = room_restwidth - FurnitureSize.Dishwasher[DishwasherType.Standard_Size].Item1;
                    SetkitchenFurniture(room, FurnitureType.Dishwasher, origin, xtoplocation,
                                    ytoplocation, topx_axis, topy_axis, DishwasherType.Standard_Size, FurnitureSize.Dishwasher);
                }
                SetkitchenFurniture(room, FurnitureType.Electrical, origin, firstxbottomlocation, ybottomlocation, bottomx_axis, bottomy_axis,
                                ElectricalType.Standard_Size, FurnitureSize.Electrical);//放Electrical
            }
            rest_Length = room_restwidth - FurnitureSize.Washer[WasherType.Standard_Size].Item1;
            xtoplocation += rest_Length * 0.5;
            SetkitchenFurniture(room, FurnitureType.FillBlock, origin, rest_Length,
                standard_width, xtoplocation, ytoplocation, topx_axis, topy_axis);//放FillBlock
        }
        //}

    }
}

