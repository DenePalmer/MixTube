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
using MixTube.ServiceReference1;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;

namespace MixTube.Pages
{
    public partial class RandomTrackPage : PhoneApplicationPage
    {
        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            Service1Client svc = new Service1Client();
            svc.GetRandomSingleCompleted += new EventHandler<GetRandomSingleCompletedEventArgs>(svc_GetRandomSingleCompleted);
            svc.GetRandomSingleAsync();
        }

        // executes when the random single data from Top 40 charts has been downloaded

        void svc_GetRandomSingleCompleted(object sender, GetRandomSingleCompletedEventArgs e)
        {

            if (e.Error == null)
            {
                // create string array and store the results for the returned random single
                // split the returned spring to get the trackname and cover art
                string[] songDetails = e.Result.Split(',');
                string songName = songDetails.GetValue(0).ToString();
                string songImageURL = songDetails.GetValue(1).ToString();
                // bind randomSongName TextBlock and image control for displaying
                randomSongName.Text = songName;
                Uri uri = new Uri("http://www.bbc.co.uk" + songImageURL, UriKind.Absolute);
                image1.Source = new BitmapImage(uri);
            }

        }

        public RandomTrackPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Store the songName as a string variable and navigates to the YouTube search page taking songName with it
            string songName = randomSongName.Text;
            NavigationService.Navigate(new Uri(string.Format("/Pages/YouTubeSearch.xaml?songName={0}", songName), UriKind.Relative));
        }

    }
}