using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePress.Authentication
{
    public class Response
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
    }
}