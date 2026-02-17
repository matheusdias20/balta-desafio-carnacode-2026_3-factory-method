using System;

namespace DesignPatternChallenge
{
    public class EmailFactory : NotificationFactory
    {
        public override ISender CreateNotification(string recipient)
        {
            return new EmailNotification { Recipient = recipient };
        }
    }

    public class SmsFactory : NotificationFactory
    {
        public override ISender CreateNotification(string recipient)
        {
            return new SmsNotification { PhoneNumber = recipient };
        }
    }

    public class PushFactory : NotificationFactory
    {
        public override ISender CreateNotification(string recipient)
        {
            return new PushNotification { DeviceToken = recipient };
        }
    }

    public class WhatsAppFactory : NotificationFactory
    {
        public override ISender CreateNotification(string recipient)
        {
            return new WhatsAppNotification { PhoneNumber = recipient };
        }
    }
}
