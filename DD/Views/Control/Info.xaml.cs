using System;
using System.Windows.Controls;

namespace DD.Views
{
    /// <summary>
    /// Interaction logic for Data.xaml
    /// </summary>
    public partial class Info : Page
    {
        public double T { get; set; }
        //public List<object> Rooms { get; set; }
        public Info()
        {
            InitializeComponent();
            //var canvas = (Canvas)this.DataContext;
            //Rooms = new List<object>() {
            //    new { Name = "Bedroom",Width=40,Color="#FF2E3E97",Area = 40},
            //    new { Name = "Restroom",Width=26,Color="#FF4AAD4B",Area = 26},
            //};
            //this.DataContext = Rooms;
            total.Text = String.Format("总面积 : {0:0.0} ㎡", T);
        }

        public Info(double area)
        {
            InitializeComponent();

            total.Text = String.Format("总面积 : {0:0.0} ㎡", area);
        }
    }

    public class Room
    {
        public double RectWidth { get; set; }
        public string Color { get; set; }
        public double Area { get; set; }

    }
}
