using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DD
{
    /// <summary>
    /// Interaction logic for Win.xaml
    /// </summary>
    public partial class Win : UserControl
    {
        public Win()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Win), new PropertyMetadata(0.0));

        public static readonly DependencyProperty LineLengthProperty =
            DependencyProperty.Register("LineLength", typeof(int), typeof(Win),
            new FrameworkPropertyMetadata(25, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LinePositionProperty =
            DependencyProperty.Register("LinePosition", typeof(LinePosition), typeof(Win),
            new FrameworkPropertyMetadata(LinePosition.Top, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register("Visible", typeof(bool), typeof(Win),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register("LeftOffset", typeof(int), typeof(Win),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RightOffsetProperty =
            DependencyProperty.Register("RightOffset", typeof(int), typeof(Win),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty WinStyleProperty =
            DependencyProperty.Register("WinStyle", typeof(int), typeof(Win),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public int WinStyle
        {
            get { return (int)GetValue(WinStyleProperty); }
            set { SetValue(WinStyleProperty, value); }
        }

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

        public int LineLength
        {
            get { return (int)GetValue(LineLengthProperty); }
            set { SetValue(LineLengthProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public double X_Coordinate { get; set; }
        public double Y_Coordinate { get; set; }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var pen = new Pen(Brushes.LightGray, 1);
            double offset = 0.8;
            Rect rect;

            switch (LinePosition)
            {
                case LinePosition.Top:
                    if (this.WinStyle == 0)
                    {
                        X_Coordinate = (ActualWidth + LeftOffset - RightOffset) * 0.5;
                        Y_Coordinate = 0;
                        rect = new Rect(new Point(X_Coordinate + LineLength * 0.5, Y_Coordinate + 0.5 * StrokeThickness - offset),
                            new Point(X_Coordinate - LineLength * 0.5, Y_Coordinate - 0.5 * StrokeThickness + offset));

                        break;
                    }
                    else
                    {
                        X_Coordinate = (ActualWidth + LeftOffset - RightOffset) * 0.5;
                        Y_Coordinate = 0;
                        rect = new Rect(new Point(X_Coordinate + LineLength * 0.5, Y_Coordinate + 0.5 * StrokeThickness - offset),
                            new Point(X_Coordinate - LineLength * 0.5, Y_Coordinate - 0.5 * StrokeThickness));

                        var thick = 4;
                        var off = 10;
                        var rect1 = new Rect(new Point(X_Coordinate - LineLength * 0.5 - thick, Y_Coordinate - 0.5 * StrokeThickness - off),
                            new Point(X_Coordinate - LineLength * 0.5, Y_Coordinate - 0.5 * StrokeThickness));
                        var rect2 = new Rect(new Point(X_Coordinate + LineLength * 0.5, Y_Coordinate - 0.5 * StrokeThickness - off),
                            new Point(X_Coordinate + LineLength * 0.5 + thick, Y_Coordinate - 0.5 * StrokeThickness));
                        var rect3 = new Rect(new Point(X_Coordinate - LineLength * 0.5 - thick, Y_Coordinate - 0.5 * StrokeThickness - off - thick),
                            new Point(X_Coordinate + LineLength * 0.5 + thick, Y_Coordinate - 0.5 * StrokeThickness - off));

                        drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.White, 0), rect);
                        drawingContext.DrawRectangle(Brushes.White, pen, rect1);
                        drawingContext.DrawRectangle(Brushes.White, pen, rect2);
                        drawingContext.DrawRectangle(Brushes.White, pen, rect3);
                        return;
                    }

                case LinePosition.Right:
                    X_Coordinate = ActualWidth;
                    Y_Coordinate = (ActualHeight + LeftOffset - RightOffset) * 0.5;
                    rect = new Rect(new Point(X_Coordinate + 0.5 * StrokeThickness - offset, Y_Coordinate + LineLength * 0.5),
                        new Point(X_Coordinate - 0.5 * StrokeThickness + offset, Y_Coordinate - LineLength * 0.5));
                    break;
                case LinePosition.Bottom:
                    X_Coordinate = (ActualWidth + LeftOffset - RightOffset) * 0.5;
                    Y_Coordinate = ActualHeight;
                    rect = new Rect(new Point(X_Coordinate - LineLength * 0.5, Y_Coordinate - 0.5 * StrokeThickness + offset),
                        new Point(X_Coordinate + LineLength * 0.5, Y_Coordinate + 0.5 * StrokeThickness - offset));
                    break;
                case LinePosition.Left:
                    X_Coordinate = 0;
                    Y_Coordinate = (ActualHeight - LeftOffset + RightOffset) * 0.5;
                    rect = new Rect(new Point(X_Coordinate + 0.5 * StrokeThickness - offset, Y_Coordinate + LineLength * 0.5),
                        new Point(X_Coordinate - 0.5 * StrokeThickness + offset, Y_Coordinate - LineLength * 0.5));
                    break;
                default:
                    rect = new Rect();
                    break;
            };

            if (Visible)
            {
                drawingContext.DrawRectangle(Brushes.White, pen, rect);
            }
        }
    }
}

