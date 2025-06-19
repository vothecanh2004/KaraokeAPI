namespace KaraokeAPI.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public double DiscountPercent { get; set; } // Ví dụ: 10 = 10%
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Tự kiểm tra còn hạn
        public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= EndDate;
    }
}
