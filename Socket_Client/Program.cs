namespace Socket_Client
{
    public class Program
    {
        static Client Client = new();

        static void Main(string[] args)
        {
            Client.InitializeConnection();

            Console.ReadLine();

            //Client.TestRun();
        }
    }
}