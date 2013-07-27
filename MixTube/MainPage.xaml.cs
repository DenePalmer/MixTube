using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
//NetworkInformation allows us to use the DeviceNetworkInformation command
using Microsoft.Phone.Net.NetworkInformation;


namespace MixTube
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Uses the DeviceNetworkInformation to find out if the device has a data connection, and outputs a warning message if not.
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DeviceNetworkInformation.IsCellularDataEnabled == false)
            {
                MessageBox.Show("Your Mobile Data connection appears to be turned off, please exit the app and turn it on before re-trying. Or continue if you have a WiFi connection.");
            }
        }

        //Code for all button navigation from the main page, using NavigationService Uris
        private void Top40Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Top40Page.xaml", UriKind.Relative));
        }

        private void No1SingleBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/No1Page.xaml", UriKind.Relative));
        }

        private void RandomSingleBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/RandomTrackPage.xaml", UriKind.Relative));
        }

        //Over-rides what happens when the device's back key is pressed, this helps the application exit cleanly
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
        }
    }
}