using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DD
{
    /// <summary>
    /// Interaction logic for Controlbar.xaml
    /// </summary>
    public partial class Controlbar : UserControl
    {

        public Controlbar()
        {
            InitializeComponent();
        }

        public Controlbar(AxisDirection AxisDirection, Point point, double length)
        {
            InitializeComponent();
            this.AxisDirection = AxisDirection;
            this.Center = point;
            this.Length = length;
            InitializeThumb();
        }

        public List<RoomTest> leftRooms = new List<RoomTest>();
        public List<RoomTest> righRrooms = new List<RoomTest>();
        public List<RoomTest> topRooms = new List<RoomTest>();
        public List<RoomTest> bottomRooms = new List<RoomTest>();


        public static readonly DependencyProperty AxisDirectionProperty =
            DependencyProperty.Register("AxisDirection", typeof(AxisDirection), typeof(Controlbar),
            new FrameworkPropertyMetadata(AxisDirection.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(Controlbar),
            new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register("Length", typeof(double), typeof(Controlbar),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public AxisDirection AxisDirection
        {
            get { return (AxisDirection)GetValue(AxisDirectionProperty); }
            set { SetValue(AxisDirectionProperty, value); }
        }

        public void InitializeThumb()
        {
            double width = 8, height = 20;

            switch (AxisDirection)
            {
                case AxisDirection.Horizontal:
                    thumb.Width = height;
                    thumb.Height = width;
                    Canvas.SetTop(thumb, Center.Y - width * 0.5);
                    Canvas.SetLeft(thumb, Center.X + Length * 0.5 + height);
                    break;

                case AxisDirection.Vertical:
                    thumb.Width = width;
                    thumb.Height = height;
                    Canvas.SetTop(thumb, Center.Y - Length * 0.5 - height * 2);
                    Canvas.SetLeft(thumb, Center.X - width * 0.5);
                    break;
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var controlPoint = sender as Thumb;

            bool check = CheckSize(e.VerticalChange, e.HorizontalChange);
            double delta = 0.0;

            switch (AxisDirection)
            {
                case AxisDirection.Horizontal:

                    delta = check ? e.VerticalChange : 0;
                    controlPoint.SetValue(Canvas.TopProperty, Canvas.GetTop(controlPoint) + delta);
                    double y = (double)controlPoint.GetValue(Canvas.TopProperty);
                    Center = new Point(Center.X, y + controlPoint.Height / 2);

                    if (this.topRooms.Count > 0)
                    {
                        this.topRooms.ForEach(x => x.Height += delta);

                    }
                    if (this.bottomRooms.Count > 0)
                    {
                        this.bottomRooms.ForEach(x => Canvas.SetTop(x, Canvas.GetTop(x) + delta));
                        this.bottomRooms.ForEach(x => x.Height -= delta);
                    }


                    break;
                case AxisDirection.Vertical:

                    delta = check ? e.HorizontalChange : 0;
                    controlPoint.SetValue(Canvas.LeftProperty, Canvas.GetLeft(controlPoint) + delta);
                    double x = (double)controlPoint.GetValue(Canvas.LeftProperty);
                    Center = new Point(x + controlPoint.Width / 2, Center.Y);

                    if (this.leftRooms.Count > 0)
                    {
                        this.leftRooms.ForEach(x => x.Width += delta);

                    }
                    if (this.righRrooms.Count > 0)
                    {
                        this.righRrooms.ForEach(x => Canvas.SetLeft(x, Canvas.GetLeft(x) + delta));
                        this.righRrooms.ForEach(x => x.Width -= delta);
                    }

                    break;
            }

            MainWindow.Instance.CalArea();
            CheckAllFurnitures();
            e.Handled = true;
        }

        public void CheckAllFurnitures()
        {
            for (int i = 0; i < MainWindow.Instance.canvas2.Children.Count; i++)
            {
                if (MainWindow.Instance.canvas2.Children[i] is RoomTest)
                {
                    ((RoomTest)MainWindow.Instance.canvas2.Children[i]).Interaction();
                }
            }
        }

        //Check the room size after dragging the controlLine
        public bool CheckSize(double vertical_size, double horizontal_size)
        {
            double tolerance = 10;

            if (this.AxisDirection == AxisDirection.Horizontal)
            {
                if (this.topRooms.Count > 0)
                {
                    for (int i = 0; i < this.topRooms.Count; i++)
                    {
                        if (this.topRooms[i].ActualHeight + vertical_size < tolerance) return false;

                    }
                }
                if (this.bottomRooms.Count > 0)
                {
                    for (int i = 0; i < this.bottomRooms.Count; i++)
                    {
                        if (this.bottomRooms[i].ActualHeight - vertical_size < tolerance) return false;

                    }
                }
            }
            else
            {
                if (this.leftRooms.Count > 0)
                {
                    for (int i = 0; i < this.leftRooms.Count; i++)
                    {
                        if (this.leftRooms[i].ActualWidth + horizontal_size < tolerance) return false;

                    }
                }
                if (this.righRrooms.Count > 0)
                {
                    for (int i = 0; i < this.righRrooms.Count; i++)
                    {
                        if (this.righRrooms[i].ActualWidth - horizontal_size < tolerance) return false;

                    }
                }
            }
            return true;

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            switch (AxisDirection)
            {
                case AxisDirection.Horizontal:
                    drawingContext.DrawLine(new Pen(Brushes.LightGray, 0.5), Center + new Vector(-Length * 0.5, 0), Center + new Vector(Length * 0.5, 0));
                    break;

                case AxisDirection.Vertical:
                    drawingContext.DrawLine(new Pen(Brushes.LightGray, 0.5), Center + new Vector(0, -Length * 0.5), Center + new Vector(0, Length * 0.5));
                    break;
            }
        }
    }

    public enum AxisDirection
    {
        Vertical,
        Horizontal
    }
}
