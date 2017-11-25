using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Karaoke
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private YouTubeService youTubeService { get; set; }
        private ObservableCollection<Models.Video> ListVideo { get; set; }
        private List<string> ListChanel { get; set; }
        private List<string> ListPlayList { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            ListVideo = new ObservableCollection<Models.Video>();
            ListChanel = new List<string>();
            ListPlayList = new List<string>();
            youTubeService = this.getService();
            FrameView.Navigate(typeof(Views.ListVideo));
        }

        private YouTubeService getService ()
        {
            return new YouTubeService(new BaseClientService.Initializer() {
                ApiKey = "AIzaSyDFddmJXZ1vCHxXgRQrNlw83-yUVjtchMM",
                ApplicationName = "API youtube Pikalong"
            });
        }

        private SearchResource.ListRequest  SearchYoutube(string query, int limit)
        {
            var searchYoutube = youTubeService.Search.List("snippet");
            searchYoutube.Q = query;
            searchYoutube.MaxResults = limit;
            searchYoutube.Order = SearchResource.ListRequest.OrderEnum.Relevance;
            return searchYoutube;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AutoSuggestBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            
        }

        private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var searchResource = this.SearchYoutube(Search.Text, 50);
            FrameView.Navigate(typeof(Views.SearchPage), searchResource);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FrameView.Navigate(typeof(Views.ListVideo));
        }
    }
}
