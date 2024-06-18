using PlantifyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace PlantifyApp.Apis.Helpers
{
    public interface ISmsMessage
    {
        MessageResource sendSms(SMS sms);

    }
}
