using Microsoft.Extensions.Configuration;
using OrdersApi.Data.Database.Interfaces;
using OrdersApi.Logging;
using OrdersApi.Security.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace OrdersApi.Security.Database;

public class SecurityDatabaseConnection : ISecurityDatabaseConnections
{
    protected readonly IConfiguration Configuration;

    CLogSession _logSession = CLogProcessor.GetSession("SecurityDatabaseConnection");

    public SecurityDatabaseConnection(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private SqlConnection GetSqlConnection()
    {
        return new SqlConnection(Configuration.GetConnectionString("OrdersDatabase"));
    }

    public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        try
        {
            using(SqlConnection sqlConnection = GetSqlConnection())
            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
            
                foreach (SqlParameter parameter in parameters)
                    command.Parameters.Add(parameter);

                DataTable dataTable = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                    da.Fill(dataTable);

                return dataTable;
            }
        }
        catch (Exception ex)
        {
            string paramString = "";

            bool first = true;

            foreach(SqlParameter parameter in parameters)
            {
                if(first)
                {
                    paramString += parameter.ParameterName + ": " + parameter.Value;
                }
                else
                {
                    paramString += ", " + parameter.ParameterName + ": " + parameter.Value;
                }
            }

            _logSession.LogGeneral($"Exception while executing query: '{query}' \n\r With parameters: {paramString} \n\r Exception message: {ex.Message}", ProcessLogLevel.PRODUCTION);
            throw ex;
        }
    }

    public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        int result = 0;

        try
        {
            using (SqlConnection sqlConnection = GetSqlConnection())
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                foreach (SqlParameter parameter in parameters)
                    cmd.Parameters.Add(parameter);

                result = cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            string paramString = "";

            bool first = true;

            foreach (SqlParameter parameter in parameters)
            {
                if (first)
                {
                    paramString += parameter.ParameterName + ": " + parameter.Value;
                }
                else
                {
                    paramString += ", " + parameter.ParameterName + ": " + parameter.Value;
                }
            }

            _logSession.LogGeneral($"Exception while executing non query: '{query}' \n\r With parameters: {paramString} \n\r Exception message: {ex.Message}", ProcessLogLevel.PRODUCTION);
            throw ex;
        }

        return result;

    }

    public bool TestConnection()
    {
        try
        {
            ExecuteQuery("SELECT 1");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void Dispose()
    {

    }
}
