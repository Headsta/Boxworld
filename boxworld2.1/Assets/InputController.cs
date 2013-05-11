using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour {
	
	private bool clicklean = false;
	private List<GameObject> selection;
	private List<Texture2D> selectedTextures;
	Texture2D selectionTexture;
	// Use this for initialization
	void Start(){
		selectionTexture = new Texture2D(2,2);
		selectionTexture.SetPixels(new Color[]{Color.green,Color.green,Color.green,Color.green});
		selectionTexture.Apply();
	}
	
	private Texture2D RoadTexture(){
		Texture2D t = new Texture2D(2,2);
		t.SetPixels(new Color[]{Color.gray,Color.gray,Color.gray,Color.gray});
		t.Apply();
		t.filterMode = FilterMode.Point;
		return t;
	}
	
	private void Unselect(){
		if(selection != null){
			for(int i = 0; i<selection.Count;i++){
				selection[i].renderer.material.mainTexture = selectedTextures[i];
			}
			selectedTextures = null;
			selection = null;
		}
	}
	
	
	// Update is called once per frame
	void Update(){
		if(Input.GetMouseButton(0)){
			if(!clicklean){
				Unselect();
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)){
					if(hit.collider.gameObject.name.Contains("ground")){
						selection = new List<GameObject>();
						selectedTextures = new List<Texture2D>();
						selection.Add(hit.collider.gameObject);
						selectedTextures.Add((Texture2D)hit.collider.gameObject.renderer.material.mainTexture);
						clicklean = true;
						hit.collider.gameObject.renderer.material.mainTexture = selectionTexture;
					}
				}
			}else{
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)){
					if(hit.collider.gameObject.name.Contains("ground")){
						if(!selection.Contains(hit.collider.gameObject)){
							selection.Add(hit.collider.gameObject);
							selectedTextures.Add((Texture2D)hit.collider.gameObject.renderer.material.mainTexture);
							hit.collider.gameObject.renderer.material.mainTexture = selectionTexture;
						}
					}
				}
			}
		}else{
			clicklean = false;
		}
		
		if(Input.GetKeyUp(KeyCode.R)){
			// Road
			Debug.Log("Road");
			List<GameObject> theSelection = selection;
			Unselect();
			foreach(GameObject g in theSelection){
				g.renderer.material.mainTexture = RoadTexture();
			}
		}
		if(Input.GetKeyUp(KeyCode.F)){
			// Flatn
			Debug.Log("Flatn");
			Unselect();
		}
	}
}
