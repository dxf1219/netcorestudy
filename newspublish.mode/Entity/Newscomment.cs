using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode.Entity
{
    public class Newscomment
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string Contents { get; set; }
        public DateTime AddTime { get; set; }
        public string Remark { get; set; }
        public virtual News News { get; set; }
    }
}
