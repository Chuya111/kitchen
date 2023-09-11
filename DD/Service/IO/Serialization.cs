using DD.Data;
using DD.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DD
{
    public partial class ToolKit
    {
        private static void DoorSerialization(MyWall myWall, Wall wall, int wall_index, ref int window_index, ref int door_index,
           double x_coordinate, double y_coordinate)
        {
            myWall.Doors = new List<MyDoor>();


            var wall_grid = VisualTreeHelper.GetChild(wall, 0) as Grid;
            for (int i = 0; i < wall_grid.Children.Count; i++)
            {
                if (wall_grid.Children[i] is Win)
                {
                    var win = wall_grid.Children[i] as Win;
                    if (win.Visible)
                    {
                        MyWindow myWin = new MyWindow()
                        {
                            id = window_index,
                            Host = wall_index,
                            Length = win.LineLength,
                            X_coordinate = win.X_Coordinate + x_coordinate,
                            Y_coordinate = win.Y_Coordinate + y_coordinate,
                        };

                        myWall.Win = myWin;
                        window_index++;
                    }
                }

                if (wall_grid.Children[i] is Door)
                {
                    var door = wall_grid.Children[i] as Door;
                    if (door.Visible)
                    {
                        MyDoor myDoor = new MyDoor()
                        {
                            id = door_index,
                            Host = wall_index,
                            Size = door.DoorSize,
                            X_coordinate = door.X_Coordinate + x_coordinate,
                            Y_coordinate = door.Y_Coordinate + y_coordinate,
                        };

                        myWall.Doors.Add(myDoor);
                        door_index++;
                    }
                }
            }
        }

        public static void Serialization()
        {
            var rooms = new List<MyRoom>();
            var walls = new List<MyWall>();
            //var windows = new List<MyWindow>();
            //var doors = new List<MyDoor>();

            int wall_index = 0;
            int window_index = 0;
            int door_index = 0;

            for (int i = 0; i < MainWindow.Instance.canvas2.Children.Count; i++)
            {
                if (MainWindow.Instance.canvas2.Children[i] is RoomTest)
                {
                    var temp_walls = new List<MyWall>();
                    var rect = MainWindow.Instance.canvas2.Children[i] as RoomTest;

                    var x_coordinate = Canvas.GetLeft(rect);
                    var y_coordinate = Canvas.GetTop(rect);
                    var width = rect.ActualWidth;
                    var height = rect.ActualHeight;

                    try
                    {
                        var rect_grid = VisualTreeHelper.GetChild(rect, 0) as Grid;

                        for (int j = 0; j < rect_grid.Children.Count; j++)
                        {
                            if (rect_grid.Children[j] is Wall)
                            {
                                Wall wall = rect_grid.Children[j] as Wall;
                                double startx = 0, starty = 0, endx = 0, endy = 0;
                                if (wall.Visible)
                                {
                                    switch (wall.LinePosition)
                                    {
                                        case LinePosition.Top:
                                            startx = x_coordinate + width - wall.RightOffset;
                                            starty = y_coordinate;
                                            endx = x_coordinate + wall.LeftOffset;
                                            endy = y_coordinate;
                                            break;
                                        case LinePosition.Right:
                                            startx = x_coordinate + width;
                                            starty = y_coordinate + wall.LeftOffset;
                                            endx = x_coordinate + width;
                                            endy = y_coordinate + height - wall.RightOffset;
                                            break;
                                        case LinePosition.Bottom:
                                            startx = x_coordinate + wall.RightOffset;
                                            starty = y_coordinate + height;
                                            endx = x_coordinate + width - wall.RightOffset;
                                            endy = y_coordinate + height;
                                            break;
                                        case LinePosition.Left:
                                            startx = x_coordinate;
                                            starty = y_coordinate + wall.RightOffset;
                                            endx = x_coordinate;
                                            endy = y_coordinate + height - wall.LeftOffset;
                                            break;
                                    }
                                    MyWall myWall = new MyWall()
                                    {
                                        id = wall_index,
                                        Host = i,
                                        Start_x = startx,
                                        Start_y = starty,
                                        End_x = endx,
                                        End_y = endy,
                                        Width = wall.StrokeThickness,
                                    };
                                    temp_walls.Add(myWall);
                                    DoorSerialization(myWall, wall, wall_index, ref window_index, ref door_index, x_coordinate, y_coordinate);
                                    wall_index++;
                                }
                            }

                        }
                        MyRoom room = new MyRoom()
                        {
                            Id = i,
                            Name = rect.Text,
                            X_coordinate = x_coordinate,
                            Y_coordinate = y_coordinate,
                            Width = rect.Width,
                            Height = rect.Height,
                            Walls = temp_walls,
                        };
                        walls.AddRange(temp_walls);
                        rooms.Add(room);
                    }
                    catch { }
                }

            }

            var roomList = new MyModule() { Rooms = rooms };
            rooms.ForEach(x => x.module = roomList);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            options.IncludeFields = true;
            //MainWindow.output = Newtonsoft.Json.JsonConvert.SerializeObject(roomList);
            MainWindow.Output = System.Text.Json.JsonSerializer.Serialize<MyModule>(roomList, options);
        }
    }
}
