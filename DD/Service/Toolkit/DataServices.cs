using MySql.Data.MySqlClient;

namespace DD.Service
{
    public interface IDataProcess
    {
        void DataProcess(string sql);
        MySqlDataReader DataQuery(string sql);
    }
    public class DataBaseServices : IDataProcess
    {
        public void DataProcess(string? sql)
        {
            MySqlConnection connection = this.ConnectDatabase();
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }

        public MySqlDataReader DataQuery(string? sql)
        {
            MySqlConnection connection = this.ConnectDatabase();
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            return reader;
        }

        private MySqlConnection ConnectDatabase()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.UserID = "guest";
            builder.Password = "password";
            builder.Server = "10.88.116.173";
            builder.Database = "plan_database_230714";
            MySqlConnection connection = new MySqlConnection(builder.ConnectionString);

            //MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            //builder.UserID = "root";
            //builder.Password = "120375"; 
            //builder.Server = "localhost";
            //builder.Database = "plan_database_230714"; 
            //MySqlConnection connection = new MySqlConnection(builder.ConnectionString);


            return connection;
        }
    }
}
