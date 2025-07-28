using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Handle_Responses
{
    public class Custom_Exception : Response
    {
        public string? Details { get; set; }
        public Custom_Exception(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            this.Details = details;
        }
    }
}
