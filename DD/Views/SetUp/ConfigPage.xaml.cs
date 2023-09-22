using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DD.Views
{
    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page
    {
        public static ConfigPage Instance { get; set; }
        public ConfigPage()
        {
            InitializeComponent();
            Instance = this;
        }

        private void GenerateDefault(object sender, RoutedEventArgs e)
        {


            var moduleType = module.SelectedIndex;
            var bedType = bed.SelectedIndex;
            var toiletType = toilet.SelectedIndex;

            if (moduleType == 0 && bedType == 0 && toiletType == 0)
            {
                MainWindow.Instance.Clear();
                MainWindow.Instance.axis.Height = 312;
                MainWindow.Instance.axis.Width = 168;
                MainWindow.Instance.axis.Center = new Point(250, 250);

                GenerateRoom(166, 94, 168, 40, "Balcony", "#FFFFC300", true, 100, -1, false, -1, -1, false, -1, 0, false, -1, -1);
                GenerateRoom(166, 134, 168, 100, "Bedroom", "#FF4270AD", true, 100, -1, false, -1, -1, false, -1, 0, false, -1, -1);
                GenerateRoom(166, 234, 168, 60, "LivingRoom", "#FF4AAD4B", false, -1, 0, false, -1, -1, false, -1, 0, false, -1, -1);
                GenerateRoom(238, 294, 96, 112, "Kitchen", "#FF359A80", false, -1, 0, false, -1, -1, true, 48, -1, false, -1, 0);
                GenerateRoom(166, 294, 72, 112, "Restroom", "#FF65CED6", false, -1, -1, true, 48, -1, false, -1, -1, false, -1, -1);

            }
            else if (moduleType == 0 && bedType == 2 && toiletType == 1)
            {
                MainWindow.Instance.Clear();
                MainWindow.Instance.axis.Height = 320;
                MainWindow.Instance.axis.Width = 192;
                MainWindow.Instance.axis.Center = new Point(250, 250);

                GenerateRoom(214, 90, 132, 60, "Balcony", "#FFFFC300", true, 90, -1, false, -1, -1, false, -1, 0, false, -1, -1);
                GenerateRoom(154, 270, 60, 50, "Restroom", "#FF65CED6", false, -1, 0, false, -1, 0, false, -1, 0, false, -1, -1);
                GenerateRoom(154, 150, 60, 120, "Kitchen", "#FF359A80", true, 40, -1, false, -1, 0, true, -1, -1, false, -1, -1);
                GenerateRoom(214, 270, 132, 142, "LivingRoom", "#FF4AAD4B", false, -1, 0, false, -1, -1, true, -1, -1, false, -1, 0);
                GenerateRoom(154, 320, 60, 92, "Restroom", "#FF65CED6", true, -1, -1, false, -1, -1, false, -1, -1, false, -1, -1);
                GenerateRoom(214, 150, 132, 120, "Bedroom", "#FF4270AD", true, 90, -1, false, -1, -1, true, -1, -1, false, -1, -1);

            }
            else if (moduleType == 2 && bedType == 2 && toiletType == 0)
            {
                MainWindow.Instance.Clear();
                MainWindow.Instance.axis.Height = 264;
                MainWindow.Instance.axis.Width = 352;
                MainWindow.Instance.axis.Center = new Point(250, 250);

                GenerateRoom(380, 338, 46, 120, "Porch", "#FF65CED6", false, -1, 0, false, -1, -1, true, -1, -1, false, -1, 0);
                GenerateRoom(302, 68, 124, 50, "Balcony", "#FFFFC300", true, 90, -1, false, -1, -1, false, -1, 0, false, -1, -1);
                GenerateRoom(302, 118, 124, 220, "LivingRoom", "#FF4AAD4B", true, 80, -1, false, -1, -1, false, -1, 0, false, -1, 0);
                GenerateRoom(194, 230, 108, 48, "LivingRoom", "#FF4AAD4B", false, -1, 0, false, -1, 0, false, -1, 0, false, -1, 0);
                GenerateRoom(74, 118, 120, 160, "Bedroom", "#FF4270AD", true, 50, -1, false, -1, 126, false, -1, 0, false, -1, -1);
                GenerateRoom(194, 118, 108, 112, "Bedroom", "#FF4270AD", true, 50, -1, false, -1, -1, false, -1, -1, false, -1, -1);
                GenerateRoom(138, 278, 100, 104, "Study", "#FFFF9800", false, -1, 60, false, -1, -1, false, -1, -1, false, -1, -1);
                GenerateRoom(74, 278, 64, 104, "Restroom", "#FF65CED6", true, 30, -1, false, -1, -1, true, 30, -1, false, -1, -1);
                GenerateRoom(238, 278, 64, 104, "Restroom", "#FF65CED6", true, 30, -1, false, -1, 64, true, 30, 48, false, -1, -1);
                GenerateRoom(288, 338, 92, 120, "Kitchen", "#FF359A80", false, -1, -1, true, 80, -1, false, -1, -1, false, -1, -1);

            }
            else
            {
                MessageBox.Show("户型库暂无此类户型", "提示");
            }

        }
        private void GenerateRoom(
            int xCoor, int yCoor, int width, int height,
            string name, string color,
            bool topWin, int topWinLength, int topWallLength,
            bool rightWin, int rightWinLength, int rightWallLength,
            bool bottomWin, int bottomWinLength, int bottomWallLength,
            bool leftWin, int leftWinLength, int leftWallLength)
        {
            RoomTest newRoomTest = new RoomTest()
            {
                Width = width,
                Height = height,
                Stroke = (Brush)new BrushConverter().ConvertFromString(color),
                StrokeThickness = 8,
                Text = name,
                Foreground = (Brush)new BrushConverter().ConvertFromString(color),

            };

            Canvas.SetLeft(newRoomTest, xCoor);
            Canvas.SetTop(newRoomTest, yCoor);

            newRoomTest.PreviewMouseMove += DesignPage.Instance.Rect_PreviewMouseMove;
            newRoomTest.PreviewMouseDown += DesignPage.Instance.Rect_PreviewMouseDown;
            newRoomTest.PreviewMouseUp += DesignPage.Instance.Rect_PreviewMouseUp;

            MainWindow.Instance.canvas2.Children.Add(newRoomTest);
            MainWindow.Instance.UpdateLayout();

            var count = VisualTreeHelper.GetChildrenCount(newRoomTest);
            var rect_grid = VisualTreeHelper.GetChild(newRoomTest, 0) as Grid;

            for (int j = 0; j < rect_grid.Children.Count; j++)
            {
                if (rect_grid.Children[j] is Wall)
                {
                    Wall wall = (Wall)rect_grid.Children[j];
                    Win win;
                    switch (wall.LinePosition)
                    {
                        case LinePosition.Top:
                            win = wall.Template.FindName("window", wall) as Win;
                            win.Visible = topWin;
                            if (topWinLength != -1) win.LineLength = topWinLength;
                            if (topWallLength != -1) wall.LineLength = topWallLength;
                            if (topWallLength == 0) wall.Visible = false;
                            break;
                        case LinePosition.Right:
                            win = wall.Template.FindName("window", wall) as Win;
                            win.Visible = rightWin;
                            if (rightWinLength != -1) win.LineLength = rightWinLength;
                            if (rightWallLength != -1) wall.LineLength = rightWallLength;
                            if (rightWallLength == 0) wall.Visible = false;
                            break;
                        case LinePosition.Bottom:
                            win = wall.Template.FindName("window", wall) as Win;
                            win.Visible = bottomWin;
                            if (bottomWinLength != -1) win.LineLength = bottomWinLength;
                            if (bottomWallLength != -1) wall.LineLength = bottomWallLength;
                            if (bottomWallLength == 0) wall.Visible = false;
                            break;
                        case LinePosition.Left:
                            win = wall.Template.FindName("window", wall) as Win;
                            win.Visible = leftWin;
                            if (leftWinLength != -1) win.LineLength = leftWinLength;
                            if (leftWallLength != -1) wall.LineLength = leftWallLength;
                            if (leftWallLength == 0) wall.Visible = false;
                            break;
                    }
                }
            }

            newRoomTest.Interaction();

        }


        private void width_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isDragged)
            {
                var delta = (e.NewValue - e.OldValue) * 0.5;
                bool leftCheck = MainWindow.Instance.axis.CheckLeft(delta);
                bool rightCheck = MainWindow.Instance.axis.CheckRight(-1 * delta);
                if (!leftCheck || !rightCheck)
                {
                    var slider = sender as Slider;
                    slider.Value = e.OldValue;
                }

                MainWindow.Instance.axis.leftRooms.ForEach(x => x.Width += delta);
                MainWindow.Instance.axis.righRrooms.ForEach(x => Canvas.SetLeft(x, Canvas.GetLeft(x) - delta));
                MainWindow.Instance.axis.righRrooms.ForEach(x => x.Width += delta);

                MainWindow.Instance.CalArea();
                e.Handled = true;
            }

        }
        private void height_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isDragged)
            {

                var delta = (e.NewValue - e.OldValue) * 0.5; ;
                bool topCheck = MainWindow.Instance.axis.CheckTop(delta);
                bool bottomCheck = MainWindow.Instance.axis.CheckBottom(-1 * delta);

                if (!topCheck || !bottomCheck)
                {
                    var slider = sender as Slider;
                    slider.Value = e.OldValue;
                }

                MainWindow.Instance.axis.topRooms.ForEach(x => x.Height += delta);
                MainWindow.Instance.axis.bottomRooms.ForEach(x => Canvas.SetTop(x, Canvas.GetTop(x) - delta));
                MainWindow.Instance.axis.bottomRooms.ForEach(x => x.Height += delta);

                MainWindow.Instance.CalArea();
                e.Handled = true;
            }
        }


        private bool isDragged = false;
        private void DragStarted(object sender, DragStartedEventArgs e) => isDragged = true;
        private void DragCompleted(object sender, DragCompletedEventArgs e) => isDragged = false;

    }
}


