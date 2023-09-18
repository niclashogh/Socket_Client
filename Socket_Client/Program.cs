namespace Socket_Client
{
    public class Program
    {
        static Client Client = new();

        static void Main(string[] args)
        {
            Client.InitEndPoint();

            Console.ReadKey();
            Client.CloseConnection();

            //Client.TestRun();
        }
    }
}