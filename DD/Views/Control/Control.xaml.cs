using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DD.Views
{
    /// <summary>
    /// Interaction logic for Control.xaml
    /// </summary>
    public partial class Controls : Page
    {
        public static Controls Instance { get; set; }
        public Controls()
        {
            InitializeComponent();
            Instance = this;
            combo.ItemsSource = new List<string>()
            {
                "类型1","类型2","类型3"
            };
        }

        private void width_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ToolKit.Output();
        }
    }
}
