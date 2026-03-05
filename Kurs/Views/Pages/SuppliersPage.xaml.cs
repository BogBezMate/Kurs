using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;
using Kurs.Views.Dialogs;

namespace Kurs.Views.Pages
{
    public partial class SuppliersPage : Page
    {
        private List<Supplier> _all = new();
        public SuppliersPage() { InitializeComponent(); Load(); }
        private void Load() { using var db = new CarServiceContext(); _all = db.Suppliers.OrderBy(s => s.Name).ToList(); var _list_s = _all.ToList(); GridSuppliers.ItemsSource = _list_s; TxtCount.Text = $"Всего записей: {_list_s.Count}"; }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) { var q = TxtSearch.Text.ToLower(); var _list_s = string.IsNullOrEmpty(q) ? _all : _all.Where(x => x.Name.ToLower().Contains(q) || (x.Phone?.Contains(q) ?? false)).ToList(); GridSuppliers.ItemsSource = _list_s; TxtCount.Text = $"Всего записей: {_list_s.Count}"; }
        private void BtnAdd_Click(object sender, RoutedEventArgs e) { if (new SupplierDialog(null).ShowDialog() == true) Load(); }
        private void BtnEdit_Click(object sender, RoutedEventArgs e) { if ((sender as Button)?.DataContext is Supplier s && new SupplierDialog(s.ID_Suppliers).ShowDialog() == true) Load(); }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not Supplier s) return;
            if (MessageBox.Show($"Удалить поставщика «{s.Name}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try { using var db = new CarServiceContext(); var entity = db.Suppliers.Find(s.ID_Suppliers); if (entity != null) { db.Suppliers.Remove(entity); db.SaveChanges(); } Load(); }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
