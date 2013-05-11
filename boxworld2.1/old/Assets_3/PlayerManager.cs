using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	private GameObject player;
	
	void Start () {
		player = null;
	}
	
	
	void Update () {
		if(Input.GetKey(KeyCode.Return)){
			SpawnPlayer();
		}
	}
	
	private void SpawnPlayer(){
		GameObject groundMesh = GameObject.Find("groundMesh");
		if(groundMesh == null || player != null){
			return;
		}
		
		float x = groundMesh.renderer.bounds.max.x+(groundMesh.renderer.bounds.min.x*0.5f);
		float y = groundMesh.renderer.bounds.max.y+(groundMesh.renderer.bounds.min.y*0.5f);
		float z = groundMesh.renderer.bounds.max.z+(groundMesh.renderer.bounds.min.z);
		
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
