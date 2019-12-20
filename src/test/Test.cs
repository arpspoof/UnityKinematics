using System; 
using System.Threading;
  
class Test { 
  
    // Main Method 
    static public void Main(String[] args) 
    { 
        UnityServerAPI.RPCStart(8080);
        var buffer = UnityServerAPI.RPCGetBuffer();
        while (true)
        {
            int n = buffer.GetNumOfAvailableElements();
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"{i}/{n}");
                var data = buffer.ReadAndErase(0);
                Console.WriteLine($"{data.sessionName}, {data.objectStates[0].transform.p.x}");
            }
            Thread.Sleep(800);
        }
    } 
} 
