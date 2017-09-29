using System;
using WampSharp.V2.PubSub;

namespace ConsoleApp.CrossBar.Publisher
{
    public interface ICounterPublisher
    {
        [WampTopic("com.example.oncounter")]
        event Action<int> OnCounter;
    }
}
