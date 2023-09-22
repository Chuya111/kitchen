using System.Windows;
using System.Windows.Controls;

namespace DD.Views
{
    /// <summary>
    /// Interaction logic for Wall_Control.xaml
    /// </summary>
    public partial class Wall_Control : Page
    {

        public static Wall_Control Instance { get; set; }
        public Wall_Control()
        {
            InitializeComponent();
            Instance = this;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToolKit.Output();
        }

        private void window_length_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ToolKit.Output();
        }
    }
}
