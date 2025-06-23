using System.ComponentModel.DataAnnotations.Schema;

namespace KaraokeAPI.Models
{
    public class Bill
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public int BookingId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // ✅ Đảm bảo có thuộc tính CreatedAt
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ✅ Nếu cần theo logic bạn trước đó
        public string Status { get; set; } = "Chưa thanh toán";

        public Room? Room { get; set; }
        public Booking? Booking { get; set; }
    }
}
