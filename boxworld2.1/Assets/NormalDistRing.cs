using System;
using UnityEngine;


class NormalDistRing {

	float[] numbers;
	int current, size;
	
	System.Random rand = new System.Random();
	
	public NormalDistRing(int size) {  
		
		this.size = size;
		this.numbers = new float[this.size];
		this.current = 0;
		
		while (size-- > 0) {
			
			float a = (float) rand.NextDouble(), b = (float) rand.NextDouble();
			
			float Nx = (float) (Math.Sqrt(-2 * Math.Log(a)) * Math.Cos(2 * Math.PI * b));
		
			//UnityEngine.MonoBehaviour.print("new: " + Nx);
			this.numbers[size] = Nx;
			
		}
	}
	
	public float GetFloat(float E, float stddev) {
		
		if (++this.current >= this.size) this.current = 0;
	
		return (E + (this.numbers[this.current] * stddev));
		
	}
	
}


