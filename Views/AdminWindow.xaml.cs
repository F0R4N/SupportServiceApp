using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace SupportServiceApp.Views
{
    public partial class AdminWindow : Window
    {
        private AppDbContext _context;

        public AdminWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadData();
        }

        // Загрузка данных из базы
        private void LoadData()
        {
            ClientsListBox.Items.Clear();
            foreach (var client in _context.Clients.ToList())
                ClientsListBox.Items.Add(client);

            EquipmentListBox.Items.Clear();
            foreach (var eq in _context.Equipment.ToList())
                EquipmentListBox.Items.Add(eq);

            CategoryListBox.Items.Clear();
            foreach (var cat in _context.Categories.ToList())
                CategoryListBox.Items.Add(cat);
        }

        // Подсказка для TextBox
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as System.Windows.Controls.TextBox;
            if (tb.Text.StartsWith("Введите"))
                tb.Text = "";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as System.Windows.Controls.TextBox;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                if (tb.Name == "ClientTextBox") tb.Text = "Введите имя клиента";
                else if (tb.Name == "EquipmentTextBox") tb.Text = "Введите название оборудования";
                else if (tb.Name == "CategoryTextBox") tb.Text = "Введите категорию";
            }
        }

        // Добавление клиента
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            string name = ClientTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(name) && name != "Введите имя клиента")
            {
                var client = new Client { Name = name };
                _context.Clients.Add(client);
                _context.SaveChanges();
                ClientsListBox.Items.Add(client);
                ClientTextBox.Text = "Введите имя клиента";
            }
        }

        // Удаление клиента
        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsListBox.SelectedItem is Client client)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
                ClientsListBox.Items.Remove(client);
            }
        }

        // Добавление оборудования
        private void AddEquipment_Click(object sender, RoutedEventArgs e)
        {
            string name = EquipmentTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(name) && name != "Введите название оборудования")
            {
                var eq = new Equipment { Name = name };
                _context.Equipment.Add(eq);
                _context.SaveChanges();
                EquipmentListBox.Items.Add(eq);
                EquipmentTextBox.Text = "Введите название оборудования";
            }
        }

        private void DeleteEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentListBox.SelectedItem is Equipment eq)
            {
                _context.Equipment.Remove(eq);
                _context.SaveChanges();
                EquipmentListBox.Items.Remove(eq);
            }
        }

        // Добавление категории
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = CategoryTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(name) && name != "Введите категорию")
            {
                var cat = new Category { Name = name };
                _context.Categories.Add(cat);
                _context.SaveChanges();
                CategoryListBox.Items.Add(cat);
                CategoryTextBox.Text = "Введите категорию";
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryListBox.SelectedItem is Category cat)
            {
                _context.Categories.Remove(cat);
                _context.SaveChanges();
                CategoryListBox.Items.Remove(cat);
            }
        }

        // Закрытие окна
        protected override void OnClosed(System.EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}