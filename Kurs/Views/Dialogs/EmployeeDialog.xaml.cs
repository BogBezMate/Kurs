using System.Windows;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class EmployeeDialog : Window
    {
        private readonly int? _id;

        public EmployeeDialog(int? id)
        {
            InitializeComponent();
            _id = id;
            TxtTitle.Text = id == null ? "Новый сотрудник" : "Редактировать сотрудника";
            if (id != null) LoadData(id.Value);
        }

        private void LoadData(int id)
        {
            using var db = new CarServiceContext();
            var e = db.Employees.Find(id);
            if (e == null) return;
            TxtLastName.Text = e.Last_Name;
            TxtFirstName.Text = e.First_Name;
            TxtMiddleName.Text = e.Middle_Name;
            TxtPosition.Text = e.Position;
            TxtPhone.Text = e.Phone;
            TxtEmail.Text = e.Email;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                string.IsNullOrWhiteSpace(TxtPosition.Text))
            {
                MessageBox.Show("Заполните обязательные поля (*).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                using var db = new CarServiceContext();
                if (_id == null)
                {
                    db.Employees.Add(new Employee
                    {
                        Last_Name = TxtLastName.Text.Trim(),
                        First_Name = TxtFirstName.Text.Trim(),
                        Middle_Name = string.IsNullOrWhiteSpace(TxtMiddleName.Text) ? null : TxtMiddleName.Text.Trim(),
                        Position = TxtPosition.Text.Trim(),
                        Phone = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(TxtEmail.Text) ? null : TxtEmail.Text.Trim()
                    });
                }
                else
                {
                    var emp = db.Employees.Find(_id.Value);
                    if (emp == null) return;
                    emp.Last_Name = TxtLastName.Text.Trim();
                    emp.First_Name = TxtFirstName.Text.Trim();
                    emp.Middle_Name = string.IsNullOrWhiteSpace(TxtMiddleName.Text) ? null : TxtMiddleName.Text.Trim();
                    emp.Position = TxtPosition.Text.Trim();
                    emp.Phone = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim();
                    emp.Email = string.IsNullOrWhiteSpace(TxtEmail.Text) ? null : TxtEmail.Text.Trim();
                }
                db.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
