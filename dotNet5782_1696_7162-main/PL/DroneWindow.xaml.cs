using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        BlApi.IBL bL;
        private Drone drone = new Drone();
        public int id { get; set; }
        private StationToList station { get; set; }
        private DronesListWindow dronesListWindow = null;
        StationWindow stationWindow = null;
        PackageWindow packagesWindow = null;

        /// <summary>
        /// constructor for add drone
        /// </summary>
        /// <param name="_bL"></param>
        /// <param name="dronesListWindow"></param>
        public DroneWindow(BlApi.IBL _bL, DronesListWindow dronesListWindow)
        {
            InitializeComponent();
            bL = _bL;
            enam();
            AddOrUpdateButton.Content = "ADD";

            DataContext = drone;
            VisibilityOfGridsAndButtons();
            this.dronesListWindow = dronesListWindow;
            ListStations.ItemsSource = bL.GetAllStations();
        }

        /// <summary>
        /// constructor for update drone.
        /// </summary>
        /// <param name="_bL"></param>
        /// <param name="_id"></param>
        /// <param name="dronesListWindow"></param>
        public DroneWindow(BlApi.IBL _bL, int _id, DronesListWindow dronesListWindow)
        {
            InitializeComponent();

            enam();
            bL = _bL;
            id = _id;
            this.dronesListWindow = dronesListWindow;
            drone = bL.GetDrone(id);
            DataContext = drone;
            AllButtonAndTextChanges();
        }

        /// <summary>
        /// constructor for display drone in other windows
        /// </summary>
        /// <param name="_bL"></param>
        /// <param name="_id"></param>
        /// <param name="s"></param>
        public DroneWindow(BlApi.IBL _bL, int _id, StationWindow _stationWindow = null, PackageWindow _packagesWindow = null)
        {
            InitializeComponent();
            stationWindow = _stationWindow;
            packagesWindow = _packagesWindow;
            enam();
            bL = _bL;
            id = _id;
            drone = bL.GetDrone(id);
            DataContext = drone;
            AllButtonAndTextChanges();
            if (stationWindow != null)
                actWithPackage.Visibility = Visibility.Collapsed;
            AddOrUpdateButton.IsEnabled = false;
            deleteDrone.IsEnabled = false;
            modelTextBox.IsEnabled = false;
        }

        private void ListStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            station = (StationToList)ListStations.SelectedItem;
        }

        private void modelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            drone.Model = modelTextBox.Text;
        }



        #region AddOrUpdateButton_Click

        private void AddOrUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            switch (AddOrUpdateButton.Content)
            {
                case "ADD":
                    if (station != null && drone.ID != 0 && drone.Model != null && maxWeightComboBox.SelectedIndex != -1)
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה רוצה להוסיף", " הוספת רחפן",
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
                                    bL.AddDrone(drone, station.ID);
                                    MessageBox.Show("ההוספה התבצעה בהצלחה!");
                                    dronesListWindow.StatusAndWeightComboBoxSelector();
                                }
                                catch (IdExistException ex)
                                {
                                    MessageBox.Show("הרחפן כבר קיים במערכת.");
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
                        MessageBox.Show("הכנס את כל פרטי הרחפן");
                    }
                    break;

                case "UPDATE":
                    if (modelTextBox.Text == "")
                        MessageBox.Show("הכנס שם מודל חדש");
                    else
                    {
                        MessageBoxResult messageBoxResult1 = MessageBox.Show("האם לעדכן את הרחפן", " עידכון רחפן",
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
                                    bL.UpdateDroneName(drone);
                                    MessageBox.Show("הרחפן עודכן בהצלחה!");
                                    if (dronesListWindow != null)
                                        dronesListWindow.DronesListView.Items.Refresh();
                                }
                                catch (IdIsNotExistExeption ex)
                                {
                                    MessageBox.Show("הרחפן לא נמצא!");
                                }
                                break;
                            case MessageBoxResult.No:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }

        }
        #endregion


        #region actWithPackageBatton_Click

        private void actWithPackageBatton_Click(object sender, RoutedEventArgs e)
        {
            switch (actWithPackage.Content)
            {
                case "assign drone to package.":
                    MessageBoxResult messageBoxResult = MessageBox.Show("האם תרצה לשייך חבילה לרחפן", " שיוך רחפן לחבילה",
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
                                bL.assigningPackageToDrone(drone);
                                drone = bL.GetDrone(id);
                                DataContext = drone;
                                changButtonsContent();
                                if (dronesListWindow != null)
                                    dronesListWindow.StatusAndWeightComboBoxSelector();
                                MessageBox.Show("הפעולה התבצעה בהצלחה!");
                            }
                            catch (NoPackageToAssighn ex)
                            {
                                MessageBox.Show("אין חבילה לשייך לרחפן!", "תקלה!");
                            }
                            catch (NoBatteryToPath ex)
                            {
                                MessageBox.Show("אין לרחפן מספיק סוללה לקחת חבילה!", "תקלה");
                            }
                            catch (IdIsNotExistExeption ex)
                            {
                                MessageBox.Show(ex.Message, "error");
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;
                case "get package from sender.":
                    MessageBoxResult messageBoxResult1 = MessageBox.Show("האם לאסוף את החבילה מהשולח", "איסוף חבילה מלקוח",
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
                                bL.ColectingPackageFromSender(drone);
                                drone = bL.GetDrone(id);
                                DataContext = drone;
                                actWithPackage.Content = "send package to target.";
                                if (dronesListWindow != null)
                                    dronesListWindow.StatusAndWeightComboBoxSelector();
                                else if (packagesWindow != null)
                                    packagesWindow.InitializeData();
                                MessageBox.Show("הפעולה התבצעה בהצלחה!");
                            }
                            catch (NotImplementedException ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            catch (IdIsNotExistExeption ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;
                case "send package to target.":
                    MessageBoxResult messageBoxResult2 = MessageBox.Show("האם לספק את החבילה ליעד", "אספקת חבילה ללקוח",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    switch (messageBoxResult2)
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
                                bL.PackageDelivering(drone);
                                drone = bL.GetDrone(id);
                                DataContext = drone;
                                changButtonsContent();
                                if (dronesListWindow != null)
                                    dronesListWindow.StatusAndWeightComboBoxSelector();
                                else if (packagesWindow != null)
                                {
                                    packagesWindow.InitializeData();
                                    Close();
                                }
                                MessageBox.Show("הפעולה התבצעה בהצלחה!");
                            }
                            catch (NotImplementedException ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            catch (IdIsNotExistExeption ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region initialize buttons

        private void VisibilityOfGridsAndButtons()
        {
            batteryTextBox.Visibility = Visibility.Collapsed;
            batteryLabel.Visibility = Visibility.Collapsed;
            BatteryBox.Visibility = Visibility.Collapsed;
            statusComboBox.Visibility = Visibility.Collapsed;
            statusLabel.Visibility = Visibility.Collapsed;
            PackageGrid.Visibility = Visibility.Collapsed;
            PackageButton.IsEnabled = false;
            actWithPackage.Visibility = Visibility.Collapsed;
            chargingButton.Visibility = Visibility.Collapsed;
        }

        private void AllButtonAndTextChanges()
        {
            IsReadOnleTextBox();
            ListStations.Visibility = Visibility.Collapsed;
            idTextBox.IsEnabled = false;
            batteryTextBox.IsEnabled = false;
            maxWeightComboBox.IsEnabled = false;
            statusComboBox.IsEnabled = false;
            situationCheckBox.IsEnabled = false;
            PackageGrid.Visibility = Visibility.Collapsed;
            changButtonsContent();
        }

        private void changButtonsContent()
        {
            //AddOrUpdateButton
            AddOrUpdateButton.Content = "UPDATE";

            //actWithPackage and chargingButton
            switch (drone.Status)
            {
                case DroneStatuses.Available:
                    actWithPackage.Content = "assign drone to package.";
                    chargingButton.Content = "send drone to charge";
                    chargingButton.Visibility = Visibility.Visible;
                    actWithPackage.Visibility = Visibility.Visible;
                    time.Visibility = Visibility.Collapsed;
                    time1.Visibility = Visibility.Collapsed;
                    break;
                case DroneStatuses.InRepair:
                    actWithPackage.Visibility = Visibility.Collapsed;
                    chargingButton.Content = "release drone from charging";
                    time.Visibility = Visibility.Visible;
                    time1.Visibility = Visibility.Visible;
                    break;
                case DroneStatuses.OnDelivery:
                    Package p = bL.GetPackage(drone.Package.ID);
                    actWithPackage.Content = p.PickedUp == null ? "get package from sender." : "send package to target.";
                    chargingButton.Visibility = Visibility.Collapsed;
                    time.Visibility = Visibility.Collapsed;
                    time1.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        private void IsReadOnleTextBox()
        {
            distanceTextBox.IsReadOnly = true;
            iDTextBox1.IsReadOnly = true;
            priorityTextBox.IsReadOnly = true;
            weightTextBox.IsReadOnly = true;
            latitudeTextBox.IsReadOnly = true;
            longitudeTextBox.IsReadOnly = true;
            iDTextBox2.IsReadOnly = true;
            nameTextBox.IsReadOnly = true;
            latitudeTextBox2.IsReadOnly = true;
            longitudeTextBox2.IsReadOnly = true;
            iDTextBox3.IsReadOnly = true;
            nameTextBox1.IsReadOnly = true;
            latitudeTextBox1.IsReadOnly = true;
            longitudeTextBox1.IsReadOnly = true;
        }

        private void enam()
        {
            maxWeightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
        }

        #endregion


        #region chargingButton_Click

        private void chargingButton_Click(object sender, RoutedEventArgs e)
        {
            switch (chargingButton.Content)
            {
                case "send drone to charge":
                    MessageBoxResult messageBoxResult = MessageBox.Show("האם תרצה לשלוח את הרחפן לטעינה", "שליחת רחפן לטעינה",
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
                                bL.SendDroneToCharge(drone);
                                drone = bL.GetDrone(id);
                                DataContext = drone;
                                changButtonsContent();
                                if (dronesListWindow != null)
                                    dronesListWindow.StatusAndWeightComboBoxSelector();
                                MessageBox.Show("הפעולה התבצעה בהצלחה!");
                            }
                            catch (NotImplementedException ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            catch (IdIsNotExistExeption ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;
                case "release drone from charging":
                    if (time1.Text == "")
                    {
                        MessageBox.Show("הכנס זמו טעינה");
                        break;
                    }
                    MessageBoxResult messageBoxResult1 = MessageBox.Show("האם תרצה לשחרר את הרחפן מהטעינה", "שחרור רחפן מטעינה",
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
                                bL.releaseDroneFromCharging(drone, Convert.ToInt32(time1.Text));
                                drone = bL.GetDrone(id);
                                DataContext = drone;
                                changButtonsContent();
                                if (dronesListWindow != null)
                                    dronesListWindow.StatusAndWeightComboBoxSelector();
                                else if (stationWindow != null)
                                {
                                    stationWindow.initializeListView();
                                    Close();
                                }
                                MessageBox.Show("הפעולה התבצעה בהצלחה!");
                            }
                            catch (NotImplementedException ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            catch (IdIsNotExistExeption ex)
                            {
                                MessageBox.Show("error", ex.Message);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DroneButton_Click(object sender, RoutedEventArgs e)
        {
            DroneGrid.Visibility = Visibility.Visible;
            PackageGrid.Visibility = Visibility.Collapsed;
        }

        private void PackageButton_Click(object sender, RoutedEventArgs e)
        {
            PackageGrid.Visibility = Visibility.Visible;
            DroneGrid.Visibility = Visibility.Collapsed;
        }

        private void deleteDrone_Click(object sender, RoutedEventArgs e)
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
                        Drone drone1 = framework.DataContext as Drone;
                        bL.RemoveDrone(drone1.ID);
                        if (dronesListWindow != null)
                            dronesListWindow.StatusAndWeightComboBoxSelector();
                        MessageBox.Show("הפעולה התבצעה בהצלחה!");
                        this.Close();
                    }
                    catch (NotImplementedException ex)
                    {
                        MessageBox.Show("הרחפן באמצע משלוח הפעולה נכשלה");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void goToPackage_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Package != null)
            {
                new PackageWindow(bL, drone.Package.ID, "").Show();
            }
        }

        private void goToSendr_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Package != null)
            {
                new CustomerWindow(bL, drone.Package.Sender.ID, "").Show();
            }
        }

        private void goToTarget_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Package != null)
            {
                new CustomerWindow(bL, drone.Package.Target.ID, "").Show();
            }
        }
    }
}
