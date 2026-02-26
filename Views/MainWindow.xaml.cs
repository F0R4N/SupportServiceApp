using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace SupportServiceApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadTickets();
        }

        private void LoadTickets()
        {
            using (var db = new AppDbContext())
            {
                TicketsGrid.ItemsSource = db.Tickets
                    .Include(t => t.Client)
                    .Include(t => t.Equipment)
                    .Include(t => t.Category)
                    .ToList();
            }
        }

        private void AddTicket_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditTicketWindow();
            if (window.ShowDialog() == true)
                LoadTickets();
        }

        private void EditTicket_Click(object sender, RoutedEventArgs e)
        {
            if (TicketsGrid.SelectedItem is Ticket selectedTicket)
            {
                var window = new AddEditTicketWindow(selectedTicket.Id);
                if (window.ShowDialog() == true)
                    LoadTickets();
            }
            else
                MessageBox.Show("Выберите заявку для редактирования.");
        }

        private void DeleteTicket_Click(object sender, RoutedEventArgs e)
        {
            if (TicketsGrid.SelectedItem is Ticket selectedTicket)
            {
                if (MessageBox.Show("Удалить заявку?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var db = new AppDbContext())
                    {
                        var ticket = db.Tickets.Find(selectedTicket.Id);
                        if (ticket != null)
                        {
                            db.Tickets.Remove(ticket);
                            db.SaveChanges();
                        }
                    }
                    LoadTickets();
                }
            }
            else
                MessageBox.Show("Выберите заявку для удаления.");
        }
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.ShowDialog();
        }
    }
}
