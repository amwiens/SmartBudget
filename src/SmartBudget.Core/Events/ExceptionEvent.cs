using Prism.Events;

using System;

namespace SmartBudget.Core.Events
{
    public class ExceptionEvent : PubSubEvent<Exception>
    {
    }
}