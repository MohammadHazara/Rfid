using Rfid.Core.Models;

namespace Rfid.Core.Interfaces.Services
{
    public interface IRfidService
    {
        Task<Guid> AddAsync(RfidTokenDTO token);
        Task<RfidTokenDTO> GetByIdAsync(Guid id);
    }
}
