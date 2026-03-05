using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;
using Kurs.Views.Dialogs;

namespace Kurs.Views.Pages
{
    public partial class EmployeesPage : Page
    {
        private List<Employee> _all = new();
        public EmployeesPage() { InitializeComponent(); Load(); }

        private void Load()
        {
            using var db = new CarServiceContext();
            _all = db.Employees.OrderBy(e => e.Last_Name).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var q = TxtSearch.Text.ToLower();
            var list = string.IsNullOrEmpty(q) ? _all
                : _all.Where(x => x.FullName.ToLower().Contains(q) ||
                                   (x.Position?.ToLower().Contains(q) ?? false) ||
                                   (x.Phone?.Contains(q) ?? false)).ToList();
            GridEmployees.ItemsSource = list;
            TxtCount.Text = $"Всего записей: {list.Count}";
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        { if (new EmployeeDialog(null).ShowDialog() == true) Load(); }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        { if ((sender as Button)?.DataContext is Employee emp && new EmployeeDialog(emp.ID_Employees).ShowDialog() == true) Load(); }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not Employee emp) return;
            if (MessageBox.Show($"Удалить сотрудника {emp.FullName}?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try
            {
                using var db = new CarServiceContext();
                var entity = db.Employees.Find(emp.ID_Employees);
                if (entity != null) { db.Employees.Remove(entity); db.SaveChanges(); }
                Load();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
