using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using CSP.RSA.Cipher;
namespace PipeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".","testpipe", 
                PipeDirection.InOut, PipeOptions.None);
            RSAChiper rsa;
            if (pipeClient.IsConnected != true) { pipeClient.Connect(); }

            StreamReader sr = new StreamReader(pipeClient);
            StreamWriter sw = new StreamWriter(pipeClient);

            string temp;
            temp = sr.ReadLine();

            if (temp == "Waiting")
            {
                try
                {
                    sw.WriteLine("Connected");
                    sw.Flush();
                    pipeClient.WaitForPipeDrain();
                    temp = sr.ReadLine();
                    rsa = new RSAChiper(temp);

                    Console.WriteLine("Enviar datos:");
                    string texto=Console.ReadLine();
                    sw.WriteLine(rsa.RSAEncrypt(texto));
                    sw.Flush();
                    pipeClient.WaitForPipeDrain();
                    
                }
                catch (Exception ex) { throw ex; }
            }
           
            pipeClient.Close();
        }
    }
}
