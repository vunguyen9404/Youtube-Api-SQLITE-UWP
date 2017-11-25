using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.Models
{
    public class Video
    {
        public string Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public bool IsLike { get; set; }
        public LocalPlaylist playlist { get; set; }
    }
}
