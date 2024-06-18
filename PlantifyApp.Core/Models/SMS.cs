using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantifyApp.Core.Models
{
    public class SMS
    {
        public string PhoneNumber { get; set; }

        public string Body { get; set; }

        public string VerficationCode { get; set; }

        public string? Token { get; set; }

    }
}
