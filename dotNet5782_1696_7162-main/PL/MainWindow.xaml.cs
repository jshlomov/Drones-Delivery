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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        BlApi.IBL Bl = BlFactory.GetBL();

        private void ShowDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DronesListWindow(Bl).Show();
        }

        private void ShowPackageButton_Click(object sender, RoutedEventArgs e)
        {
            new PackagesListWindow(Bl).Show();
        }

        private void ShowStationsButton_click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(Bl).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowCustomersButton_Click_1(object sender, RoutedEventArgs e)
        {
            new CustomersListWindow(Bl).Show();
        }
    }

}
