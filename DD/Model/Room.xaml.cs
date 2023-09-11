using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DD
{
    /// <summary>
    /// Interaction logic for RoomTest.xaml
    /// </summary>
    public partial class RoomTest : UserControl
    {
        public RoomTest()
        {
            InitializeComponent();
        }

        public List<Furniture> furnitures { get; set; } = new List<Furniture>();
        public Controlbar leftControBar { get; set; } = null;
        public Controlbar rightControlBar { get; set; } = null;
        public Controlbar topControBar { get; set; } = null;
        public Controlbar bottomControlBar { get; set; } = null;

        public Vector X_Axis { get; set; } = new Vector(-1, 0);
        public Vector Y_Axis { get; set; } = new Vector(0, -1);


        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
        "Fill", typeof(Brush), typeof(RoomTest), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(RoomTest), new PropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(RoomTest));

        public static readonly DependencyProperty DataValueProperty =
        DependencyProperty.Register("DataValue", typeof(object), typeof(RoomTest), new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(RoomTest), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register("StrokeThickness", typeof(double), typeof(RoomTest), new PropertyMetadata(0.0));


        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public object DataValue
        {
            get { return (object)GetValue(DataValueProperty); }
            set { SetValue(DataValueProperty, value); }
        }
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public bool isDynamic { get; set; } = true;
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var rect = new Rect(0, 0, ActualWidth, ActualHeight);

            if (this.Text == "Restroom" && isDynamic)
            {
                //this.Background = Brushes.Transparent;
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/DD;component/Resource/Furniture/Mapping/Restroom_Tiles.png", UriKind.RelativeOrAbsolute));
                brush.TileMode = TileMode.Tile;
                brush.Viewport = new Rect(0, 0, 1, 1);
                brush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                this.Fill = brush;
            }

            drawingContext.DrawRectangle(Fill, new Pen(Stroke, 0), rect);

            if (this.Text == "Restroom" && isDynamic)
            {
                var shaft = new Rect() { Width = 16, Height = 40, };
                drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Orange, 4), shaft);
            }

        }

        /// 本户间所属的户型，没有隶属为0
        public int LayoutTypeID { set; get; }
        /// 本房间对应的模块id
        public int LayoutModuleID { set; get; }


        public void Interaction()
        {

            this.UpdateLayout();
            ToolKit.CheckAxis(this);
            ToolKit.CheckControlLine(this);
            ToolKit.CheckFurnitures(this);

        }

    }
}
