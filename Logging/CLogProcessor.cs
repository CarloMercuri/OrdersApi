namespace OrdersApi.Logging
{
    public class CLogProcessor
    {
        private static CLogProcessor instance = null;
        private static readonly object singletonPadlock = new object();
        private static readonly object MainLogLock = new object();
        private static readonly object FileLock = new object();

        // Configuration variables
        public static bool TestMode { get; private set; }
        public static ProcessLogLevel LogLevel { get; private set; } = ProcessLogLevel.NONE;


        private static CLogProcessor iinstance
        {
            get
            {
                lock (singletonPadlock)
                {
                    if (instance == null)
                    {
                        instance = new CLogProcessor();
                    }
                    return instance;
                }
            }
        }

        public static void Instantiate(bool _testMode, ProcessLogLevel _logLevel)
        {
            TestMode = _testMode;
            LogLevel = _logLevel;
        }

        private CLogProcessor()
        {

        }

        private Dictionary<string, CLogSession> LoggingSessions = new Dictionary<string, CLogSession>();

        public static CLogSession GetSession(string sessionName)
        {
            lock (MainLogLock)
            {
                if (!iinstance.LoggingSessions.ContainsKey(sessionName))
                {
                    iinstance.LoggingSessions.Add(sessionName, new CLogSession(sessionName));
                }
            }

            return iinstance.LoggingSessions[sessionName];
        }
    }
}
