using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ListVideo : Page
    {
        Models.SqlConnection conn { get; set; }
        public ObservableCollection<Models.Video> VideoLiked { get; set; }
        public ObservableCollection<Models.LocalPlaylist> Playlist { get; set; }
        public ObservableCollection<Models.Video> VideoPlaylist { get; set; }
        public ListVideo()
        {
            this.InitializeComponent();
            conn = new Models.SqlConnection();
            VideoLiked = new ObservableCollection<Models.Video>();
            VideoPlaylist = new ObservableCollection<Models.Video>();
            conn.LoadDatabase();
            this.GetLikedVideo();
        }

        public List<Models.Video> GetVideo()
        {
            return conn.getVideo();
        }

        public void GetLikedVideo()
        {
            VideoLiked.Clear();
            GetVideo().ForEach(X =>
            {
                if (X.IsLike)
                {
                    VideoLiked.Add(X);
                }
            });
            VideoLiked.Reverse();
        }

        public void GetVideoPlayList(Models.LocalPlaylist playlist)
        {
            GetVideo().ForEach(x =>
            {
                if (x.playlist.PlaylistID == playlist.PlaylistID)
                {
                    VideoPlaylist.Add(x);
                }
            });
            
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Models.Video video = e.ClickedItem as Models.Video;
            this.Frame.Navigate(typeof(PlayVideo), video);
        }

        private void Dislike_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            conn.DeleteVideo(new Models.Video()
            {
                Id = btn.Tag.ToString()
            });
            var item = VideoLiked.First(x => x.Id == btn.Tag.ToString());
            VideoLiked.Remove(item);
        }
    }
}
