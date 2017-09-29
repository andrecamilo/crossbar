using WampSharp.V2.Rpc;

namespace ConsoleApp.CrossBar.Publisher
{
    public interface IAdd2Service
    {
        [WampProcedure("com.example.add2")]
        int Add(int x, int y);
    }
}
