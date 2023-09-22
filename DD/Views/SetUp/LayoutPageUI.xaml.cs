using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DD.Views
{
    public partial class LayoutPage : Page
    {
        public void Rect_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                ToolKit.DeleteRoom(sender as RoomTest);
                canvas2.Children.Remove(sender as RoomTest);
                frame.Content = null;
                return;
            }

            startPoint = e.GetPosition(canvas2);
            var draggingRoomTest = sender as RoomTest;
            var rect = draggingRoomTest;


            // 判断鼠标是否在矩形边缘
            double left = Canvas.GetLeft(rect);
            double top = Canvas.GetTop(rect);
            double right = left + rect.Width;
            double bottom = top + rect.Height;
            double tolerance = 4;

            isLeftEdge = Math.Abs(startPoint.X - left) < tolerance;
            isRightEdge = Math.Abs(startPoint.X - right) < tolerance;
            isTopEdge = Math.Abs(startPoint.Y - top) < tolerance;
            isBottomEdge = Math.Abs(startPoint.Y - bottom) < tolerance;

            isResizing = isLeftEdge || isRightEdge || isTopEdge || isBottomEdge;

            if (!isResizing)
            {
                frame.Content = new Controls() { DataContext = rect };
                if (e.Source is RoomTest)
                {
                    draggingRoomTest = (RoomTest)e.Source;
                    Point position = e.GetPosition(canvas2);
                    if (position.X > canvas2.MinWidth && position.X < canvas2.MaxWidth
                        && position.Y > canvas2.MinHeight && position.Y < canvas2.MaxHeight)
                    {
                        copy = false;
                    }
                    else
                    {
                        copy = true;
                    }
                    try
                    {
                        DragDrop.DoDragDrop(draggingRoomTest, draggingRoomTest, DragDropEffects.Copy);
                    }
                    catch { }
                }
            }

            rect.CaptureMouse();
        }

        public void Rect_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var rect = (RoomTest)sender;
            if (isResizing && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(canvas2);

                double deltaX = currentPoint.X - startPoint.X;
                double deltaY = currentPoint.Y - startPoint.Y;

                // 根据鼠标位置调整矩形大小
                if (isLeftEdge)
                {
                    double newWidth = rect.Width - deltaX;
                    if (newWidth > 0)
                    {
                        Canvas.SetLeft(rect, Canvas.GetLeft(rect) + deltaX);
                        rect.Width = newWidth;
                    }
                }
                else if (isRightEdge)
                {
                    double newWidth = rect.Width + deltaX;
                    if (newWidth > 0)
                    {
                        rect.Width = newWidth;
                    }
                }

                if (isTopEdge)
                {
                    double newHeight = rect.Height - deltaY;
                    if (newHeight > 0)
                    {
                        Canvas.SetTop(rect, Canvas.GetTop(rect) + deltaY);
                        rect.Height = newHeight;
                    }
                }
                else if (isBottomEdge)
                {
                    double newHeight = rect.Height + deltaY;
                    if (newHeight > 0)
                    {
                        rect.Height = newHeight;
                    }
                }

                startPoint = currentPoint;
            }
            else
            {
                if (rect != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    Point position = e.GetPosition(canvas2);
                    Canvas.SetLeft(rect, MainWindow.Instance.x_canvas);
                    Canvas.SetTop(rect, MainWindow.Instance.y_canvas);
                }
            }

            //Serialization();
            //this.comp?.ExpireSolution(true);

        }

        public void Rect_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
            var rect = (RoomTest)sender;

            if (rect != null)
            {
                rect.Interaction();
            }
            rect.ReleaseMouseCapture();
        }
    }
}
