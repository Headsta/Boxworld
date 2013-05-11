using UnityEngine;
using System.Collections;

public class NewDwarfAnimation : MonoBehaviour {
	
	float startrun, stoprun;
	
	float lastUpdt, updtFreq = 0.05f;
	Vector3 target, moveDirection;
	bool standingStill = false;
	
	public Rigidbody body;
	
	public GameObject gotoObj;
	
	private Vector2 currentPosition;
	private Vector2 lastPosition;
	
	public WorldRender wr;
	
	// Use this for initialization
	void Start () {
		
		/*animation.AddClip(animation.clip,"Run", 21, 24);
		animation.AddClip(animation.clip,"Idle", 290, 350 );
		animation.AddClip(animation.clip,"Jump", 29, 40 );
		animation["Run"].speed = 0.5f;
		animation["Run"].wrapMode = WrapMode.PingPong;*/
		
		/*animation.AddClip(animation.clip,"Run", 15, 25);
		animation.AddClip(animation.clip,"Idle", 290, 350 );*/
		
	}
	

	
	// Update is called once per frame
	void Update () {
	
		/*if (animation["Animation"].time > stoprun) {
			
			animation["Animation"].enabled = false;
			
			animation["Animation"].time = startrun;
			animation["Animation"].enabled = true;
		}*/
		
		/*if (Input.GetMouseButton(0)) {
			
			//print("Mouse.x " + Input.mousePosition.x + " width " + Screen.width);
			
			float r = 15.0f + (Input.mousePosition.y / Screen.height) * 60.0f;
			float len = 30.0f / Mathf.Cos(r * Mathf.Deg2Rad);
			
			Vector3 hitPoint = transform.position;
			RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
			foreach(RaycastHit hit in hits){
				if(hit.collider.gameObject.name.Contains(wr.groundName)){
					hitPoint = hit.point;
				}
			}
			Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
			Vector3 pos = ray.origin + ray.direction * len;
			
			target = hitPoint;
			gotoObj.transform.position = new Vector3(pos.x, 0, pos.z);
			target.y = transform.parent.position.y;
			Debug.DrawRay(ray.origin, ray.direction*1000.0f,Color.green);
			//Camera.mainCamera.GetComponent
			
			//body.velocity *= 0.5f;
		

			animation.CrossFade("Run");
			transform.parent.LookAt(target);
			target = hitPoint;
			
		}
		
		if (Input.GetMouseButton(1)) {
			wr.con.dig(); 
		}*/
		
		
		getPos(ref currentPosition);
		
		if (lastPosition != currentPosition) { 
			//RenderDepender(pm.player.transform);
			Debug.Log("We are now at "  + currentPosition.x + ", " + currentPosition.y);
			
			wr.UpdateLocalCoordinates((int) currentPosition.x,(int) currentPosition.y);
			lastPosition = currentPosition;
			
			//wr.dispUpDown();
			
		}		
		
		
		
		//return;
		
		/*if ((target - transform.position).sqrMagnitude > 5) {
			
			if (!animation.isPlaying) {
				//animation.Stop();
				animation.CrossFade("Run",0.1f);
			}
			    
			
			float forceFactor = 40.0f;
			if ((target - transform.position).sqrMagnitude <= 20) 
				forceFactor = 40 * ((target - transform.position).sqrMagnitude / 20.0f);

			body.constantForce.force = (target - transform.position).normalized * forceFactor;
			
		}
		else {
			body.velocity *= 0.25f;

			if (!animation.isPlaying) {
				animation.CrossFade("Idle", 0.25f);
			}
		}*/
		
		
	}
	
	public void getPos(ref Vector2 vec) {
		
		float size = (float) (WorldRender.BOXRES-1) * WorldRender.BOXSCALE;
		
		//Debug.Log("Im at " + transform.position);
		
		int x = (int) transform.position.x;
		int y = (int) transform.position.z;
		
		int gx = (int) (((int) Mathf.Abs(x) + (size/2)) / size) * (int) Mathf.Sign(x);
		int gy = (int) (((int) Mathf.Abs(y) + (size/2)) / size) * (int) Mathf.Sign(y);
		
		vec.x = gx; 
		vec.y = gy;
		
	}
	
}
