using System.Windows;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class VehicleDialog : Window
    {
        private readonly int? _id;
        public VehicleDialog(int? id)
        {
            InitializeComponent();
            _id = id;
            TxtTitle.Text = id == null ? "Новый автомобиль" : "Редактировать автомобиль";
            using var db = new CarServiceContext();
            CmbCustomer.ItemsSource = db.Customers.OrderBy(c => c.Last_Name).ToList();
            if (id != null) LoadData(id.Value);
        }
        private void LoadData(int id)
        {
            using var db = new CarServiceContext();
            var v = db.Vehicles.Find(id);
            if (v == null) return;
            CmbCustomer.SelectedValue = v.ID_Customers;
            TxtVIN.Text = v.VIN; TxtPlate.Text = v.Plate_Number;
            TxtBrand.Text = v.Brand; TxtModel.Text = v.Model;
            TxtYear.Text = v.Year?.ToString();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbCustomer.SelectedValue == null || string.IsNullOrWhiteSpace(TxtVIN.Text) || string.IsNullOrWhiteSpace(TxtPlate.Text))
            { MessageBox.Show("Заполните обязательные поля (*).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (TxtVIN.Text.Trim().Length != 17)
            { MessageBox.Show("VIN должен содержать ровно 17 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            int? year = int.TryParse(TxtYear.Text, out int y) ? y : null;
            try
            {
                using var db = new CarServiceContext();
                if (_id == null) db.Vehicles.Add(new Vehicle { ID_Customers = (int)CmbCustomer.SelectedValue, VIN = TxtVIN.Text.Trim().ToUpper(), Plate_Number = TxtPlate.Text.Trim().ToUpper(), Brand = string.IsNullOrWhiteSpace(TxtBrand.Text) ? null : TxtBrand.Text.Trim(), Model = string.IsNullOrWhiteSpace(TxtModel.Text) ? null : TxtModel.Text.Trim(), Year = year });
                else { var v = db.Vehicles.Find(_id.Value); if (v == null) return; v.ID_Customers = (int)CmbCustomer.SelectedValue; v.VIN = TxtVIN.Text.Trim().ToUpper(); v.Plate_Number = TxtPlate.Text.Trim().ToUpper(); v.Brand = string.IsNullOrWhiteSpace(TxtBrand.Text) ? null : TxtBrand.Text.Trim(); v.Model = string.IsNullOrWhiteSpace(TxtModel.Text) ? null : TxtModel.Text.Trim(); v.Year = year; }
                db.SaveChanges(); DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
