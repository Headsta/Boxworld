using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	private GameObject player;
	private Camera playerCamera;
	Vector3 playerPosition;
	public GameObject groundMesh;
	public WorldRender wr;
	void Start () {
		player = null;
		playerCamera = Camera.mainCamera;
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.Return)){
			SpawnPlayer();
		}
		if(player != null){
			ScanPosition();
		}
		
		
	}
	private void ScanPosition(){
		Ray down = new Ray(playerCamera.transform.position, Vector3.down);
		RaycastHit hit;
		if(Physics.Raycast(down,out hit, 1000f)){
			if(hit.collider != null){
				if(hit.collider.gameObject.name == "groundMesh"){
					if(groundMesh != hit.collider.gameObject){
						groundMesh = hit.collider.gameObject;
						Vector2 coordinates = wr.GetCoordinatesFor(groundMesh);
						Debug.Log(groundMesh.name+": x:"+coordinates.x+" y:"+coordinates.y);
						wr.SetCurrentPosition(coordinates);
					}
				}
			}
		}
	}
	private void SpawnPlayer(){
		GameObject groundMesh = wr.GetGroundMesh(wr.globalStartPosition);
		if(groundMesh == null || player != null){
			return;
		}
		
		float x = groundMesh.renderer.bounds.max.x-(groundMesh.renderer.bounds.max.x+(groundMesh.renderer.bounds.min.x*0.5f));
		float y = groundMesh.renderer.bounds.max.y+(groundMesh.renderer.bounds.max.y+(groundMesh.renderer.bounds.min.y));
		float z = groundMesh.renderer.bounds.max.z-(groundMesh.renderer.bounds.max.z-(groundMesh.renderer.bounds.min.z*0.5f));
		
		 player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		
		
		player.transform.localScale = new Vector3(3.469746f,4.222249f,3.469746f);
		float height = player.renderer.bounds.max.y-player.renderer.bounds.min.y;
		Vector3 center = new Vector3(x,y+height,z);
		player.transform.position = center;
		player.AddComponent<Rigidbody>();
		Rigidbody rb = player.GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |  RigidbodyConstraints.FreezePositionX |  RigidbodyConstraints.FreezePositionZ ;
		//transform.parent = player.transform;
		
		player.AddComponent<CameraTraveler>();
		player.GetComponent<CameraTraveler>().player = player;
	}
}
