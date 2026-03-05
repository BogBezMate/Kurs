using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;

namespace Kurs.Views.Dialogs
{
    public partial class StockMovementDialog : Window
    {
        private readonly string _type; // "Приход" или "Списание"

        public StockMovementDialog(string type)
        {
            InitializeComponent();
            _type = type;
            TxtTitle.Text = type == "Приход" ? "📥 Поступление товара на склад" : "📤 Списание товара со склада";
            BtnConfirm.Style = (Style)FindResource(type == "Приход" ? "SuccessButton" : "WarningButton");
            using var db = new CarServiceContext();
            CmbPart.ItemsSource = db.Parts.OrderBy(p => p.Name).ToList();
            CmbPart.SelectionChanged += CmbPart_SelectionChanged;
        }

        private void CmbPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbPart.SelectedItem is Part p)
                LblStock.Text = $"Текущий остаток: {p.Quantity} шт.";
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPart.SelectedValue == null) { MessageBox.Show("Выберите запчасть.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (!int.TryParse(TxtQuantity.Text, out int qty) || qty <= 0) { MessageBox.Show("Введите корректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            try
            {
                using var db = new CarServiceContext();
                var partId = (int)CmbPart.SelectedValue;
                var part = db.Parts.Find(partId);
                if (part == null) return;

                if (_type == "Приход")
                {
                    part.Quantity += qty;
                }
                else // Списание
                {
                    if (qty > part.Quantity) { MessageBox.Show($"Недостаточно товара на складе.\nДоступно: {part.Quantity} шт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
                    part.Quantity -= qty;
                }

                db.SaveChanges();
                MessageBox.Show($"{(_type == "Приход" ? "Поступление" : "Списание")} проведено.\nНовый остаток: {part.Quantity} шт.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);                DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
