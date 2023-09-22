using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DD
{
    /// <summary>
    /// Interaction logic for Furniture.xaml
    /// </summary>
    public partial class Furniture : UserControl
    {
        public Furniture()
        {
            InitializeComponent();
        }

        public Furniture(FurnitureType FurnitureType, double XCoordinate, double YCoordinate)
        {
            InitializeComponent();
            this.FurnitureType = FurnitureType;
            this.XCoordinate = XCoordinate;
            this.YCoordinate = YCoordinate;

        }

        public Furniture(FurnitureType FurnitureType, double XCoordinate, double YCoordinate,
            double X_Size, double Y_Size, Vector X_Axis, Vector Y_Axis)
        {
            InitializeComponent();
            this.FurnitureType = FurnitureType;
            this.XCoordinate = XCoordinate;
            this.YCoordinate = YCoordinate;
            this.X_Size = X_Size;
            this.Y_Size = Y_Size;
            this.X_Axis = X_Axis;
            this.Y_Axis = Y_Axis;

            this.PreviewMouseDown += Furniture_PreviewMouseDown;
        }

        private void Furniture_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public static readonly DependencyProperty FurnitureTypeProperty =
            DependencyProperty.Register("FurnitureType", typeof(FurnitureType), typeof(Furniture),
            new FrameworkPropertyMetadata(FurnitureType.Bed, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty XCoordinateProperty =
            DependencyProperty.Register("XCoordinate", typeof(double), typeof(Furniture),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty YCoordinateProperty =
            DependencyProperty.Register("YCoordinate", typeof(double), typeof(Furniture),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register("Visible", typeof(bool), typeof(Furniture),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool Visible
        {
            get { return (bool)GetValue(VisibleProperty); }
            set { SetValue(VisibleProperty, value); }
        }

        public double YCoordinate
        {
            get { return (double)GetValue(YCoordinateProperty); }
            set { SetValue(YCoordinateProperty, value); }
        }

        public double XCoordinate
        {
            get { return (double)GetValue(XCoordinateProperty); }
            set { SetValue(XCoordinateProperty, value); }
        }

        public FurnitureType FurnitureType
        {
            get { return (FurnitureType)GetValue(FurnitureTypeProperty); }
            set { SetValue(FurnitureTypeProperty, value); }
        }

        public string ImagePath { get; set; } = "";
        public Vector X_Axis { get; set; }
        public Vector Y_Axis { get; set; }
        public double X_Size { get; set; }
        public double Y_Size { get; set; }
        public Point Location => new Point(XCoordinate, YCoordinate);
        public Transform Transform { get; set; } = Transform.Identity;

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Pen pen = new Pen(Brushes.DarkGray, 0.75);
            var rect = DrawRectangle();

            if (Visible && ImagePath != "")
            {
                drawingContext.DrawRectangle(Brushes.White, pen, rect);
                BitmapSource img = new BitmapImage(new Uri("pack://application:,,,/DD;component/Resource/Furniture/" + ImagePath, UriKind.RelativeOrAbsolute));
                img = new TransformedBitmap(img, Transform);
                drawingContext.DrawImage(img, rect);
            }
            //drawingContext.DrawRectangle(Brushes.Black, pen, rect);
        }

        private Rect DrawRectangle()
        {
            var rect = this.FurnitureType switch
            {
                FurnitureType.Wardrobe => new Rect(Location + X_Axis * X_Size * 0.5 - Y_Axis * Y_Size * 0.5, Location + X_Axis * X_Size * -0.5 + Y_Axis * Y_Size * 0.5),
                _ => new Rect(Location + X_Axis * X_Size * 0.5 + Y_Axis * Y_Size, Location + X_Axis * X_Size * -0.5) //默认情况底边中心
            };

            return rect;
        }
    }
}
