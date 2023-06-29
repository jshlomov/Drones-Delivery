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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        IBL BL = BlApi.BlFactory.GetBL();
        Customer customer;
        CustomersListWindow CustomersListWindow;

        /// <summary>
        /// constructor for add customer
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_CustomersListWindow"></param>
        public CustomerWindow(IBL bL, CustomersListWindow _CustomersListWindow)
        {
            InitializeComponent();
            BL = bL;
            CustomersListWindow = _CustomersListWindow;
            customer = new Customer();
            customer.Location = new Location();
            DataContext = customer;
            AddOrUpdateButton.Content = "ADD";
            SentListViewGrid.Visibility = Visibility.Collapsed;
            RecievedListViewGrid.Visibility = Visibility.Collapsed;
            ShowSendedPackagesButton.IsEnabled = false;
            ShowReciveredPackagesButton.IsEnabled = false;

        }

        /// <summary>
        /// constructor for update customer
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="id"></param>
        /// <param name="_CustomersListWindow"></param>
        public CustomerWindow(IBL bL, int id, CustomersListWindow _CustomersListWindow)
        {
            InitializeComponent();
            BL = bL;
            CustomersListWindow = _CustomersListWindow;
            customer = BL.GetCustomer(id);
            DataContext = customer;
            //foreach (var item in customer.PackageRecivedBy)
            //{
            //    sendingPackagesListView.Items.Add(item);
            //}
            //foreach (var item in customer.PackageSentBy)
            //{
            //    RecievedPackagesListView.Items.Add(item);
            //}
            AddOrUpdateButton.Content = "UPDATE";
            SentListViewGrid.Visibility = Visibility.Collapsed;
            RecievedListViewGrid.Visibility = Visibility.Collapsed;
            LatitudeTextBox.IsEnabled = false;
            LontitudeTextBox.IsEnabled = false;
            iDTextBox.IsEnabled = false;
        }

        /// <summary>
        /// constructor for dorne and package window.
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="id"></param>
        /// <param name="s"></param>
        public CustomerWindow(IBL bL, int id, string s)
        {
            InitializeComponent();
            BL = bL;
            customer = BL.GetCustomer(id);
            DataContext = customer;
            SentListViewGrid.Visibility = Visibility.Collapsed;
            RecievedListViewGrid.Visibility = Visibility.Collapsed;
            AddOrUpdateButton.Visibility = Visibility.Collapsed;
            LatitudeTextBox.IsEnabled = false;
            LontitudeTextBox.IsEnabled = false;
            iDTextBox.IsEnabled = false;
            NameTextBox.IsEnabled = false;
            PhoneTextBox.IsEnabled = false;
        }

        private void ShowCustomersData_ClickButton(object sender, RoutedEventArgs e)
        {
            ShowCustomerGrid.Visibility = Visibility.Visible;
            SentListViewGrid.Visibility = Visibility.Collapsed;
            RecievedListViewGrid.Visibility = Visibility.Collapsed;
        }

        private void ShowSendedPackagesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCustomerGrid.Visibility = Visibility.Collapsed;
            SentListViewGrid.Visibility = Visibility.Visible;
            RecievedListViewGrid.Visibility = Visibility.Collapsed;
        }

        private void ShowReciveredPackagesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCustomerGrid.Visibility = Visibility.Collapsed;
            SentListViewGrid.Visibility = Visibility.Collapsed;
            RecievedListViewGrid.Visibility = Visibility.Visible;
        }

        #region AddOrUpdateButton_Click

        private void AddOrUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            switch (AddOrUpdateButton.Content)
            {
                case "ADD":
                    if (customer.ID.ToString().Length <= 9 && customer.Name != null && (customer.Phone.Length == 10 || (customer.Phone.Length == 11 && customer.Phone.Any(i => i == '-') == true)) 
                        && customer.Location.Latitude != 0 && customer.Location.Longitude != 0)
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה רוצה להוסיף", " הוספת לקוח",
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
                                    BL.AddCustomer(customer);
                                    CustomersListWindow.initialize();
                                    MessageBox.Show("ההוספה התבצעה בהצלחה!");
                                    Close();
                                }
                                catch (IdExistException ex)
                                {
                                    MessageBox.Show("הלקוח כבר קיים במערכת.");
                                }
                                catch (IdIsNotExistExeption ex)
                                {
                                    MessageBox.Show("הלקוח לא נמצא!");
                                }
                                break;
                            case MessageBoxResult.No:
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (customer.ID.ToString().Length > 9)
                        {
                            MessageBox.Show("הכנס מספר זהות תקין");
                            break;
                        }
                        if (customer.Phone.Length != 10)
                        {
                            MessageBox.Show("הכנס מספר פלאפון תקין");
                            break;
                        }
                        MessageBox.Show("הכנס את כל פרטי הלקוח");
                    }
                    break;

                case "UPDATE":
                    try
                    {
                        if (customer.Phone.Length == 10 || (customer.Phone.Length == 11 && customer.Phone.Any(i => i == '-') == true))
                        {
                            MessageBoxResult messageBoxResult1 = MessageBox.Show("האם לעדכן את הלקוח", " עידכון לקוח",
                                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            switch (messageBoxResult1)
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
                                        BL.UpdateCustomer(customer);
                                        CustomersListWindow.initialize();
                                        MessageBox.Show("הלקוח עודכן בהצלחה!");
                                        Close();
                                    }
                                    catch (IdIsNotExistExeption ex)
                                    {
                                        MessageBox.Show("הלקוח לא נמצא");
                                    }
                                    break;
                                case MessageBoxResult.No:
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            MessageBox.Show("הכנס מספר פלאפון תקין");
                    }
                    catch (IdIsNotExistExeption ex)
                    {
                        MessageBox.Show("לקוח לא נמצא");
                    }
                    break;
                default:
                    break;
            }

        }
        #endregion

        private void sendingPackagesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void RecievedPackagesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
