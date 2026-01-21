using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Models
{
    public class Calendar
    {
        public DateTime Date { get; set; }
        
        public string Day
        {
            get
            {
                return Date.ToString("dd");
            }
        }

        public string DayOfWeek
        {
            get
            {
                return Date.ToString("ddd");
            }
        }

        public string Month
        {
            get
            {
                return Date.ToString("MMM");
            }
        }
    }
}
