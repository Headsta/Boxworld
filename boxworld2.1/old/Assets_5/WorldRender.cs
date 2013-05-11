using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WorldRender : MonoBehaviour {
	
	// yo alex! Hola Clint!
	
	ArrayList boxelWorld;
	Shader boxelShader;
	//GameObject groundMesh;
	GameObject boxel;
	
	// BOXES x BOXES of size (power of two) units
	const int BOXES = 7, BOXSIZE = 33;
	
	int global_x, global_y;
	
	float[][] worldNodes; 
	GameObject[][] groundMeshes;
	Dictionary<Vector2,GameObject> localCoordinates;
	
	NormalDistRing randGen; 
	private Vector2 currentPosition;
	private Vector2 lastPosition;
	private int renderDist = 3;
	
	LocalWorld world;
	
	void Start () {
		
		Init();
	}
	
	void GenerateMesh(int gx, int gy) {
		
		float[][] meshbox = new float[BOXSIZE][];
		for (int i = 0; i < BOXSIZE; i++) 
			meshbox[i] = new float[BOXSIZE]; 
		
		ConnectionHandler c1 = new ConnectionHandler();
		c1.get_block_data(gx, gy, meshbox);
		
		GameObject groundMesh = WeaveMaster(gx, gy, meshbox);
		groundMesh.AddComponent<GroundMesh>();
		groundMesh.GetComponent<GroundMesh>().gX = gx;
		groundMesh.GetComponent<GroundMesh>().gY = gy;
		localCoordinates.Add(new Vector2(gx,gy),groundMesh);
		
		PlaceGroundMeshGXGY(groundMesh,gx,gy);
	}
	
	private void Init(){
		DefineWorldNodes();
		//DefineBoxel();
		
		global_x = global_y = 0;
		
		CreateWorldNodes();
		
		world = new LocalWorld();
		
		for (int i = 0; i < 5; i++) for (int j = 0; j < 5; j++) GenerateMesh(i,j);
		
		
	}
	

	private void PlaceGroundMeshesOldWay(){
		for(int x = 0; x<groundMeshes.Length; x++){
			for(int y =0; y<groundMeshes[x].Length; y++){
				float gx = (x*(groundMeshes[x][y].renderer.bounds.max.x-groundMeshes[x][y].renderer.bounds.min.x))-((groundMeshes[x][y].renderer.bounds.max.x-groundMeshes[x][y].renderer.bounds.min.x)*0.5f);
				float gy = (y*(groundMeshes[x][y].renderer.bounds.max.z-groundMeshes[x][y].renderer.bounds.min.z))-((groundMeshes[x][y].renderer.bounds.max.z-groundMeshes[x][y].renderer.bounds.min.z)*0.5f);
				groundMeshes[x][y].transform.position = new Vector3(gx,groundMeshes[x][y].transform.position.y,gy);
			}
		}
	}
	
	private void PlaceGroundMeshGXGY(GameObject groundMesh, int gX, int gY){
		groundMesh.renderer.enabled = true;
		float x = (gX*(groundMesh.renderer.bounds.max.x-groundMesh.renderer.bounds.min.x))-((groundMesh.renderer.bounds.max.x-groundMesh.renderer.bounds.min.x)*0.5f);
		float y = (gY*(groundMesh.renderer.bounds.max.z-groundMesh.renderer.bounds.min.z))-((groundMesh.renderer.bounds.max.z-groundMesh.renderer.bounds.min.z)*0.5f);
		groundMesh.transform.position = new Vector3(x,groundMesh.transform.position.y,y);
	}
	
	public Vector2 GetCoordinatesFor(GameObject groundMesh){
		Vector2 coordinates = Vector2.zero;
		for (int x = 0; x < groundMeshes.Length; x++) {
			for (int y = 0; y < groundMeshes[x].Length; y++) {
				if(groundMesh == groundMeshes[x][y]){
					coordinates = new Vector2(x,y);
					break;
				}
			}
		}
		return coordinates;
	}
	public void SetCurrentPosition(Vector2 coordinates){
		currentPosition = coordinates;
	}
	
	private void DefineBoxel(){
		boxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
		boxelShader = Shader.Find("VertexLit");
		boxel.renderer.material.shader = boxelShader;
		
		
	}
	
	private GameObject CreateGroundComponent(){
		GameObject tempGroundMesh = new GameObject("groundMesh");
		tempGroundMesh.AddComponent<MeshFilter>();
		tempGroundMesh.AddComponent<MeshRenderer>();
		tempGroundMesh.renderer.material.shader = Shader.Find("VertexLit");
		
		return tempGroundMesh;
	}
	
	private void CreateWorldNodes() {
				
		//for ( int multiples = 0; ((SIZE >> multiples) & 1) == 0; ++multiples ); 
				
		//worldNodes[16][16] = -10.0f;
		
		/*worldNodes[16][16] = 25.0f;
		worldNodes[32][32] = 50.0f;
		worldNodes[48][48] = 42.0f;
		worldNodes[64][64] = 1.0f;
		
		worldNodes[16][32] = 12.0f;
		worldNodes[16][48] = 7.0f;
		
		worldNodes[32][16] = 15.0f;
		worldNodes[48][16] = 6.0f;
		
		worldNodes[48][32] = 55.0f;
		worldNodes[32][48] = 30.0f;*/
		
	}
	
	private void DefineWorldNodes(){
		 
		//worldNodes = new float[25,SIZE,SIZE];
		worldNodes = new float[((BOXSIZE * BOXES) + 1)][];
		groundMeshes = new GameObject[BOXES][];
		//print("size "  + worldNodes.Length );
		//for (int i = 0; i < 25; i++) {
			
			//worldNodes[i] = new float[SIZE+1][];
			
		for(int j = 0; j < worldNodes.Length; j++) {
			//for(int k = 0; k < worldNodes[i][j].Length; k++) {
			worldNodes[j] = new float[((BOXSIZE * BOXES) + 1)];
			//}				
		}
		//}
		
		for(int x = 0; x < groundMeshes.Length; x++){
			groundMeshes[x] = new GameObject[BOXES];
		}
		
		localCoordinates = new Dictionary<Vector2, GameObject>();
		
	}
	private ArrayList CreateCubeWorld(float[][] nodes){
		
		ArrayList tempWorld = new ArrayList();
		for(int b = 0; b<nodes.Length; b++){
			
			for(int h = 0; h<nodes[b].Length; h++){
				GameObject tempBoxel = (GameObject)GameObject.Instantiate(boxel);
				//tempBoxel.renderer.material.SetColor("_Color", new Color());
				tempBoxel.transform.position = new Vector3(b,(float)nodes[b][h], h);
				tempWorld.Add(GameObject.Instantiate(boxel));
			}
		}
		
		return tempWorld;
	}
	
	
	void Update () {
		if(lastPosition != currentPosition){
			
			float dx = currentPosition.x - lastPosition.x, dy = currentPosition.y - lastPosition.y;
			
			global_x += (int) dx;
			global_y += (int) dy;
			
			Debug.Log("dx " + dx + " dy " + dy + " => global x,y = " + global_x + "," + global_y );
			
			for (int x = 0; x < groundMeshes.Length; x++) {
				for (int y = 0; y < groundMeshes[x].Length; y++) {
					if(x > currentPosition.x-renderDist && x < currentPosition.x+renderDist && y > currentPosition.y-renderDist && y < currentPosition.y+renderDist){
						groundMeshes[x][y].renderer.enabled = true;
					}else{
						groundMeshes[x][y].renderer.enabled = false;
					}
				}
				
			}
		}
		lastPosition = currentPosition;
	}
	
	private GameObject WeaveMaster(int x, int y, float[][] nodes){
		
		ArrayList weave = new ArrayList();
		GameObject groundMesh = CreateGroundComponent();
		//int xoff = (box % 5), yoff = (box / 5);
		
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
			//should work
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

	

