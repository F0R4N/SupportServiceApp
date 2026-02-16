using System.Linq;
using System.Windows;

namespace SupportServiceApp.Views
{
    public partial class AddEditTicketWindow : Window
    {
        private int? ticketId;

        public AddEditTicketWindow(int? id = null)
        {
            InitializeComponent();
            ticketId = id;

            LoadData();

            if (ticketId.HasValue)
                LoadTicket();
        }

        // Загрузка клиентов, оборудования и категорий
        private void LoadData()
        {
            using (var db = new AppDbContext())
            {
                ClientCombo.ItemsSource = db.Clients.ToList();
                EquipmentCombo.ItemsSource = db.Equipment.ToList();
                CategoryCombo.ItemsSource = db.Categories.ToList();
            }
        }

        // Загрузка данных существующей заявки
        private void LoadTicket()
        {
            using (var db = new AppDbContext())
            {
                var ticket = db.Tickets
                    .Where(t => t.Id == ticketId.Value)
                    .FirstOrDefault();

                if (ticket != null)
                {
                    TitleBox.Text = ticket.Title ?? "";
                    DescriptionBox.Text = ticket.Description ?? "";
                    ClientCombo.SelectedItem = ticket.Client;
                    EquipmentCombo.SelectedItem = ticket.Equipment;
                    CategoryCombo.SelectedItem = ticket.Category;
                }
            }
        }

        // Сохранение новой или редактируемой заявки
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();
            Ticket ticket;

            if (ticketId.HasValue)
                ticket = db.Tickets.Find(ticketId.Value)!;
            else
            {
                ticket = new Ticket
                {
                    CreatedDate = DateTime.UtcNow, // <-- UTC
                    IsActual = true
                };
                db.Tickets.Add(ticket);
            }

            ticket.Title = TitleBox.Text;
            ticket.Description = DescriptionBox.Text;

            ticket.ClientId = (ClientCombo.SelectedItem as Client)?.Id;
            ticket.EquipmentId = (EquipmentCombo.SelectedItem as Equipment)?.Id;
            ticket.CategoryId = (CategoryCombo.SelectedItem as Category)?.Id;

            db.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}