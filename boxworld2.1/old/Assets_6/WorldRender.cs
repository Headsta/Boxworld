using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRender : MonoBehaviour {
	
	// yo alex! Hola Clint!
	
	// BOXES x BOXES of size (power of two) units
	public static int BOXES = 5, BOXSIZE = 33;
	
	GameObject[][] groundMeshes;
	Dictionary<Vector2,GameObject> localCoordinates;
	private Vector2 currentPosition = new Vector2(0,0);
	private Vector2 lastPosition= new Vector2(0.1f,0.1f);
	private int renderDist = 3;
	LocalWorld world;
	public Vector2 globalStartPosition = new Vector2(0,0);
		
	bool otherway = false;
	
	void Start () {
		
		Init();
	}
	
	private void GetStartSegments(){
		int x = 0;
		int y = 0;
		
		//int gx = 0, gy = 0;
				
		int off = (BOXES - 1) / 2;
		
		if (otherway) groundMeshes[0][0] = GenerateMesh(0,0);
		else {
			for(int gx = (int)globalStartPosition.x-off; gx<=(int)globalStartPosition.x+off; gx++){
				for(int gy = (int)globalStartPosition.y-off; gy<=(int)globalStartPosition.y+off; gy++){
					groundMeshes[x][y] = GenerateMesh(gx,gy);
					//groundMeshes[0][1] = GenerateMesh(0,1);
			
					y++;
				}
				y = 0;
				x++;
			}
		}
	}
	
	private GameObject GenerateMesh(int gx, int gy){
		
		
		//int b = BOXSIZE - ( gx != 0 ? 0 : 0 );
		//int h = BOXSIZE - ( gy != 0 ? 0 : 0 );
		
		int numfloats = (1 + (BOXSIZE-1) * BOXES);
		
		GameObject groundMesh;
		
		float[][] meshbox = new float[BOXSIZE][];
		for (int i = 0; i < BOXSIZE; i++) meshbox[i] = new float[BOXSIZE];
		
		float[][] meshbox2 = new float[numfloats][];
		for (int i = 0; i < meshbox2.Length; i++) meshbox2[i] = new float[numfloats];

		if (otherway) {
			
			
			int mid = (BOXES-1)/2;

			
			for (int i = 0; i < BOXES; i++) {
				for (int j = 0; j < BOXES; j++) {
		
					ConnectionHandler c1 = new ConnectionHandler();
					c1.get_block_datao(j - mid, i - mid, meshbox2, i * (BOXSIZE-1), j * (BOXSIZE-1));
					
				}
			}
			
			groundMesh = WeaveMaster(gx, gy, meshbox2);
			
		} 
		else {
			
			ConnectionHandler c1 = new ConnectionHandler();
			c1.get_block_data(gx, gy, meshbox);
			
			groundMesh = WeaveMaster(gx, gy, meshbox);
		
		}
		
		groundMesh.AddComponent<GroundMesh>();
		groundMesh.GetComponent<GroundMesh>().gX = gx;
		groundMesh.GetComponent<GroundMesh>().gY = gy;
		localCoordinates.Add(new Vector2(gx,gy),groundMesh);
		PlaceGroundMeshGXGY(groundMesh,gx,gy);
		
		return groundMesh;
		
	}
	
	private void Init(){
		DefineWorldNodes();
		//world = new LocalWorld();
		GetStartSegments();
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
		GameObject tempGroundMesh = new GameObject("groundMesh");
		tempGroundMesh.AddComponent<MeshFilter>();
		tempGroundMesh.AddComponent<MeshRenderer>();
		tempGroundMesh.renderer.material.shader = Shader.Find("VertexLit");
		
		return tempGroundMesh;
	}
	
	private void DefineWorldNodes(){
		localCoordinates = new Dictionary<Vector2, GameObject>();
		groundMeshes = new GameObject[BOXES][];
		for(int i = 0; i<groundMeshes.Length; i++){
			groundMeshes[i] = new GameObject[groundMeshes.Length];
		}
	}

	void Update(){
		if(lastPosition != currentPosition){
			float dx = currentPosition.x - lastPosition.x, dy = currentPosition.y - lastPosition.y;
			RenderLogic();
		}
		lastPosition = currentPosition;
	}
	
	private void UpdatePosition(){
		
	}
	private void UpdateLocalCoordinates(int dx, int dy){
		/*for(int i = x; x<groundMeshes.Length; x++){
			for(int y = 0; y<groundMeshes[y].Length; y++){
				
			}
		}
		*/
		
	}
	private void RenderLogic(){
		/*for (int x = 0; x < groundMeshes.Length; x++) {
			for (int y = 0; y < groundMeshes[x].Length; y++) {
				GroundMesh gm = groundMeshes[x][y].GetComponent<GroundMesh>();
				int gx = gm.gX;
				int gy = gm.gY;
				if(gx > currentPosition.x-renderDist && gx < currentPosition.x+renderDist && gy > currentPosition.y-renderDist && gy < currentPosition.y+renderDist){
					groundMeshes[x][y].renderer.enabled = true;
				}else{
					groundMeshes[x][y].renderer.enabled = false;
				}
			}	
		}*/
	}
	
	private GameObject WeaveMaster(int x, int y, float[][] nodes){
		
		ArrayList weave = new ArrayList();
		GameObject groundMesh = CreateGroundComponent();
		
		int h, hmax, b, bmax;
		
		h = 0 + (y < 0 ? 0 : 0);
		hmax = (nodes.Length - 1) - (y < 0 ? 0 : 0);
		
		for( ; h < hmax; h++ ) {
			
			b = 0 + (x > 0 ? 0 : 0);
			bmax = (nodes[h].Length - 1) - (x > 0 ? 0 : 0);
			
			for( ; b < bmax; b++ ) {
	
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