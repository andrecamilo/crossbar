using System;

namespace ConsoleApp.CrossBar.Subscriber
{
    public class HelloSubscriber : IHelloSubscriber
    {
        public void OnHello(string msg)
        {
            Console.WriteLine("event for 'onhello' received: {0}", msg);
        }
    }
}
