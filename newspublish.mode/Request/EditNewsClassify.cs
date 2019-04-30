using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode.Request
{
    public class EditNewsClassify
    {
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }
        public int Id { get; set; }
    }
}
