using KaraokeAPI.Models;
public class FoodOrder
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int FoodId { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderedAt { get; set; } = DateTime.Now;

    public Room? Room { get; set; }
    public Food? Food { get; set; }
}
