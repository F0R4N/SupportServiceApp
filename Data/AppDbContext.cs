using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Note> Notes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(
            "Host=localhost;Port=5432;Database=support_service_db;Username=postgres;Password=1234");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Таблицы
        modelBuilder.Entity<Client>().ToTable("clients");
        modelBuilder.Entity<Equipment>().ToTable("equipment");
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Ticket>().ToTable("tickets");
        modelBuilder.Entity<Note>().ToTable("notes");

        // Колонки
        modelBuilder.Entity<Client>().Property(c => c.Id).HasColumnName("id");
        modelBuilder.Entity<Client>().Property(c => c.Name).HasColumnName("name");
        modelBuilder.Entity<Client>().Property(c => c.Phone).HasColumnName("phone");
        modelBuilder.Entity<Client>().Property(c => c.Email).HasColumnName("email");

        modelBuilder.Entity<Equipment>().Property(e => e.Id).HasColumnName("id");
        modelBuilder.Entity<Equipment>().Property(e => e.Name).HasColumnName("name");
        modelBuilder.Entity<Equipment>().Property(e => e.InventoryNumber).HasColumnName("inventory_number");

        modelBuilder.Entity<Category>().Property(c => c.Id).HasColumnName("id");
        modelBuilder.Entity<Category>().Property(c => c.Name).HasColumnName("name");

        modelBuilder.Entity<Ticket>().Property(t => t.Id).HasColumnName("id");
        modelBuilder.Entity<Ticket>().Property(t => t.Title).HasColumnName("title");
        modelBuilder.Entity<Ticket>().Property(t => t.Description).HasColumnName("description");
        modelBuilder.Entity<Ticket>().Property(t => t.CreatedDate).HasColumnName("created_date");
        modelBuilder.Entity<Ticket>().Property(t => t.ClosedDate).HasColumnName("closed_date");
        modelBuilder.Entity<Ticket>().Property(t => t.IsActual).HasColumnName("is_actual");
        modelBuilder.Entity<Ticket>().Property(t => t.ClientId).HasColumnName("client_id");
        modelBuilder.Entity<Ticket>().Property(t => t.EquipmentId).HasColumnName("equipment_id");
        modelBuilder.Entity<Ticket>().Property(t => t.CategoryId).HasColumnName("category_id");

        modelBuilder.Entity<Note>().Property(n => n.Id).HasColumnName("id");
        modelBuilder.Entity<Note>().Property(n => n.TicketId).HasColumnName("ticket_id");
        modelBuilder.Entity<Note>().Property(n => n.Text).HasColumnName("text");
        modelBuilder.Entity<Note>().Property(n => n.CreatedAt).HasColumnName("created_at");
    }
}
