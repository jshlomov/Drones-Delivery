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

//לעשות מחיקה
namespace PL
{
    public enum WeightCategories { Lite, Medium, heavy, All }
    public enum Priorities { Ordinary, Urgently, Emergency, All }

    /// <summary>
    /// Interaction logic for PackageListWindow.xaml
    /// </summary>
    public partial class PackagesListWindow : Window
    {
        IBL Bl;

        public PackagesListWindow(BlApi.IBL Bo)
        {
            InitializeComponent();
            Bl = Bo;

            priorityComboBox.ItemsSource = Enum.GetValues(typeof(Priorities));
            weightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            groupNameComboBox.Items.Add("Sender's name");
            groupNameComboBox.Items.Add("Target's name");

            initializeFilterWeightAndPriority();
        }

        private void PackagesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PackageToList package = (PackageToList)PackagesListView.SelectedItem;
            if (package != null)
            {
                int index = PackagesListView.SelectedIndex;
                new PackageWindow(Bl, package.ID, this).Show();
            }
        }

        internal void initializeFilterWeightAndPriority()
        {
            if (weightComboBox.SelectedIndex == -1)
                weightComboBox.SelectedIndex = 3;
            if (priorityComboBox.SelectedIndex == -1)
                priorityComboBox.SelectedIndex = 3;
            BO.Priorities p = (BO.Priorities)priorityComboBox.SelectedItem;
            BO.WeightCategories w = (BO.WeightCategories)weightComboBox.SelectedItem;
            if (weightComboBox.SelectedIndex == 3 && priorityComboBox.SelectedIndex == 3)
                PackagesListView.ItemsSource = Bl.GetAllPackages();
            else if (weightComboBox.SelectedIndex == 3)
                PackagesListView.ItemsSource = Bl.GetAllPackages(x => x.Priority == p);
            else if (priorityComboBox.SelectedIndex == 3)
                PackagesListView.ItemsSource = Bl.GetAllPackages(x => x.Weight == w);
            else
                PackagesListView.ItemsSource = Bl.GetAllPackages(x => x.Weight == w && x.Priority == p);

            //חשוב ל גרופ
            if (groupNameComboBox.SelectedIndex != -1)
            {
                string s = groupNameComboBox.SelectedIndex == 0 ? "SenderName" : "TargetName";
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(PackagesListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription(s);
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        private void PackagesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddPackage_Click(object sender, RoutedEventArgs e)
        {
            new PackageWindow(Bl, this).Show();
        }

        private void priorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initializeFilterWeightAndPriority();
        }

        private void weightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initializeFilterWeightAndPriority();
        }

        private void groupNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initializeFilterWeightAndPriority();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            groupNameComboBox.SelectedIndex = -1;
            weightComboBox.SelectedIndex = -1;
            priorityComboBox.SelectedIndex = -1;

        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void from_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

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
                        PackageToList package = framework.DataContext as PackageToList;
                        Bl.RemovePackage(package.ID);
                        initializeFilterWeightAndPriority();
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

// to grouping in xamel look https://www.c-sharpcorner.com/article/wpf-listview-grouping/
// or https://wpf-tutorial.com/listview-control/listview-grouping/
// https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-group-items-in-a-listview-that-implements-a-gridview?view=netframeworkdesktop-4.8