using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class LocalWorld
{
		
	/*bool[][] cached;
	float[][] worldData;
	
	int gx, gy;	
	
	public void getWorldData(int x, int y) {
	
		
	
	}
	
	private Vector2 offsetToMatrix(int a, int b) {
		int m = (WorldRender.BOXES >> 1);
		return new Vector2( m + a, m + b);
	}*/
	
	public LocalWorld (int startx, int starty) {
		
		/*gx = startx; gy = starty;
		
		cached = new bool[WorldRender.BOXES][];
		for (int i = 0; i < cached[i].Length; i++) cached[i] = new bool[WorldRender.BOXES];
		
		int dim = (1 + (WorldRender.BOXSIZE - 1) * WorldRender.BOXES);
		
		worldData = new float[dim][];
		for (int i = 0; i < worldData[i].Length; i++) worldData[i] = new float[dim];
		
		int bounds = ((WorldRender.BOXES - 1) / 2), mid = (WorldRender.BOXES >> 1);
		
		for (int x = -bounds; x <= bounds; x++) {
			for (int y = -bounds; y <= bounds; y++) {

				ConnectionHandler c1 = new ConnectionHandler();
				c1.get_block_datao(gx + x, gy + y, worldData, (mid + x) * (WorldRender.BOXSIZE-1), (mid + y) * (WorldRender.BOXSIZE-1));
				
				
			}
		}*/
		
		
		
		

		
		//ConnectionHandler c2 = new ConnectionHandler();
		//new Thread(new ThreadStart(c2.get_block_data)).Start();

		//con.get_block_data(10,0);
		
	}
	
	void weave() {
		
	}
}


