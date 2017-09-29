using WampSharp.V2.PubSub;

namespace ConsoleApp.CrossBar.Subscriber
{
    public interface ICounterSubscriber
    {
        [WampTopic("com.example.oncounter")]
        void OnCounter(string msg);
    }
}
