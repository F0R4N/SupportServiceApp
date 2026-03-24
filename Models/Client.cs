using System.Net.Sockets;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Ticket>? Tickets { get; set; }
}
