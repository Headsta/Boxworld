using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using UnityEngine;
using System.Collections;

public class ConnectionHandler{
	
	Socket sock;
	public static int bytesSent = 0;
	public static int bytesRecv = 0;
	
	public ConnectionHandler () {
		
		Debug.Log("Trying to connect\n");
		
		//string server = "85.24.142.30";
		string server = "192.168.0.202";
		//string server = "rcp.funkar.nu";
		int port = 1213;
			
        // Create a socket connection with the specified   server and port.
        Socket s = null;
        IPHostEntry hostEntry = null;

        // Get host related information.
        //hostEntry = Dns.GetHostEntry(server);
		  
        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
		
        //foreach(IPAddress address in hostEntry.AddressList) {
			
			//89.253.106.101
			byte [] ip = { 192,168,0,123 };
			//byte[] ip = { 192,168,0,2 };
			IPAddress ipaddr = new IPAddress(ip);
			IPEndPoint ipe = new IPEndPoint(ipaddr, port);
			Socket tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
            /*IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);*/
			
            tempSocket.Connect(ipe);

            if(tempSocket.Connected)
            {
                sock = tempSocket;
				Debug.Log("Connected!");  
                //break;
            }
            else
            {
				Debug.Log("Connection failed");
                //continue;
            }
        //}
		
        if (sock == null) {
        
			Debug.Log("Connection failed");
			return;
			
		}
    }
	
	public byte[] pack_int(int x) {
		byte[] intdata = new byte[4];
		for (int i = 0; i < 4; i++) intdata[i] = (byte) (((0xFF << (i << 3)) & x) >> (i << 3));
		return intdata;
	}
	
	public void get_block_data(ArrayList blockSpace, int _gx, int _gy) { 
		
		//byte[] cmddata = pack_int(0);
		//sock.Send(cmddata, cmddata.Length, 0); 
		
		//byte[] gxdata = pack_int(_gx);
		//sock.Send(gxdata, gxdata.Length, 0); 	
		
		//byte[] gydata = pack_int(_gy);
		//sock.Send(gydata, gydata.Length, 0); 	
		
		byte[] intdata = pack_int(blockSpace.Count);
		sock.Send(intdata, intdata.Length, 0); 
		
		//byte[] intdata = new byte[4];
		for (int bc = 0; bc < blockSpace.Count; bc++ ) {
			
			BlockData bd = (BlockData) blockSpace[bc];
			int gx = bd.gx, gy = bd.gy;
			
			intdata = pack_int(gx); 
			sock.Send(intdata, intdata.Length, 0); 
	
			intdata = pack_int(gy);
			sock.Send(intdata, intdata.Length, 0);  
			
			bytesSent += (intdata.Length * 2);
		}
		
		int floats_to_receive = blockSpace.Count * WorldRender.BOXRES * WorldRender.BOXRES;
		int bytes_to_receive = floats_to_receive * sizeof(float);
		
		byte[] tfloatdata = new byte[bytes_to_receive];
		
		//Debug.Log(Time.time + " start reading..");
		
		int read = 0, total = 0;
		do {
			read = sock.Receive(tfloatdata, total, bytes_to_receive - total, 0);
			total += read;
			//Debug.Log(Time.time + " reading..");
		} while (total < bytes_to_receive);
		
		//Debug.Log(Time.time + " done reading.. read "+ total);
		
		bytesRecv += total;
		for (int bc = 0, i = 0; bc < blockSpace.Count; bc++ ) {
			BlockData bd = (BlockData) blockSpace[bc];
			for (int x = 0; x < WorldRender.BOXRES; x++) {
				for (int y = 0; y < WorldRender.BOXRES; y++) {
	
					bd.floatData[x][y] = (float) BitConverter.ToSingle( tfloatdata, i * sizeof(float));
	
					i++;
				}
				
			}
			//Debug.Log(Time.time + " placing");
		}
		
		//Debug.Log(Time.time + " done placing.. read "+ total);
		
		//print("Read block " + gx + ", " + gy);
		//Debug.Log("Read block " + gx + ", " + gy);
		
	}
	
	public void dig() {
		
		byte[] cmddata = {1,0,0,0};
		sock.Send(cmddata, cmddata.Length, 0); 
		
	}
/*
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
		
	}	*/
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


