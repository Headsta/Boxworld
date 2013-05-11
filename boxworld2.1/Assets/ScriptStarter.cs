using UnityEngine;
using System.Collections;

public class ScriptStarter : MonoBehaviour {
	
	public GameObject textUp;
	public GameObject textDown;
	
	public GameObject dwarf;
	public GameObject dwarfAnim;
	
	public GameObject gotoobj;
	
	//ConnectionHandler con = new ConnectionHandler();
	
	void Awake () {
		
		
		WorldRender wr = dwarf.AddComponent<WorldRender>();
		dwarfAnim.AddComponent<NewDwarfAnimation>();
		
		//gameObject.AddComponent<PlayerManager>();  
		
		//gameObject.GetComponent<WorldRender>().pm = gameObject.GetComponent<PlayerManager>();
		//gameObject.GetComponent<PlayerManager>().wr = gameObject.GetComponent<WorldRender>();
		
		dwarfAnim.GetComponent<NewDwarfAnimation>().wr = wr;
		dwarfAnim.GetComponent<NewDwarfAnimation>().body = dwarf.rigidbody;  
		
		
		dwarfAnim.GetComponent<NewDwarfAnimation>().gotoObj = gotoobj;  
		//gameObject.AddComponent<ParticleManager>();

	}
	
	void Update () {
	}
}
