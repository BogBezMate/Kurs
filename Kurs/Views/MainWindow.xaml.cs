using System.Windows;
using System.Windows.Controls;
using Kurs.Views.Pages;

namespace Kurs.Views
{
    public partial class MainWindow : Window
    {
        private Button? _activeButton;

        public MainWindow()
        {
            InitializeComponent();
            TxtCurrentUser.Text = App.CurrentUser?.Employee?.FullName ?? App.CurrentUser?.Username ?? "";
            Navigate(BtnWorkOrders, new WorkOrdersPage());
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            Page? page = btn.Tag?.ToString() switch
            {
                "Employees"  => new EmployeesPage(),
                "Customers"  => new CustomersPage(),
                "Vehicles"   => new VehiclesPage(),
                "Suppliers"  => new SuppliersPage(),
                "Parts"      => new PartsPage(),
                "WorkOrders" => new WorkOrdersPage(),
                _ => null
            };
            if (page != null) Navigate(btn, page);
        }

        private void Navigate(Button btn, Page page)
        {
            if (_activeButton != null)
                _activeButton.Style = (Style)FindResource("SidebarButton");
            _activeButton = btn;
            btn.Style = (Style)FindResource("SidebarButtonActive");
            MainFrame.Navigate(page);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            var login = new LoginWindow();
            login.Show();
            Close();
        }
    }
}
