namespace Rfid.Core.Interfaces.Services
{
    public interface ILogService
    {
        Task LogInfoAsync(Type type, string message);
        Task LogErrorAsync(Type type, string message, Exception ex);
    }
}
