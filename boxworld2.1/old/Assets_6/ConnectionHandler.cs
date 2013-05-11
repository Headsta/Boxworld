using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using UnityEngine;

public class ConnectionHandler
{
	Socket sock;
	
	public ConnectionHandler (){
		
		//Debug.Log("Trying to connect\n");
		
		string server = "192.168.0.186";
		int port = 1010;
			
        // Create a socket connection with the specified server and port.
        //Socket s = null;
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
                sock = tempSocket;
                break;
            }
            else
            {
                continue;
            }
        }
		
        if (sock == null) {
        
			Debug.Log("Connection failed");
			return;
			
		}

		//Debug.Log("Connected!");
		
		/*byte[] ints = new byte[4];
		int bytes = 0; 
			
        // The following will block until te page is transmitted.
        //do {
              
			bytes = s.Receive(ints, 4, 0);
            //page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
			
			Debug.Log(BitConverter.ToInt32(ints, 0));
		
			byte[] response = { 0xFF , 0xFF , 0 , 0 };
			s.Send(response, response.Length, 0);  
			
        //}
        //while (bytes > 0);*/

    }
	
	public byte[] pack_int(int x) {
		byte[] intdata = new byte[4];
		for (int i = 0; i < 4; i++) intdata[i] = (byte) (((0xFF << (i << 3)) & x) >> (i << 3));
		return intdata;
	}
	
	public void get_block_data(int gx, int gy, float[][] currentMesh) {
				
		byte[] intdata = pack_int(gx);
		sock.Send(intdata, intdata.Length, 0); 

		intdata = pack_int(gy);
		sock.Send(intdata, intdata.Length, 0); 
		
		//int numfloats = WorldRender.BOXES * WorldRender.BOXSIZE; //(((LocalWorld.BLOCKS) * (LocalWorld.BLOCKSIZE - 1)) + 1);
		//numfloats *= numfloats;
		
		//Debug.Log("Getting " + numfloats + " floats of data, bytes: " + (numfloats * 4));
		
		//byte[] floatdata = new byte[numfloats];
		//float[] floatdata = new float[numfloats];

		
		int floats_to_receive = WorldRender.BOXSIZE * WorldRender.BOXSIZE, bytes_to_receive = floats_to_receive * sizeof(float);
		byte[] tfloatdata = new byte[bytes_to_receive];
							
		int read = 0, total = 0;
		do {
			read = sock.Receive(tfloatdata, total, bytes_to_receive - total, 0);
			total += read;
		} while (read != 0);
		
		Debug.Log("read " + read);
		
		//for (int cblock = 0; cblock < LocalWorld.BLOCKS; cblock++) {
		for (int x = 0, i = 0; x < WorldRender.BOXSIZE; x++) {
			for (int y = 0; y < WorldRender.BOXSIZE; y++) {
				//sock.Receive(t, 4, 0);
				
				currentMesh[x][y] = (float) BitConverter.ToSingle( tfloatdata, i * sizeof(float));
				i++;
			}
		}
		
		//Debug.Log("random val: " + floatdata[10]);
		
		//Debug.Log("done");
		
	}

	public void get_block_datao(int gx, int gy, float[][] currentMesh, int xo, int yo) {
				
		byte[] intdata = pack_int(gx);
		sock.Send(intdata, intdata.Length, 0); 

		intdata = pack_int(gy);
		sock.Send(intdata, intdata.Length, 0); 
		
		//int numfloats = WorldRender.BOXES * WorldRender.BOXSIZE; //(((LocalWorld.BLOCKS) * (LocalWorld.BLOCKSIZE - 1)) + 1);
		//numfloats *= numfloats;
		
		//Debug.Log("Getting " + numfloats + " floats of data, bytes: " + (numfloats * 4));
		
		//byte[] floatdata = new byte[numfloats];
		//float[] floatdata = new float[numfloats];
		
		byte[] tfloatdata = new byte[4];			
			
		//for (int cblock = 0; cblock < LocalWorld.BLOCKS; cblock++) {
		for (int x = 0; x < WorldRender.BOXSIZE; x++) {
			for (int y = 0; y < WorldRender.BOXSIZE; y++) {
				sock.Receive(tfloatdata, 4, 0);
				currentMesh[x+xo][y+yo] = (float) BitConverter.ToSingle(tfloatdata, 0);
			}
		}
		
		//Debug.Log("random val: " + floatdata[10]);
		
		//Debug.Log("done");
		
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


