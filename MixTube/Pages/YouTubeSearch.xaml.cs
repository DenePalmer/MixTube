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
using System.Xml.Linq;
using Microsoft.Phone.Tasks;
//MyToolkit is a selection of 3rd party classes, the Multimedia commands allow the playback of YouTube videos within a WP application
using MyToolkit.Multimedia;
using System.Windows.Navigation;

namespace MixTube.Pages
{
    public partial class YouTubeSearch : PhoneApplicationPage
    {
        public YouTubeSearch()
        {
            InitializeComponent();
        }

        //Method executes when page is navigated to
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string songName;
            //Uses query string to retrieve the songName string 
            NavigationContext.QueryString.TryGetValue("songName", out songName);
            SearchText.Text = songName;
        }

        //This method takes the text stored in SearchText and uses it to search Youtube using Google's Gdata api
        //It adds the word official on to the SearchText to make sure appropriate videos are returned
        private void VideoSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var wc = new WebClient();
            wc.DownloadStringCompleted += DownloadStringCompleted;
            var searchUri = string.Format(
              "http://gdata.youtube.com/feeds/api/videos?q={0}&format=6",
              HttpUtility.UrlEncode(SearchText.Text + "official"));
            wc.DownloadStringAsync(new Uri(searchUri));
        }

        //This method takes the text stored in SearchText and uses it to search Youtube using Google's Gdata api
        //It adds the word remix on to the SearchText to make sure appropriate videos are returned
        private void RemixSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var wc = new WebClient();
            wc.DownloadStringCompleted += DownloadStringCompleted;
            var searchUri = string.Format(
              "http://gdata.youtube.com/feeds/api/videos?q={0}&format=6",
              HttpUtility.UrlEncode(SearchText.Text + "remix"));
            wc.DownloadStringAsync(new Uri(searchUri));
        }

        //This method takes the text stored in SearchText and uses it to search Youtube using Google's Gdata api
        //It adds the word parody on to the SearchText to make sure appropriate videos are returned
        private void ParodySearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var wc = new WebClient();
            wc.DownloadStringCompleted += DownloadStringCompleted;
            var searchUri = string.Format(
              "http://gdata.youtube.com/feeds/api/videos?q={0}&format=6",
              HttpUtility.UrlEncode(SearchText.Text + "parody"));
            wc.DownloadStringAsync(new Uri(searchUri));
        }

        public class YouTubeVideo
        {
            public string Title { get; set; }
            public string VideoId { get; set; }
        }

        void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var atomns = XNamespace.Get("http://www.w3.org/2005/Atom");
            var medians = XNamespace.Get("http://search.yahoo.com/mrss/");
            var xml = XElement.Parse(e.Result);
            var videos = 
            (
              from entry in xml.Descendants(atomns.GetName("entry"))
              select new YouTubeVideo
              {
                  VideoId = entry.Element(atomns.GetName("id")).Value,                  
                  Title = entry.Element(atomns.GetName("title")).Value
              }
            ).ToArray();

            ResultsList.ItemsSource = videos;
        }

        //Allows the ResultsList List Box to be clickable
        private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Creates a new variable called video, the chosen video link is stored as this variable
            var video = ResultsList.SelectedItem as YouTubeVideo;
            if (video != null)//If statement executes if video variable contains something
            {
                //Using the parsed and id variables, these 2 lines extract the video's ID from it's URL
                var parsed = video.VideoId.Split('/');
                var id = parsed[parsed.Length - 1];
                //Using the YouTube,Play method from MyToolkit, the video is played back using the video ID retrieved earlier
                YouTube.Play(id, true, YouTubeQuality.Quality480P);
            }
        }

        //Over ride back key press code    
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
            {
                if (YouTube.CancelPlay())//Cancels the playback of the YouTube video when back key is presses
                    e.Cancel = true;
                else
                {
                    //Navigates back to the applications main page
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    e.Cancel = true;
                }
                base.OnBackKeyPress(e);
            }
        }

    }

