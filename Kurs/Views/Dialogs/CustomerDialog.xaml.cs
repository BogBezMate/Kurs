using System.Windows;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class CustomerDialog : Window
    {
        private readonly int? _id;
        public CustomerDialog(int? id) { InitializeComponent(); _id = id; TxtTitle.Text = id == null ? "Новый клиент" : "Редактировать клиента"; if (id != null) LoadData(id.Value); }
        private void LoadData(int id) { using var db = new CarServiceContext(); var c = db.Customers.Find(id); if (c == null) return; TxtLastName.Text = c.Last_Name; TxtFirstName.Text = c.First_Name; TxtMiddleName.Text = c.Middle_Name; TxtPhone.Text = c.Phone; }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtLastName.Text) || string.IsNullOrWhiteSpace(TxtFirstName.Text)) { MessageBox.Show("Заполните обязательные поля (*).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            try
            {
                using var db = new CarServiceContext();
                if (_id == null) db.Customers.Add(new Customer { Last_Name = TxtLastName.Text.Trim(), First_Name = TxtFirstName.Text.Trim(), Middle_Name = string.IsNullOrWhiteSpace(TxtMiddleName.Text) ? null : TxtMiddleName.Text.Trim(), Phone = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim() });
                else { var c = db.Customers.Find(_id.Value); if (c == null) return; c.Last_Name = TxtLastName.Text.Trim(); c.First_Name = TxtFirstName.Text.Trim(); c.Middle_Name = string.IsNullOrWhiteSpace(TxtMiddleName.Text) ? null : TxtMiddleName.Text.Trim(); c.Phone = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim(); }
                db.SaveChanges(); DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
