using System.Data.SqlClient;
using System.Data;

namespace OrdersApi.Security.Interfaces
{
    public interface ISecurityDatabaseConnections
    {
        DataTable ExecuteQuery(string query, params SqlParameter[] parameters);
        int ExecuteNonQuery(string query, params SqlParameter[] parameters);
        bool TestConnection();
    }
}
