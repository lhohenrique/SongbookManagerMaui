using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Models
{
    public class Music
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Key { get; set; }
        public string Lyrics { get; set; }
        public string Chords { get; set; }
        public string Category { get; set; }
        public string Version { get; set; }
        public string Notes { get; set; }
        public DateTime CreationDate { get; set; }

        public string CategoryFormated
        {
            get
            {
                string categoryFormated = string.Empty;
                
                if (Category is not null)
                {
                    string[] categories = Category.Split(";");
                    foreach (string category in categories)
                    {
                        if (!string.IsNullOrEmpty(category))
                        {
                            categoryFormated += string.IsNullOrEmpty(categoryFormated) ? category : $", {category}" ;
                        }
                    }
                    return categoryFormated;
                }

                return categoryFormated;
            }
        }
    }
}
