using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePress.Services
{
    public class Response<T>
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public class Response
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
    }
}