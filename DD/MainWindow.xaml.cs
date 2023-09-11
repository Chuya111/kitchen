using DD.Service;
using DD.Views;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; set; }
        //public GH_Component comp { get; set; }
        public List<Controlbar> ControlLines = new List<Controlbar>();
        public static string Output { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // 为画布2注册拖放事件
            canvas2.Drop += Canvas_Drop;
            canvas2.DragOver += Canvas_Move;
            canvas2.DragEnter += Canvas_DragEnter;

            Instance = this;
        }

        //public MainWindow(GH_Component component)
        //{
        //    InitializeComponent();

        //    // 为画布2注册拖放事件
        //    canvas2.Drop += Canvas_Drop;
        //    canvas2.DragOver += Canvas_Move;
        //    canvas2.DragEnter += Canvas_DragEnter;

        //    Instance = this;
        //    this.comp = component;
        //}



        public double x_shape, x_canvas, y_shape, y_canvas;
        double oldHeight, oldWidth, oldTop, oldLeft;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalArea();
        }
        public void CalArea()
        {
            Dictionary<string, Room> dict = new Dictionary<string, Room>();
            var canvas = canvas2;

            for (int i = 0; i < canvas.Children.Count; i++)
            {
                if (canvas.Children[i] is RoomTest)
                {
                    var rect = canvas.Children[i] as RoomTest;

                    if (!dict.Keys.Contains(rect.Text))
                    {
                        dict[rect.Text] = new Room()
                        {
                            RectWidth = rect.Text == "Balcony" ? rect.ActualWidth * rect.ActualHeight * 0.003 : rect.ActualWidth * rect.ActualHeight * 0.003,
                            Color = rect.Stroke.ToString(),
                            Area = rect.Text == "Balcony" ? rect.ActualWidth * rect.ActualHeight * 0.000625 : rect.ActualWidth * rect.ActualHeight * 0.000625,
                        };
                    }
                    else
                    {
                        dict[rect.Text].RectWidth += rect.Text == "Balcony" ? rect.ActualWidth * rect.ActualHeight * 0.003 : rect.ActualWidth * rect.ActualHeight * 0.003;
                        dict[rect.Text].Area += rect.Text == "Balcony" ? rect.ActualWidth * rect.ActualHeight * 0.000625 : rect.ActualWidth * rect.ActualHeight * 0.000625;
                    }

                }
            }

            var area = (dict.Keys.Select(x => dict[x].Area).ToList()).Sum();

            frame.Content = new Info(area)
            {
                DataContext = dict.Keys.Select(x => new
                {
                    Name = x,
                    Width = dict[x].RectWidth,
                    Color = dict[x].Color,
                    Area = dict[x].Area,
                }).ToList(),
            };
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            frame.Content = Controls.Instance;
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {

            RoomTest sourceRoomTest = (RoomTest)e.Data.GetData(typeof(RoomTest));
            var startPosition = e.GetPosition(canvas2);
            x_canvas = Canvas.GetLeft(sourceRoomTest);
            y_canvas = Canvas.GetTop(sourceRoomTest);
            x_shape = startPosition.X;
            y_shape = startPosition.Y;

            oldHeight = sourceRoomTest.ActualHeight;
            oldWidth = sourceRoomTest.ActualWidth;
            oldLeft = x_canvas;
            oldTop = y_canvas;

        }

        private void Canvas_Move(object sender, DragEventArgs e)
        {
            RoomTest sourceRoomTest = (RoomTest)e.Data.GetData(typeof(RoomTest));
            Point position = e.GetPosition(canvas2);
            if (!DesignPage.Instance.copy)
            {
                x_canvas += position.X - x_shape;
                y_canvas += position.Y - y_shape;

                Canvas.SetLeft(sourceRoomTest, x_canvas);
                x_shape = position.X;
                Canvas.SetTop(sourceRoomTest, y_canvas);
                y_shape = position.Y;
            }

            ToolKit.CheckFurnitures(sourceRoomTest);
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(RoomTest)))
            {
                RoomTest sourceRoomTest = (RoomTest)e.Data.GetData(typeof(RoomTest));
                Point position = e.GetPosition(canvas2);
                if (!DesignPage.Instance.copy)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        Canvas.SetLeft(sourceRoomTest, x_canvas);
                        Canvas.SetTop(sourceRoomTest, y_canvas);
                    }
                    else
                    {

                        for (int i = 0; i < canvas2.Children.Count; i++)
                        {
                            if (canvas2.Children[i] is RoomTest && canvas2.Children[i] != sourceRoomTest)
                            {
                                var rect = canvas2.Children[i] as RoomTest;
                                var left = Canvas.GetLeft(rect);
                                var top = Canvas.GetTop(rect);
                                var right = left + rect.ActualWidth;
                                var bottom = top + rect.ActualHeight;

                                ToolKit.MoveMagnet(sourceRoomTest, top, bottom, right, left, ref x_canvas, ref y_canvas);
                            }

                            if (canvas2.Children[i] is Axis)
                            {
                                var axis = canvas2.Children[i] as Axis;
                                ToolKit.MoveMagnet(sourceRoomTest, axis.top, axis.bottom, axis.right, axis.left, ref x_canvas, ref y_canvas);
                            }
                        }
                        Canvas.SetLeft(sourceRoomTest, x_canvas);
                        Canvas.SetTop(sourceRoomTest, y_canvas);
                    }


                    var new_height = sourceRoomTest.ActualHeight;
                    var new_width = sourceRoomTest.ActualWidth;
                    var new_top = Canvas.GetTop(sourceRoomTest);
                    var new_left = Canvas.GetLeft(sourceRoomTest);

                    var old_Height = oldHeight;
                    var old_Width = oldWidth;
                    var old_Top = oldTop;
                    var old_Left = oldLeft;

                    History.Instance.Add(
                        () =>
                        {
                            sourceRoomTest.Height = old_Height;
                            sourceRoomTest.Width = old_Width;
                            Canvas.SetLeft(sourceRoomTest, old_Left);
                            Canvas.SetTop(sourceRoomTest, old_Top);
                            sourceRoomTest.Interaction();

                        },
                        () =>
                        {
                            sourceRoomTest.Height = new_height;
                            sourceRoomTest.Width = new_width;
                            Canvas.SetLeft(sourceRoomTest, new_left);
                            Canvas.SetTop(sourceRoomTest, new_top);
                            sourceRoomTest.Interaction();
                        });

                    sourceRoomTest.Interaction();
                }
                else
                {
                    RoomTest newRoomTest = new RoomTest()
                    {
                        Width = sourceRoomTest.Width,
                        Height = sourceRoomTest.Height,
                        Fill = sourceRoomTest.Fill,
                        Name = sourceRoomTest.Name,
                        Stroke = sourceRoomTest.Stroke,
                        StrokeThickness = 8,

                        Text = sourceRoomTest.Text,
                        Foreground = sourceRoomTest.Foreground,
                    };

                    Canvas.SetLeft(newRoomTest, position.X - newRoomTest.Width / 2);
                    Canvas.SetTop(newRoomTest, position.Y - newRoomTest.Height / 2);


                    newRoomTest.PreviewMouseMove += DesignPage.Instance.Rect_PreviewMouseMove;
                    newRoomTest.PreviewMouseDown += DesignPage.Instance.Rect_PreviewMouseDown;
                    newRoomTest.PreviewMouseUp += DesignPage.Instance.Rect_PreviewMouseUp;

                    canvas2.Children.Add(newRoomTest);
                    sourceRoomTest.Interaction();

                }
            }

            ToolKit.Output();

        }

        private void Setup_Click(object sender, RoutedEventArgs e)
        {
            //var button = sender as Button;
            Config.Background = new SolidColorBrush(Color.FromArgb(255, 66, 112, 173));
            Rooms.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
            Layouts.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));

            Setup.Content = ConfigPage.Instance ?? new ConfigPage() { DataContext = axis };
        }
        private void Rooms_Click(object sender, RoutedEventArgs e)
        {
            //var button = sender as Button;
            Rooms.Background = new SolidColorBrush(Color.FromArgb(255, 66, 112, 173));
            Config.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
            Layouts.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
            Setup.Content = new DesignPage();

            //btSave.Visibility = Visibility.Visible;
        }
        private void Layouts_Click(object sender, RoutedEventArgs e)
        {
            //var button = sender as Button;
            Layouts.Background = new SolidColorBrush(Color.FromArgb(255, 66, 112, 173));
            Rooms.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
            Config.Background = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
            Setup.Content = new LayoutPage();

            //btSave.Visibility=Visibility.Collapsed;
        }

        private void Generate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("scs");
            ToolKit.Serialization();
        }

        public void Clear(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < canvas2.Children.Count; i++)
            {
                if (!(canvas2.Children[i] is Axis))
                {
                    canvas2.Children.RemoveAt(i);
                    i--;
                }
            }

            frame.Content = null;
            History.Instance.ClearHistory();
        }

        private void axis_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double radius = 20;

            axis.top_thumb.Width = radius;
            axis.top_thumb.Height = radius;
            Canvas.SetTop(axis.top_thumb, axis.Center.Y - axis.ActualHeight * 0.5 - radius * 0.5);
            Canvas.SetLeft(axis.top_thumb, axis.Center.X - axis.ActualWidth * 0.5 - radius * 3.5);

            axis.right_thumb.Width = radius;
            axis.right_thumb.Height = radius;
            Canvas.SetTop(axis.right_thumb, axis.Center.Y + axis.ActualHeight * 0.5 + radius * 2.5);
            Canvas.SetLeft(axis.right_thumb, axis.Center.X + axis.ActualWidth * 0.5 - radius * 0.5);

            axis.bottom_thumb.Width = radius;
            axis.bottom_thumb.Height = radius;
            Canvas.SetTop(axis.bottom_thumb, axis.Center.Y + axis.ActualHeight * 0.5 - radius * 0.5);
            Canvas.SetLeft(axis.bottom_thumb, axis.Center.X - axis.ActualWidth * 0.5 - radius * 3.5);

            axis.left_thumb.Width = radius;
            axis.left_thumb.Height = radius;
            Canvas.SetTop(axis.left_thumb, axis.Center.Y + axis.ActualHeight * 0.5 + radius * 2.5);
            Canvas.SetLeft(axis.left_thumb, axis.Center.X - axis.ActualWidth * 0.5 - radius * 0.5);

        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            History.Instance.Undo();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            History.Instance.Redo();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Savelayout();
        }
        /// 当前画板上面有没有存在房间
        public bool IsHasRoom()
        {
            var r = false;
            for (int i = 0; i < canvas2.Children.Count; i++)
            {
                if ((canvas2.Children[i] is RoomTest))
                {
                    r = true;
                    break;
                }
            }
            return r;
        }
        void Savelayout()
        {

            var modules = canvas2.Children;
            if (!IsHasRoom())
            {
                MessageBox.Show("每个户型至少有一个方块组件");
                return;
            }

            var dbs = new DataBaseServices();
            MySqlDataReader maxer_reader = dbs.DataQuery("select max(type) from layout_storage");
            var parentID = 1;
            if (maxer_reader.Read())
            {
                int.TryParse(maxer_reader.GetValue(0).ToString(), out parentID);
                parentID++;
            }
            var imgFileName = $"screenshot_{parentID}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png";
            var imgPath = System.IO.Path.Combine(Service.Config.LayoutImageSavePath, imgFileName);
            Capture.SaveBitmap(canvas2 as Canvas, imgPath);

            var layout_name = "户型" + parentID;
            var axisWidth = axis.Width;
            var axisHeight = axis.Height;
            var sql = $"insert into layout_storage (type,layout_name,axisWidth,axisHeight,image_path) values({parentID},'{layout_name}',{axisWidth},{axisHeight},'{imgFileName}')";
            dbs.DataProcess(sql);
            var wallSql = $"insert into layout_storage_module_wall (id,parentID,parentModuleID,rightOffset,leftOffset,visible,position,lineLength,strokeThickness) values";
            var windowSql = $"insert into layout_storage_module_window (parentID,parentModuleID,parentModuleWallID,lineLength,winStyle,position,LeftOffset,RightOffset,Visible,StrokeThickness) values";
            var doorSql = $"insert into layout_storage_module_door (parentID,parentModuleID,parentModuleWallID,IsMainDoor,IsInside,IsLeft,RightOffset,LeftOffset,Visible,position,DoorSize,StrokeThickness,IsStartPosition) values";

            maxer_reader = dbs.DataQuery($"select max(id) from layout_storage_module_wall");
            var parentModuleWallID = 0;
            if (maxer_reader.Read())
                int.TryParse(maxer_reader.GetValue(0).ToString(), out parentModuleWallID);
            foreach (var item in modules)
            {
                var roomObj = item as RoomTest;
                if (roomObj == null)
                    continue;
                var ui = (UIElement)item;
                var rr = VisualTreeHelper.GetChildrenCount(roomObj);
                sql = $"insert into layout_storage_module (parentID,width,length,color,room,locationX,locationY)";
                sql += $" values({parentID},{roomObj.Width},{roomObj.Height},'{roomObj.Stroke.ToString()}','{roomObj.Text}',{Canvas.GetLeft(ui)},{Canvas.GetTop(ui)})";
                dbs.DataProcess(sql);

                maxer_reader = dbs.DataQuery($"select max(id) from layout_storage_module where parentID={parentID}");
                var parentModuleID = 1;
                if (maxer_reader.Read())
                    int.TryParse(maxer_reader.GetValue(0).ToString(), out parentModuleID);

                #region 保存wall
                var rect_grid = VisualTreeHelper.GetChild(roomObj, 0) as Grid;
                for (int j = 0; j < rect_grid.Children.Count; j++)
                {
                    if (rect_grid.Children[j] is Wall wall)
                    {
                        parentModuleWallID++;
                        wallSql += $" ({parentModuleWallID},{parentID},{parentModuleID},{wall.RightOffset},{wall.LeftOffset},{wall.Visible},{(int)wall.LinePosition},{wall.LineLength},{wall.StrokeThickness}),";

                        var gWall = VisualTreeHelper.GetChild(wall, 0) as Grid;
                        for (int w = 0; w < gWall.Children.Count; w++)
                        {
                            if (gWall.Children[w] is Win _win)
                            {
                                if (_win.Visible)
                                    windowSql += $" ({parentID},{parentModuleID},{parentModuleWallID},{_win.LineLength},{_win.WinStyle},{(int)wall.LinePosition},{_win.LeftOffset},{_win.RightOffset},{_win.Visible},{_win.StrokeThickness}),";
                            }
                            else if (gWall.Children[w] is Door _door)
                            {
                                if (_door.Visible)
                                    doorSql += $" ({parentID},{parentModuleID},{parentModuleWallID},{_door.IsMainDoor},{_door.IsInside},{_door.IsLeft},{_door.RightOffset},{_door.LeftOffset},{_door.Visible},{(int)wall.LinePosition},{_door.DoorSize},{_door.StrokeThickness},{_door.IsStartPosition}),";
                            }
                        }
                    }

                }

                #endregion
            }

            if (!wallSql.EndsWith("values"))
                dbs.DataProcess(wallSql.TrimEnd(','));
            if (!windowSql.EndsWith("values"))
                dbs.DataProcess(windowSql.TrimEnd(','));

            if (!doorSql.EndsWith("values"))
                dbs.DataProcess(doorSql.TrimEnd(','));
            MessageBox.Show("户型保存成功！");

        }
    }
}
