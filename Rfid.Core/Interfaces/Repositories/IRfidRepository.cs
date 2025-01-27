using Rfid.Core.Models;

namespace Rfid.Core.Interfaces.Repositories
{
    public interface IRfidRepository
    {
        Task<Guid> AddAsync(RfidTokenDTO rfidToken);
        Task<RfidTokenDTO> GetByIdAsync(Guid id);
    }
}
