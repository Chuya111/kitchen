using DD.Views;
using System.Windows.Controls;
using System.Windows.Input;


namespace DD
{
    /// <summary>
    /// Interaction logic for Wall.xaml
    /// </summary>
    public partial class Wall : UserControl
    {

        private void Wall_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                var wallControl = new Wall_Control()
                {
                    DataContext = this,

                };

                wallControl.winProperty.DataContext = this.GetTemplateChild("window") as Win;
                wallControl.leftDoor.DataContext = this.GetTemplateChild("leftDoor") as Door;
                wallControl.rightDoor.DataContext = this.GetTemplateChild("rightDoor") as Door;

                MainWindow.Instance.frame.Content = wallControl;
            }

        }
    }
}
