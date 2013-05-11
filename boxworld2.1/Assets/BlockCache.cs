using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCache {
	
	private Hashtable blockCache = new Hashtable();
	private Queue blockCacheOrder = new Queue();
	
	public void Add(object a, object b) {
		blockCache.Add(a, b);
		blockCacheOrder.Enqueue(a);
	}
	
	// current_position, to_remove
	public void Trim(Vector2 cp, int n) {
		
		int close = (int) (WorldRender.BOXES / 2);

		Vector2 gv;
		GameObject toRemove;
		
		for ( int i = 0; i < n; i++ ) {
			
			gv = (Vector2) blockCacheOrder.Dequeue();
			
			if (System.Math.Abs(gv.x-cp.x) <= close && System.Math.Abs(gv.y-cp.y) <= close) {
				blockCacheOrder.Enqueue(gv);
			}
			else {
				toRemove = (GameObject) blockCache[gv];
				blockCache.Remove(gv);
				UnityEngine.Object.Destroy(toRemove);
			}
		}
	}
	
	public bool HasBlock(object a) {
		return blockCache.ContainsKey(a);
	}
	
	public object Get(object a) {
		return blockCache[a];
	}
	
	public Hashtable GetHashTable(){
		return blockCache;
	}
	
	public int Size() {
		return blockCache.Count;
	}
}
