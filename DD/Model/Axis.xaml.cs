using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DD
{
    /// <summary>
    /// Interaction logic for Axis.xaml
    /// </summary>
    public partial class Axis : UserControl
    {

        //public Point Center { get; set; } = new Point(250, 250);
        public double left => Center.X - 0.5 * ActualWidth;
        public double right => Center.X + 0.5 * ActualWidth;
        public double top => Center.Y - 0.5 * ActualHeight;
        public double bottom => Center.Y + 0.5 * ActualHeight;

        public Axis()
        {
            InitializeComponent();
        }

        public List<RoomTest> leftRooms = new List<RoomTest>();
        public List<RoomTest> righRrooms = new List<RoomTest>();
        public List<RoomTest> topRooms = new List<RoomTest>();
        public List<RoomTest> bottomRooms = new List<RoomTest>();

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(Axis),
            new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var controlPoint = sender as Thumb;
            int delta = 0;

            switch (controlPoint.Name)
            {
                case "top_thumb":
                    delta = CheckBottom(e.VerticalChange) ? (int)e.VerticalChange : 0;
                    controlPoint.SetValue(Canvas.TopProperty, Canvas.GetTop(controlPoint) + delta);
                    Height -= delta;
                    Center += new Vector(0, delta * 0.5);
                    this.bottomRooms.ForEach(x => Canvas.SetTop(x, Canvas.GetTop(x) + delta));
                    this.bottomRooms.ForEach(x => x.Height -= delta);
                    break;
                case "right_thumb":
                    delta = CheckLeft(e.HorizontalChange) ? (int)e.HorizontalChange : 0;
                    Width += delta;
                    Center += new Vector(delta * 0.5, 0);
                    this.leftRooms.ForEach(x => x.Width += delta);
                    break;
                case "bottom_thumb":
                    delta = CheckTop(e.VerticalChange) ? (int)e.VerticalChange : 0;
                    Height += delta;
                    Center += new Vector(0, delta * 0.5);
                    this.topRooms.ForEach(x => x.Height += delta);
                    break;
                case "left_thumb":
                    delta = CheckRight(e.HorizontalChange) ? (int)e.HorizontalChange : 0;
                    controlPoint.SetValue(Canvas.LeftProperty, Canvas.GetLeft(controlPoint) + delta);
                    Width -= delta;
                    Center += new Vector(delta * 0.5, 0);
                    this.righRrooms.ForEach(x => Canvas.SetLeft(x, Canvas.GetLeft(x) + delta));
                    this.righRrooms.ForEach(x => x.Width -= delta);
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

        public bool CheckTop(double vertical_size)
        {
            double tolerance = 10;

            if (this.topRooms.Count > 0)
            {
                for (int i = 0; i < this.topRooms.Count; i++)
                {
                    if (this.topRooms[i].ActualHeight + vertical_size < tolerance) return false;
                }
            }
            return true;
        }

        public bool CheckBottom(double vertical_size)
        {
            double tolerance = 10;

            if (this.bottomRooms.Count > 0)
            {
                for (int i = 0; i < this.bottomRooms.Count; i++)
                {
                    if (this.bottomRooms[i].ActualHeight - vertical_size < tolerance) return false;

                }
            }

            return true;
        }

        public bool CheckLeft(double horizontal_size)
        {
            double tolerance = 10;

            if (this.leftRooms.Count > 0)
            {
                for (int i = 0; i < this.leftRooms.Count; i++)
                {
                    if (this.leftRooms[i].ActualWidth + horizontal_size < tolerance) return false;

                }
            }
            return true;

        }
        public bool CheckRight(double horizontal_size)
        {
            double tolerance = 10;

            if (this.righRrooms.Count > 0)
            {
                for (int i = 0; i < this.righRrooms.Count; i++)
                {
                    if (this.righRrooms[i].ActualWidth - horizontal_size < tolerance) return false;

                }
            }

            return true;

        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            base.OnRender(drawingContext);
            //Point Center = new Point(250, 250);
            double offset = 25;

            double[] dashes = new[] { 10.0, 5.0 };
            Pen pen = new Pen(Brushes.DarkGray, 0.75);
            pen.DashStyle = new DashStyle(dashes, 0);

            // 绘制虚线
            Point left_start = Center + new Vector(-0.5 * ActualWidth, -0.5 * ActualHeight - offset);
            Point left_end = Center + new Vector(-0.5 * ActualWidth, 0.5 * ActualHeight + offset * 2);
            drawingContext.DrawLine(pen, left_start, left_end);

            Point right_start = Center + new Vector(0.5 * ActualWidth, -0.5 * ActualHeight - offset);
            Point right_end = Center + new Vector(0.5 * ActualWidth, 0.5 * ActualHeight + offset * 2);
            drawingContext.DrawLine(pen, right_start, right_end);

            // 绘制虚线
            Point top_start = Center + new Vector(-0.5 * ActualWidth - offset * 2, -0.5 * ActualHeight);
            Point top_end = Center + new Vector(0.5 * ActualWidth + offset, -0.5 * ActualHeight);
            drawingContext.DrawLine(pen, top_start, top_end);

            Point bottom_start = Center + new Vector(-0.5 * ActualWidth - offset * 2, 0.5 * ActualHeight);
            Point bottom_end = Center + new Vector(0.5 * ActualWidth + offset, 0.5 * ActualHeight);
            drawingContext.DrawLine(pen, bottom_start, bottom_end);


            Pen pen_axis = new Pen(Brushes.Gray, 0.50);
            pen_axis.DashStyle = new DashStyle(dashes, 0);
            double note_offset = 5.0;
            double text_offset = 5.0;

            // 绘制轴网数据
            Point length_start = Center + new Vector(-0.5 * ActualWidth - offset, -0.5 * ActualHeight);
            Point length_end = Center + new Vector(-0.5 * ActualWidth - offset, 0.5 * ActualHeight);

            drawingContext.DrawLine(pen_axis, length_start, length_end);
            drawingContext.DrawLine(pen_axis, length_start + new Vector(-note_offset, note_offset), length_start + new Vector(note_offset, -note_offset));
            drawingContext.DrawLine(pen_axis, length_end + new Vector(-note_offset, note_offset), length_end + new Vector(note_offset, -note_offset));

            Point width_start = Center + new Vector(-0.5 * ActualWidth, 0.5 * ActualHeight + offset);
            Point width_end = Center + new Vector(0.5 * ActualWidth, 0.5 * ActualHeight + offset);

            drawingContext.DrawLine(pen_axis, width_start, width_end);
            drawingContext.DrawLine(pen_axis, width_start + new Vector(-note_offset, note_offset), width_start + new Vector(note_offset, -note_offset));
            drawingContext.DrawLine(pen_axis, width_end + new Vector(-note_offset, note_offset), width_end + new Vector(note_offset, -note_offset));

            FontFamily font = new FontFamily("Arial");
            Typeface typeface = new Typeface(font, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            // 创建一个 FormattedText 对象
            int h = System.Convert.ToInt16(ActualHeight * 0.5) * 50;
            int w = System.Convert.ToInt16(ActualWidth * 0.5) * 50;
            FormattedText height = new FormattedText(h.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 16, Brushes.Gray);
            FormattedText width = new FormattedText(w.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 16, Brushes.Gray);
            // 绘制文本
            RotateTransform rotate = new RotateTransform(90, Center.X - 0.5 * ActualWidth - offset - text_offset, Center.Y);
            drawingContext.PushTransform(rotate);
            drawingContext.DrawText(height, Center + new Vector(-0.5 * ActualWidth - offset - text_offset - 15, 0));
            drawingContext.Pop();
            drawingContext.DrawText(width, Center + new Vector(-15, 0.5 * ActualHeight + offset + text_offset));



            double radius = 10.0;
            Pen pen_circle = new Pen(Brushes.Gray, 1.0);
            var brush = new SolidColorBrush(Colors.Gray);
            drawingContext.DrawEllipse(brush, pen_circle,
                Center + new Vector(-0.5 * ActualWidth, 0.5 * ActualHeight + offset * 2 + radius), radius, radius);
            drawingContext.DrawEllipse(brush, pen_circle,
                Center + new Vector(0.5 * ActualWidth, 0.5 * ActualHeight + offset * 2 + radius), radius, radius);
            drawingContext.DrawEllipse(brush, pen_circle,
                Center + new Vector(-0.5 * ActualWidth - offset * 2 - radius, -0.5 * ActualHeight), radius, radius);
            drawingContext.DrawEllipse(brush, pen_circle,
                Center + new Vector(-0.5 * ActualWidth - offset * 2 - radius, 0.5 * ActualHeight), radius, radius);

        }
    }
}
