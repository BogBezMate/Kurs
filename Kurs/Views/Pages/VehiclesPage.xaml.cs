using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;
using Kurs.Views.Dialogs;
using Microsoft.EntityFrameworkCore;

namespace Kurs.Views.Pages
{
    public partial class VehiclesPage : Page
    {
        private List<Vehicle> _all = new();
        public VehiclesPage() { InitializeComponent(); Load(); }
        private void Load() { using var db = new CarServiceContext(); _all = db.Vehicles.Include(v => v.Customer).OrderBy(v => v.Brand).ToList(); var _list_v = _all.ToList(); GridVehicles.ItemsSource = _list_v; TxtCount.Text = $"Всего записей: {_list_v.Count}"; }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) { var q = TxtSearch.Text.ToLower(); var _list_v = string.IsNullOrEmpty(q) ? _all : _all.Where(x => (x.Brand?.ToLower().Contains(q) ?? false) || (x.Model?.ToLower().Contains(q) ?? false) || x.Plate_Number.ToLower().Contains(q) || x.Customer.FullName.ToLower().Contains(q)).ToList(); GridVehicles.ItemsSource = _list_v; TxtCount.Text = $"Всего записей: {_list_v.Count}"; }
        private void BtnAdd_Click(object sender, RoutedEventArgs e) { if (new VehicleDialog(null).ShowDialog() == true) Load(); }
        private void BtnEdit_Click(object sender, RoutedEventArgs e) { if ((sender as Button)?.DataContext is Vehicle v && new VehicleDialog(v.ID_Vehicles).ShowDialog() == true) Load(); }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not Vehicle v) return;
            if (MessageBox.Show($"Удалить автомобиль {v.DisplayName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try { using var db = new CarServiceContext(); var entity = db.Vehicles.Find(v.ID_Vehicles); if (entity != null) { db.Vehicles.Remove(entity); db.SaveChanges(); } Load(); }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
