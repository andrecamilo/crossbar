using ConsoleApp.CrossBar.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.Core.Listener;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Fluent;
using WampSharp.V2.Realm;

namespace ConsoleApp.CrossBar.Publisher
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WampSharp Hello demo iniciando ...");

            string wsuri = Constantes.IP_SERVER;
            string realm = Constantes.REALM;

            if (args.Length > 0)
            {
                wsuri = args[0];
                if (args.Length > 1)
                {
                    realm = args[1];
                }
            }

            Task runTask = Run(wsuri, realm);

            Console.ReadLine();
        }

        private async static Task Run(string wsuri, string realm)
        {
            Console.WriteLine("Conectando com {0}, realm {1}", wsuri, realm);

            WampChannelFactory factory = new WampChannelFactory();

            IWampChannel channel = factory.ConnectToRealm(realm)
                .WebSocketTransport(wsuri)
                .JsonSerialization()
                .Build();

            IWampClientConnectionMonitor monitor = channel.RealmProxy.Monitor;

            monitor.ConnectionBroken += OnClose;
            monitor.ConnectionError += OnError;
            monitor.ConnectionEstablished += OnEstablished;

            await channel.Open().ConfigureAwait(false);

            IWampRealmServiceProvider services = channel.RealmProxy.Services;            

            // REGISTER a procedure for remote calling
            Add2Service callee = new Add2Service();
            IAsyncDisposable registrationDisposable = await services.RegisterCallee(callee).ConfigureAwait(false);
            Console.WriteLine("procedimento add2() registrado");

            // PUBLISH and CALL every second... forever
            CounterPublisher publisher = new CounterPublisher();
            IDisposable publisherDisposable = channel.RealmProxy.Services.RegisterPublisher(publisher);
            IMul2Service proxy = services.GetCalleeProxy<IMul2Service>();

            int counter = 0;

            while (true)
            {
                // PUBLISH an event
                publisher.Publish(counter);
                Console.WriteLine("publicado para 'oncounter' com o contador {0}", counter);

                counter++;

                // CALL a remote procedure
                try
                {
                    int result = await proxy.Multiply(counter, 3).ConfigureAwait(false);

                    Console.WriteLine("mul2() chamado com o resultado: {0}", result);
                }
                catch (WampException ex)
                {
                    if (ex.ErrorUri != "wamp.error.no_such_procedure")
                    {
                        Console.WriteLine("call of mul2() failed: " + ex);
                    }
                }
            }
        }

        static void Thread1()
        {
            for (int i = 0; i < 10000; i++) Console.Write("2");
        }

        private static void OnEstablished(object sender, WampSessionCreatedEventArgs e)
        {
            Console.WriteLine("Conexao estabelecida.");
        }

        private static void OnClose(object sender, WampSessionCloseEventArgs e)
        {
            Console.WriteLine("Conexao fechada. Causa: " + e.Reason);
        }

        private static void OnError(object sender, WampConnectionErrorEventArgs e)
        {
            Console.WriteLine("Erro na conexao. Erro: " + e.Exception);
        }
    }
}