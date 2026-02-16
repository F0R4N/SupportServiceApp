public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool IsActual { get; set; }

    public int? ClientId { get; set; }
    public Client Client { get; set; }

    public int? EquipmentId { get; set; }
    public Equipment Equipment { get; set; }

    public int? CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Note>? Notes { get; set; }
}
