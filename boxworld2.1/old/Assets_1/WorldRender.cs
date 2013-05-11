using UnityEngine;
using System.Collections;

public class WorldRender : MonoBehaviour {
	
	// yo alex! Hola Clint!
	
	ArrayList boxelWorld;
	Shader boxelShader;
	//GameObject groundMesh;
	GameObject boxel;
	
	// BOXES x BOXES of size (power of two) units
	const int BOXES = 7, BOXSIZE = 32;
	
	float[][] worldNodes; 
	
	NormalDistRing randGen; 
	
	void Awake () {
		Init();
	}
	
	private void Init(){
		DefineWorldNodes();
		//DefineBoxel();
		
		CreateWorldNodes();
		
		float[][] meshbox = new float[BOXSIZE+1][];
		
		for (int x = 0; x < BOXES; x++) {
			for (int y = 0; y < BOXES; y++) { 
				
				randGen = new NormalDistRing(555);
				
				int xoff = (x*BOXSIZE), yoff = (y*BOXSIZE);
				
				if (x <= 3 && y <= 3)
					SquareAndDiamond( xoff, yoff , BOXSIZE , 1.31f); 
				else if ((x == 4 && y < 5) || (y == 4 && x < 5)) 
					SquareAndDiamond( xoff, yoff , BOXSIZE , 1.71f);
				else if (x == 6 || y == 6)
					SquareAndDiamond( xoff, yoff , BOXSIZE , 1.71f); 
				else 
					SquareAndDiamond( xoff, yoff , BOXSIZE , 0.31f);
				

				for (int i = 0; i <= BOXSIZE; i++) 
					meshbox[i] = new float[BOXSIZE+1]; 
			
				for (int i = 0; i <= BOXSIZE; i++) {	
					for (int j = 0; j <= BOXSIZE; j++) {
			
						meshbox[j][i] = worldNodes[i+xoff][j+yoff];	
			
					}
				}
			
				WeaveMaster((x*(BOXSIZE)) , (y*(BOXSIZE)) , meshbox); 
						
			}
		}
		
		//CreateCubeWorld(worldNodes);
		//WeaveMaster(0,0,worldNodes);  
		
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
	
	private void SquareAndDiamond(int tlx, int tly, int size, float range) {
			
		if (size == 1) return;
		
		//print(tlx + ", " + tly + " size " + size + " -> " + (size >> 1) + " range " + range);
		//print("maxx " + (tlx+size) + " maxy " + (tly+size) );
		
		float mid_avrg = (	worldNodes[tlx][tly] +
							worldNodes[tlx+size][tly] +
							worldNodes[tlx][tly+size] +
							worldNodes[tlx+size][tly+size] ) / 4;
		
		int halfsize = (size >> 1), midx = tlx + halfsize, midy = tly + halfsize;

		float r1 = randGen.GetFloat(range);
		float r2 = randGen.GetFloat(range * 0.75f);
		range /= 2;
		
		worldNodes[midx][midy] = (mid_avrg + r1);
				
		// N
		if (worldNodes[midx][tly] == 0.0f) worldNodes[midx][tly] = r2 + (	worldNodes[tlx][tly] + 
																			worldNodes[tlx+size][tly] +
																			worldNodes[midx][midy]	) / 3;

		// S
		if (worldNodes[midx][tly+size] == 0.0f) worldNodes[midx][tly+size] = r2 + (	worldNodes[tlx][tly+size] + 
																					worldNodes[tlx+size][tly+size] + 
																					worldNodes[midx][midy]	) / 3;
		
		// W
		if (worldNodes[tlx][midy] == 0.0f) worldNodes[tlx][midy] = r2 + (	worldNodes[tlx][tly] + 
																			worldNodes[tlx][tly+size] + 
																			worldNodes[midx][midy]	) / 3;
		
		// E
		if (worldNodes[tlx+size][midy] == 0.0f) worldNodes[tlx+size][midy] = r2 + (	worldNodes[tlx+size][tly] + 
																					worldNodes[tlx+size][tly+size] + 
																					worldNodes[midx][midy]	) / 3;
		
		
		SquareAndDiamond( tlx , tly, halfsize, range );
		SquareAndDiamond( tlx + halfsize, tly, halfsize, range );
		SquareAndDiamond( tlx, tly + halfsize, halfsize, range );
		SquareAndDiamond( tlx + halfsize, tly + halfsize, halfsize, range );
		
		
	}
	private void DefineWorldNodes(){
		 
		//worldNodes = new float[25,SIZE,SIZE];
		worldNodes = new float[((BOXSIZE * BOXES) + 1)][];
		
		//print("size "  + worldNodes.Length );
		//for (int i = 0; i < 25; i++) {
			
			//worldNodes[i] = new float[SIZE+1][];
			
		for(int j = 0; j < worldNodes.Length; j++) {
			//for(int k = 0; k < worldNodes[i][j].Length; k++) {
			worldNodes[j] = new float[((BOXSIZE * BOXES) + 1)];
				
			//}				
		}
		//}
		
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
	
	}
	
	private void WeaveMaster(int x, int y, float[][] nodes){
		
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

	

