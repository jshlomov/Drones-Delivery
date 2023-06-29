using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        IBL Bl;

        public CustomersListWindow(BlApi.IBL Bo)
        {
            InitializeComponent();
            Bl = Bo;
            initialize();
        }

        internal void initialize()
        {
            CustomersListView.ItemsSource = Bl.GetAllCustomers();
        }

        private void CustomersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustToList customer = (CustToList)CustomersListView.SelectedItem;
            if (customer != null)
            {
                int index = CustomersListView.SelectedIndex;
                new CustomerWindow(Bl, customer.ID, this).Show();
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(Bl, this).Show();
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void groupNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            initialize();
        }

        private void CustomersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה למחוק", " מחיקת לקוח",
MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    try
                    {
                        FrameworkElement framework = sender as FrameworkElement;
                        CustToList customer1 = framework.DataContext as CustToList;
                        Bl.RemoveCustomer(customer1.ID);
                        initialize();
                        MessageBox.Show("הפעולה התבצעה בהצלחה!");
                    }
                    catch (NotImplementedException ex)
                    {
                        MessageBox.Show("יש חבילה באמצע משלוח הפעולה נכשלה");
                    }
                    catch (IdIsNotExistExeption ex)
                    {
                        MessageBox.Show("החבילה לא נמצאה");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
    }
}
