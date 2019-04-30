using newspublish.mode.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode
{
    public class News
    {
        public News()
        {
            this.Newscomments = new HashSet<Newscomment>();
        }
        public int Id { get; set; }
        public int NewsClassifyId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Contents { get; set; }
        public DateTime PublishDate { get; set; }
        public string Remark { get; set; }
        public virtual Newsclassify Newsclassify { get; set; }
        public virtual ICollection<Newscomment> Newscomments { get; set; }
    }
}
