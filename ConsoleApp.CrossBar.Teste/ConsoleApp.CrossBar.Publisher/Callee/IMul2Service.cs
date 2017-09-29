using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace ConsoleApp.CrossBar.Publisher
{ 
    public interface IMul2Service
    {
        [WampProcedure("com.example.mul2")]
        Task<int> Multiply(int x, int y);
    }
}
