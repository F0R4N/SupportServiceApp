using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SupportServiceApp.Views
{
    public partial class MainWindow : Window
    {
        private List<Ticket> _allTickets;

        public MainWindow()
        {
            InitializeComponent();
            LoadTickets();
        }

        private void LoadTickets()
        {
            using (var db = new AppDbContext())
            {
                _allTickets = db.Tickets
                    .Include(t => t.Client)
                    .Include(t => t.Equipment)
                    .Include(t => t.Category)
                    .ToList();

                TicketsGrid.ItemsSource = _allTickets;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_allTickets == null)
                return;

            var searchText = SearchBox.Text.ToLower();

            // если поле пустое — показываем всё
            if (string.IsNullOrWhiteSpace(searchText))
            {
                TicketsGrid.ItemsSource = _allTickets;
                return;
            }

            var filtered = _allTickets
                .Where(t =>
                    (t.Equipment != null && t.Equipment.Name.ToLower().Contains(searchText)) ||
                    (t.Client != null && t.Client.Name.ToLower().Contains(searchText)) ||
                    (t.Description != null && t.Description.ToLower().Contains(searchText))
                )
                .ToList();

            TicketsGrid.ItemsSource = filtered;
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