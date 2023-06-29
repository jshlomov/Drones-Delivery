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

//בהוספה יש בעיה במיקום לבדוק
//גם לבדוק אם זה מעדכן את הרשימה

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        BlApi.IBL BL;
        string s;
        Station station = new Station();
        private StationListWindow stationListWindow;
        public int ID { get; set; }

        /// <summary>
        /// ADD Constructor
        /// </summary>
        public StationWindow(BlApi.IBL Bl, StationListWindow stationListWindow)
        {
            InitializeComponent();
            BL = Bl;
            this.stationListWindow = stationListWindow;
            station.Location = new Location();
            DataContext = station;
            AddOrUpdateButton.Content = "ADD";
            dronesView.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// UPDATE Constructor
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="ID"></param>
        /// <param name="stationListWindow"></param>
        public StationWindow(BlApi.IBL bL, int id, StationListWindow stationListWindow)
        {
            InitializeComponent();
            BL = bL;
            this.ID = id;
            station = BL.GetStation(ID);
            this.stationListWindow = stationListWindow;
            DataContext = station;
            AllButtonsChanges();
            dronesView.Visibility = Visibility.Collapsed;
        }

        public void initializeListView()
        {
            dronesChargeListView.DataContext = BL.GetStation(ID);
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            station.Name = NameTextBox.Text;
        }

        private void newSlotsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            s = newSlotsTextBox.Text;
        }

        #region helping methodes
        private void AllButtonsChanges()
        {
            enabledButtons();
            AddOrUpdateButton.Content = "UPDATE";
            newSlotsTextBox.Text = (station.ChargingDrones.Count() + station.FreeChargeSlots).ToString();
        }

        private void enabledButtons()
        {
            IDTextBox.IsEnabled = false;
            LatitudeTextBox.IsEnabled = false;
            LongitudeTextBox.IsEnabled = false;
            FreeChargeSlotsTextBox.IsEnabled = false;
        }
        #endregion


        #region Add or update button

        private void AddOrUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            switch (AddOrUpdateButton.Content)
            {
                case "ADD":
                    if (1 == 1)
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
                                    BL.AddStation(station);                                    
                                    stationListWindow.listViewInitialize();
                                    MessageBox.Show("ההוספה התבצעה בהצלחה!");
                                    Close();
                                }
                                catch (IdExistException ex)
                                {
                                    MessageBox.Show("התחנה כבר קיים במערכת.");
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
                        MessageBox.Show("הכנס את כל פרטי התחנה");
                    }
                    break;

                case "UPDATE":
                    if (NameTextBox.Text == "" && newSlotsTextBox.Text == "")
                    {
                        MessageBox.Show("לא נעשו שום שינויים");
                        break;
                    }
                    else
                    {
                        MessageBoxResult messageBoxResult1 = MessageBox.Show("האם לעדכן פרטי תחנה", " עידכון תחנה",
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
                                    station.FreeChargeSlots = int.Parse(s);
                                    BL.UpadateStation(station);
                                    station = BL.GetStation(station.ID);
                                    DataContext = station;
                                    stationListWindow.listViewInitialize();
                                    MessageBox.Show("התחנה עודכנה בהצלחה!");
                                    Close();
                                }
                                catch (FormatException ex)
                                {
                                    MessageBox.Show("uncurrent value!");
                                }
                                catch (IdIsNotExistExeption ex)
                                {
                                    MessageBox.Show("התחנה לא נמצאה");
                                }
                                catch (NotImplementedException ex)
                                {
                                    MessageBox.Show("מס' עמדות לא תקין. ישנם יותר רחפנים לטעינה!");
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

        private void dronesChargeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneAtCharge drone = (DroneAtCharge)dronesChargeListView.SelectedItem;
            if (drone != null)
                new DroneWindow(BL, drone.ID, this).Show();
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void chargingDrones_Click(object sender, RoutedEventArgs e)
        {
            mainView.Visibility = Visibility.Collapsed;
            dronesView.Visibility = Visibility.Visible;
        }

        private void stationView_Click(object sender, RoutedEventArgs e)
        {
            mainView.Visibility = Visibility.Visible;
            dronesView.Visibility = Visibility.Collapsed;
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
                        Station station1 = framework.DataContext as Station;
                        BL.RemoveStation(station1.ID);
                        stationListWindow.listViewInitialize();
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

        private void dronesChargeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stationListWindow.listViewInitialize();
        }
    }
}

