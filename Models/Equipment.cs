using System.Net.Sockets;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? InventoryNumber { get; set; }

    public ICollection<Ticket>? Tickets { get; set; }
}
