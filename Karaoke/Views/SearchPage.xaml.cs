using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Karaoke.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private ObservableCollection<Models.Video> ListVideo { get; set; }
        private ObservableCollection<Models.Chanel> ListChanel { get; set; }
        private ObservableCollection<Models.Playlist> ListPlaylist { get; set; }
        private Models.SqlConnection conn { get; set; }
        public SearchPage()
        {
            this.InitializeComponent();
            ListVideo = new ObservableCollection<Models.Video>();
            ListChanel = new ObservableCollection<Models.Chanel>();
            ListPlaylist = new ObservableCollection<Models.Playlist>();
            conn = new Models.SqlConnection();
            conn.LoadDatabase();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var searchYoutube = e.Parameter as SearchResource.ListRequest;
            var searchResult = await searchYoutube.ExecuteAsync();
            searchResult.Items.ToList().ForEach(item =>
            {
                switch (item.Id.Kind)
                {
                    case "youtube#video":
                        ListVideo.Add(new Models.Video()
                        {
                            Id = item.Id.VideoId,
                            Title = item.Snippet.Title,
                            Img = item.Snippet.Thumbnails.Default__.Url
                        });
                        break;
                    case "youtube#chanel":
                        ListChanel.Add(new Models.Chanel()
                        {
                            Id = item.Id.VideoId,
                            Title = item.Snippet.Title,
                            Img = item.Snippet.Thumbnails.Default__.Url
                        });
                        break;
                    case "youtube#playlist":
                        ListPlaylist.Add(new Models.Playlist() {
                            Id = item.Id.VideoId,
                            Title = item.Snippet.Title,
                            Img = item.Snippet.Thumbnails.Default__.Url
                        });
                        break;
                }
            });
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Models.Video video = e.ClickedItem as Models.Video;
            this.Frame.Navigate(typeof(PlayVideo), video);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                Models.Video video = ListVideo.First(x => x.Id.Equals(btn.Tag));
                video.IsLike = true;
                conn.AddVideo(video);
            } catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }
    }
}
