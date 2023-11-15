namespace Socket_Client
{
    internal class Program
    {
        private static Client client = new();

        static void Main(string[] args)
        {
            client.InitializeConnection();

            Console.ReadLine();
            
        }
    }
}