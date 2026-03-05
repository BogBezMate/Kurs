using System.Windows;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class PartDialog : Window
    {
        private readonly int? _id;
        public PartDialog(int? id)
        {
            InitializeComponent();
            _id = id;
            TxtTitle.Text = id == null ? "Новая запчасть" : "Редактировать запчасть";
            using var db = new CarServiceContext();
            var suppliers = db.Suppliers.OrderBy(s => s.Name).ToList();
            suppliers.Insert(0, new Supplier { ID_Suppliers = 0, Name = "— не выбрано —" });
            CmbSupplier.ItemsSource = suppliers;
            CmbSupplier.SelectedIndex = 0;
            if (id != null) LoadData(id.Value);
        }
        private void LoadData(int id)
        {
            using var db = new CarServiceContext();
            var p = db.Parts.Find(id); if (p == null) return;
            TxtName.Text = p.Name;
            CmbSupplier.SelectedValue = p.ID_Suppliers ?? 0;
            TxtQuantity.Text = p.Quantity.ToString();
            TxtPrice.Text = p.Price.ToString("F2");
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text)) { MessageBox.Show("Введите наименование.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (!decimal.TryParse(TxtPrice.Text.Replace(",", "."), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal price) || price < 0) { MessageBox.Show("Некорректная цена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (!int.TryParse(TxtQuantity.Text, out int qty) || qty < 0) { MessageBox.Show("Некорректный остаток.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            int? suppId = CmbSupplier.SelectedValue is int sid && sid > 0 ? sid : null;
            try
            {
                using var db = new CarServiceContext();
                if (_id == null) db.Parts.Add(new Part { Name = TxtName.Text.Trim(), ID_Suppliers = suppId, Quantity = qty, Price = price });
                else { var p = db.Parts.Find(_id.Value); if (p == null) return; p.Name = TxtName.Text.Trim(); p.ID_Suppliers = suppId; p.Quantity = qty; p.Price = price; }
                db.SaveChanges(); DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
