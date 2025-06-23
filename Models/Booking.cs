namespace KaraokeAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime BookingTime { get; set; }
        public int DurationHours { get; set; }
        public int RoomId { get; set; } // để tạo hóa đơn đúng
        public Room? Room { get; set; }
    }
}
