using DD.Data;
using System.Linq;

namespace DD
{
    public partial class ToolKit
    {

        public static void Deserilization(string input)
        {


            if (input != null)
            {
                MyModule data = Newtonsoft.Json.JsonConvert.DeserializeObject<MyModule>(input);

                var names = data.Rooms.Select(x => x.Walls).ToList();

                //MessageBox.Show(names[0][0].Length.ToString());
                //P = data.Rooms.Select(x => new Rectangle3d(
                //  new Plane(new Point3d(-x.X_coordinate, x.Y_coordinate + (int)x.Height, 0), -Vector3d.XAxis, -Vector3d.YAxis), (int)x.Width, (int)x.Height)
                //  ).ToList();
                //data.Rooms.ForEach(x => walls.AddRange(GenerateLine(x.Walls)));
                //Wall = walls;

                //data.Rooms.ForEach(x => GenerateWin(x.Walls));
                //Win = windows.Where(x => x != null).Select(x => new Point3d(x.X_coordinate, x.Y_coordinate, 0)).ToList();
                //Windex = windows.Where(x => x != null).Select(x => x.Host).ToList();
                //WinLen = windows.Where(x => x != null).Select(x => x.Length * 25).ToList();

                //data.Rooms.ForEach(x => GenerateDoor(x.Walls));
                //Door = doors.Where(x => x != null).Select(x => new Point3d(x.X_coordinate, x.Y_coordinate, 0)).ToList();
                //DoorIndex = doors.Where(x => x != null).Select(x => x.Host).ToList();
                //DoorSize = doors.Where(x => x != null).Select(x => x.Size * 25).ToList();

            }
        }

    }
}
