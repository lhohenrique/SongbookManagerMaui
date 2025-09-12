using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Models
{
    public class MusicRep
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string SingerKey { get; set; }
        public bool IsSelected { get; set; }
        public bool IsReordering { get; set; }
    }
}
