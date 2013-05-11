using UnityEngine;
using System.Collections;

public class ScriptStarter : MonoBehaviour {

	void Awake () {
		gameObject.AddComponent<WorldRender>();
		gameObject.AddComponent<PlayerManager>();
		gameObject.GetComponent<PlayerManager>().wr = gameObject.GetComponent<WorldRender>();
	}
	
	void Update () {
	
	}
}
