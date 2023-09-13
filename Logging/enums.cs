namespace OrdersApi.Logging
{
    public enum LogEventLevel
    {
        DEBUG = 1,
        NORMAL = 2,
        WARNING = 3,
        ERROR = 4,
        EXCEPTION = 5,
        FATAL = 6
    }

    public enum ProcessLogLevel
    {
        NONE = 0,
        DEVELOPMENT = 1,
        DEBUG = 2,
        INFORMATIVE = 3,
        PRODUCTION = 4,
    }
}
