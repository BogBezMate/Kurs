using System.Windows;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class AddServiceDialog : Window
    {
        private readonly int _orderId;
        public AddServiceDialog(int orderId) { InitializeComponent(); _orderId = orderId; }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDesc.Text)) { MessageBox.Show("Введите описание.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (!decimal.TryParse(TxtPrice.Text.Replace(",", "."), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal price) || price < 0) { MessageBox.Show("Некорректная стоимость.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            try
            {
                using var db = new CarServiceContext();
                db.WorkOrderServices.Add(new WorkOrderService { ID_WorkOrders = _orderId, Description = TxtDesc.Text.Trim(), Price = price });
                db.SaveChanges(); DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
