using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace rm
{
    public class Track
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public double Duration { get; set; }
        public string Genre { get; set; }
        public Image Image { get; set; }

        public Track (string title, string artist, double duration, string genre, Image image)
        {
            Title = title;
            Artist = artist;
            Duration = duration;
            Genre = genre;
            Image = image;
        }
    }
}
