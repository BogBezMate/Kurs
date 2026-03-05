using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace Kurs.Views.Dialogs
{
    public partial class WorkOrderDialog : Window
    {
        private readonly int? _id;

        public WorkOrderDialog(int? id)
        {
            InitializeComponent();
            _id = id;
            TxtTitle.Text = id == null ? "Новый заказ-наряд" : "Редактировать заказ-наряд";
            CmbStatus.ItemsSource = new[] { "Открыт", "В работе", "Закрыт", "Отменён" };
            CmbStatus.SelectedIndex = 0;

            using var db = new CarServiceContext();
            CmbCustomer.ItemsSource = db.Customers.OrderBy(c => c.Last_Name).ToList();

            if (id == null)
            {
                // Автоматический номер
                var lastNum = db.WorkOrders.Count() + 1;
                TxtNumber.Text = $"НАР-{lastNum:D5}";
            }
            else LoadData(id.Value);
        }

        private void LoadData(int id)
        {
            using var db = new CarServiceContext();
            var o = db.WorkOrders.Find(id); if (o == null) return;
            TxtNumber.Text = o.Number;
            CmbCustomer.SelectedValue = o.ID_Customers;
            // Автомобили клиента загрузим через SelectionChanged
            CmbVehicle.SelectedValue = o.ID_Vehicles;
            CmbStatus.SelectedItem = o.Status;
            TxtNotes.Text = o.Notes;
        }

        private void CmbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbCustomer.SelectedValue is not int custId) return;
            using var db = new CarServiceContext();
            CmbVehicle.ItemsSource = db.Vehicles.Where(v => v.ID_Customers == custId).ToList();
            CmbVehicle.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNumber.Text) || CmbCustomer.SelectedValue == null || CmbVehicle.SelectedValue == null)
            { MessageBox.Show("Заполните обязательные поля (*).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            try
            {
                using var db = new CarServiceContext();
                if (_id == null)
                {
                    db.WorkOrders.Add(new WorkOrder
                    {
                        Number = TxtNumber.Text.Trim(),
                        ID_Customers = (int)CmbCustomer.SelectedValue,
                        ID_Vehicles = (int)CmbVehicle.SelectedValue,
                        Date_Open = DateTime.Today,
                        Status = CmbStatus.SelectedItem?.ToString() ?? "Открыт",
                        Notes = string.IsNullOrWhiteSpace(TxtNotes.Text) ? null : TxtNotes.Text.Trim()
                    });
                }
                else
                {
                    var o = db.WorkOrders.Find(_id.Value); if (o == null) return;
                    o.Number = TxtNumber.Text.Trim();
                    o.ID_Customers = (int)CmbCustomer.SelectedValue;
                    o.ID_Vehicles = (int)CmbVehicle.SelectedValue;
                    o.Status = CmbStatus.SelectedItem?.ToString() ?? o.Status;
                    o.Notes = string.IsNullOrWhiteSpace(TxtNotes.Text) ? null : TxtNotes.Text.Trim();
                }
                db.SaveChanges(); DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
