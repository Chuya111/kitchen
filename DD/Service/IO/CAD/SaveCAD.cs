using DD.Data;
using DD.Views;
using K4os.Compression.LZ4.Streams;
using netDxf;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;
using Rhino.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DD
{

    public partial class ToolKit
    {
        static string path = "C:\\Users\\user\\Desktop\\test.dxf";

        public static void SaveCAD()
        {
            CADLayer layer = new CADLayer() { textColor = new AciColor(0, 0, 0), layerName = "wall" };
            DrawContent DrawContent = new DrawContent() { Layer = layer };
            DrawCAD drawCAD = new DrawCAD() { DrawContent = DrawContent };
            
            drawCAD.CreateCAD(path);
        }

    }

    public interface IDrawCAD
    {
        void Draw(DxfDocument doc);
    }

    public interface ILayers
    {
        Layer CreateLayer();
    }


    public class DrawCAD
    {
        public IDrawCAD DrawContent { get; set; }
        private DxfDocument doc { get; set; }

        public void CreateCAD(string path)
        {
            Config();
            DrawContent.Draw(doc);
            doc.Save(path, false);
        }

        private void Config()
        {
            doc = new DxfDocument(DxfVersion.AutoCad2010);

            #region Header
            doc.DrawingVariables.AcadVer = DxfVersion.AutoCad2010;
            doc.DrawingVariables.LUnits = netDxf.Units.LinearUnitType.Architectural;
            doc.DrawingVariables.LUprec = 8;
            //全局变量的不同写法
            doc.DrawingVariables.LwDisplay = true;//控制显示线宽，如果不设置会导致线宽不生效
                                                  //全局比例因子,配合线型对象的比例因子参数使用，否则虚线不生效
            doc.DrawingVariables.LtScale = 1000.0;
            #endregion
        }
    }

    public class DrawContent : IDrawCAD
    {
        public ILayers Layer { get; set; }
        public void Draw(DxfDocument doc)
        {
            var layer = Layer.CreateLayer();

            MyModule data = Newtonsoft.Json.JsonConvert.DeserializeObject<MyModule>(MainWindow.output);
            List<MyWall> walls = new List<MyWall>();
            foreach (MyRoom room in data.Rooms)
            {
                walls.AddRange(room.Walls);
            }

            walls.ForEach(x => DrawWalls(doc, x, layer));
            //Vector2 p1 = new Vector2(0, 100);
            //Vector2 p2 = new Vector2(100, 100);
            //Line fram1 = new Line(p1, p2) { Layer = (Layer)layer };
            //doc.Entities.Add(fram1);
        }

        public void DrawWalls(DxfDocument doc, MyWall w, Layer layer)
        {
            double scale = 25;
            Vector2 start = new Vector2(w.Start_x, w.Start_y) * scale;
            Vector2 end = new Vector2(w.End_x, w.End_y) * scale;

            var ver = new Vector2((end - start).Y, -1 * (end - start).X);
            ver.Normalize();


            var v1 = start + ver * w.Width * 0.5 * scale;
            var v2 = start - ver * w.Width * 0.5 * scale;
            var v3 = end - ver * w.Width * 0.5 * scale;
            var v4 = end + ver * w.Width * 0.5 * scale;

            var wall = new Polyline2D(new List<Vector2>() { v1, v2, v3, v4 }, true);
            
            doc.Entities.Add(wall);
        }
    }

    public class CADLayer : ILayers
    {
        public AciColor textColor { get; set; }
        public string layerName { get; set; }
        public Layer CreateLayer() => new Layer(layerName)
        {
            Color = textColor,
            IsVisible = true,
            Linetype = Linetype.Continuous,
            Lineweight = Lineweight.W40
        };
    }
}

