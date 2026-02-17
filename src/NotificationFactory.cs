using System;

namespace DesignPatternChallenge
{
    public abstract class NotificationFactory
    {
        public abstract ISender CreateNotification(string recipient);
    }
}
