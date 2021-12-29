using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI2.Models
{
    public class Response<T>
    {
        public virtual bool Success { get; set; }
        public virtual string Message { get; set; }
        public virtual T ReturnObject { get; set; }
    }
}
