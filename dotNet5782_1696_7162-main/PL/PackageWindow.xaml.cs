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

namespace PL
{
    /// <summary>
    /// Interaction logic for PackageWindow.xaml
    /// </summary>
    public partial class PackageWindow : Window
    {
        BlApi.IBL Bl;
        PackagesListWindow packagesListWindow;
        Package Package = new Package();
        public int ID { get; set; }

        /// <summary>
        /// constructor for adding package
        /// </summary>
        /// <param name="Bo"></param>
        /// <param name="packagesListWindow"></param>
        public PackageWindow(BlApi.IBL Bo, PackagesListWindow packagesListWindow)
        {
            InitializeComponent();
            Bl = Bo;
            this.packagesListWindow = packagesListWindow;
            customersIDInitialize();
            enam();
            Package.Sender = new CustAtPackage();
            Package.Target = new CustAtPackage();
            DataContext = Package;
        }

        /// <summary>
        /// constructor for update package
        /// </summary>
        /// <param name="Bo"></param>
        /// <param name="_id"></param>
        /// <param name="packagesListWindow"></param>
        public PackageWindow(BlApi.IBL Bo, int _id, PackagesListWindow packagesListWindow)
        {
            InitializeComponent();
            Bl = Bo;
            ID = _id;
            Package = Bl.GetPackage(ID);
            customersIDInitialize();
            enam();
            this.packagesListWindow = packagesListWindow;
            DataContext = Package;
            VisabilitiAndEnabled();
        }

        /// <summary>
        /// constructor for drone window
        /// </summary>
        /// <param name="Bo"></param>
        /// <param name="_id"></param>
        /// <param name="s"></param>
        public PackageWindow(BlApi.IBL Bo, int _id, string s)
        {
            InitializeComponent();
            Bl = Bo;
            ID = _id;
            Package = Bl.GetPackage(ID);
            enam();
            customersIDInitialize();
            DataContext = Package;
            main.DataContext = Package;
            VisabilitiAndEnabled();
            deleteButton.IsEnabled = false;
        }

        #region initialize

        public void InitializeData()
        {
            Package = Bl.GetPackage(ID);
            DataContext = Package;
        }

        private void enam()
        {
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        private void customersIDInitialize()
        {
            IEnumerable<CustToList> customers = Bl.GetAllCustomers();
            SenderIdTextBox.ItemsSource = from i in customers
                                          select i.ID;
            TargetIdTextBox.ItemsSource = from i in customers
                                          select i.ID;
        }

        /// <summary>
        /// initialize visability and enabled items.
        /// </summary>
        private void VisabilitiAndEnabled()
        {
            if (Package.Drone != null) unassignStackPanel.Visibility = Visibility.Collapsed;
            iDTextBox1.IsEnabled = false;
            priorityComboBox.IsEnabled = false;
            weightComboBox.IsEnabled = false;
            SenderIdTextBox.IsEnabled = false;
            TargetIdTextBox.IsEnabled = false;
            AddButton.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region AddOButton_Click
        /// <summary>
        /// adding package to the package data and showing it in the package list window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (SenderIdTextBox.SelectedIndex == -1 || TargetIdTextBox.SelectedIndex == -1 || priorityComboBox.SelectedIndex == -1 || weightComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("הכנס את כל פרטי החבילה");
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה רוצה להוסיף", " הוספת חבילה",
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
                            Bl.AddPackage(Package);
                            MessageBox.Show("ההוספה התבצעה בהצלחה!");
                            if (packagesListWindow != null)
                                packagesListWindow.initializeFilterWeightAndPriority();
                            this.Close();
                        }
                        catch (IdExistException ex)
                        {
                            MessageBox.Show("החבילה כבר קיימת במערכת.");
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// open window of drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToDrone_Click(object sender, RoutedEventArgs e)
        {
            if (Package.Drone != null)
            {
                new DroneWindow(Bl, Package.Drone.ID, null, this).Show();
            }
        }

        /// <summary>
        /// open window of sender (customer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToSender_Click(object sender, RoutedEventArgs e)
        {
            if (Package.Sender != null)
            {
                new CustomerWindow(Bl, Package.Sender.ID, "").Show();
            }
        }

        /// <summary>
        /// open window of sender (customer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToTarget_Click(object sender, RoutedEventArgs e)
        {
            if (Package.Target != null)
            {
                new CustomerWindow(Bl, Package.Target.ID, "").Show();
            }
        }

        /// <summary>
        /// delete package from the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה למחוק", " מחיקת רחפן",
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
                        Package newPackage = framework.DataContext as Package;
                        Bl.RemovePackage(Package.ID);
                        if (packagesListWindow != null)
                            packagesListWindow.initializeFilterWeightAndPriority();
                        MessageBox.Show("הפעולה התבצעה בהצלחה!");
                        this.Close();
                    }
                    catch (NotImplementedException ex)
                    {
                        MessageBox.Show("החבילה באמצע משלוח הפעולה נכשלה");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void main_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            packagesListWindow.initializeFilterWeightAndPriority();
        }
    }
}
