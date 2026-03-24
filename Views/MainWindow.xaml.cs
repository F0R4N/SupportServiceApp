using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using Microsoft.Win32;

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

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            var currentData = TicketsGrid.ItemsSource as IEnumerable<Ticket>;

            if (currentData == null || !currentData.Any())
            {
                MessageBox.Show("Нет данных для экспорта");
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "Tickets.xlsx"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Заявки");

                // Новый порядок колонок
                worksheet.Cell(1, 1).Value = "Дата";
                worksheet.Cell(1, 2).Value = "Заголовок";
                worksheet.Cell(1, 3).Value = "Клиент";
                worksheet.Cell(1, 4).Value = "Описание";
                worksheet.Cell(1, 5).Value = "Оборудование";
                worksheet.Cell(1, 6).Value = "Категория";
                worksheet.Cell(1, 7).Value = "ID";

                int row = 2;

                // СОРТИРОВКА: от старых к новым
                foreach (var t in currentData.OrderBy(t => t.CreatedDate))
                {
                    worksheet.Cell(row, 1).Value = t.CreatedDate;
                    worksheet.Cell(row, 2).Value = t.Title;
                    worksheet.Cell(row, 3).Value = t.Client?.Name;
                    worksheet.Cell(row, 4).Value = t.Description;
                    worksheet.Cell(row, 5).Value = t.Equipment?.Name;
                    worksheet.Cell(row, 6).Value = t.Category?.Name;
                    worksheet.Cell(row, 7).Value = t.Id;

                    row++;
                }

                // Формат даты (нормальный вид)
                worksheet.Column(1).Style.DateFormat.Format = "dd.MM.yyyy HH:mm";

                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(saveFileDialog.FileName);
            }

            MessageBox.Show("Экспорт завершён");
        }
    }
}