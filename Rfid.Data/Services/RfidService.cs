using Rfid.Core.Interfaces.Repositories;
using Rfid.Core.Interfaces.Services;
using Rfid.Core.Models;

namespace Rfid.Data.Services
{
    public class RfidService : IRfidService
    {
        private IRfidRepository _repo;
        private ILogService _logger;

        public RfidService(IRfidRepository rfidRepository, ILogService logger)
        {
            _repo = rfidRepository;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(RfidTokenDTO token)
        {
            try
            {
                _ = _logger.LogInfoAsync(GetType(), "Adding token");
                if(token == null)
                {
                    throw new ArgumentNullException(nameof(token));
                }
                if(token.Id == Guid.Empty)
                {
                    token.Id = Guid.NewGuid();
                }
                token.ValidFrom ??= DateOnly.FromDateTime(DateTime.Today);
                return await _repo.AddAsync(token);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(GetType(), "Error adding token", ex);
                throw;
            }
        }

        public async Task<RfidTokenDTO> GetByIdAsync(Guid id)
        {
            try
            {
                _ = _logger.LogInfoAsync(GetType(), "Getting token");
                return await _repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(GetType(), "Error getting token", ex);
                throw;
            }
        }
    }
}
