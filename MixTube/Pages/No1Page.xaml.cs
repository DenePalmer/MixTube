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
    public partial class No1Page : PhoneApplicationPage
    {
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Service1Client svc = new Service1Client();
            svc.GetNumberOneCompleted += new EventHandler<GetNumberOneCompletedEventArgs>(svc_GetNumberOneCompleted);
            svc.GetNumberOneAsync();
        }

        // executes when current No.1 single data from Top 40 charts has been downloaded

        void svc_GetNumberOneCompleted(object sender, GetNumberOneCompletedEventArgs e)
        {

            if (e.Error == null)
            {
                // create string array and store the results for the returned no1 single
                // split the returned spring to get the trackname and cover art seperately
                string[] songDetails = e.Result.Split(',');
                string songName = songDetails.GetValue(0).ToString();
                string songImageURL = songDetails.GetValue(1).ToString();
                // bind songNo1Name TextBlock and image control for displaying
                songNo1Name.Text = songName;
                Uri uri = new Uri("http://www.bbc.co.uk" + songImageURL, UriKind.Absolute);
                image1.Source = new BitmapImage(uri);
            }

        }

        public No1Page()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            //Store the songName as a string variable and navigates to the YouTube search page taking songName with it
            string songName = songNo1Name.Text;

            NavigationService.Navigate(new Uri(string.Format("/Pages/YouTubeSearch.xaml?songName={0}", songName), UriKind.Relative));

        }

     }

  }
