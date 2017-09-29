using System;

namespace ConsoleApp.CrossBar.Publisher
{
    public class CounterPublisher : ICounterPublisher
    {
        public event Action<int> OnCounter;

        public void Publish(int value)
        {
            OnCounter?.Invoke(value);
        }
    }
}
