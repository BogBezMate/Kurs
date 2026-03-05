using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class AddPartToOrderDialog : Window
    {
        private readonly int _orderId;

        public AddPartToOrderDialog(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            using var db = new CarServiceContext();
            CmbPart.ItemsSource = db.Parts.Where(p => p.Quantity > 0).OrderBy(p => p.Name).ToList();
        }

        private void CmbPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbPart.SelectedItem is Part p)
            {
                LblInfo.Text = $"На складе: {p.Quantity} шт.  |  Цена: {p.Price:N2} ₽";
                TxtPrice.Text = p.Price.ToString("F2");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPart.SelectedValue == null) { MessageBox.Show("Выберите запчасть.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (!int.TryParse(TxtQuantity.Text, out int qty) || qty <= 0) { MessageBox.Show("Некорректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (!decimal.TryParse(TxtPrice.Text.Replace(",", "."), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal price) || price < 0) { MessageBox.Show("Некорректная цена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            try
            {
                using var db = new CarServiceContext();
                var partId = (int)CmbPart.SelectedValue;
                var part = db.Parts.Find(partId);
                if (part == null) return;
                if (qty > part.Quantity) { MessageBox.Show($"Недостаточно на складе. Доступно: {part.Quantity} шт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

                // Списываем со склада
                part.Quantity -= qty;
                db.WorkOrderParts.Add(new WorkOrderPart { ID_WorkOrders = _orderId, ID_Parts = partId, Quantity = qty, Unit_Price = price });
                db.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
