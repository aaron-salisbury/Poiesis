using GalaSoft.MvvmLight;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;

namespace Poiesis.Base
{
    public class AppLogger
    {
        public string LoggerPath { get; set; }
        public Logger Logger { get; set; }
        public InMemorySink InMemorySink { get; set; }

        public AppLogger()
        {
            LoggerPath = Path.Combine(Path.GetTempPath(), "Poiesis", "Log.txt");
            InMemorySink = new InMemorySink();

            Logger = new LoggerConfiguration()
                .WriteTo.Async(lsc => lsc.File(LoggerPath))
                .WriteTo.Async(lsc => lsc.Sink(InMemorySink))
                .CreateLogger();

            Logger.Information("Launched application.");
        }

        public void OpenLog()
        {
            try
            {
                System.Diagnostics.Process.Start(LoggerPath);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }

    // stackoverflow.com/questions/35567814/is-it-possible-to-display-serilog-log-in-the-programs-gui
    public class InMemorySink : ObservableObject, ILogEventSink
    {
        readonly ITextFormatter _textFormatter = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{Exception}", CultureInfo.InvariantCulture);

        public ConcurrentQueue<string> Events { get; } = new ConcurrentQueue<string>();

        private string _messages;
        public string Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) { throw new ArgumentNullException(nameof(logEvent)); }

            StringWriter renderSpace = new StringWriter();
            _textFormatter.Format(logEvent, renderSpace);
            string formattedLogEvent = renderSpace.ToString();
            Events.Enqueue(formattedLogEvent);

            if (Events.Count > 1)
            {
                Messages += (Environment.NewLine + formattedLogEvent);
            }
            else
            {
                Messages += formattedLogEvent;
            }
        }
    }
}