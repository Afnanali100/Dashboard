using Microsoft.Extensions.Options;
using PlantifyApp.Core.Models;
using PlantifyApp.Core.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PlantifyApp.Apis.Helpers
{
    public class SmsSetting : ISmsMessage
    {
        private  TwilioSetting options;

        public SmsSetting(IOptions<TwilioSetting> options)
        {
            this.options = options.Value;
        }

        public MessageResource sendSms(SMS sms)
        {

            TwilioClient.Init(options.AccountSID, options.AuthToken);

            var result = MessageResource.Create(
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(options.TwilioPhoneNumber),
                to: new PhoneNumber (sms.PhoneNumber)


                );

            return result;

        }
    }
}
