using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode.Request
{
    public class AddComment
    {
        public int NewsId { get; set; }
        public string Contents { get; set; }
    }
}
