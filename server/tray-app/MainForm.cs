using System.Diagnostics;
using System.Windows.Forms;

namespace tray_app;

public partial class MainForm : Form
{
	private static Mutex _mutex;
	private NotifyIcon _notifyIcon;
	private Process _serverProcess;
	private ToolStripMenuItem _startMenuItem;
	private ToolStripMenuItem _stopMenuItem;

	public MainForm()
	{
		InitializeComponent();
		
		const string appName = "Easy-pix";
		bool isNewInstance;
		_mutex = new Mutex(true, appName, out isNewInstance);

		if (!isNewInstance)
		{
			MessageBox.Show("Another instance of the application is already running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			OnExit(null, null);
			return;
		}
		
		SetupTrayIcon();

		StartWebAPI();
	}
	
	private void SetupTrayIcon()
	{
		_notifyIcon = new NotifyIcon
		{
			Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "appicon.ico")),
			Visible = true,
			ContextMenuStrip = new ContextMenuStrip()
		};

		_startMenuItem = new ToolStripMenuItem("Start", null, OnStart) { Visible = false };
		_stopMenuItem = new ToolStripMenuItem("Stop", null, OnStop);

		_notifyIcon.ContextMenuStrip.Items.Add(_startMenuItem);
		_notifyIcon.ContextMenuStrip.Items.Add(_stopMenuItem);
		_notifyIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
	}

	private void OnExit(object sender, EventArgs e)
	{
		StopServer();
		_notifyIcon.Visible = false;
		// Освобождаем мьютекс только если он был захвачен
		if (_mutex != null && _mutex.WaitOne(0, false))
		{
			_mutex.ReleaseMutex();
		}
		Application.Exit();
	}

	private void OnStart(object sender, EventArgs e)
	{
		StartWebAPI();
		_startMenuItem.Visible = false;
		_stopMenuItem.Visible = true;
	}

	private void OnStop(object sender, EventArgs e)
	{
		StopServer();
		_startMenuItem.Visible = true;
		_stopMenuItem.Visible = false;
	}

	private void StartWebAPI()
	{
		if (_serverProcess == null || _serverProcess.HasExited)
		{
			try
			{
				// Получаем текущий путь приложения (tray-app)
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

				// Поднимаемся на несколько уровней вверх к папке "easy-pix" и добавляем путь к server.exe
				string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\server\bin\Debug\net8.0-windows"));
				string serverPath = Path.Combine(projectDirectory, "server.exe");

				// Проверка существования файла
				if (!File.Exists(serverPath))
				{
					MessageBox.Show("Server application not found at path: " + serverPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				_serverProcess = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = serverPath,
						WorkingDirectory = Path.GetDirectoryName(serverPath),
						UseShellExecute = false, 
						CreateNoWindow = true,  // Если не хотите показывать окно консоли
					}
				};
				_serverProcess.EnableRaisingEvents = true;
				_serverProcess.Exited += (sender, args) =>
				{
					_startMenuItem.Visible = true;
					_stopMenuItem.Visible = false;
				};
				_serverProcess.StartInfo.EnvironmentVariables["PARENT_PROCESS_ID"] = $"{Process.GetCurrentProcess().Id}";
				_serverProcess.Start();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error starting the server: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}

	private void StopServer()
	{
		if (_serverProcess != null && !_serverProcess.HasExited)
		{
			try
			{
				_serverProcess.Kill();
				_serverProcess.Dispose();
				_serverProcess = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error stoping the server: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		this.WindowState = FormWindowState.Minimized;
		this.ShowInTaskbar = false;
	}
	protected override void OnShown(EventArgs e)
	{
		base.OnShown(e);
		this.Hide();
	}
	[STAThread]
	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainForm());
	}
}
