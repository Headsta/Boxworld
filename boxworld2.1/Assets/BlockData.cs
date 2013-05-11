using System;

public class BlockData {
	
	public int gx, gy, x,y;
	public float[][] floatData;
	
	public BlockData (int x, int y, int gx, int gy, int size) {
		
		this.gx = gx; this.gy = gy;
		this.x = x; this.y = y;
		
		floatData = new float[size][];
		for (int i = 0; i < size; i++) floatData[i] = new float[size];
		
	}
}


