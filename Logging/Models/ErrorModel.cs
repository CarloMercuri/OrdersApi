namespace OrdersApi.Logging.Models
{
    public class ErrorModel
    {
        public string Title { get; set; } = "";
        public string FailedMethodName { get; set; } = "";
        public string SessionName { get; set; } = "";
        public string TimeStamp { get; set; } = "";
        public string DeveloperMessage { get; set; } = "";
        public string ExceptionMessage { get; set; } = "";

        public LogEventLevel EventLevel { get; set; } = LogEventLevel.NORMAL;

        public string EncodedStackframe { get; set; } = "";
        public List<string> FramesList { get; set; } = new List<string>();
    }
}
