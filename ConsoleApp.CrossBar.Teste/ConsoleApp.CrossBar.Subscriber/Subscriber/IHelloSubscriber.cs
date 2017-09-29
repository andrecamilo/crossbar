using WampSharp.V2.PubSub;

namespace ConsoleApp.CrossBar.Subscriber
{
    public interface IHelloSubscriber
    {
        [WampTopic("com.example.onhello")]
        void OnHello(string msg);
    }
}
