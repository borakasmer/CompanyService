using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class Company
    {
        public string CompanyName { get; set; }
        public float CompanyCode { get; set; }
        public string CompanyUrl { get; set; }
        public string ApiUrl { get; set; }
    }
}
