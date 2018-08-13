using System.Data.SqlClient;

namespace Pharmacy
{
    public abstract class ActiveRecord
    {
        public abstract int ID { get; set; }

        protected static SqlConnection Open()
        {
            string connectionString =
                "Integrated Security=SSPI;" + "Data Source=.\\SQLEXPRESS;" + "Initial Catalog=Pharmacy;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        public abstract void Save();
        public abstract void Reload(int id);
        public abstract void Remove(int id);
        public abstract void ShowAll();
    }
}