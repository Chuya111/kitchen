using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD
{
    public enum FurnitureType
    {
        Bed,
        Wardrobe,
        TV_Table,
        Bed_Side_Table,
        Toilet,
        Basin,
        Shower,
        Washer,
        Cooker,
        Fridge,
        Storage,
        Basket,
        Electrical,
        Dishwasher,
        FillBlock,
    }
    public enum FillBlockType
    {
        Fill_Block,
    }
    public enum BedType
    {
        Single_Size,
        Full_Size,
        Queen_Size,
        King_Size,
    }

    public enum BedSideTableType
    {
        Bed_Side_Table,
    }

    public enum TVTableType
    {
        TV_Table_200,
        TV_Table_400,
    }
    public enum ToiletType
    {
        Standard_Size,
    };

    public enum ShowerType
    {
        Screen_Type,
        Fan_Shape_Type,
        Diamond_Shape_Type,
        Rectangle_Type,
    };
    public enum BasinType
    {
        Compact_Hanging_Basin,
        Standard_Hanging_Basin,
        //Compact_Custom_Size,
        Compact_Size,
        Standard_Size,
        Comfortable_Size,
        Large_Size,
    };
    public enum WasherType
    {
        Standard_Size,
    };

    public enum CookerType
    {
        Standard_Size,
    };
    
    public enum FridgeType
    {
        L333_Type,
        L488_Type,
    };


    public enum StorageType
    {
        Drawers_Type,
        Single_Type,
        Double_Type,
        
    };

    public enum BasketType
    {
        Standard_Size,
    };
 
    public enum ElectricalType
    {
        Standard_Size,
    };
    public enum DishwasherType
    {
        Standard_Size,
    };

    public static class FurnitureSize
    {
        //厨房

        //public static Dictionary<FillBlockType, Tuple<double, int, int>> FillBlock = new Dictionary<FillBlockType, Tuple<int, int, int>>()
        //{
        //  {FillBlockType.Fill_Block, new Tuple<double,int,int>(restlength, 600, 0) },
        //  };
        ////??

        public static Dictionary<WasherType, Tuple<int, int, int>> Washer = new Dictionary<WasherType, Tuple<int, int, int>>()
        {
          {WasherType.Standard_Size, new Tuple<int,int,int>(800, 600, 0) },
          };
        
        public static Dictionary<CookerType, Tuple<int, int, int>> Cooker = new Dictionary<CookerType, Tuple<int, int, int>>()
        {
          {CookerType.Standard_Size, new Tuple<int,int,int>(800, 600, 0) },
          };
        public static Dictionary<BasketType, Tuple<int, int, int>> Basket = new Dictionary<BasketType, Tuple<int, int, int>>()
        {
          {BasketType.Standard_Size, new Tuple<int,int,int>(350, 600, 0) },
          };
        public static Dictionary<ElectricalType, Tuple<int, int, int>> Electrical = new Dictionary<ElectricalType, Tuple<int, int, int>>()
        {
          {ElectricalType.Standard_Size, new Tuple<int,int,int>(600, 600, 0) },
          };
        public static Dictionary<DishwasherType, Tuple<int, int, int>> Dishwasher = new Dictionary<DishwasherType, Tuple<int, int, int>>()
        {
          {DishwasherType.Standard_Size, new Tuple<int,int,int>(600, 600, 0) },
          };
        public static Dictionary<FridgeType, Tuple<int, int, int>> Fridge = new Dictionary<FridgeType, Tuple<int, int, int>>()
        {
            { FridgeType.L333_Type, new Tuple<int,int,int>(600, 700, 0) },
            { FridgeType.L488_Type, new Tuple<int,int,int>(900, 800, 0) },
          };

        public static Dictionary<StorageType, Tuple<int, int, int>> Storage = new Dictionary<StorageType, Tuple<int, int, int>>()
        {
          { StorageType.Drawers_Type, new Tuple<int,int,int>(450, 600, 0) },
          { StorageType.Single_Type, new Tuple<int,int,int>(600, 600, 0) },
          { StorageType.Double_Type, new Tuple<int,int,int>(900, 600, 0) },
          };

        //卫生间
        public static Dictionary<ToiletType, Tuple<int, int, int>> Toilet = new Dictionary<ToiletType, Tuple<int, int, int>>()
        {
            { ToiletType.Standard_Size, new Tuple<int,int,int>(1200, 900, 0) },
        };


        public static Dictionary<ShowerType, Tuple<int, int, int>> Shower = new Dictionary<ShowerType, Tuple<int, int, int>>()
        {
            { ShowerType.Screen_Type, new Tuple<int,int,int>(1200, 900, 0) },
            { ShowerType.Fan_Shape_Type, new Tuple<int,int,int>(900, 900, 0) },
            { ShowerType.Diamond_Shape_Type, new Tuple<int,int,int>(900, 900, 0) },
            { ShowerType.Rectangle_Type, new Tuple<int,int,int>(1200, 900, 0)},
        };


        public static Dictionary<BasinType, Tuple<int, int, int>> Basin = new Dictionary<BasinType, Tuple<int, int, int>>()
        {
             { BasinType.Compact_Size, new Tuple<int,int,int>(600, 500, 0) },
             //{ BasinType.Compact_Custom_Size, new Tuple<int,int,int>(900, 500, 0) },
             { BasinType.Standard_Size, new Tuple<int,int,int>(900, 600, 0) },
             { BasinType.Comfortable_Size, new Tuple<int,int,int>(1200, 600, 0) },
             { BasinType.Large_Size, new Tuple<int,int,int>(1500, 600, 0)},
         };


        //床的类型与对应的长宽高
        public static Dictionary<BedType, Tuple<int, int, int>> Bed = new Dictionary<BedType, Tuple<int, int, int>>()
        {
            { BedType.Single_Size, new Tuple<int,int,int>(2050,950,0) },
            { BedType.Full_Size, new Tuple<int,int,int>(2050,1400,0) },
            { BedType.Queen_Size, new Tuple<int,int,int>(2150,1600,0) },
            { BedType.King_Size, new Tuple<int,int,int>(2150,1900,0)},
        };


        public static Dictionary<BedSideTableType, Tuple<int, int, int>> Bed_Side_Table = new Dictionary<BedSideTableType, Tuple<int, int, int>>()
        {
            { BedSideTableType.Bed_Side_Table , new Tuple<int,int,int>(500,450,0) },
        };


        public static Dictionary<TVTableType, Tuple<int, int, int>> TV_Table = new Dictionary<TVTableType, Tuple<int, int, int>>()
        {
            { TVTableType.TV_Table_200 , new Tuple<int,int,int>(-1,200,0) },
            { TVTableType.TV_Table_400, new Tuple<int,int,int>(-1,400,0) }, // -代表这个尺寸可以连续变化，不设置
        };

    }


    /// <summary>
    /// 王老师写的部品类与工厂模式，用于生成部品
    /// </summary>
    public class Bed
    {

        public Bed(int width, int length)
        {
            Width = width;
            Length = length;
        }

        public int Width { get; set; }
        public int Length { get; set; }


        public static Bed CreateBed(BedType bedType)
        {
            return bedType switch
            {
                BedType.Single_Size => new Bed(100, 200),
                BedType.Queen_Size => new Bed(150, 200),
            };

            var l = new List<int>() { };
            l.Select(x=>x).First();
        }
    }

}
