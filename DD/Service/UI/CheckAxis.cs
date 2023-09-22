using System.Windows.Controls;

namespace DD
{
    public partial class ToolKit
    {
        public static void CheckAxis(RoomTest room)
        {
            Canvas canvas = MainWindow.Instance.canvas2;
            Axis axis = MainWindow.Instance.axis;

            var top_y = Canvas.GetTop(room);
            var left_x = Canvas.GetLeft(room);
            var bottom_y = top_y + room.Height;
            var right_x = left_x + room.Width;

            if (axis.top == top_y && !axis.bottomRooms.Contains(room))
            {
                axis.bottomRooms.Add(room);
            }
            else if (axis.top != top_y && axis.bottomRooms.Contains(room))
            {
                axis.bottomRooms.Remove(room);
            }

            if (axis.bottom == bottom_y && !axis.topRooms.Contains(room))
            {
                axis.topRooms.Add(room);
            }
            else if (axis.bottom != bottom_y && axis.topRooms.Contains(room))
            {
                axis.topRooms.Remove(room);
            }

            if (axis.left == left_x && !axis.righRrooms.Contains(room))
            {
                axis.righRrooms.Add(room);
            }
            else if (axis.left != left_x && axis.righRrooms.Contains(room))
            {
                axis.righRrooms.Remove(room);
            }

            if (axis.right == right_x && !axis.leftRooms.Contains(room))
            {
                axis.leftRooms.Add(room);
            }
            else if (axis.right != right_x && axis.leftRooms.Contains(room))
            {
                axis.leftRooms.Remove(room);
            }

        }
    }
}

