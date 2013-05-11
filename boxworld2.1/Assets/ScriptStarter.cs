using UnityEngine;
using System.Collections;

public class ScriptStarter : MonoBehaviour {
	
	public GameObject textUp;
	public GameObject textDown;
	
	
	//ConnectionHandler con = new ConnectionHandler();
	
	void Awake(){
		
		gameObject.AddComponent<InputController>();
		
		WorldRender wr = gameObject.AddComponent<WorldRender>();
		
		
		//gameObject.AddComponent<PlayerManager>();  
		
		//gameObject.GetComponent<WorldRender>().pm = gameObject.GetComponent<PlayerManager>();
		//gameObject.GetComponent<PlayerManager>().wr = gameObject.GetComponent<WorldRender>();

		//gameObject.AddComponent<ParticleManager>();

	}
	
	void Update () {
	}
}
