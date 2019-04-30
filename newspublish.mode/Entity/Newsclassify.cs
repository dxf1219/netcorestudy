using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode.Entity
{
    public class Newsclassify
    {
        public Newsclassify()
        {
            this.NewsList = new HashSet<News>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }
        public virtual ICollection<News>NewsList { get; set; }
    }
       
 }
