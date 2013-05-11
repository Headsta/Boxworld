using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class LocalWorld
{
	
	public static int BLOCKS = 1, BLOCKSIZE = 33;
	
	Dictionary<Vector2, Block> map = new Dictionary<Vector2, Block>();
	
	int gx, gy;	
	
	private class Block {
		
		float[] block_data;
		GameObject groundmesh;
		
		public Block() {
		
		}

	}
	
	public void getMesh(float[][] mesh) {
		ConnectionHandler c1 = new ConnectionHandler();
		//new Thread(new ThreadStart(c1.get_block_data)).Start();
		//c1.get_block_data(mesh);
	}
	
	public LocalWorld () {
				
		int bounds = (BLOCKS - 1) / 2;
		
		for (int x = -bounds; x <= bounds; x++) {
			for (int y = -bounds; y <= bounds; y++) {
				map.Add(new Vector2((float) x, (float) y), new Block());
				//Debug.Log("x " + x + " y " + y);
			}
		}
		

		
		//ConnectionHandler c2 = new ConnectionHandler();
		//new Thread(new ThreadStart(c2.get_block_data)).Start();

		//con.get_block_data(10,0);
		
	}
	
	void weave() {
		
	}
}


