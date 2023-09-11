using DD.Service;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DD.Views
{
    /// <summary>
    /// Interaction logic for DesignPage.xaml
    /// </summary>
    public partial class DesignPage : Page
    {
        public static DesignPage Instance { get; set; } = new DesignPage();

        public DesignPage()
        {
            InitializeComponent();
            Initialization();
            Instance = this;
        }

        Canvas canvas2 = MainWindow.Instance?.canvas2;
        Frame frame = MainWindow.Instance?.frame;

        public void Initialization()
        {
            //canvas2.Visibility = Visibility.Visible;
            var dbs = new DataBaseServices();
            var value_list = new List<object>();
            string sql = "Select * from room_storage order by type";

            MySqlDataReader value_reader = dbs.DataQuery(sql);
            while (value_reader.Read())
            {

                value_list.Add(new
                {
                    Name = value_reader["room"].ToString(),
                    Height = double.Parse(value_reader["length"].ToString()),
                    Width = double.Parse(value_reader["width"].ToString()),
                    Color = value_reader["color"].ToString(),
                    Id = int.Parse(value_reader["type"].ToString()),
                }); ;
            }

            this.DataContext = value_list;
        }

    }
}
