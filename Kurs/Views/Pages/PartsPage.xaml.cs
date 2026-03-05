using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;
using Kurs.Views.Dialogs;
using Microsoft.EntityFrameworkCore;

namespace Kurs.Views.Pages
{
    public partial class PartsPage : Page
    {
        private List<Part> _all = new();
        public PartsPage() { InitializeComponent(); Load(); }

        private void Load()
        {
            using var db = new CarServiceContext();
            _all = db.Parts.Include(p => p.Supplier).OrderBy(p => p.Name).ToList();
            var _list_p = _all.ToList(); GridParts.ItemsSource = _list_p; TxtCount.Text = $"Всего записей: {_list_p.Count}";
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var q = TxtSearch.Text.ToLower();
            GridParts.ItemsSource = string.IsNullOrEmpty(q) ? _all
                : _all.Where(x => x.Name.ToLower().Contains(q) || (x.Supplier?.Name.ToLower().Contains(q) ?? false)).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e) { if (new PartDialog(null).ShowDialog() == true) Load(); }
        private void BtnEdit_Click(object sender, RoutedEventArgs e) { if ((sender as Button)?.DataContext is Part p && new PartDialog(p.ID_Parts).ShowDialog() == true) Load(); }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not Part p) return;
            if (MessageBox.Show($"Удалить запчасть «{p.Name}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try { using var db = new CarServiceContext(); var entity = db.Parts.Find(p.ID_Parts); if (entity != null) { db.Parts.Remove(entity); db.SaveChanges(); } Load(); }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        // Поступление на склад
        private void BtnReceive_Click(object sender, RoutedEventArgs e)
        {
            if (new StockMovementDialog("Приход").ShowDialog() == true) Load();
        }

        // Списание со склада
        private void BtnWriteOff_Click(object sender, RoutedEventArgs e)
        {
            if (new StockMovementDialog("Списание").ShowDialog() == true) Load();
        }
    }
}
