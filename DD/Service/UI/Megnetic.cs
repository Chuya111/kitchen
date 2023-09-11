using System;
using System.Windows.Controls;

namespace DD
{
    public partial class ToolKit
    {

        static double max_tolerance = 15;

        static double min_tolerance = 0.1;

        public static void MoveMagnet(RoomTest room, double top, double bottom, double right, double left, ref double x_canvas, ref double y_canvas)
        {
            if (Math.Abs(right - x_canvas) < max_tolerance && Math.Abs(right - x_canvas) > min_tolerance)
            {
                x_canvas += (right - x_canvas);
            }

            if (Math.Abs(bottom - y_canvas) < max_tolerance && Math.Abs(bottom - y_canvas) > min_tolerance)
            {
                y_canvas += (bottom - y_canvas);
            }

            if (Math.Abs(left - (x_canvas + room.Width)) < max_tolerance
                && Math.Abs(left - (x_canvas + room.Width)) > min_tolerance)
            {
                x_canvas += left - (x_canvas + room.Width);
            }

            if (Math.Abs(top - (y_canvas + room.Height)) < max_tolerance
                && Math.Abs(top - (y_canvas + room.Height)) > min_tolerance)
            {
                y_canvas += (top - (y_canvas + room.Height));
            }

            //同侧
            if (Math.Abs(left - x_canvas) < max_tolerance && Math.Abs(left - x_canvas) > min_tolerance)
            {
                x_canvas += (left - x_canvas);
            }

            if (Math.Abs(top - y_canvas) < max_tolerance && Math.Abs(top - y_canvas) > min_tolerance)
            {
                y_canvas += (top - y_canvas);
            }

            if (Math.Abs(right - (x_canvas + room.Width)) < max_tolerance
                && Math.Abs(right - (x_canvas + room.Width)) > min_tolerance)
            {
                x_canvas += right - (x_canvas + room.Width);
            }

            if (Math.Abs(bottom - (y_canvas + room.Height)) < max_tolerance
                && Math.Abs(bottom - (y_canvas + room.Height)) > min_tolerance)
            {
                y_canvas += (bottom - (y_canvas + room.Height));
            }

        }

        public static void DragMagnet(RoomTest room, bool[] isEdge, double top, double bottom, double right, double left)
        {

            var x_canvas = Canvas.GetLeft(room);
            var y_canvas = Canvas.GetTop(room);


            if (isEdge[0])
            {
                if (Math.Abs(right - x_canvas) < max_tolerance && Math.Abs(right - x_canvas) > min_tolerance)
                {
                    Canvas.SetLeft(room, right);
                    room.Width -= (right - x_canvas);
                }
                if (Math.Abs(left - x_canvas) < max_tolerance && Math.Abs(left - x_canvas) > min_tolerance)
                {
                    Canvas.SetLeft(room, left);
                    room.Width -= (left - x_canvas);
                }
            }

            if (isEdge[1])
            {
                if (Math.Abs(left - (x_canvas + room.Width)) < max_tolerance
                && Math.Abs(left - (x_canvas + room.Width)) > min_tolerance)
                {
                    room.Width += (left - (x_canvas + room.Width));
                }
                if (Math.Abs(right - (x_canvas + room.Width)) < max_tolerance
                && Math.Abs(right - (x_canvas + room.Width)) > min_tolerance)
                {
                    room.Width += (right - (x_canvas + room.Width));
                }
            }

            if (isEdge[2])
            {
                if (Math.Abs(bottom - y_canvas) < max_tolerance && Math.Abs(bottom - y_canvas) > min_tolerance)
                {
                    Canvas.SetTop(room, bottom);
                    room.Height -= (bottom - y_canvas);
                }
                if (Math.Abs(top - y_canvas) < max_tolerance && Math.Abs(top - y_canvas) > min_tolerance)
                {
                    Canvas.SetTop(room, top);
                    room.Height -= (top - y_canvas);
                }
            }

            if (isEdge[3])
            {

                if (Math.Abs(top - (y_canvas + room.Height)) < max_tolerance
                    && Math.Abs(top - (y_canvas + room.Height)) > min_tolerance)
                {
                    room.Height += (top - (y_canvas + room.Height));
                }

                if (Math.Abs(bottom - (y_canvas + room.Height)) < max_tolerance
                    && Math.Abs(bottom - (y_canvas + room.Height)) > min_tolerance)
                {
                    room.Height += (bottom - (y_canvas + room.Height));
                }

            }
        }

    }
}

