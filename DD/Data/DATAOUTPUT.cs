using DD.Model;
using System.Collections.Generic;

namespace DD.Data
{
    public class MyWindow
    {
        public int id { set; get; }
        public int Host { set; get; }
        public double X_coordinate { get; set; }
        public double Y_coordinate { get; set; }
        public double Length { get; set; }
    }

    public class MyDoor
    {
        public int id { set; get; }
        public int Host { set; get; }
        public double X_coordinate { get; set; }
        public double Y_coordinate { get; set; }
        public double Size { get; set; }
        public bool isInside { get; set; }
        public bool isLeft { get; set; }
    }

    public class MyWall
    {
        public int id { set; get; }
        public MyWindow Win { set; get; }
        public List<MyDoor> Doors { set; get; }
        public int Host { set; get; }
        public double Start_x { get; set; }
        public double Start_y { get; set; }
        public double End_x { get; set; }
        public double End_y { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
    }

    public class MyRoom
    {
        public int Id { set; get; }
        public string? Name { get; set; }
        public double X_coordinate { get; set; } = 0;
        public double Y_coordinate { get; set; } = 0;
        public double Width { get; set; }
        public double Height { get; set; }
        public string Color { get; set; }
        public string? FurnitureType { get; set; }
        public List<MyWall> Walls { get; set; } = null;
        public MyModule module { get; set; } = null;


    }

    public class MyModule
    {
        public int Id { set; get; }
        public string? Name { get; set; }
        public double X_coordinate { get; set; } = 0;
        public double Y_coordinate { get; set; } = 0;
        public double Width { get; set; }
        public double Height { get; set; }
        public string Color { get; set; }
        public List<WallDTO> Walls { set; get; }
        public List<MyRoom> Rooms { get; set; }
    }

}
