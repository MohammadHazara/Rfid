namespace Rfid.Core.Models
{
    public class RfidTokenDTO
    {
        public Guid Id { get; set; }
        public DateOnly? ValidFrom { get; set; }
        public DateOnly? ValidToDate { get; set; }
    }
}
