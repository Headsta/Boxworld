using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using UnityEngine;
using System.Collections;

public class ConnectionHandler {
	
	Socket sock;
	public static int bytesSent = 0;
	public static int bytesRecv = 0;
	
	enum Commands { GETBLOCK, FLATTEN }
	
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
			byte [] ip = { 192,168,0,202 };
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
	
	public void GetBlockData(ArrayList blockSpace) { 

		byte[] cmddata = pack_int((int) Commands.GETBLOCK);
		sock.Send(cmddata, cmddata.Length, 0); 
		
		byte[] intdata = pack_int(blockSpace.Count);
		sock.Send(intdata, intdata.Length, 0); 
		
		Debug.Log("Requesting " + blockSpace.Count + " blocks");
		
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
		
		
		int read = 0, total = 0;
		do {
			read = sock.Receive(tfloatdata, total, bytes_to_receive - total, 0);
			total += read;
		} while (total < bytes_to_receive);
		
		
		bytesRecv += total;
		for (int bc = 0, i = 0; bc < blockSpace.Count; bc++ ) {
			BlockData bd = (BlockData) blockSpace[bc];
			for (int x = 0; x < WorldRender.BOXRES; x++) {
				for (int y = 0; y < WorldRender.BOXRES; y++) {
	
					bd.floatData[x][y] = (float) BitConverter.ToSingle( tfloatdata, i * sizeof(float));
	
					i++;
				}
				
			}
		}
	
	}
	
	public void FlatnBlocks(ArrayList blocks) {
		
		byte[] cmddata = pack_int((int) Commands.FLATTEN);
		sock.Send(cmddata, cmddata.Length, 0); 
		
		byte[] intdata = pack_int(blocks.Count);
		sock.Send(intdata, intdata.Length, 0); 
		
		intdata = pack_int(5);
		sock.Send(intdata, intdata.Length, 0); 
		
		Debug.Log("Flattening " + blocks.Count + " blocks");
		
		for (int bc = 0; bc < blocks.Count; bc++ ) {
			
			BlockData bd = (BlockData) blocks[bc];
			int gx = bd.gx, gy = bd.gy;
			
			intdata = pack_int(gx); 
			sock.Send(intdata, intdata.Length, 0); 
	
			intdata = pack_int(gy);
			sock.Send(intdata, intdata.Length, 0);  
			
		}
		
		
	}
	
	
}


