using System.Windows;
using Kurs.Data;
using Kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace Kurs.Views.Dialogs
{
    public partial class AssignEmployeeDialog : Window
    {
        private readonly int _orderId;
        public AssignEmployeeDialog(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            using var db = new CarServiceContext();
            // Только тех кто ещё не назначен
            var assigned = db.WorkOrderEmployees.Where(x => x.ID_WorkOrders == orderId).Select(x => x.ID_Employees).ToList();
            CmbEmployee.ItemsSource = db.Employees.Where(e => !assigned.Contains(e.ID_Employees)).OrderBy(e => e.Last_Name).ToList();
        }
        private void BtnAssign_Click(object sender, RoutedEventArgs e)
        {
            if (CmbEmployee.SelectedValue == null) { MessageBox.Show("Выберите сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            try
            {
                using var db = new CarServiceContext();
                db.WorkOrderEmployees.Add(new WorkOrderEmployee { ID_WorkOrders = _orderId, ID_Employees = (int)CmbEmployee.SelectedValue });
                db.SaveChanges(); DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
