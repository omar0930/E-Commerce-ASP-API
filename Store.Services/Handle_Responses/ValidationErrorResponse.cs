using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Handle_Responses
{
    public class ValidationErrorResponse : Custom_Exception
    {
        public IEnumerable<string> Errors { get; set; }
        public ValidationErrorResponse() : base(400)
        {

        }
    }
}
