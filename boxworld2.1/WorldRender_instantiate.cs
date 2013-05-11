using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Threading;

public class WorldRender : MonoBehaviour {
	
	// yo alex! Hola Clint!
	
	// BOXES x BOXES of size (power of two) units
	public static int BOXES = 7, BOXSIZE = 33;
	
	GameObject[][] groundMeshes;
	
	private Vector2 currentPosition = new Vector2(0,0);
	private Vector2 lastPosition = new Vector2(0,0);
	
	private int renderDist = 3;
	LocalWorld world;
	public Vector2 globalStartPosition = new Vector2(0,0);
	
	public PlayerManager pm;
	public GameObject originalGroundComponent;
	
	private ArrayList groundMeshQueue;
	
	void Start () {
		
		Init();
	}
	
	private void GetStartSegments(){
		int x = 0;
		int y = 0;
		for(int gx = (int)globalStartPosition.x-4; gx<=(int)globalStartPosition.x+4; gx++){
			for(int gy = (int)globalStartPosition.y-4; gy<=(int)globalStartPosition.y+4; gy++){
				groundMeshes[x][y] = GenerateMesh(gx,gy);
				y++;
			}
			y = 0;
			x++;
		}
	}
	
	private GameObject GenerateMesh(int gx, int gy){
		
		float[][] meshbox = new float[BOXSIZE][];
		for (int i = 0; i < BOXSIZE; i++)
			meshbox[i] = new float[BOXSIZE];
		
		ConnectionHandler c1 = new ConnectionHandler();
		c1.get_block_data(gx, gy, meshbox);
		
		GameObject groundMesh = WeaveMaster(gx, gy, meshbox);
		groundMesh.AddComponent<GroundMesh>();
		groundMesh.GetComponent<GroundMesh>().gX = gx;
		groundMesh.GetComponent<GroundMesh>().gY = gy;
		PlaceGroundMeshGXGY(groundMesh,gx,gy);
		
		return groundMesh;
	}
	
	private GameObject DefineOriginalGroundComponent(){
		GameObject tempGroundMesh = new GameObject("groundMesh");
		tempGroundMesh.AddComponent<MeshFilter>();
		tempGroundMesh.AddComponent<MeshRenderer>();
		tempGroundMesh.renderer.material.shader = Shader.Find("VertexLit");
		
		return tempGroundMesh;
	}
	private void Init(){
		
		originalGroundComponent = DefineOriginalGroundComponent();
		DefineWorldNodes();
		world = new LocalWorld();
		GetStartSegments();
		
		RenderLogic();
	}
	
	private void PlaceGroundMeshGXGY(GameObject groundMesh, int gX, int gY){
		groundMesh.renderer.enabled = true;
		float x = (gX*(groundMesh.renderer.bounds.max.x-groundMesh.renderer.bounds.min.x))-((groundMesh.renderer.bounds.max.x-groundMesh.renderer.bounds.min.x)*0.5f);
		float y = (gY*(groundMesh.renderer.bounds.max.z-groundMesh.renderer.bounds.min.z))-((groundMesh.renderer.bounds.max.z-groundMesh.renderer.bounds.min.z)*0.5f);
		groundMesh.transform.position = new Vector3(x,groundMesh.transform.position.y,y);
	}
	
	public Vector2 GetCoordinatesFor(GameObject groundMesh){
		Vector2 coordinates = Vector2.zero;
		for(int x = 0; x<groundMeshes.Length; x++){
			for(int y = 0; y<groundMeshes[x].Length; y++){
				if(groundMeshes[x][y] == groundMesh){
					coordinates.x = groundMesh.GetComponent<GroundMesh>().gX;
					coordinates.y = groundMesh.GetComponent<GroundMesh>().gY;
					break;
				}
			}
		}
		return coordinates;
	}
	
	public GameObject GetGroundMesh(Vector2 coordinates){
		GameObject groundMesh = null;
		int gx = (int)coordinates.x;
		int gy = (int)coordinates.y;
		
		for(int x = 0; x<groundMeshes.Length; x++){
			for(int y = 0; y<groundMeshes[x].Length; y++){
				GroundMesh gm = groundMeshes[x][y].GetComponent<GroundMesh>();
				if(gm.gX == gx && gm.gY == gy){
					groundMesh = groundMeshes[x][y];
					break;
				}
			}
		}
		return groundMesh;
	}
	
	public void SetCurrentPosition(Vector2 coordinates){
		currentPosition = coordinates;
	}
	
	private GameObject CreateGroundComponent(){
		GameObject tempGroundMesh = (GameObject)GameObject.Instantiate(originalGroundComponent);
		tempGroundMesh.name = originalGroundComponent.name;
		return tempGroundMesh;
	}
	
	private void DefineWorldNodes(){
		groundMeshes = new GameObject[9][];
		for(int i = 0; i<groundMeshes.Length; i++){
			groundMeshes[i] = new GameObject[groundMeshes.Length];
		}
		
	}
	
	void FixedUpdate(){
		
	}

	void Update(){
				
		if (pm != null) pm.getPos(ref currentPosition);
				
		if(lastPosition != currentPosition){
			
			float dx = currentPosition.x - lastPosition.x, dy = currentPosition.y - lastPosition.y;
			
			Debug.Log("We are now at "  + currentPosition.x + ", " + currentPosition.y);
			
			UpdateLocalCoordinates((int)dx,(int)dy);
			
			RenderLogic();
			
			lastPosition = currentPosition;
		}

	}
	
	
	private void UpdateLocalCoordinates(int dx, int dy){
		
		//Debug.Log("dx! " + dx + " dy " + dy );
		
		GameObject[][] newGM = new GameObject[groundMeshes.Length][];
		for(int i = 0; i < groundMeshes.Length; i++ ) newGM[i] = new GameObject[groundMeshes.Length];
		
		ArrayList newBlocks = new ArrayList();
		
		// int madness o_O;;
		int mbx = ( dx < 0 ? 0 : dx );
		int mex = ( dx > 0 ? groundMeshes.Length : groundMeshes.Length + dx );
		
		int mby = ( dy < 0 ? 0 : dy );
		int mey = ( dy > 0 ? groundMeshes.Length : groundMeshes.Length + dy );
		
		
		int rbx = ( dx < 0 ? 0 : groundMeshes.Length-dx );
		int rex = ( dx < 0 ? Mathf.Abs(dx) : groundMeshes.Length );

		int rby = ( dy < 0 ? 0 : groundMeshes.Length-dy );
		int rey = ( dy < 0 ? Mathf.Abs(dy) : groundMeshes.Length );
		
		//Debug.Log("rbx " + rbx + " rex " + rex );
		//Debug.Log("rby " + rby + " rey " + rey );
		
		for (int x = 0; x < groundMeshes.Length; x++){
			for (int y = 0; y < groundMeshes[x].Length; y++){
				
				if (x >= mbx && x < mex && y >= mby && y < mey) {
					
					newGM[x-dx][y-dy] = groundMeshes[x][y];
					
				}
				else {
					// Destroy	
					//Debug.Log("remoing block " + x + ", " + y);
					Destroy((GameObject)groundMeshes[x][y]);
				}
				
				if ((x >= rbx && x < rex) || (y >= rby && y < rey)) {
					
					// Request x, y
					
					int offset = ( groundMeshes.Length - 1 ) / 2;
					int ox = (int) currentPosition.x - ( offset - x ); 
					int oy = (int) currentPosition.y - ( offset - y );
					
									
					//newGM[x][y] = GenerateMesh(ox,oy);
					newBlocks.Add(new Vector4(x,y,ox,oy));
					
				}
			}
		}
		
		groundMeshes = newGM;
			
		new Thread(new ParameterizedThreadStart(GetBlock)).Start(newBlocks);
	}
	
	
	void GetBlock(object inobj) {
		
		Debug.Log("Thread started"); 
		
		ArrayList list = (ArrayList) inobj;
		foreach ( Vector4 c in list ) {
		
			Debug.Log("getting block " + c.z + ", "  + c.w);
			
			GameObject o = GenerateMesh((int)c.z,(int)c.w);
			
			//groundMeshes[(int)c.x][(int)c.y] = 
			Debug.Log("got block " + c.x + ", "  + c.y);
			
		}
	}
	
	/*public IEnumerator GetSegment(int gx, int gy, int x, int y){
		Debug.Log("start");
		GameObject temp = GenerateMesh(gx,gy);
		while(temp == null)yield return null;
		
		groundMeshes[x][y] = temp;
		Debug.Log("done");
		
	}*/

	private void RenderLogic(){
		for (int x = 0; x < groundMeshes.Length; x++) {
			for (int y = 0; y < groundMeshes[x].Length; y++) {
				if(groundMeshes[x][y] == null)continue;
				GroundMesh gm = groundMeshes[x][y].GetComponent<GroundMesh>();
				int gx = gm.gX;
				int gy = gm.gY;
				if(gx > currentPosition.x-renderDist && gx < currentPosition.x+renderDist && gy > currentPosition.y-renderDist && gy < currentPosition.y+renderDist){
					groundMeshes[x][y].renderer.enabled = true;
				}else{
					groundMeshes[x][y].renderer.enabled = false;
				}
			}	
		}
	}
	
	private GameObject WeaveMaster(int x, int y, float[][] nodes){
		
		ArrayList weave = new ArrayList();
		GameObject groundMesh = CreateGroundComponent();
		
		for(int h = 0; h<nodes.Length-1; h++){
			for(int b = 0; b<nodes[h].Length-1; b++){
				Vector3 v0, v1, v2, v3;
				v0 = new Vector3(b+1 ,(float)(nodes[h+1][b+1]), h+1);
				v1 = new Vector3(b+1 ,(float)(nodes[h][b+1]), h);
				v2 = new Vector3(b,(float)(nodes[h+1][b]), h+1);
				v3 = new Vector3(b,(float)(nodes[h][b]), h);
				groundMesh.GetComponent<MeshFilter>().mesh = CreatePollyMesh(new Vector3[]{v0,v1,v2,v3});
				weave.Add((MeshFilter)MeshFilter.Instantiate(groundMesh.GetComponent<MeshFilter>()));
			}
		}
		
		CombineInstance[] combine = new CombineInstance[weave.Count];
	
		for(int i = 0; i<weave.Count; i++){  
			MeshFilter meshFilter = (MeshFilter)weave[i];
		    combine[i].mesh = meshFilter.sharedMesh;
		    combine[i].transform = meshFilter.transform.localToWorldMatrix;
		    meshFilter.gameObject.active = false;
		   
		}
		groundMesh.transform.GetComponent<MeshFilter>().mesh = new Mesh();  
		groundMesh.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		groundMesh.transform.gameObject.active = true;
		groundMesh.transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();
		groundMesh.transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
		groundMesh.transform.GetComponent<MeshFilter>().mesh.Optimize();
		
		groundMesh.transform.position = new Vector3(x,0,y);
		
		for(int i = 0; i<weave.Count; i++){
			MeshFilter loseThread = (MeshFilter)weave[i];
			GameObject.Destroy(loseThread.gameObject);
		}
		groundMesh.AddComponent<MeshCollider>();
		return groundMesh;
	}
	
	Mesh CreatePollyMesh(Vector3[] segment){
        Mesh mesh = new Mesh();
        Vector3[] vertices = segment;
        Vector2[] uv = new Vector2[]{ new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0), };
        int[] triangles = new int[]{0, 1, 2, 2, 1, 3,};

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        return mesh;
    }
}