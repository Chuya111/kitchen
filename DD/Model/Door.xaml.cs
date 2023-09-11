using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DD
{
    /// <summary>
    /// Interaction logic for Door.xaml
    /// </summary>
    public partial class Door : UserControl
    {
        public Door()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Door), new PropertyMetadata(0.0));

        public static readonly DependencyProperty DoorSizeProperty =
            DependencyProperty.Register("DoorSize", typeof(int), typeof(Door),
            new FrameworkPropertyMetadata(32, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LinePositionProperty =
            DependencyProperty.Register("LinePosition", typeof(LinePosition), typeof(Door),
            new FrameworkPropertyMetadata(LinePosition.Top, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register("Visible", typeof(bool), typeof(Door),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register("LeftOffset", typeof(int), typeof(Door),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RightOffsetProperty =
            DependencyProperty.Register("RightOffset", typeof(int), typeof(Door),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsInsideProperty =
            DependencyProperty.Register("IsInside", typeof(bool), typeof(Door),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsLeftProperty =
            DependencyProperty.Register("IsLeft", typeof(bool), typeof(Door),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsMainDoorProperty =
            DependencyProperty.Register("IsMainDoor", typeof(bool), typeof(Door),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsMainDoor
        {
            get { return (bool)GetValue(IsMainDoorProperty); }
            set { SetValue(IsMainDoorProperty, value); }
        }

        public bool IsInside
        {
            get { return (bool)GetValue(IsInsideProperty); }
            set { SetValue(IsInsideProperty, value); }
        }

        public bool IsLeft
        {
            get { return (bool)GetValue(IsLeftProperty); }
            set { SetValue(IsLeftProperty, value); }
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

        public int DoorSize
        {
            get { return (int)GetValue(DoorSizeProperty); }
            set { SetValue(DoorSizeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public bool IsStartPosition { get; set; }
        public double X_Coordinate { get; set; }
        public double Y_Coordinate { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var pen = new Pen(Brushes.LightGray, 1);

            int doorOffset = 6;
            Rect rect;
            Rect door;
            PathGeometry arc;

            switch (LinePosition)
            {
                case LinePosition.Top:
                    X_Coordinate = IsStartPosition ? ActualWidth - RightOffset - doorOffset - DoorSize * 0.5 : LeftOffset + doorOffset + DoorSize * 0.5;
                    Y_Coordinate = 0;
                    var point = new Point(X_Coordinate, Y_Coordinate);
                    Draw(DoorSize, new Vector(-1, 0), new Vector(0, 1), point,
                        StrokeThickness, IsInside, IsLeft, out rect, out door, out arc);
                    break;
                case LinePosition.Right:
                    X_Coordinate = ActualWidth;
                    Y_Coordinate = IsStartPosition ? ActualHeight - RightOffset - doorOffset - DoorSize * 0.5 : LeftOffset + doorOffset + DoorSize * 0.5;
                    point = new Point(X_Coordinate, Y_Coordinate);
                    Draw(DoorSize, new Vector(0, -1), new Vector(-1, 0), point,
                        StrokeThickness, IsInside, IsLeft, out rect, out door, out arc);
                    break;
                case LinePosition.Bottom:
                    X_Coordinate = IsStartPosition ? RightOffset + doorOffset + DoorSize * 0.5 : ActualWidth - LeftOffset - doorOffset - DoorSize * 0.5;
                    Y_Coordinate = ActualHeight;
                    point = new Point(X_Coordinate, Y_Coordinate);
                    Draw(DoorSize, new Vector(1, 0), new Vector(0, -1), point,
                        StrokeThickness, IsInside, IsLeft, out rect, out door, out arc);
                    break;
                case LinePosition.Left:
                    X_Coordinate = 0;
                    Y_Coordinate = IsStartPosition ? RightOffset + doorOffset + DoorSize * 0.5 : ActualHeight - LeftOffset - doorOffset - DoorSize * 0.5;
                    point = new Point(X_Coordinate, Y_Coordinate);
                    Draw(DoorSize, new Vector(0, 1), new Vector(1, 0), point,
                    StrokeThickness, IsInside, IsLeft, out rect, out door, out arc);
                    break;
                default:
                    rect = new Rect();
                    door = new Rect();
                    arc = new PathGeometry();
                    break;
            };

            if (Visible)
            {
                drawingContext.DrawRectangle(Brushes.LightGray, null, rect);
                drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 0.15), door);
                drawingContext.DrawGeometry(null, new Pen(Brushes.Black, 0.3), arc);
            }
        }

        private void Draw(int size, Vector xAxis, Vector yAxis, Point point, double thickness, bool isInside, bool isLeft,
            out Rect rect, out Rect door, out PathGeometry arc)
        {
            int doorThickness = 2;
            rect = new Rect(point - xAxis * size * 0.5 - yAxis * thickness * 0.5, point + xAxis * size * 0.5 + yAxis * thickness * 0.5);

            if (isInside)
            {
                if (isLeft)
                {
                    var doorBase = point - xAxis * (size * 0.5 - doorThickness);
                    door = new Rect(doorBase + yAxis * (size - doorThickness) - xAxis * doorThickness, doorBase);
                    var segment = new ArcSegment()
                    {
                        Size = new Size(size - doorThickness, size - doorThickness),
                        Point = point + yAxis * (size - doorThickness) - xAxis * (size * 0.5 - doorThickness),
                        SweepDirection = SweepDirection.Counterclockwise,
                        RotationAngle = 90
                    };
                    PathFigure path = new PathFigure(point + xAxis * (size * 0.5), new PathSegment[] { segment }, false);
                    arc = new PathGeometry(new List<PathFigure>() { path });
                }
                else
                {
                    var doorBase = point + xAxis * (size * 0.5);
                    door = new Rect(doorBase + yAxis * (size - doorThickness) - xAxis * doorThickness, doorBase);
                    var segment = new ArcSegment()
                    {
                        Size = new Size(size - doorThickness, size - doorThickness),
                        Point = point + yAxis * (size - doorThickness) + xAxis * (size * 0.5 - doorThickness),
                        SweepDirection = SweepDirection.Clockwise,
                        RotationAngle = 90
                    };
                    PathFigure path = new PathFigure(point - xAxis * (size * 0.5), new PathSegment[] { segment }, false);
                    arc = new PathGeometry(new List<PathFigure>() { path });
                }
            }
            else
            {
                if (isLeft)
                {
                    var doorBase = point - xAxis * (size * 0.5);
                    door = new Rect(doorBase, doorBase - yAxis * (size - doorThickness) + xAxis * doorThickness);
                    var segment = new ArcSegment()
                    {
                        Size = new Size(size - doorThickness, size - doorThickness),
                        Point = point - yAxis * (size - doorThickness) - xAxis * (size * 0.5 - doorThickness),
                        SweepDirection = SweepDirection.Clockwise,
                        RotationAngle = 90
                    };
                    PathFigure path = new PathFigure(point + xAxis * (size * 0.5), new PathSegment[] { segment }, false);
                    arc = new PathGeometry(new List<PathFigure>() { path });

                }
                else
                {
                    var doorBase = point + xAxis * (size * 0.5 - doorThickness);
                    door = new Rect(doorBase, doorBase - yAxis * (size - doorThickness) + xAxis * doorThickness);
                    var segment = new ArcSegment()
                    {
                        Size = new Size(size - doorThickness, size - doorThickness),
                        Point = point - yAxis * (size - doorThickness) + xAxis * (size * 0.5 - doorThickness),
                        SweepDirection = SweepDirection.Counterclockwise,
                        RotationAngle = 90
                    };
                    PathFigure path = new PathFigure(point - xAxis * (size * 0.5), new PathSegment[] { segment }, false);
                    arc = new PathGeometry(new List<PathFigure>() { path });
                }
            }

        }
    }
}
