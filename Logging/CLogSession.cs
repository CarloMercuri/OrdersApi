using OrdersApi.Logging.Models;
using System.Diagnostics;
using System.Reflection;

namespace OrdersApi.Logging
{
    public class CLogSession
    {
        public string SessionName { get; set; }

        private string LogDir;
        private string RawDataLogsFile;
        private string FullFilePath;
        private string WarningFilePath;
        private string SystemName = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private string ErrorLineHighLight = "==========================================================================";

        private object LogFileLock = new object();

        public CLogSession(string sessionName)
        {
            LogDir = @"\LogData\";
            RawDataLogsFile = @$"{sessionName}.txt";
            FullFilePath = Directory.GetCurrentDirectory() + LogDir + RawDataLogsFile;
            WarningFilePath = Directory.GetCurrentDirectory() + LogDir + @$"Warnings.txt";
            this.SessionName = sessionName;
            //Creates all missing directories to the filePath. If they already exist, nothing happens.
            Directory.CreateDirectory(Path.GetDirectoryName(FullFilePath));
        }

        public void LogStringsList(List<string> list)
        {
            try
            {
                WriteListToFile(list);
            }
            catch (Exception ex)
            {

            }
        }

        public void LogGeneral(string Message, ProcessLogLevel logLevel)
        {
            ErrorModel model = new ErrorModel();
            model.SessionName = SessionName;
            model.DeveloperMessage = Message;
            model.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.Title = SystemName;

            Task t = Task.Run(() => LogGeneralAction(model));
        }

        private async void LogGeneralAction(ErrorModel model)
        {
            try
            {
                // Log always
                await WriteGeneralToFile(model);
            }
            catch (Exception ex)
            {
            }
        }

        public void LogWarning(string warningMessage)
        {
            ErrorModel model = new ErrorModel();
            model.SessionName = SessionName;
            model.EventLevel = LogEventLevel.WARNING;
            model.DeveloperMessage = warningMessage;
            model.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.Title = SystemName + " WARNING";
            StackTrace trace = new System.Diagnostics.StackTrace();
            StackFrame[] frames = trace.GetFrames();
            model.FramesList = BuildFramesLog(frames);
            model.EncodedStackframe = EncodeListToString(model.FramesList);
            if (model.FramesList.Count > 0)
            {
                string failedMethod = model.FramesList[1];
                model.FailedMethodName = failedMethod.Split(' ')[1];

            }

            Task t = Task.Run(() => LogWarningAction(model));
        }

        private async void LogWarningAction(ErrorModel model)
        {
            try
            {
                WriteWarningToFile(model);
            }
            catch (Exception ex)
            {
            }

        }

        public void LogGeneralError(string message)
        {
            ErrorModel model = new ErrorModel();
            model.SessionName = SessionName;
            model.EventLevel = LogEventLevel.ERROR;
            model.FramesList = new List<string>();
            model.DeveloperMessage = message;
            model.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.Title = SystemName + " General Error";

            Task t = Task.Run(() => LogGeneralErrorAction(model));
        }

        private async Task LogGeneralErrorAction(ErrorModel model)
        {
            try
            {
                // Log always
                WriteErrorToFile(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }

        }

        public void LogException(string developerMessage, Exception ex)
        {
            StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            StackFrame[] frames = trace.GetFrames();
            ErrorModel model = new ErrorModel();
            model.SessionName = SessionName;
            model.EventLevel = LogEventLevel.EXCEPTION;
            List<string> framesLog = new List<string>();

            model.ExceptionMessage = ex.Message;
            framesLog.Add($"");

            framesLog.Add("");

            framesLog.AddRange(BuildFramesLog(frames));

            MethodBase b = frames[0].GetMethod();

            if (b.DeclaringType != null)
            {
                model.FailedMethodName = b.DeclaringType.Name + "." + b.Name;
            }
            else
            {
                model.FailedMethodName = frames[0].GetMethod().Name;
            }
            model.DeveloperMessage = developerMessage;
            model.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.FramesList = framesLog;
            model.EncodedStackframe = EncodeListToString(model.FramesList);
            model.Title = SystemName + " Exception";

            Task t = Task.Run(() => LogExceptionAction(model));
        }

        private async void LogExceptionAction(ErrorModel model)
        {
            try
            {
                // Log always
                await WriteExceptionToFile(model);
            }
            catch (Exception exc)
            {
                // Well... what now?
            }

        }

        private List<string> BuildFramesLog(StackFrame[] frames)
        {
            List<string> framesLog = new List<string>();

            if (frames is null)
            {
                framesLog.Add("Could not build stack frame");
                return framesLog;
            }

            if (frames.Length == 0)
            {
                framesLog.Add("Stack frame not available");
                return framesLog;
            }

            var method = frames[0].GetMethod();
            string methodName = frames[0].GetMethod().Name;
            string refType = method.ReflectedType.Name;
            string naSpace = method.ReflectedType.Namespace;
            string fileName = frames[0].GetFileName();
            string line = frames[0].GetFileLineNumber().ToString();



            framesLog.Add($"Exception happened in: {refType}.{methodName} at line: {line}");

            for (int i = 1; i < frames.Length; i++)
            {
                method = frames[i].GetMethod();
                methodName = frames[i].GetMethod().Name;
                if (method.ReflectedType != null)
                {
                    refType = method.ReflectedType.Name;
                    naSpace = method.ReflectedType.Namespace;
                }

                fileName = frames[i].GetFileName();
                line = frames[i].GetFileLineNumber().ToString();
                if (i == frames.Length - 1)
                {
                    framesLog.Add($"Exception caught at: {refType}.{methodName} at line: {line}");
                }
                else
                {
                    framesLog.Add($"{i}: {refType}.{methodName} at line: {line}");
                }

            }

            return framesLog;
        }

        private string EncodeListToString(List<string> list)
        {
            if (list is null)
            {
                return string.Empty;
            }
            string mainString = "";

            foreach (string s in list)
            {
                mainString += s + "\n\r";
            }

            return mainString;
        }

        private string FormatString(string message)
        {
            // DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "   " + message;
        }

        private async Task WriteExceptionToFile(ErrorModel model)
        {
            try
            {
                string encodedFrames = EncodeListToString(model.FramesList);
                lock (LogFileLock)
                {
                    //Uses StreamWriter to write data to file. 
                    using (StreamWriter writer = new StreamWriter(FullFilePath, true))
                    {
                        writer.WriteLine($"Exception at: {model.FailedMethodName}");
                        writer.WriteLine($"{model.TimeStamp}");
                        writer.WriteLine($"Session: {model.SessionName}");
                        writer.WriteLine($"Exception Message: {model.ExceptionMessage}");
                        writer.WriteLine($"Developer message: {model.DeveloperMessage}");
                        writer.WriteLine($"Stackframe: {encodedFrames}");
                        writer.WriteLine(ErrorLineHighLight);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void WriteErrorToFile(ErrorModel model)
        {
            try
            {
                lock (LogFileLock)
                {
                    //Uses StreamWriter to write data to file. 
                    using (StreamWriter writer = new StreamWriter(FullFilePath, true))
                    {
                        writer.WriteLine(ErrorLineHighLight);
                        writer.WriteLine($"{model.TimeStamp}  Title: {model.Title}");
                        writer.WriteLine($"{model.TimeStamp}  Session: {model.SessionName}");
                        writer.WriteLine($"{model.TimeStamp}  Error message: {model.DeveloperMessage}");
                        writer.WriteLine(ErrorLineHighLight);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void WriteWarningToFile(ErrorModel model)
        {
            try
            {
                lock (LogFileLock)
                {
                    //Uses StreamWriter to write data to file. 
                    using (StreamWriter writer = new StreamWriter(WarningFilePath, true))
                    {
                        writer.WriteLine($"{model.TimeStamp}");
                        writer.WriteLine($"Session: {model.SessionName}");
                        writer.WriteLine($"Warning message: {model.DeveloperMessage}");
                        writer.WriteLine(ErrorLineHighLight);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        private async Task WriteGeneralToFile(ErrorModel model)
        {
            try
            {
                lock (LogFileLock)
                {
                    //Uses StreamWriter to write data to file. 
                    using (StreamWriter writer = new StreamWriter(FullFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  {model.DeveloperMessage}");
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }
       
        private void WriteListToFile(List<string> list)
        {
            lock (LogFileLock)
            {
                //Uses StreamWriter to write data to file. 
                using (StreamWriter writer = new StreamWriter(FullFilePath, true))
                {
                    foreach (string s in list)
                    {
                        writer.WriteLine(s);
                    }

                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }
}
