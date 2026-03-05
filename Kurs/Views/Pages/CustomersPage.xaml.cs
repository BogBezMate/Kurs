using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;
using Kurs.Views.Dialogs;

namespace Kurs.Views.Pages
{
    public partial class CustomersPage : Page
    {
        private List<Customer> _all = new();
        public CustomersPage() { InitializeComponent(); Load(); }

        private void Load()
        {
            using var db = new CarServiceContext();
            _all = db.Customers.OrderBy(c => c.Last_Name).ToList();
            var _list_c = _all.ToList(); GridCustomers.ItemsSource = _list_c; TxtCount.Text = $"Всего записей: {_list_c.Count}";
            TxtCount.Text = $"Всего записей: {((System.Collections.ICollection)_all.ToList()).Count}";
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var q = TxtSearch.Text.ToLower();
            GridCustomers.ItemsSource = string.IsNullOrEmpty(q) ? _all
                : _all.Where(x => x.FullName.ToLower().Contains(q) || (x.Phone?.Contains(q) ?? false)).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        { if (new CustomerDialog(null).ShowDialog() == true) Load(); }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        { if ((sender as Button)?.DataContext is Customer c && new CustomerDialog(c.ID_Customers).ShowDialog() == true) Load(); }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not Customer c) return;
            if (MessageBox.Show($"Удалить клиента {c.FullName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try
            {
                using var db = new CarServiceContext();
                var entity = db.Customers.Find(c.ID_Customers);
                if (entity != null) { db.Customers.Remove(entity); db.SaveChanges(); }
                Load();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка удаления:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
