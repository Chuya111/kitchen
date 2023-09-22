using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DD
{
    public partial class ToolKit
    {
        static double curtain_width = 150;
        static double wardrobe_width = 600;
        static string Path = "Bedroom/";
        static Transform transform = null;

        public static void CheckBedroom(RoomTest room)
        {
            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Remove(x)); //这个写法有点蠢，需要优化一下
            room.furnitures.Clear();

            transform = GetTransform(room.X_Axis, room.Y_Axis);
            SetBedKit(room, true, true, out double width, out double length, out Point location);
            SetTVTable(room, location, length);
            SetWardrobe(room, width);

            room.furnitures.ForEach(x => MainWindow.Instance.canvas2.Children.Add(x));
        }

        static private void SetWardrobe(RoomTest room, double width)
        {
            var room_height = (room.ActualHeight - room.StrokeThickness) * scale_rate;
            var room_width = (room.ActualWidth - room.StrokeThickness) * scale_rate;

            var rest_height = room_height - width;
            var rest_width = room_width - decorate_thickness * 2 - 1050; //1050是门的预留尺寸

            if (rest_height < 700) { }
            else if (rest_height < 1700)
            {
                GenerateWardrobe(room, rest_width);
            }
            else if (rest_height < 2200)
            {
                rest_height -= 100;
                GenerateWardrobe(room, rest_width);
            }
            else
            {
                rest_height -= 100;
                GenerateWardrobe(room, rest_width);
            }
        }

        static private void GenerateWardrobe(RoomTest room, double width)
        {
            var x_axis = new Vector(-1, 0);
            var y_axis = new Vector(0, 1);
            var origin_x = (Canvas.GetLeft(room) + room.ActualWidth - room.StrokeThickness * 0.5) * scale_rate;
            var origin_y = (Canvas.GetTop(room) + room.StrokeThickness * 0.5) * scale_rate;

            var room_height = (room.ActualHeight - room.StrokeThickness) * scale_rate;
            var room_width = (room.ActualWidth - room.StrokeThickness) * scale_rate;

            var corner = new Point(origin_x, origin_y)
                        + (room_width - decorate_thickness) * x_axis
                        + (room_height - decorate_thickness) * y_axis;


            if (width < 1200 + 90)
            {

            }
            else if (width < 1500 + 90)
            {
                var wardrobe_pos = corner + x_axis * -600 + y_axis * -300;
                var wardrobe = new Furniture(FurnitureType.Wardrobe, (int)(wardrobe_pos.X / scale_rate), (int)(wardrobe_pos.Y / scale_rate));
                wardrobe.X_Size = 1200 / scale_rate;
                wardrobe.Y_Size = 600 / scale_rate;
                wardrobe.X_Axis = -1 * x_axis;
                wardrobe.Y_Axis = -1 * y_axis;
                wardrobe.ImagePath = Path + "Wardrobe_1200.png";
                wardrobe.Transform = GetTransform(wardrobe.X_Axis, wardrobe.Y_Axis);
                room.furnitures.Add(wardrobe);

            }
            else if (width < 1800 + 90)
            {
                var wardrobe_pos = corner + x_axis * -750 + y_axis * -300;
                var wardrobe = new Furniture(FurnitureType.Wardrobe, (int)(wardrobe_pos.X / scale_rate), (int)(wardrobe_pos.Y / scale_rate));
                wardrobe.X_Size = 1500 / scale_rate;
                wardrobe.Y_Size = 600 / scale_rate;
                wardrobe.X_Axis = -1 * x_axis;
                wardrobe.Y_Axis = -1 * y_axis;
                wardrobe.ImagePath = Path + "Wardrobe_1500.png";
                wardrobe.Transform = GetTransform(wardrobe.X_Axis, wardrobe.Y_Axis);
                room.furnitures.Add(wardrobe);

            }
            else if (width < 2000 + 90)
            {
                var wardrobe_pos = corner + x_axis * -900 + y_axis * -300;
                var wardrobe = new Furniture(FurnitureType.Wardrobe, (int)(wardrobe_pos.X / scale_rate), (int)(wardrobe_pos.Y / scale_rate));
                wardrobe.X_Size = 1800 / scale_rate;
                wardrobe.Y_Size = 600 / scale_rate;
                wardrobe.X_Axis = -1 * x_axis;
                wardrobe.Y_Axis = -1 * y_axis;
                wardrobe.ImagePath = Path + "Wardrobe_1800.png";
                wardrobe.Transform = GetTransform(wardrobe.X_Axis, wardrobe.Y_Axis);
                room.furnitures.Add(wardrobe);
            }
            else if (width < 2400 + 90)
            {
                var wardrobe_pos = corner + x_axis * -1000 + y_axis * -300;
                var wardrobe = new Furniture(FurnitureType.Wardrobe, (int)(wardrobe_pos.X / scale_rate), (int)(wardrobe_pos.Y / scale_rate));
                wardrobe.X_Size = 2000 / scale_rate;
                wardrobe.Y_Size = 600 / scale_rate;
                wardrobe.X_Axis = -1 * x_axis;
                wardrobe.Y_Axis = -1 * y_axis;
                wardrobe.ImagePath = Path + "Wardrobe_2000.png";
                wardrobe.Transform = GetTransform(wardrobe.X_Axis, wardrobe.Y_Axis);
                room.furnitures.Add(wardrobe);

            }
            else
            {
                var wardrobe_pos = corner + x_axis * -1200 + y_axis * -300;
                var wardrobe = new Furniture(FurnitureType.Wardrobe, (int)(wardrobe_pos.X / scale_rate), (int)(wardrobe_pos.Y / scale_rate));
                wardrobe.X_Size = 2400 / scale_rate;
                wardrobe.Y_Size = 600 / scale_rate;
                wardrobe.X_Axis = -1 * x_axis;
                wardrobe.Y_Axis = -1 * y_axis;
                wardrobe.ImagePath = Path + "Wardrobe_2400.png";
                wardrobe.Transform = GetTransform(wardrobe.X_Axis, wardrobe.Y_Axis);
                room.furnitures.Add(wardrobe);

            }
        }

        static private void SetTVTable(RoomTest room, Point location, double length)
        {

            var x_axis = new Vector(-1, 0);
            var y_axis = new Vector(0, 1);
            var room_height = (room.ActualHeight - room.StrokeThickness) * scale_rate;
            var room_width = (room.ActualWidth - room.StrokeThickness) * scale_rate;

            var TV_table_length = 1500.0;
            TV_table_length = room_height - 1050 > 0.5 * TV_table_length ? TV_table_length : (room_height - 1050) * 2;

            var TV_table_pos = location - x_axis * (room_width - decorate_thickness * 2);
            var TV_table = new Furniture(FurnitureType.TV_Table, TV_table_pos.X / scale_rate, TV_table_pos.Y / scale_rate);
            TV_table.X_Axis = -1 * y_axis;
            TV_table.Y_Axis = x_axis;

            var rest = room_width - length;

            if (rest < 700) { }
            else if (rest < FurnitureSize.TV_Table[TVTableType.TV_Table_400].Item2 + 700)
            {
                var TV_table_width = FurnitureSize.TV_Table[TVTableType.TV_Table_200].Item2;
                TV_table.X_Size = TV_table_length / scale_rate;
                TV_table.Y_Size = TV_table_width / scale_rate;
                TV_table.ImagePath = Path + "TV_Table_200.png";
                TV_table.Transform = GetTransform(TV_table.X_Axis, TV_table.Y_Axis);
                room.furnitures.Add(TV_table);
            }
            else if (rest < FurnitureSize.TV_Table[TVTableType.TV_Table_400].Item2 + 600 + 300) //间距变化 + 300床前凳尺寸
            {
                var TV_table_width = FurnitureSize.TV_Table[TVTableType.TV_Table_400].Item2;
                TV_table.X_Size = TV_table_length / scale_rate;
                TV_table.Y_Size = TV_table_width / scale_rate;
                TV_table.ImagePath = Path + "TV_Table_400.png";
                TV_table.Transform = GetTransform(TV_table.X_Axis, TV_table.Y_Axis);
                room.furnitures.Add(TV_table);

            }
            else
            {
                var TV_table_width = FurnitureSize.TV_Table[TVTableType.TV_Table_400].Item2;
                TV_table.X_Size = TV_table_length / scale_rate;
                TV_table.Y_Size = TV_table_width / scale_rate;
                TV_table.ImagePath = Path + "TV_Table_400.png";
                TV_table.Transform = GetTransform(TV_table.X_Axis, TV_table.Y_Axis);
                room.furnitures.Add(TV_table);
            }
        }

        static private void SetBedKit(RoomTest room, bool left, bool right, out double width, out double length, out Point location)
        {
            width = 150.0;
            length = 0.0;
            var bed_side_table_length = FurnitureSize.Bed_Side_Table[BedSideTableType.Bed_Side_Table].Item1;
            var bed_side_table_width = FurnitureSize.Bed_Side_Table[BedSideTableType.Bed_Side_Table].Item2;

            var origin_x = (Canvas.GetLeft(room) + room.ActualWidth - room.StrokeThickness * 0.5) * scale_rate;
            var origin_y = (Canvas.GetTop(room) + room.StrokeThickness * 0.5) * scale_rate;
            var x_axis = new Vector(-1, 0);
            var y_axis = new Vector(0, 1);

            var room_height = (room.ActualHeight - room.StrokeThickness) * scale_rate;
            var room_width = (room.ActualWidth - room.StrokeThickness) * scale_rate;

            var bed_type = CheckBedSize(room_height);
            var bed_length = FurnitureSize.Bed[bed_type].Item1;
            var bed_width = FurnitureSize.Bed[bed_type].Item2;

            var bed_pos = new Point(origin_x, origin_y)
                          + x_axis * (room_width - decorate_thickness)
                          + y_axis * (bed_width * 0.5 + (right ? bed_side_table_width : 400) + decorate_thickness + curtain_width);

            var bed = new Furniture(FurnitureType.Bed, bed_pos.X / scale_rate, bed_pos.Y / scale_rate,
                bed_width / scale_rate, bed_length / scale_rate, y_axis, -1 * x_axis);
            bed.ImagePath = Path + bed_type.ToString() + "_Bed.png";
            bed.Transform = GetTransform(bed.X_Axis, bed.Y_Axis);
            room.furnitures.Add(bed);

            width += bed_width;
            length += bed_length;
            //MessageBox.Show(bed_pos.Y.ToString());


            if (left && bed_type != BedType.Single_Size)
            {
                var left_bed_side_table_pos = bed_pos + y_axis * (bed_width + bed_side_table_width) * 0.5;
                var left_bed_side_table = new Furniture(FurnitureType.Bed_Side_Table, left_bed_side_table_pos.X / scale_rate, left_bed_side_table_pos.Y / scale_rate);
                left_bed_side_table.X_Size = bed_side_table_length / scale_rate;
                left_bed_side_table.Y_Size = bed_side_table_width / scale_rate;
                left_bed_side_table.X_Axis = y_axis;
                left_bed_side_table.Y_Axis = -1 * x_axis;
                left_bed_side_table.ImagePath = Path + "Bed_Side_Table.png";
                left_bed_side_table.Transform = GetTransform(left_bed_side_table.X_Axis, left_bed_side_table.Y_Axis);
                room.furnitures.Add(left_bed_side_table);

                width += bed_side_table_width;
            }

            if (right)
            {
                var right_bed_side_table_pos = bed_pos - y_axis * (bed_width + bed_side_table_width) * 0.5;
                var right_bed_side_table = new Furniture(FurnitureType.Bed_Side_Table, right_bed_side_table_pos.X / scale_rate, right_bed_side_table_pos.Y / scale_rate);
                right_bed_side_table.X_Size = bed_side_table_length / scale_rate;
                right_bed_side_table.Y_Size = bed_side_table_width / scale_rate;
                right_bed_side_table.X_Axis = y_axis;
                right_bed_side_table.Y_Axis = -1 * x_axis;
                right_bed_side_table.ImagePath = Path + "Bed_Side_Table.png";
                right_bed_side_table.Transform = GetTransform(right_bed_side_table.X_Axis, right_bed_side_table.Y_Axis);
                room.furnitures.Add(right_bed_side_table);

                width += bed_side_table_width;
            }

            location = bed_pos;
        }

        static private BedType CheckBedSize(double length)
        {

            var rest = length - wardrobe_width - decorate_thickness * 2 - curtain_width; //减去柜子的预留宽度、腻子的厚度、窗帘的预留宽度

            if (rest < FurnitureSize.Bed[BedType.Full_Size].Item2 + FurnitureSize.Bed_Side_Table[BedSideTableType.Bed_Side_Table].Item2 * 2 + 100) // 100是床头柜和衣柜之间的预留距离
            {
                return BedType.Single_Size;
            }
            else if (rest < FurnitureSize.Bed[BedType.Queen_Size].Item2 + FurnitureSize.Bed_Side_Table[BedSideTableType.Bed_Side_Table].Item2 * 2 + 100)
            {
                return BedType.Full_Size;
            }
            else if (rest < FurnitureSize.Bed[BedType.King_Size].Item2 + FurnitureSize.Bed_Side_Table[BedSideTableType.Bed_Side_Table].Item2 * 2 + 100)
            {
                return BedType.Queen_Size;
            }
            else
            {
                return BedType.King_Size;
            }
        }
    }
}
