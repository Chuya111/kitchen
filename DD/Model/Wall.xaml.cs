using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace DD
{
    /// <summary>
    /// Interaction logic for Wall.xaml
    /// </summary>
    public partial class Wall : UserControl
    {
        public Wall()
        {
            InitializeComponent();
            this.PreviewMouseDown += Wall_MouseDown;
        }


        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(Wall), new PropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Wall),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LinePositionProperty =
            DependencyProperty.Register("LinePosition", typeof(LinePosition), typeof(Wall),
            new FrameworkPropertyMetadata(LinePosition.Top, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LineLengthProperty =
            DependencyProperty.Register("LineLength", typeof(double), typeof(Wall),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register("Visible", typeof(bool), typeof(Wall),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register("LeftOffset", typeof(int), typeof(Wall),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RightOffsetProperty =
            DependencyProperty.Register("RightOffset", typeof(int), typeof(Wall),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public int RightOffset
        {
            get { return (int)GetValue(RightOffsetProperty); }
            set { SetValue(RightOffsetProperty, value); }
        }

        public int LeftOffset
        {
            get { return (int)GetValue(LeftOffsetProperty); }
            set { SetValue(LeftOffsetProperty, value); }
        }

        public bool Visible
        {
            get { return (bool)GetValue(VisibleProperty); }
            set { SetValue(VisibleProperty, value); }
        }

        public LinePosition LinePosition
        {
            get { return (LinePosition)GetValue(LinePositionProperty); }
            set { SetValue(LinePositionProperty, value); }
        }

        public double LineLength
        {
            get { return (double)GetValue(LineLengthProperty); }
            set { SetValue(LineLengthProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var pen = new Pen(Stroke, StrokeThickness);
            var width = ActualWidth;
            var height = ActualHeight;
            var margin = StrokeThickness * 0.5;

            if (Visible)
            {
                switch (LinePosition)
                {
                    // 逆时针右手螺旋定则
                    case LinePosition.Top:
                        drawingContext.DrawLine(pen, new Point(LineLength + margin - RightOffset, 0), new Point(LeftOffset - margin, 0));
                        break;
                    case LinePosition.Right:
                        drawingContext.DrawLine(pen, new Point(width, LineLength + margin - RightOffset), new Point(width, LeftOffset - margin));
                        break;
                    case LinePosition.Bottom:
                        drawingContext.DrawLine(pen, new Point(RightOffset - margin, height), new Point(LineLength - LeftOffset + margin, height));
                        break;
                    case LinePosition.Left:
                        drawingContext.DrawLine(pen, new Point(0, RightOffset - margin), new Point(0, LineLength - LeftOffset + margin));
                        break;
                }
            }
        }

    }
    public enum LinePosition
    {
        Top,
        Right,
        Bottom,
        Left
    }
}
