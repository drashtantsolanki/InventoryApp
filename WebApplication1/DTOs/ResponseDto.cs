using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DTOs
{
    public class ResponseDto
    {
        public bool Status { get; set; }
        public Object Result  { get; set; }
        public string Error { get; set; }
    }
}
