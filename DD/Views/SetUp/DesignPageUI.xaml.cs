using DD.Service;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DD.Views
{
    /// <summary>
    /// Interaction logic for DesignPage.xaml
    /// </summary>
    public partial class DesignPage : Page
    {
        RoomTest draggingRoom { get; set; }
        public bool copy;
        Point startPoint;
        bool isResizing, isLeftEdge, isRightEdge, isTopEdge, isBottomEdge;
        double oldHeight, oldWidth, oldTop, oldLeft;

        public void Rect_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            draggingRoom = sender as RoomTest;
            startPoint = e.GetPosition(canvas2);

            if (e.LeftButton == MouseButtonState.Pressed && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                ToolKit.DeleteRoom(sender as RoomTest);
                canvas2.Children.Remove(sender as RoomTest); //需要修改
                frame.Content = null;
                return;
            }

            // 判断鼠标是否在矩形边缘
            double left = Canvas.GetLeft(draggingRoom);
            double top = Canvas.GetTop(draggingRoom);
            double right = left + draggingRoom.Width;
            double bottom = top + draggingRoom.Height;
            double tolerance = 4;

            isLeftEdge = Math.Abs(startPoint.X - left) < tolerance;
            isRightEdge = Math.Abs(startPoint.X - right) < tolerance;
            isTopEdge = Math.Abs(startPoint.Y - top) < tolerance;
            isBottomEdge = Math.Abs(startPoint.Y - bottom) < tolerance;

            isResizing = isLeftEdge || isRightEdge || isTopEdge || isBottomEdge;

            if (!isResizing)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (startPoint.X > canvas2.MinWidth && startPoint.X < canvas2.MaxWidth
                        && startPoint.Y > canvas2.MinHeight && startPoint.Y < canvas2.MaxHeight)
                    {
                        frame.Content = new Controls() { DataContext = draggingRoom };
                        copy = false;
                    }
                    else
                    {
                        copy = true;
                    }

                    DragDrop.DoDragDrop(draggingRoom, draggingRoom, DragDropEffects.Copy);
                }
            }
            else
            {
                draggingRoom.CaptureMouse();

                oldHeight = draggingRoom.ActualHeight;
                oldWidth = draggingRoom.ActualWidth;
                oldLeft = Canvas.GetLeft(draggingRoom);
                oldTop = Canvas.GetTop(draggingRoom);

            }
            //e.Handled = true;
        }

        public void Rect_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (isResizing && e.LeftButton == MouseButtonState.Pressed)
            {

                Point currentPoint = e.GetPosition(canvas2);
                double deltaX = currentPoint.X - startPoint.X;
                double deltaY = currentPoint.Y - startPoint.Y;

                // 根据鼠标位置调整矩形大小
                if (isLeftEdge)
                {
                    double newWidth = draggingRoom.Width - deltaX;
                    if (newWidth > 0)
                    {
                        Canvas.SetLeft(draggingRoom, Canvas.GetLeft(draggingRoom) + deltaX);
                        draggingRoom.Width = newWidth;
                    }
                }
                else if (isRightEdge)
                {
                    double newWidth = draggingRoom.Width + deltaX;
                    if (newWidth > 0)
                    {
                        draggingRoom.Width = newWidth;
                    }
                }

                if (isTopEdge)
                {
                    double newHeight = draggingRoom.Height - deltaY;
                    if (newHeight > 0)
                    {
                        Canvas.SetTop(draggingRoom, Canvas.GetTop(draggingRoom) + deltaY);
                        draggingRoom.Height = newHeight;
                    }
                }
                else if (isBottomEdge)
                {
                    double newHeight = draggingRoom.Height + deltaY;
                    if (newHeight > 0)
                    {
                        draggingRoom.Height = newHeight;
                    }
                }

                startPoint = currentPoint; //更新当前的坐标位置

            }
        }

        public void Rect_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

            if (draggingRoom == null) return;
            var isEdge = new bool[4] { isLeftEdge, isRightEdge, isTopEdge, isBottomEdge };

            for (int i = 0; i < canvas2.Children.Count; i++)
            {
                if (canvas2.Children[i] is Axis)
                {
                    var axis = canvas2.Children[i] as Axis;
                    ToolKit.DragMagnet(draggingRoom, isEdge, axis.top, axis.bottom, axis.right, axis.left);
                }
                if (canvas2.Children[i] is RoomTest && canvas2.Children[i] != draggingRoom)
                {
                    var rect = canvas2.Children[i] as RoomTest;

                    var left = Canvas.GetLeft(rect);
                    var top = Canvas.GetTop(rect);
                    var right = left + rect.ActualWidth;
                    var bottom = top + rect.ActualHeight;

                    ToolKit.DragMagnet(draggingRoom, isEdge, top, bottom, right, left);
                }
            }

            isResizing = false;
            draggingRoom.ReleaseMouseCapture();
            draggingRoom.Interaction();

            var room = sender as RoomTest;

            var new_height = room.ActualHeight;
            var new_width = room.ActualWidth;
            var new_top = Canvas.GetTop(room);
            var new_left = Canvas.GetLeft(room);

            var old_Height = oldHeight;
            var old_Width = oldWidth;
            var old_Top = oldTop;
            var old_Left = oldLeft;

            History.Instance.Add(
                () =>
            {
                room.Height = old_Height;
                room.Width = old_Width;
                Canvas.SetLeft(room, old_Left);
                Canvas.SetTop(room, old_Top);
                room.Interaction();

            },
                () =>
            {
                room.Height = new_height;
                room.Width = new_width;
                Canvas.SetLeft(room, new_left);
                Canvas.SetTop(room, new_top);
                room.Interaction();
            });

        }

    }

}
