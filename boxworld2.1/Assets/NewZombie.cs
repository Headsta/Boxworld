using UnityEngine;
using System.Collections;

public class NewZombie : MonoBehaviour {
	
	public GameObject meshObj;
	
	float lastUpdt, updtFreq = 0.5f;
	Vector3 target, moveDirection;
	bool standingStill = false;
	
	void Start() {
		
		target = transform.position;
		moveDirection = (target - transform.position);
		
		GetComponentInChildren<Animation>().animation.CrossFade("Take 001",0.25f);

	}
	
	void Update() {
		
		if (Input.GetMouseButton(0)) {
			Vector3 v = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 50);
			target = new Vector3(v.x, transform.position.y, v.z);
			standingStill = false;
		}
		
		if ((target - transform.position).sqrMagnitude > 5 && !GetComponentInChildren<Animation>().isPlaying) {
			
			//if (!) {
				GetComponentInChildren<Animation>().animation.CrossFade("ZombieWalk",0.25f);
			//}
			    
		    if ((Time.time - lastUpdt) > updtFreq) {
	
				//float sway = 2.0f;
				moveDirection = (target - transform.position).normalized; //+ new Vector3(Random.Range(-sway,sway), 0, Random.Range(-sway, sway));
				
				lastUpdt = Time.time;
				
				transform.LookAt(target); 
				
			}
		
			//rigidbody.AddForce(Vector3.forward);
			
			//transform.LookAt(target);
			
			float f = (target - transform.position).sqrMagnitude; 
			//if (f < 500) 
			if ((target - transform.position).sqrMagnitude > 10) f = 500;
			else f = (target - transform.position).sqrMagnitude * 50;
			
			//else if (f > 750) f = 750;
			
			rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.5f, rigidbody.velocity.y, rigidbody.velocity.z * 0.5f);
			rigidbody.AddForce(moveDirection * f);
			
		}
		/*else if (!GetComponentInChildren<Animation>().isPlaying && !standingStill) {
			GetComponentInChildren<Animation>().animation.CrossFade("Take 001",0.25f);
			standingStill = true;
		}*/
	}
	
}
