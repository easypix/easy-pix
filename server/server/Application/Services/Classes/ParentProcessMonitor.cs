using System.Diagnostics;

namespace server.Application.Services.Classes;

public class ParentProcessMonitor
{
    private Timer _timer;
    private Process _parentProcess;

    public ParentProcessMonitor(int parentProcessId)
    {
        try
        {
            _parentProcess = Process.GetProcessById(parentProcessId);
        }
        catch (Exception ex)
        {
            // Логирование ошибки при получении процесса
            Console.WriteLine($"Ошибка при получении процесса: {ex.Message}");
        }

        // Запуск таймера для проверки каждые 5 секунд
        _timer = new Timer(CheckParentProcess, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    private void CheckParentProcess(object state)
    {
        if (_parentProcess == null || _parentProcess.HasExited)
        {
            // Родительский процесс завершен — завершаем сервер
            Console.WriteLine("Родительский процесс завершился. Завершение работы сервера...");
            Environment.Exit(0); // Завершение WebAPI
        }
    }
}