using Rfid.Core.Interfaces.Services;

namespace Rfid.Data.Services
{
    public class LogService : ILogService
    {
        public Task LogInfoAsync(Type type, string message)
        {
            throw new NotImplementedException();
        }

        public Task LogErrorAsync(Type type, string message, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
