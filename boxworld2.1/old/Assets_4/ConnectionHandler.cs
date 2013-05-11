using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using UnityEngine;
using System.Threading;

public class ConnectionHandler
{
	public ConnectionHandler () {
		
		Thread t = new Thread(new ThreadStart(Run));
		t.Start();
		
	}
	
	public void Run() 
	{
		string server = "localhost"; 
		int port = 1010;
			
        // Create a socket connection with the specified server and port.
        Socket s = null;
        IPHostEntry hostEntry = null;

        // Get host related information.
        hostEntry = Dns.GetHostEntry(server);

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
        foreach(IPAddress address in hostEntry.AddressList)
        {
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);

            if(tempSocket.Connected)
            {
                s = tempSocket;
                break;
            }
            else
            {
                continue;
            }
        }
		
		
		
        if (s == null) {
        
			Debug.Log("Connection failed");
			return;
			
		}

		Debug.Log("Connected!");
		
		byte[] ints = new byte[4];
		int bytes = 0; 
			
        // The following will block until te page is transmitted.
        //do {
              
			bytes = s.Receive(ints, 4, 0);
            //page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
			
			Debug.Log(BitConverter.ToInt32(ints, 0));
		
			byte[] response = { 0xFF , 0xFF , 0 , 0 };
			s.Send(response, response.Length, 0);  
			
        //}
        //while (bytes > 0);

    }
	
	
	/*

		
		byte[] bytes = { 0, 0, 0, 25 };
		
		// If the system architecture is little-endian (that is, little end first),
		// reverse the byte array.
		if (BitConverter.IsLittleEndian)
		    Array.Reverse(bytes);
		
		int i = BitConverter.ToInt32(bytes, 0);
		Console.WriteLine("int: {0}", i);
		// Output: int: 25


	*/
	
	
	
}


