using DD.Data;
using DD.Model;
using DD.Service;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DD.Views
{
    /// <summary>
    /// LayoutPage.xaml 的交互逻辑
    /// </summary>
    public partial class LayoutPage : Page
    {
        public int curSelectedLayerID = 0;
        public static LayoutPage Instance { get; set; }
        public LayoutPage()
        {
            InitializeComponent();
            Initialization();
            Instance = this;
        }
        public DataBaseServices dbs = new DataBaseServices();
        public bool copy;
        Canvas canvas2 = MainWindow.Instance?.canvas2;
        Frame frame = MainWindow.Instance?.frame;
        //RoomTest draggingRoomTest;

        Point startPoint;
        bool isResizing, isLeftEdge, isRightEdge, isTopEdge, isBottomEdge;

        public void Initialization()
        {
            canvas2.Visibility = Visibility.Visible;
            frame.Content = null;
            var value_list = new List<LayOutObj>();
            #region 加载户型
            string sql = "Select * from layout_storage order by type";
            MySqlDataReader value_reader = dbs.DataQuery(sql);
            while (value_reader.Read())
            {
                int.TryParse(value_reader.GetValue(0).ToString(), out int type);
                var layoutObj = new LayOutObj()
                {
                    Name = value_reader.GetValue(1).ToString(),
                    //Height = 100,
                    //Width = 100,
                    AxisHeight = double.Parse(value_reader["axisHeight"].ToString()),
                    AxisWidth = double.Parse(value_reader["axisWidth"].ToString()),
                    //Color = "#999999",
                    Id = type,
                    Modules = new List<MyModule>(),
                    ImagePath = System.IO.Path.Combine(Config.LayoutImageSavePath, value_reader["image_path"].ToString()),

                };
                //MessageBox.Show(layoutObj.ImagePath);
                value_list.Add(layoutObj);
            }

            this.DataContext = value_list;
            Debug.WriteLine("load layout");

            #endregion
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ClearCanvas
            // 获取按钮所关联的 layoutObj
            Button button = (Button)sender;
            LayOutObj layoutObj = (LayOutObj)button.DataContext;

            // 调用原来的逻辑
            LayoutMouseDown(layoutObj, null);
        }

        /// 选择一个户型
        public void LayoutMouseDown(object sender, MouseButtonEventArgs e)
        {
            var layoutObj = (LayOutObj)sender;
            if (layoutObj == null)
                return;

            curSelectedLayerID = layoutObj.Id;
            frame.Content = null;
            ChearOtherLayoutModules(layoutObj.Id);
            SetAxis(layoutObj);
            #region 加载本户型房间
            if (layoutObj.Modules.Count == 0)
            {
                var parentID = layoutObj.Id;

                #region 加载本户型下所有窗户
                List<WindowDTO> lstWindow = new List<WindowDTO>();
                string sqlWindow = $"Select * from layout_storage_module_window where parentID={parentID}";
                MySqlDataReader win_reader = dbs.DataQuery(sqlWindow);
                while (win_reader.Read())
                {
                    var _window = new WindowDTO()
                    {
                        parentModuleID = int.Parse(win_reader["parentModuleID"].ToString()),
                        parentModuleWallID = int.Parse(win_reader["parentModuleWallID"].ToString()),
                        lineLength = int.Parse(win_reader["lineLength"].ToString()),
                        winStyle = int.Parse(win_reader["winStyle"].ToString()),
                        position = int.Parse(win_reader["position"].ToString()),
                        leftOffset = int.Parse(win_reader["leftOffset"].ToString()),
                        rightOffset = int.Parse(win_reader["rightOffset"].ToString()),
                        strokeThickness = double.Parse(win_reader["strokeThickness"].ToString()),
                        visible = win_reader["visible"].ToString() == "1"

                    };
                    lstWindow.Add(_window);
                }

                #endregion


                #region 加载本户型下所有门
                List<DoorDTO> lstDoor = new List<DoorDTO>();
                string sqlDoor = $"Select * from layout_storage_module_door where parentID={parentID}";
                MySqlDataReader door_reader = dbs.DataQuery(sqlDoor);
                while (door_reader.Read())
                {
                    var _door = new DoorDTO()
                    {
                        parentModuleID = int.Parse(door_reader["parentModuleID"].ToString()),
                        parentModuleWallID = int.Parse(door_reader["parentModuleWallID"].ToString()),
                        isMainDoor = door_reader["isMainDoor"].ToString() == "1",
                        isInside = door_reader["isInside"].ToString() == "1",
                        isLeft = door_reader["isLeft"].ToString() == "1",
                        leftOffset = int.Parse(door_reader["leftOffset"].ToString()),
                        rightOffset = int.Parse(door_reader["rightOffset"].ToString()),
                        visible = door_reader["visible"].ToString() == "1",
                        position = int.Parse(door_reader["position"].ToString()),
                        doorSize = int.Parse(door_reader["doorSize"].ToString()),

                        strokeThickness = double.Parse(door_reader["strokeThickness"].ToString()),
                        isStartPosition = door_reader["isStartPosition"].ToString() == "1",


                    };
                    lstDoor.Add(_door);
                }

                #endregion


                #region 加载本户型下所有墙
                List<WallDTO> lstWall = new List<WallDTO>();
                string sqlWall = $"Select * from layout_storage_module_wall where parentID={parentID}";
                MySqlDataReader wall_reader = dbs.DataQuery(sqlWall);
                while (wall_reader.Read())
                {
                    var _wall = new WallDTO()
                    {
                        parentModuleID = int.Parse(wall_reader["parentModuleID"].ToString()),
                        rightOffset = int.Parse(wall_reader["rightOffset"].ToString()),
                        leftOffset = int.Parse(wall_reader["leftOffset"].ToString()),
                        visible = wall_reader["visible"].ToString() == "1",
                        position = int.Parse(wall_reader["position"].ToString()),
                        lineLength = double.Parse(wall_reader["lineLength"].ToString()),
                        strokeThickness = double.Parse(wall_reader["strokeThickness"].ToString()),
                    };
                    var wallID = int.Parse(wall_reader["id"].ToString());
                    _wall.lstWindows = lstWindow.Where(a => a.parentModuleWallID.Equals(wallID)).ToList();
                    _wall.lstDoors = lstDoor.Where(a => a.parentModuleWallID.Equals(wallID)).ToList();
                    lstWall.Add(_wall);
                }

                #endregion


                string sqlModule = $"Select * from layout_storage_module where parentID={parentID} order by id";
                MySqlDataReader module_reader = dbs.DataQuery(sqlModule);
                while (module_reader.Read())
                {
                    var mym = new MyModule()
                    {
                        Name = module_reader["room"].ToString(),
                        Height = int.Parse(module_reader["length"].ToString()),
                        Width = int.Parse(module_reader["width"].ToString()),
                        Color = module_reader["color"].ToString(),
                        X_coordinate = double.Parse(module_reader["locationX"].ToString()),
                        Y_coordinate = double.Parse(module_reader["locationY"].ToString()),
                        Walls = null,
                        Id = int.Parse(module_reader["id"].ToString()),
                    };


                    mym.Walls = lstWall.Where(a => a.parentModuleID.Equals(mym.Id)).ToList();

                    layoutObj.Modules.Add(mym);
                }
            }
            #endregion

            #region 添加进画布
            foreach (var item in layoutObj.Modules)
            {
                Color color = (Color)ColorConverter.ConvertFromString(item.Color);
                Brush brush = new SolidColorBrush(color);
                RoomTest rt = new RoomTest()
                {
                    Width = item.Width,
                    Height = item.Height,
                    Text = item.Name,
                    Stroke = brush,
                    StrokeThickness = 8,
                    Foreground = brush,
                    Name = item.Name,
                    LayoutTypeID = layoutObj.Id,
                    LayoutModuleID = item.Id
                };

                rt.DataContext = item;
                canvas2.Children.Add(rt);

                Canvas.SetLeft(rt, item.X_coordinate);
                Canvas.SetTop(rt, item.Y_coordinate);

                rt.MouseDown += Rect_PreviewMouseLeftButtonDown;
                rt.MouseMove += Rect_PreviewMouseMove;
                rt.MouseUp += Rect_PreviewMouseUp;
                rt.Loaded += Rt_Loaded;
            }
            #endregion
        }

        /// 户型加载完毕渲染墙面
        private void Rt_Loaded(object sender, RoutedEventArgs e)
        {
            var roomObj = (RoomTest)sender;
            if (roomObj == null)
                return;
            var _myModule = (MyModule)roomObj.DataContext;
            if (_myModule == null)
                return;

            var rectGrid = VisualTreeHelper.GetChild(roomObj, 0) as Grid;
            for (int j = 0; j < rectGrid.Children.Count; j++)
            {
                if (rectGrid.Children[j] is Wall wall)
                {
                    var wallInfo = _myModule.Walls.FirstOrDefault(a => a.position == (int)wall.LinePosition);
                    if (wallInfo == null)
                        continue;
                    wall.LineLength = wallInfo.lineLength;
                    wall.StrokeThickness = wallInfo.strokeThickness;
                    wall.LeftOffset = wallInfo.leftOffset;
                    wall.RightOffset = wallInfo.rightOffset;
                    wall.Visible = wallInfo.visible;
                    var gWall = VisualTreeHelper.GetChild(wall, 0) as Grid;
                    for (int w = 0; w < gWall.Children.Count; w++)
                    {
                        if (gWall.Children[w] is Win _win)
                        {
                            var winInfo = wallInfo.lstWindows.FirstOrDefault(a => a.position.Equals((int)_win.LinePosition));
                            if (winInfo == null)
                                continue;

                            _win.Visible = winInfo.visible;
                            _win.LineLength = winInfo.lineLength;
                            _win.LeftOffset = winInfo.leftOffset;
                            _win.RightOffset = winInfo.rightOffset;
                            _win.WinStyle = winInfo.winStyle;
                            _win.StrokeThickness = winInfo.strokeThickness;
                        }
                        else if (gWall.Children[w] is Door _door)
                        {
                            var doorInfo = wallInfo.lstDoors.FirstOrDefault(a => a.position.Equals((int)_door.LinePosition) && a.isStartPosition.Equals(_door.IsStartPosition));
                            if (doorInfo == null)
                                continue;
                            if (doorInfo.visible)
                                Debug.WriteLine("true");
                            _door.IsMainDoor = doorInfo.isMainDoor;
                            _door.IsInside = doorInfo.isInside;
                            _door.IsLeft = doorInfo.isLeft;
                            _door.RightOffset = doorInfo.rightOffset;
                            _door.LeftOffset = doorInfo.leftOffset;
                            _door.Visible = doorInfo.visible;
                            _door.DoorSize = doorInfo.doorSize;
                            _door.StrokeThickness = doorInfo.strokeThickness;
                            _door.IsStartPosition = doorInfo.isStartPosition;

                        }

                    }

                }
            }

            ToolKit.CheckControlLine(roomObj);
        }

        /// 切换户型时，清除上一个户型内容及吸附 ControlLine
        void ChearOtherLayoutModules(int nowLayoutID)
        {
            for (int i = 0; i < canvas2.Children.Count; i++)
            {
                if (canvas2.Children[i] is RoomTest rt)
                {
                    if (rt != null && rt.LayoutTypeID > 0 && rt.LayoutTypeID != nowLayoutID)
                    {
                        canvas2.Children.RemoveAt(i);
                        ToolKit.DeleteRoom(rt);
                        i--;
                    }
                }
            }
        }

        /// 显示指定户型的axis
        void SetAxis(LayOutObj layout)
        {
            //当前画板如果有房间则不加载本户型的axis
            if (MainWindow.Instance.IsHasRoom())
                return;
            if (layout.AxisWidth > 0)
                MainWindow.Instance.axis.Width = layout.AxisWidth;
            if (layout.AxisHeight > 0)
                MainWindow.Instance.axis.Height = layout.AxisHeight;

        }
    }
}

