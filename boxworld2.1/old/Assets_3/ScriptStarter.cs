using UnityEngine;
using System.Collections;

public class ScriptStarter : MonoBehaviour {

	void Awake () {
		gameObject.AddComponent<WorldRender>();
		gameObject.AddComponent<PlayerManager>();
	}
	
	void Update () {
	
	}
}
