using System.Net.Sockets;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Ticket>? Tickets { get; set; }
}
