using System;
using System.Windows;
using System.Windows.Controls;

namespace DD
{
    public partial class ToolKit
    {
        public static void CheckControlLine(RoomTest room)
        {
            Canvas canvas = MainWindow.Instance.canvas2;

            CheckRoom(room); // Clear ControlLines

            CheckControlBar(room); //check the existing ControlLine

            for (int i = 0; i < canvas.Children.Count; i++)
            {
                if (canvas.Children[i] is RoomTest && (canvas.Children[i] != room))
                {
                    var testroom = (RoomTest)canvas.Children[i];
                    AddRoomBoudary(testroom, room); //Add ControlLines
                }
            }
        }

        static void CheckRoom(RoomTest room)
        {
            Canvas canvas = MainWindow.Instance.canvas2;

            var top_y = Canvas.GetTop(room);
            var left_x = Canvas.GetLeft(room);
            var bottom_y = top_y + room.Height;
            var right_x = left_x + room.Width;

            if (room.topControBar != null && room.topControBar.Center.Y != top_y)
            {
                if (room.topControBar.topRooms.Count == 0 && room.topControBar.bottomRooms.Count <= 2 ||
                    room.topControBar.topRooms.Count == 1 && room.topControBar.bottomRooms.Count == 1)
                {
                    canvas.Children.Remove(room.topControBar);
                }
                room.topControBar.bottomRooms.Remove(room);
                room.topControBar = null;

            }
            if (room.bottomControlBar != null && room.bottomControlBar.Center.Y != bottom_y)
            {
                if (room.bottomControlBar.topRooms.Count <= 2 && room.bottomControlBar.bottomRooms.Count == 0 ||
                    room.bottomControlBar.topRooms.Count == 1 && room.bottomControlBar.bottomRooms.Count == 1)
                {
                    canvas.Children.Remove(room.bottomControlBar);
                }
                room.bottomControlBar.topRooms.Remove(room);
                room.bottomControlBar = null;
            }

            if (room.leftControBar != null && room.leftControBar.Center.X != left_x)
            {
                if (room.leftControBar.leftRooms.Count == 0 && room.leftControBar.righRrooms.Count <= 2 ||
                    room.leftControBar.leftRooms.Count == 1 && room.leftControBar.righRrooms.Count == 1)
                {
                    canvas.Children.Remove(room.leftControBar);
                }
                room.leftControBar.righRrooms.Remove(room);
                room.leftControBar = null;
            }
            if (room.rightControlBar != null && room.rightControlBar.Center.X != right_x)
            {
                if (room.rightControlBar.righRrooms.Count == 0 && room.rightControlBar.leftRooms.Count <= 2 ||
                    room.rightControlBar.righRrooms.Count == 1 && room.rightControlBar.leftRooms.Count == 1)
                {
                    canvas.Children.Remove(room.rightControlBar);
                }
                room.rightControlBar.leftRooms.Remove(room);
                room.rightControlBar = null;
            }

        }
        static void AddRoomBoudary(RoomTest testroom, RoomTest room)
        {
            var top_y = Canvas.GetTop(room);
            var left_x = Canvas.GetLeft(room);
            var bottom_y = top_y + room.Height;
            var right_x = left_x + room.Width;

            var test_top_y = Canvas.GetTop(testroom);
            var test_left_x = Canvas.GetLeft(testroom);
            var test_bottom_y = test_top_y + testroom.Height;
            var test_right_x = test_left_x + testroom.Width;

            if (top_y == test_top_y && room.topControBar == null && top_y != MainWindow.Instance.axis.top)
            {
                //MessageBox.Show("scs");
                var cl = new Controlbar(AxisDirection.Horizontal, new Point(250, top_y), 350);
                cl.bottomRooms.Add(room);
                room.topControBar = cl;
                cl.bottomRooms.Add(testroom);
                testroom.topControBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }
            if (top_y == test_bottom_y && room.topControBar == null)
            {
                var cl = new Controlbar(AxisDirection.Horizontal, new Point(250, top_y), 350);
                cl.bottomRooms.Add(room);
                room.topControBar = cl;
                cl.topRooms.Add(testroom);
                testroom.bottomControlBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }
            if (bottom_y == test_top_y && room.bottomControlBar == null)
            {
                var cl = new Controlbar(AxisDirection.Horizontal, new Point(250, bottom_y), 350);
                cl.topRooms.Add(room);
                room.bottomControlBar = cl;
                cl.bottomRooms.Add(testroom);
                testroom.topControBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }
            if (bottom_y == test_bottom_y && room.bottomControlBar == null && bottom_y != MainWindow.Instance.axis.bottom)
            {
                var cl = new Controlbar(AxisDirection.Horizontal, new Point(250, bottom_y), 350);
                cl.topRooms.Add(room);
                room.bottomControlBar = cl;
                cl.topRooms.Add(testroom);
                testroom.bottomControlBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }

            //////////////////////////////////////////
            if (left_x == test_left_x && room.leftControBar == null && left_x != MainWindow.Instance.axis.left)
            {
                var cl = new Controlbar(AxisDirection.Vertical, new Point(left_x, 250), 350);
                cl.righRrooms.Add(room);
                room.leftControBar = cl;
                cl.righRrooms.Add(testroom);
                testroom.leftControBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }
            if (left_x == test_right_x && room.leftControBar == null)
            {
                var cl = new Controlbar(AxisDirection.Vertical, new Point(left_x, 250), 350);
                cl.righRrooms.Add(room);
                room.leftControBar = cl;
                cl.leftRooms.Add(testroom);
                testroom.rightControlBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }
            if (right_x == test_left_x && room.rightControlBar == null)
            {
                var cl = new Controlbar(AxisDirection.Vertical, new Point(right_x, 250), 350);
                cl.leftRooms.Add(room);
                room.rightControlBar = cl;
                cl.righRrooms.Add(testroom);
                testroom.leftControBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }
            if (right_x == test_right_x && room.rightControlBar == null && right_x != MainWindow.Instance.axis.right)
            {
                var cl = new Controlbar(AxisDirection.Vertical, new Point(right_x, 250), 350);
                cl.leftRooms.Add(room);
                room.rightControlBar = cl;
                cl.leftRooms.Add(testroom);
                testroom.rightControlBar = cl;
                MainWindow.Instance.canvas2.Children.Add(cl);
            }

        }
        public static void DeleteRoom(RoomTest room)
        {
            Canvas canvas = MainWindow.Instance.canvas2;


            if (room.topControBar != null)
            {
                if (room.topControBar.topRooms.Count == 0 && room.topControBar.bottomRooms.Count <= 2 ||
                    room.topControBar.topRooms.Count == 1 && room.topControBar.bottomRooms.Count == 1)
                {
                    canvas.Children.Remove(room.topControBar);
                }
                room.topControBar.bottomRooms.Remove(room);
                room.topControBar = null;

            }
            if (room.bottomControlBar != null)
            {
                if (room.bottomControlBar.topRooms.Count <= 2 && room.bottomControlBar.bottomRooms.Count == 0 ||
                    room.bottomControlBar.topRooms.Count == 1 && room.bottomControlBar.bottomRooms.Count == 1)
                {
                    canvas.Children.Remove(room.bottomControlBar);
                }
                room.bottomControlBar.topRooms.Remove(room);
                room.bottomControlBar = null;
            }

            if (room.leftControBar != null)
            {
                if (room.leftControBar.leftRooms.Count == 0 && room.leftControBar.righRrooms.Count <= 2 ||
                    room.leftControBar.leftRooms.Count == 1 && room.leftControBar.righRrooms.Count == 1)
                {
                    canvas.Children.Remove(room.leftControBar);
                }
                room.leftControBar.righRrooms.Remove(room);
                room.leftControBar = null;
            }
            if (room.rightControlBar != null)
            {
                if (room.rightControlBar.righRrooms.Count == 0 && room.rightControlBar.leftRooms.Count <= 2 ||
                    room.rightControlBar.righRrooms.Count == 1 && room.rightControlBar.leftRooms.Count == 1)
                {
                    canvas.Children.Remove(room.rightControlBar);
                }
                room.rightControlBar.leftRooms.Remove(room);
                room.rightControlBar = null;
            }

        }
        static void CheckControlBar(RoomTest room)
        {
            Canvas canvas = MainWindow.Instance.canvas2;

            var top_y = Canvas.GetTop(room);
            var left_x = Canvas.GetLeft(room);
            var bottom_y = top_y + room.Height;
            var right_x = left_x + room.Width;
            var threshold = 0.1;

            for (int i = 0; i < canvas.Children.Count; i++)
            {
                if (canvas.Children[i] is Controlbar)
                {
                    Controlbar cb = (Controlbar)canvas.Children[i];
                    if (cb.AxisDirection == AxisDirection.Horizontal)
                    {
                        if (Math.Abs(cb.Center.Y - top_y) < threshold && room.topControBar == null)
                        {
                            cb.bottomRooms.Add(room);
                            room.topControBar = cb;
                        }
                        if (Math.Abs(cb.Center.Y - bottom_y) < threshold && room.bottomControlBar == null)
                        {

                            cb.topRooms.Add(room);
                            room.bottomControlBar = cb;
                        }
                    }
                    else
                    {
                        if (Math.Abs(cb.Center.X - left_x) < threshold && room.leftControBar == null)
                        {
                            cb.righRrooms.Add(room);
                            room.leftControBar = cb;
                        }
                        if (Math.Abs(cb.Center.X - right_x) < threshold && room.rightControlBar == null)
                        {
                            cb.leftRooms.Add(room);
                            room.rightControlBar = cb;
                        }
                    }
                }
            }
        }
    }
}

