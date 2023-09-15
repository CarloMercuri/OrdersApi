using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace OrdersApi.Data.Database
{
    public class DatabaseUtilities
    {
        public string ConvertDateToDatabaseStandart(DateTime? time)
        {
            string standart = "";
            if (time != null)
                standart = Convert.ToDateTime(time).ToString("yyyy-MM-dd HH:mm:ss");
            return standart;
        }

        public SqlParameter AddSqlParameter(string name, object value)
        {
            var dt = value as DateTime?;

            if (dt != null)
            {
                DateTime check = (DateTime)dt;

                if (check.Year < 2000)
                {
                    value = null;
                }
            }

            if (value != null)
            {
                return new SqlParameter(name, value);
            }
            else
            {
                return new SqlParameter(name, DBNull.Value);
            }
        }


        public string GetDatabaseName(string connectionString)
        {
            string dbName = "";

            int firstIndex = connectionString.IndexOf("Database=");

            if (firstIndex == -1)
            {
                return dbName;
            }

            firstIndex = firstIndex + 9;

            int lastIndex = connectionString.IndexOf(';', firstIndex);

            if (lastIndex < 0)
            {
                dbName = connectionString.Substring(firstIndex);
            }
            else
            {
                dbName = connectionString.Substring(firstIndex, lastIndex - firstIndex);
            }

            return dbName;
        }

        public DateTime FetchAsDateTime(object value, DateTime defaultValue)
        {
            try
            {
                if (value != null && value != DBNull.Value)
                {
                    if (DateTime.TryParse(value.ToString(), out DateTime dt))
                    {
                        return dt;
                    }
                }

                return defaultValue;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public string FetchAsDateTimeString(object value, string format = "")
        {
            try
            {
                if (value != null && value != DBNull.Value)
                {
                    if (DateTime.TryParse(value.ToString(), out DateTime dt))
                    {
                        if (format == "") return dt.ToString();
                        return dt.ToString(format);
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public long FetchAsLong(object value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(value);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int FetchAsInt32(object value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(value);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public long FetchAsInt64(object value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(value);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool FetchAsEnumerator<T>(string value, out T result)
        {
            try
            {
                bool autoParseResult = Enum.TryParse(typeof(T), value, true, out object res);
                if (autoParseResult)
                {
                    result = (T)res;
                    return true;
                }

                // Attempt Manual
                foreach (string enumName in Enum.GetNames(typeof(T)))
                {
                    if (value.Equals(enumName))
                    {
                        result = (T)Enum.Parse(typeof(T), enumName);
                        return true;
                    }
                }
                result = default;
                return false;
            }
            catch (Exception ex)
            {
                result = default;
                return false;
            }
        }

        public string FetchAsString(object value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return "";
                }
                else
                {
                    return Convert.ToString(value);
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public bool FetchAsBool(object value, bool returnTrueOnNull = false)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return returnTrueOnNull;
                }
                else
                {
                    return Convert.ToBoolean(value);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public double FetchAsDouble(object value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return 0D;
                }
                else
                {
                    return Convert.ToDouble(value);
                }
            }
            catch (Exception ex)
            {
                return 0D;
            }
        }

        public float FetchAsFloat(object value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return 0f;
                }
                else
                {
                    return (float)Convert.ToDecimal(value);
                }
            }
            catch (Exception ex)
            {
                return 0f;
            }
        }

        public decimal FetchAsDecimal(object value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                {
                    return 0M;
                }
                else
                {
                    return Convert.ToDecimal(value);
                }
            }
            catch (Exception ex)
            {
                return 0M;
            }
        }
    }
}
