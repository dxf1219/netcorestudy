using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode.Response
{
    public class NewsCommentModel
    {
      
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string Contents { get; set; }
        public DateTime AddTime { get; set; }
        public string Remark { get; set; }
        public string NewsTitle { get; set; }
        public string Floor { get; set; } //评论楼层

    }
}
