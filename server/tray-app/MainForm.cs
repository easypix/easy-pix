using System.Diagnostics;
using System.Windows.Forms;

namespace tray_app;

public partial class MainForm : Form
{
	private NotifyIcon _notifyIcon;
	private Process _serverProcess;
	private ToolStripMenuItem _startMenuItem;
	private ToolStripMenuItem _stopMenuItem;

	public MainForm()
	{
		InitializeComponent();
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
						CreateNoWindow = true  // Если не хотите показывать окно консоли
					}
				};

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

	[STAThread]
	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainForm());
	}
}
