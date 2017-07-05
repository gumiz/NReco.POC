using System.Diagnostics;
using System.ServiceProcess;

namespace Nreco.Win
{
	public partial class NrecoService : ServiceBase
	{

		private const int NumberOfPagesToGenerate = 50;
		private const int WinServiceTimeIntervalInSeconds = 10;

		private readonly EventLog _eventLog1;
		private int _fileId;

		public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
		{
			var filename = $@"c:\temp\NRecoSample{_fileId++:D5}.pdf";
			_eventLog1.WriteEntry($"GeneratePdf operation started", EventLogEntryType.Information);
			var pdfBytes = new PrintEngine.PdfEngine().GeneratePdf(NumberOfPagesToGenerate);
			_eventLog1.WriteEntry($"GeneratePdf operation finished", EventLogEntryType.Information);
			System.IO.File.WriteAllBytes(filename, pdfBytes);

			_eventLog1.WriteEntry($"File: {filename} saved", EventLogEntryType.Information);
		}

		public NrecoService()
		{
			InitializeComponent();
			_fileId = 1;
			_eventLog1 = new EventLog();
			if (!EventLog.SourceExists("NRecoPOC"))
			{
				EventLog.CreateEventSource("NRecoPOC", "NRecoLog");
			}
			_eventLog1.Source = "NRecoPOC";
			_eventLog1.Log = "NRecoLog";
		}

		protected override void OnStart(string[] args)
		{
			_eventLog1.WriteEntry("NReco.POC.WinService started");
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Interval = 1000 * WinServiceTimeIntervalInSeconds;
			timer.Elapsed += OnTimer;
			timer.Start();
		}

		protected override void OnStop()
		{
			_eventLog1.WriteEntry("NReco.POC.WinService stopped.");
		}
	}
}
