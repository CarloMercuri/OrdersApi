using Microsoft.Extensions.Configuration;
using OrdersApi.Data.Database.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace OrdersApi.Data.Database.DataQueries
{
    public class DatabaseConnection : IDatabaseConnection
    {
        protected readonly IConfiguration Configuration;
        protected SqlConnection _connection;

        public DatabaseConnection(IConfiguration configuration)
        {
            Configuration = configuration;
            _connection = GetSqlConnection();
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(Configuration.GetConnectionString("OrdersDatabase"));
        }

        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, _connection))
                {

                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);

                    _connection.Open();
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                        da.Fill(dataTable);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                //_logSession.LogException("Error starting a query " + MethodBase.GetCurrentMethod().Name, OutputAction.ToTeams, ex);
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            int result = 0;

            try
            {
                _connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    foreach (SqlParameter parameter in parameters)
                        cmd.Parameters.Add(parameter);

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //_logSession.LogException("Error starting a non query " + MethodBase.GetCurrentMethod().Name, OutputAction.ToTeams, ex);
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
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
}
