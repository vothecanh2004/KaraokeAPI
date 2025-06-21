namespace KaraokeAPI.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public required string Name { get; set; }          
        public int Rating { get; set; }                     
        public string? Comment { get; set; }               
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Room? Room { get; set; }
    }
}