using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.mode.Response
{
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Result { get; set; }
        public dynamic data { get; set; }
    }
}
