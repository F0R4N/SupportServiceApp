using System;
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
                ClientCombo.DisplayMemberPath = "Name";
                ClientCombo.SelectedValuePath = "Id";

                EquipmentCombo.ItemsSource = db.Equipment.ToList();
                EquipmentCombo.DisplayMemberPath = "Name";
                EquipmentCombo.SelectedValuePath = "Id";

                CategoryCombo.ItemsSource = db.Categories.ToList();
                CategoryCombo.DisplayMemberPath = "Name";
                CategoryCombo.SelectedValuePath = "Id";
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

                    // Автовыбор клиента, оборудования и категории по Id
                    ClientCombo.SelectedValue = ticket.ClientId;
                    EquipmentCombo.SelectedValue = ticket.EquipmentId;
                    CategoryCombo.SelectedValue = ticket.CategoryId;
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
                    CreatedDate = DateTime.UtcNow,
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