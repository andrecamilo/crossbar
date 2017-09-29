using System;

namespace ConsoleApp.CrossBar.Publisher
{
    public class Add2Service : IAdd2Service
    {
        public int Add(int x, int y)
        {
            Console.WriteLine("add2() called with {0} and {1}", x, y);
            return x + y;
        }
    }
}
