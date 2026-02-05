using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Models
{
    public class Repertoire
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string Owner { get; set; }
        public List<MusicRep> Musics { get; set; }
        public List<UserKey> Keys { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public string DateFormated
        {
            get
            {
                return Date.ToString("dd MMMM");
            }
        }

        public string TimeFormated
        {
            get
            {
                return Time.ToString(@"hh\:mm");
            }
        }
    }
}
