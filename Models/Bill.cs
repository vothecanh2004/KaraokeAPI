using System.ComponentModel.DataAnnotations.Schema;

namespace KaraokeAPI.Models
{
    public class Bill
    {
        public int Id { get; set; }

        // Foreign keys
        public int RoomId { get; set; }
        public int BookingId { get; set; }

        // Số tiền cần thanh toán
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Thời gian tạo hóa đơn
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Room? Room { get; set; }
        public Booking? Booking { get; set; }
    }
}
