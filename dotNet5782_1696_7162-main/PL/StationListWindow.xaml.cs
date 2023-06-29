using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

//הגרופינג לא תקין צריך לבדוק למה
//צריך לשנות עיצוב
//לעשות מחיקה
namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        //public ObservableCollection<BO.StationToList> stationsToList;

        BlApi.IBL Bl;

        public StationListWindow(BlApi.IBL _Bl)
        {
            InitializeComponent();
            Bl = _Bl;

            StationsListView.ItemsSource = Bl.GetAllStations();
            groupNameComboBox.Items.Add("more of slots");
            groupNameComboBox.Items.Add("more of free slots");

            //stationsToList = new ObservableCollection<StationToList>();
            //IEnumerable<StationToList> stations = Bl.GetAllStations();
            //foreach (var item in stations)
            //{
            //    stationsToList.Add(item);
            //}
            //StationsListView.ItemsSource = stationsToList;
            //stationsToList.CollectionChanged += stationsToList_CollectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void listViewInitialize()
        {
            //צריך את זה כדי שיתעדכן ברגע שעושים עידכון בחלון התחנה. זה שונה מהרחפן כי לא שמור ב בשכבה הלוגית תחנות.
            StationsListView.ItemsSource = Bl.GetAllStations();

            //stationsToList.Clear();
            //IEnumerable<StationToList> stations = Bl.GetAllStations();
            //foreach (var item in stations)
            //{
            //    stationsToList.Add(item);
            //}
            //StationsListView.ItemsSource = stationsToList;
        }

        private void stationsToList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            listViewInitialize();
        }

        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList station = (StationToList)StationsListView.SelectedItem;
            if (station != null)
            {
                new StationWindow(Bl, station.ID, this).Show();
            }
        }

        private void StationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //listViewInitialize();
        }

        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(Bl, this).Show();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            listViewInitialize();
        }

        private void groupNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (groupNameComboBox.SelectedItem)
            {
                case "more of slots":
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("FreeChargeSlots");
                    view.GroupDescriptions.Add(groupDescription);
                    break;
                case "more of free slots":
                    CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
                    PropertyGroupDescription groupDescription1 = new PropertyGroupDescription();
                    view1.GroupDescriptions.Add(groupDescription1);
                    break;
                default:
                    break;
            }

        }

        private void delete_Click(object sender, RoutedEventArgs e)
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
                        StationToList station = framework.DataContext as StationToList;
                        Bl.RemoveStation(station.ID);
                        listViewInitialize();
                        MessageBox.Show("הפעולה התבצעה בהצלחה!");
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
    }
}
