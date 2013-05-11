using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour {
	
	private bool clicklean = false;
	private bool clicklean2 = false;
	Transform _transform;
	GameObject _gameObject;
	private List<GameObject> selection;
	private List<Texture2D> selectedTextures;
	Texture2D selectionTexture;
	Texture2D roadTexture;
	private Vector3 travelHere;
	// Use this for initialization
	void Start(){
		_transform = transform;
		_gameObject = gameObject;
		CreateSelectionTexture();
		CreateRoadTexture();
		travelHere = _transform.position;
	}
	
	private void CreateSelectionTexture(){
		selectionTexture = new Texture2D(2,2);
		selectionTexture.SetPixels(new Color[]{Color.green,Color.green,Color.green,Color.green});
		selectionTexture.filterMode = FilterMode.Point;
		selectionTexture.Apply();
	}
	
	private void CreateRoadTexture(){
		roadTexture = new Texture2D(2,2);
		roadTexture.SetPixels(new Color[]{Color.gray,Color.gray,Color.gray,Color.gray});
		roadTexture.filterMode = FilterMode.Point;
		roadTexture.Apply();
	
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
	
	void Update(){
		if(Input.GetMouseButton(0)){
			if(!clicklean){
				Unselect();
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)){
					if(hit.collider.gameObject.name.Contains("ground")){
						selection = new List<GameObject>();
						selectedTextures = new List<Texture2D>();
						AddSelection(hit.collider.gameObject);
						clicklean = true;
					}
				}
			}else{
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)){
					if(hit.collider.gameObject.name.Contains("ground")){
						if(!selection.Contains(hit.collider.gameObject)){
							AddSelection(hit.collider.gameObject);
						}
					}
				}
			}
		}else{
			clicklean = false;
		}
		
		if(Input.GetMouseButton(1)){
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)){
					if(hit.collider.gameObject.name.Contains("ground")){
						travelHere = hit.point;
						travelHere.y = _transform.position.y;
						clicklean2 = true;
					}
				}
		}else{
			clicklean2 = false;
		}
		
		if(Input.GetAxis ("Mouse ScrollWheel") != 0.0f){
			CameraZoom(Input.GetAxis ("Mouse ScrollWheel")*40);
		}
		
		
		if(Input.GetKeyUp(KeyCode.R)){
			// Road
			Debug.Log("Road");
			List<GameObject> theSelection = selection;
			Unselect();
			MakeSelectionToRoad(theSelection);
			
		}
		if(Input.GetKeyUp(KeyCode.F)){
			// Flatn
			Debug.Log("Flatn");
			List<GameObject> theSelection = selection;
			Unselect();
			foreach(GameObject g in theSelection){
				//g.renderer.material.mainTexture = RoadTexture();
				g.GetComponent<GroundMesh>().Flatten();
			}
		}
		
		LerpCameraPosition();
	}
	
	private void CameraZoom(float amount){
		if(travelHere.y+amount > 85 && travelHere.y+amount < 200){
			travelHere.y+=amount;
		}
	}
	
	private void LerpCameraPosition(){
		_transform.position = Vector3.Lerp(_transform.position, travelHere, Time.smoothDeltaTime*4.0f);
	}
	
	private void AddSelection(GameObject g){
		selection.Add(g);
		selectedTextures.Add((Texture2D)g.renderer.material.mainTexture);
		g.renderer.material.mainTexture = selectionTexture;
	}
	
	private void MakeSelectionToRoad(List<GameObject> theSelection){
		foreach(GameObject g in theSelection){
			g.renderer.material.mainTexture = roadTexture;
		}
	}
}
