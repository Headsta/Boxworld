using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Threading;

public class WorldRender : MonoBehaviour {
	
	// yo alex! Hola Clint!
	
	/*[DllImport("RandomDLL2.dll")]
	public static extern void get_random_int(int [] nums);
	[DllImport("RandomDLL2.dll")]
	public static extern void seed_random(long seed);*/

	// BOXES x BOXES of size (BOXSIZE-1) x (BOXSIZE-1) units
	public static int BOXES = 41, BOXRES = 3, BOXSCALE = 10;
	public int blockCacheSize = 500;
	//public PlayerManager pm;
	
	private BlockCache BC;
	private GameObject[][] groundMeshes;
	//private GameObject[][] newGM;
	
	private Vector2 currentPosition;
	private Vector2 lastPosition;
	
	public ConnectionHandler con = new ConnectionHandler();
	
	public string groundName = "Newground";
	
	void Start () {
		
		int nouse = 0;

		Init();
		
	}

	private void Init(){
		
		BC = new BlockCache();
		
		groundMeshes = new GameObject[BOXES][];
		for (int i = 0; i < groundMeshes.Length; i++) groundMeshes[i] = new GameObject[groundMeshes.Length];
		
		UpdateLocalCoordinates(0,0);
		
		
		//con = new ConnectionHandler();
		
		
		/*System.Random fixRand = new System.Random( 0 );
		
		int[] nums = new int[1000];
		seed_random(0);
		get_random_int(nums);
		for( int j = 0; j < 6; j++ ) 
			print((nums[j] % 10) + 1);
        
		*/
		


	}

	public void placeBlock(GameObject groundMesh, int gx, int gy, int x, int y) {
		
		AddBlockComponents(groundMesh);
		PlaceGroundMeshGXGY( groundMesh, gx , gy); 
		
		groundMeshes[x][y] = groundMesh;
		BC.Add(new Vector2(gx, gy),groundMesh);
		
		// cahce full, remove blocks
		if (BC.Size() > blockCacheSize) BC.Trim(currentPosition, 100);
	
	}
	
	private void AddBlockComponents(GameObject block){
			block.AddComponent<GroundMesh>();
	}
	
	private void PlaceGroundMeshGXGY(GameObject groundMesh, int gX, int gY) {
		
		groundMesh.renderer.enabled = true;
		
		float y = gY * (groundMesh.renderer.bounds.max.z - groundMesh.renderer.bounds.min.z); 
		float x = gX * (groundMesh.renderer.bounds.max.z - groundMesh.renderer.bounds.min.z); 

		groundMesh.transform.position = new Vector3(x,groundMesh.transform.position.y,y);
	
	}
	
	void Update() {
				
		/*if (pm != null && pm.player != null) {
			pm.getPos(ref currentPosition);
			//Camera.main.transform.position = new Vector3(pm.player.transform.position.x,50,pm.player.transform.position.z);
		}
		if (lastPosition != currentPosition) { 
			//RenderDepender(pm.player.transform);
			Debug.Log("We are now at "  + currentPosition.x + ", " + currentPosition.y);
			
			UpdateLocalCoordinates((int) currentPosition.x,(int) currentPosition.y);
			lastPosition = currentPosition;
			
			dispUpDown();
			
		}*/
		
	}
	
	/*private void RenderDepender(Transform t){
		Hashtable ht = BC.GetHashTable();
		foreach(Vector2 v in ht.Keys){
			GameObject temp = (GameObject)BC.Get(v);
				if(Mathf.Abs(v.x-currentPosition.x)<=4 && Mathf.Abs(v.y-currentPosition.y)<=4){
				temp.renderer.enabled = true;
			}else{
				temp.renderer.enabled = false;
			}
		}
	}*/
	
	public void dispUpDown() {
		
		string up, down;
		if (ConnectionHandler.bytesSent > 1000000) {
			up = System.Math.Round((double) ConnectionHandler.bytesSent / 1000000, 2) + " MB";
		}
		else up = System.Math.Round((double) ConnectionHandler.bytesSent / 1000, 2) + " KB";
		 
		if (ConnectionHandler.bytesRecv > 1000000) {
			down = System.Math.Round((double) ConnectionHandler.bytesRecv / 1000000, 2) + " MB";
		}
		else down = System.Math.Round((double) ConnectionHandler.bytesRecv / 1000, 2) + " KB";
		
		//Camera.main.GetComponent<ScriptStarter>().textUp.GetComponent<TextMesh>().text = up;
		//Camera.main.GetComponent<ScriptStarter>().textDown.GetComponent<TextMesh>().text = down;
			
	}
	
	public void UpdateLocalCoordinates(int gx, int gy) {
		
		int offset = (BOXES / 2);
		ArrayList newBlocks = new ArrayList();
		
		for (int x = -offset; x <= offset; x++) {
			for (int y = -offset; y <= offset; y++) {
				
				Vector2 gv = new Vector2(gx+x, gy+y);
				int lx = offset+x, ly = offset+y;
				
				if (BC.HasBlock(gv)) groundMeshes[lx][ly] = (GameObject) BC.Get(gv); 
				else newBlocks.Add(new Vector4(lx, ly, gx+x, gy+y));
				
			}
		}
		
		if (newBlocks.Count > 0) {
			print("Getting " + newBlocks.Count + " blocks");  
			GetBlocks(newBlocks, gx, gy);
		}
		
	}
	
	
	void GetBlocks(ArrayList list, int _gx, int _gy) {  
		
		ArrayList blockData = new ArrayList();  
		
		for (int i = 0; i < list.Count; i++) {
			 
			Vector4 c = (Vector4) list[i];
			int x = (int) c.x, y = (int) c.y, gx = (int) c.z, gy = (int) c.w;
			//BlockData block = new BlockData(x, y, gx, gy, BOXRES);  
			
			blockData.Add(new BlockData(x, y, gx, gy, BOXRES));

		}
		
		
		//ConnectionHandler _con = new ConnectionHandler();
		con.GetBlockData(blockData);
		print("Got blocks! " + blockData.Count);
		
		foreach (BlockData bd in blockData) {
			placeBlock(WeaveMaster(bd.floatData), bd.gx, bd.gy, bd.x, bd.y);
			
		}
		
	}
	
	private GameObject GetFlatSegment() {
		float [][] zeros = new float[BOXRES][];
		for (int i = 0; i < BOXRES; i++) zeros[i] = new float[BOXRES];
		return WeaveMaster(zeros);
	}
	
	private GameObject WeaveMaster(float[][] nodes) {  	
		
		//return GameObject.CreatePrimitive(PrimitiveType.Plane);
		
		int n = nodes.Length;
		int numVerts = n * n;
		int numTris = (n - 1) * (n - 1) * 6;
		
		int halfsize = n / 2;
		int triIt = 0, tl, tr, bl, br;
		
		int[] triangles = new int[numTris];
		Vector3[] verts = new Vector3[numVerts];
		

		Vector2[] uv = new Vector2[numVerts];
		float length = (float)nodes.Length;
		int index = 0;
		for ( int i=0;i<length;i++){
			for(int ii=0;ii<length;ii++){
				uv[index] = new Vector2((float)i/4,(float)ii/4);
				index++;
			}
		}
		
		GameObject newGround = new GameObject("Newground");
		
		MeshFilter mf = newGround.AddComponent<MeshFilter>();
		MeshRenderer mr = newGround.AddComponent<MeshRenderer>();
		MeshCollider mc = newGround.AddComponent<MeshCollider>();
		
		verts[0] = new Vector3(-halfsize * BOXSCALE, nodes[0][0], -halfsize * BOXSCALE);
		
		for ( int i = 1; i < n; i++ ) {
			verts[i] = new Vector3((-halfsize + i) * BOXSCALE, nodes[0][i], -halfsize * BOXSCALE);
			verts[i * n] = new Vector3(-halfsize * BOXSCALE, nodes[i][0], (-halfsize + i) * BOXSCALE);
		}
		
		
		for ( int i = 1; i < n; i++ ) {
			for ( int j = 1; j < n; j++ ) {
				
				tl = (j-1) + (i-1) * n;
				tr = j + (i-1) * n;
				bl = (j-1) + i * n;
				br = j + i * n;
				
				verts[br] = new Vector3((-halfsize + j) * BOXSCALE, nodes[i][j],  (-halfsize + i) * BOXSCALE);
				
				if (Random.Range(0,2) > 0) {
					
					triangles[triIt++] = bl;
					triangles[triIt++] = tr;
					triangles[triIt++] = tl;
					
					triangles[triIt++] = bl;
					triangles[triIt++] = br;
					triangles[triIt++] = tr;
				}
				else {
					triangles[triIt++] = br;
					triangles[triIt++] = tr;
					triangles[triIt++] = tl;
					
					triangles[triIt++] = bl;
					triangles[triIt++] = br;
					triangles[triIt++] = tl;
				}
			}
		}
		
		Mesh mesh = new Mesh();
		
		mesh.vertices = verts;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		
		mf.mesh = mesh;
		mc.sharedMesh = mesh;
		
		//Material mat = new Material(Shader.Find("Mobile/Diffuse"));
		//mat.mainTexture = (Texture2D) Resources.Load("Sand_Texture");
		//Material mat = Resources.Load("Sand_Texture") as Material;
		//mr.renderer.material = mat;
		
		//mr.material.shader = Shader.Find("Mobile/Diffuse");
		//mr.material.mainTexture = (Texture2D) Resources.Load("Sand_Texture");

		return newGround;
	}
	
	
}