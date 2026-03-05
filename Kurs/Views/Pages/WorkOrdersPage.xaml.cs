using System.Windows;
using System.Windows.Controls;
using Kurs.Data;
using Kurs.Models;
using Kurs.Views.Dialogs;
using Microsoft.EntityFrameworkCore;

namespace Kurs.Views.Pages
{
    public partial class WorkOrdersPage : Page
    {
        private List<WorkOrder> _all = new();
        private WorkOrder? _selected;

        public WorkOrdersPage()
        {
            InitializeComponent();
            CmbStatus.ItemsSource = new[] { "Все", "Открыт", "В работе", "Закрыт", "Отменён" };
            CmbStatus.SelectedIndex = 0;
            Load();
        }

        private void Load()
        {
            using var db = new CarServiceContext();
            _all = db.WorkOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.WorkOrderParts).ThenInclude(p => p.Part)
                .Include(o => o.WorkOrderServices)
                .Include(o => o.WorkOrderEmployees).ThenInclude(e => e.Employee)
                .OrderByDescending(o => o.Date_Open)
                .ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var q = TxtSearch.Text.ToLower();
            var status = CmbStatus.SelectedItem?.ToString();
            var filtered = _all.AsEnumerable();
            if (!string.IsNullOrEmpty(q))
                filtered = filtered.Where(o => o.Number.ToLower().Contains(q) ||
                    o.Customer.FullName.ToLower().Contains(q) ||
                    o.Vehicle.DisplayName.ToLower().Contains(q));
            if (status != "Все" && !string.IsNullOrEmpty(status))
                filtered = filtered.Where(o => o.Status == status);
            GridOrders.ItemsSource = filtered.ToList();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();
        private void CmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();

        private void GridOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selected = GridOrders.SelectedItem as WorkOrder;
            LoadDetails();
        }

        private void LoadDetails()
        {
            if (_selected == null)
            {
                GridParts.ItemsSource = null;
                GridServices.ItemsSource = null;
                GridEmployees.ItemsSource = null;
                TxtDetailTitle.Text = "Выберите наряд для просмотра деталей";
                TxtTotalParts.Text = TxtTotalServices.Text = TxtGrandTotal.Text = "";
                return;
            }
            // Перезагружаем данные с БД чтобы получить актуальные данные
            using var db = new CarServiceContext();
            var order = db.WorkOrders
                .Include(o => o.WorkOrderParts).ThenInclude(p => p.Part)
                .Include(o => o.WorkOrderServices)
                .Include(o => o.WorkOrderEmployees).ThenInclude(e => e.Employee)
                .FirstOrDefault(o => o.ID_WorkOrders == _selected.ID_WorkOrders);
            if (order == null) return;

            TxtDetailTitle.Text = $"Наряд № {order.Number}  |  Статус: {order.Status}";
            GridParts.ItemsSource = order.WorkOrderParts.ToList();
            GridServices.ItemsSource = order.WorkOrderServices.ToList();
            GridEmployees.ItemsSource = order.WorkOrderEmployees.ToList();

            TxtTotalParts.Text = $"{order.TotalParts:N2} ₽";
            TxtTotalServices.Text = $"{order.TotalServices:N2} ₽";
            TxtGrandTotal.Text = $"{order.GrandTotal:N2} ₽";
        }

        // Создать наряд
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        { if (new WorkOrderDialog(null).ShowDialog() == true) Load(); }

        // Открыть / редактировать наряд
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is WorkOrder o)
            { if (new WorkOrderDialog(o.ID_WorkOrders).ShowDialog() == true) Load(); }
        }

        // Закрыть наряд
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not WorkOrder o) return;
            if (o.Status == "Закрыт") { MessageBox.Show("Наряд уже закрыт.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            if (MessageBox.Show($"Закрыть наряд № {o.Number}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                using var db = new CarServiceContext();
                var order = db.WorkOrders.Find(o.ID_WorkOrders);
                if (order == null) return;
                order.Status = "Закрыт";
                order.Date_Close = DateTime.Today;
                db.SaveChanges();
                Load();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        // Удалить наряд
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not WorkOrder o) return;
            if (MessageBox.Show($"Удалить наряд № {o.Number}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try
            {
                using var db = new CarServiceContext();
                var order = db.WorkOrders
                    .Include(x => x.WorkOrderParts)
                    .Include(x => x.WorkOrderServices)
                    .Include(x => x.WorkOrderEmployees)
                    .FirstOrDefault(x => x.ID_WorkOrders == o.ID_WorkOrders);
                if (order == null) return;
                // Возвращаем запчасти на склад
                foreach (var p in order.WorkOrderParts)
                {
                    var part = db.Parts.Find(p.ID_Parts);
                    if (part != null) part.Quantity += p.Quantity;
                }
                db.WorkOrders.Remove(order);
                db.SaveChanges();
                Load();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        // Добавить запчасть в наряд
        private void BtnAddPart_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null) { MessageBox.Show("Выберите наряд.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            int orderId = _selected.ID_WorkOrders;
            if (new AddPartToOrderDialog(orderId).ShowDialog() == true) { Load(); SelectOrder(orderId); }
        }

        // Удалить запчасть из наряда
        private void BtnRemovePart_Click(object sender, RoutedEventArgs e)
        {
            if (GridParts.SelectedItem is not WorkOrderPart wop) return;
            if (_selected == null) return;
            int orderId = _selected.ID_WorkOrders;
            if (MessageBox.Show("Удалить запчасть из наряда? Количество вернётся на склад.", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try
            {
                using var db = new CarServiceContext();
                var item = db.WorkOrderParts.Find(wop.ID_WorkOrderParts);
                if (item == null) return;
                var part = db.Parts.Find(item.ID_Parts);
                if (part != null) part.Quantity += item.Quantity;
                db.WorkOrderParts.Remove(item);
                db.SaveChanges();
                Load(); SelectOrder(orderId);
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        // Добавить работу
        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null) { MessageBox.Show("Выберите наряд.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            int orderId = _selected.ID_WorkOrders;
            if (new AddServiceDialog(orderId).ShowDialog() == true) { Load(); SelectOrder(orderId); }
        }

        // Удалить работу
        private void BtnRemoveService_Click(object sender, RoutedEventArgs e)
        {
            if (GridServices.SelectedItem is not WorkOrderService wos) return;
            if (_selected == null) return;
            int orderId = _selected.ID_WorkOrders;
            if (MessageBox.Show("Удалить работу из наряда?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try
            {
                using var db = new CarServiceContext();
                var item = db.WorkOrderServices.Find(wos.ID_WorkOrderServices);
                if (item != null) { db.WorkOrderServices.Remove(item); db.SaveChanges(); }
                Load(); SelectOrder(orderId);
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        // Назначить сотрудника
        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null) { MessageBox.Show("Выберите наряд.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            int orderId = _selected.ID_WorkOrders;
            if (new AssignEmployeeDialog(orderId).ShowDialog() == true) { Load(); SelectOrder(orderId); }
        }

        // Снять сотрудника
        private void BtnRemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (GridEmployees.SelectedItem is not WorkOrderEmployee woe) return;
            if (_selected == null) return;
            int orderId = _selected.ID_WorkOrders;
            try
            {
                using var db = new CarServiceContext();
                var item = db.WorkOrderEmployees.Find(woe.ID_WorkOrderEmployees);
                if (item != null) { db.WorkOrderEmployees.Remove(item); db.SaveChanges(); }
                Load(); SelectOrder(orderId);
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void SelectOrder(int id)
        {
            _selected = _all.FirstOrDefault(o => o.ID_WorkOrders == id);
            LoadDetails();
        }
    }
}
