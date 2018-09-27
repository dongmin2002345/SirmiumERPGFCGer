using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Base
{
    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}   
