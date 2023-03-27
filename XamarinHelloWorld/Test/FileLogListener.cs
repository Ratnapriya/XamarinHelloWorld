using Abbott.Services.Platform.Common.ServiceLocator;
using Abbott.Services.Platform.File;
using Abbott.Services.Platform.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PatientApp.Logging    
{
    public class FileLogListener : LogListener
    {
        static readonly IFileSystem fileServices = ServiceUtil.Get<IFileSystem>();

        static readonly string LogFileName = fileServices.LocalStorage.FullPath + "/app.log";

        readonly Thread flushThread;

        BlockingCollection<LogEntry> persistanceQueue = new BlockingCollection<LogEntry>(new ConcurrentQueue<LogEntry>());

        object locker = new object();

        #region Constructors

        public FileLogListener()
        {
            flushThread = new Thread(new ThreadStart(FlushLogsToFile));
            flushThread.Start();
        }

        #endregion
        #region Overrides

        public override void Write(LogEntry logEntry)
        {
            persistanceQueue.Add(logEntry);
        }

        #endregion
        #region Public Methods

        public void DeleteLogFile()
        {
            lock (locker)
            {
                System.IO.File.Delete(LogFileName);
            }
        }

        #endregion
        #region Private Methods

        void FlushLogsToFile()
        {
            while (true)
            {
                // Take an item from the queue or wait until one is available
                if (persistanceQueue.TryTake(out var logEntry, -1))
                {
                    // Take up to 19 elements from the backlog
                    var logLines = new List<string>() { logEntry.FormattedMessage };

                    while (persistanceQueue.Count > 0 && logLines.Count < 19)
                    {
                        logLines.Add(persistanceQueue.Take().FormattedMessage);
                    }

                    try
                    {
                        lock (locker)
                        {
                            fileServices.AppendToFile(LogFileName, logLines);
                        }
                    }
                    catch (IOException e)
                    {
                        // Losing log entries is not cool, need to make sure this doesn't happen
                        Debug.WriteLine($"Error writing logs to file: {e.Message}");
                    }
                }
            }
        }

        #endregion
    }
}