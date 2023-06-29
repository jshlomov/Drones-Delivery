using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Collections.Specialized;

//עדיין צריך לעשות את האייקון של המחיקה, וגם כפתור סגירה.

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {

        public enum WeightCategories { Lite, Medium, heavy, all }
        public enum DroneStatuses { Available, InRepair, OnDelivery, all }
        public ObservableCollection<BO.DroneToList> droneToLists;

        //מה נותן לי המשקיף אם עושים פור איצ' אין לי מושג...
        BlApi.IBL Bl;
        public DronesListWindow(BlApi.IBL Bo)
        {
            InitializeComponent();
            Bl = Bo;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            //IEnumerable<DroneToList> drones = Bl.GetAllDrones();
            //droneToLists = new ObservableCollection<DroneToList>();
            //foreach (var item in drones)
            //{
            //    droneToLists.Add(item);
            //}
            WeightSelector.SelectedIndex = 3;

            //droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
        }

        //private void DroneToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    StatusAndWeightComboBoxSelector();
        //}

        public void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusAndWeightComboBoxSelector();
        }

        #region filter buttons
        public void StatusAndWeightComboBoxSelector()
        {
            if (StatusSelector.SelectedIndex == -1)
            {
                StatusSelector.SelectedIndex = 3;
            }
            WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
            DroneStatuses status = (DroneStatuses)StatusSelector.SelectedItem;
            DronesListView.ItemsSource = null;
            if (weight == WeightCategories.all && status == DroneStatuses.all)
                DronesListView.ItemsSource = Bl.GetAllDrones();
            else if (status == DroneStatuses.all)
                DronesListView.ItemsSource = Bl.GetAllDrones(x => x.MaxWeight == (BO.WeightCategories)weight);
            else if (weight == WeightCategories.all)
                DronesListView.ItemsSource = Bl.GetAllDrones(x => x.Status == (BO.DroneStatuses)status);
            else
                DronesListView.ItemsSource = Bl.GetAllDrones(x => x.Status == (BO.DroneStatuses)status && x.MaxWeight == (BO.WeightCategories)weight);

            if (orderByStatus.SelectedIndex != -1)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
                //view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusAndWeightComboBoxSelector();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusAndWeightComboBoxSelector();
        }

        private void orderByStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusAndWeightComboBoxSelector();
        }

        #endregion

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drone = (DroneToList)DronesListView.SelectedItem;
            if (drone != null)
            {
                int index = DronesListView.SelectedIndex;
                new DroneWindow(Bl, drone.ID, this).Show();
            }
        }

        private void AddDrone_Click_1(object sender, RoutedEventArgs e)
        {
            new DroneWindow(Bl, this).Show();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PackIcon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

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
                        DroneToList droneToList = framework.DataContext as DroneToList;
                        Bl.RemoveDrone(droneToList.ID);
                        StatusAndWeightComboBoxSelector();
                        MessageBox.Show("הפעולה התבצעה בהצלחה!");
                        //droneToLists.Remove(droneToList);
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

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            orderByStatus.SelectedIndex = -1;
            StatusSelector.SelectedIndex = 3;
            WeightSelector.SelectedIndex = 3;
        }

    }
}


//the function with a observer

//public void StatusAndWeightComboBoxSelector()
//{
//    if (StatusSelector.SelectedIndex == -1)
//    {
//        StatusSelector.SelectedIndex = 3;
//    }
//    WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
//    DroneStatuses status = (DroneStatuses)StatusSelector.SelectedItem;
//    DronesListView.ItemsSource = null;
//    if (weight == WeightCategories.all && status == DroneStatuses.all)
//        DronesListView.ItemsSource = droneToLists;
//    else if (status == DroneStatuses.all)
//        DronesListView.ItemsSource = droneToLists.Where(x => x.MaxWeight == (BO.WeightCategories)weight);
//    else if (weight == WeightCategories.all)
//        DronesListView.ItemsSource = droneToLists.Where(x => x.Status == (BO.DroneStatuses)status);
//    else
//        DronesListView.ItemsSource = droneToLists.Where(x => x.Status == (BO.DroneStatuses)status && x.MaxWeight == (BO.WeightCategories)weight);

//    if (orderByStatus.SelectedIndex != -1)
//    {
//        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
//        PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
//        view.GroupDescriptions.Clear();
//        view.GroupDescriptions.Add(groupDescription);
//    }
//}
