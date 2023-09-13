using System.Data.SqlClient;
using System.Data;

namespace OrdersApi.Data.Database.Interfaces
{
    public interface IDatabaseConnection : IDisposable
    {
        DataTable ExecuteQuery(string query, params SqlParameter[] parameters);
        int ExecuteNonQuery(string query, params SqlParameter[] parameters);
        bool TestConnection();
    }
}
