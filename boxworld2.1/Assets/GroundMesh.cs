using UnityEngine;
using System.Collections;

public class GroundMesh : MonoBehaviour {
	
	
	private GameObject myGameObject;
	private Transform myTransfom;
	public int gX;
	public int gY;
	public Mesh mesh;
	private GameObject grass;
	
	private void Start () {
		SetupScriptReferences();


	}
	
	private void SetupScriptReferences(){
		myGameObject = gameObject;
		myTransfom = transform;

		
	
		//SetUpParticleSystem(myGameObject);
				//myGameObject.AddComponent<MeshRenderer>();
		//myGameObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
		//myGameObject.GetComponent<MeshFilter>().mesh.name = "Grass";
		myGameObject.renderer.material = CheckerdDiffuse();
		
	}
	
	/*private void SetUpParticleSystem(GameObject ground){
		Transform t = ground.transform;
		GameObject g = ground;
		grass = new GameObject("grass");
	 
		
		//CreateGrassField(10,10, g);
		
		
	}*/
	
	/*private void CreateGrassField(int xAmount, int yAmount, GameObject g){
		g.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		Mesh[][] meshes = new Mesh[xAmount][];
		for(int i = 0; i<meshes.Length; i++){
			meshes[i] = new Mesh[yAmount];
		}
		
		for(int b = 0; b<xAmount; b++){
			for(int d = 0; d<yAmount; d++){
				meshes[b][d] = CreateGrassStraw(b,d,g);
			}
		}
  		MeshFilter mf = g.AddComponent<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[xAmount*yAmount];
        transform.gameObject.active = true;
    
		int index = 0;
		for(int b = 0; b<xAmount; b++){
			for(int d = 0; d<yAmount; d++){
				combine[index].mesh = meshes[b][d];
				combine[index].transform = transform.localToWorldMatrix;
				index++;
			}
		}
		
		g.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		
	}*/
	
	/*private Mesh CreateGrassStraw(int b, int d, GameObject g){
		Transform t = g.transform;
		float width =.5f;
		float height = 1.0f;
		float depth = 1.0f/d;
		float x = g.renderer.bounds.min.x;
		float y = t.position.y;
		float z = g.renderer.bounds.min.y;
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]{new Vector3(x+(b*width),y,z+d ),new Vector3(x+b(b*width),y+height,z+d),new Vector3((b*width)+width,y+height,z+d),new Vector3((b*width)+width,y,z+d)};
		mesh.triangles = new []{0,1,2, 0,2,3};
		return mesh;
	}
	*/
	private void Update () {
	
	}
	
	private Material CheckerdDiffuse(){
		Material m = new Material(Shader.Find("VertexLit"));
		Texture2D t = new Texture2D(2,2);
		Color w = new Color(1f,1f,1f,1f);
		Color g = new Color(.5f,.5f,.5f,1f);
			t.SetPixel(0,0,g);
			t.SetPixel(1,0,w);
			t.SetPixel(1,1,g);
			t.SetPixel(0,1,w);
			t.Apply();
			t.filterMode = FilterMode.Point;
			m.mainTexture = t;
		return m;
	}
	
	private Mesh CreateGrassShape(){
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]{new Vector3(0,0,0),new Vector3(0,1,0),new Vector3(0.1f,1,0),new Vector3(0.1f,0,0)};
		mesh.triangles = new int[]{0,1,2 ,0,3,2};
		mesh.uv = new Vector2[]{new Vector2(0,0),new Vector2(0,1),new Vector2(1,1),new Vector2(0,1)};
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		return mesh;
	}
	
	public void Flatten() {
		
		MeshFilter mf = GetComponent<MeshFilter>();
		//MeshRenderer mr = GetComponentt<MeshRenderer>();
		//MeshCollider mc = GetComponent<MeshCollider>();
		
		Vector3 [] newverts = new Vector3[mf.mesh.vertices.Length];
		float avrgy = 0;
		for ( int i = 0; i < mf.mesh.vertices.Length; i++ ) {
			//newverts[i] = new Vector3(mf.mesh.vertices[i].x, 0, mf.mesh.vertices[i].z);
			avrgy += mf.mesh.vertices[i].y;
		}
		avrgy /= mf.mesh.vertices.Length;
		for ( int i = 0; i < mf.mesh.vertices.Length; i++ ) {
			newverts[i] = new Vector3(mf.mesh.vertices[i].x, avrgy, mf.mesh.vertices[i].z);
		}
		
		mf.mesh.vertices = newverts;
		DestroyImmediate(GetComponent<MeshCollider>());
		gameObject.AddComponent<MeshCollider>();
		//.
		//foreach (Vector3 v in mf.mesh.vertices) Debug.Log("Vec " + v);
		
	}
}
