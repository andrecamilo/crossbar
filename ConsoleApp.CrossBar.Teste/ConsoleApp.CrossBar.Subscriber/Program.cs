using ConsoleApp.CrossBar.Helper;
using System;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.Core.Listener;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Fluent;
using WampSharp.V2.Realm;

namespace ConsoleApp.CrossBar.Subscriber
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
            Console.WriteLine("Conectando para {0}, realm {1}", wsuri, realm);

            WampChannelFactory factory = new WampChannelFactory();

            IWampChannel channel = factory.ConnectToRealm(realm)
                .WebSocketTransport(wsuri)
                .JsonSerialization()
                .Build();

            IWampClientConnectionMonitor monitor = channel.RealmProxy.Monitor;

            monitor.ConnectionBroken += OnClose;
            monitor.ConnectionError += OnError;
            monitor.ConnectionEstablished += Monitor_ConnectionEstablished;

            await channel.Open().ConfigureAwait(false);

            IWampRealmServiceProvider services = channel.RealmProxy.Services;

            // SUBSCRIBE to a topic and receive events
            HelloSubscriber helloSubscriber = new HelloSubscriber();
            IAsyncDisposable subscriptionDisposable = await services.RegisterSubscriber(helloSubscriber).ConfigureAwait(false);
            Console.WriteLine("Topido inscrito 'onhello'");

            CounterSubscriber counterSubscriber = new CounterSubscriber();
            IAsyncDisposable subscriptionDisposableCounter = await services.RegisterSubscriber(counterSubscriber).ConfigureAwait(false);
            Console.WriteLine("Topido inscrito 'onCounter'");
        }

        private static void Monitor_ConnectionEstablished(object sender, WampSessionCreatedEventArgs e)
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