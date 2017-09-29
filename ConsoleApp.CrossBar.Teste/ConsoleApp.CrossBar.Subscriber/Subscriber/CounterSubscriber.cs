using System;

namespace ConsoleApp.CrossBar.Subscriber
{
    public class CounterSubscriber : ICounterSubscriber
    {
        public void OnCounter(string msg)
        {
            Console.WriteLine("event for 'counter' received: {0}", msg);
        }
    }
}
