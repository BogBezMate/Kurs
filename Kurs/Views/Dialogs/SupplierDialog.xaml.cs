using System.Windows;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class SupplierDialog : Window
    {
        private readonly int? _id;
        public SupplierDialog(int? id) { InitializeComponent(); _id = id; TxtTitle.Text = id == null ? "Новый поставщик" : "Редактировать поставщика"; if (id != null) LoadData(id.Value); }
        private void LoadData(int id) { using var db = new CarServiceContext(); var s = db.Suppliers.Find(id); if (s == null) return; TxtName.Text = s.Name; TxtContact.Text = s.Contact_Name; TxtPhone.Text = s.Phone; TxtEmail.Text = s.Email; }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text)) { MessageBox.Show("Введите название.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            try
            {
                using var db = new CarServiceContext();
                if (_id == null) db.Suppliers.Add(new Supplier { Name = TxtName.Text.Trim(), Contact_Name = string.IsNullOrWhiteSpace(TxtContact.Text) ? null : TxtContact.Text.Trim(), Phone = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim(), Email = string.IsNullOrWhiteSpace(TxtEmail.Text) ? null : TxtEmail.Text.Trim() });
                else { var s = db.Suppliers.Find(_id.Value); if (s == null) return; s.Name = TxtName.Text.Trim(); s.Contact_Name = string.IsNullOrWhiteSpace(TxtContact.Text) ? null : TxtContact.Text.Trim(); s.Phone = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim(); s.Email = string.IsNullOrWhiteSpace(TxtEmail.Text) ? null : TxtEmail.Text.Trim(); }
                db.SaveChanges(); DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
