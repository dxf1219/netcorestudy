using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode.Response
{
    public class NewsModel
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Contents { get; set; }
        public DateTime PublishDate { get; set; }
        public string Remark { get; set; }
        public int NewsClassifyId { get; set; }
        public string NewsClassifyName { get; set; }
        public int NewsCommentCount { get; set; }
    }
}
