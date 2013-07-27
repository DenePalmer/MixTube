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
//Using statement for the service reference allows the UK Top 40 data to be accessed and used within the application
using MixTube.ServiceReference1;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;

namespace MixTube.Pages
{
    public partial class Top40Page : PhoneApplicationPage
    {

        public void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Service1Client svc = new Service1Client();
            svc.GetTop40Completed += new EventHandler<GetTop40CompletedEventArgs>(svc_GetTop40Completed);
            svc.GetTop40Async();
        }

        void svc_GetTop40Completed(object sender, GetTop40CompletedEventArgs e)
        {

            XDocument xdoc = XDocument.Parse(e.Result);
            List<SongDetails> contentList = new List<SongDetails>();

            foreach (XAttribute item in xdoc.Elements("query").Elements("results").Elements("img").Attributes("alt"))
            {
                SongDetails content = new SongDetails();
                content.SongName = item.Value;
                content.ImageSource = "http://www.bbc.co.uk" + item.NextAttribute.Value;
                contentList.Add(content);
            }

            songList.ItemsSource = contentList.ToList();

        }

        public class SongDetails
        {
            public string SongName { get; set; }
            public string ImageSource { get; set; }
        }

        public Top40Page()
        {
            InitializeComponent();
        }

        //This method allows the List Box containing the song information be clickable.
        public void songList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Retrieves the SongName from SongDetails
            string songName = ((sender as ListBox).SelectedItem as SongDetails).SongName;

            //This if statement check songName and replaces any "&" characters with the word "and", as "&" is a break character within c#
            if (songName.Contains("&"))
            {
                songName = songName.Replace("&", "and");
            }
 
            //This navigation uri also takes songName as a string and passes it to the YouTube Search page
            NavigationService.Navigate(new Uri(string.Format("/Pages/YouTubeSearch.xaml?songName={0}", songName), UriKind.Relative));
        }

        //Over riding the back key press code again, to help keep the application as clean as possible
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            e.Cancel = true;
        }

    }

}
