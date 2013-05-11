using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour {
	
	private bool clicklean = false;
	private bool clicklean2 = false;
	Transform _transform;
	GameObject _gameObject;
	private List<GameObject> selection;
	private List<Material> selectedTextures;
	Texture2D selectionTexture;
	Texture2D roadTexture;
	Texture2D waterTexture;
	Texture2D demolishTexture;
	Texture2D grassTexture;
	
	Material selectionMaterial;
	Material roadTMaterial;
	Material waterMaterial;
	Material demolishMaterial;
	Material grassMaterial;
	private Vector3 travelHere;
	// Use this for initialization
	void Start(){
		_transform = transform;
		_gameObject = gameObject;
		
		
		CreateSelectionTexture();
		CreateRoadTexture();
		CreateGrassTexture();
		CreateDemolishTexture();
		CreateWaterTexture();
		
		selectionMaterial = Resources.Load("Materials/Selection") as Material;
		grassMaterial = Resources.Load("Materials/Grass") as Material;
		demolishMaterial = Resources.Load("Materials/Demolish") as Material;
		roadTMaterial = Resources.Load("Materials/Road") as Material;
		waterMaterial = Resources.Load("Materials/Water") as Material;
		
		travelHere = _transform.position;
		travelHere.y+=350;
	}
	
	private void CreateSelectionTexture(){
		selectionTexture = new Texture2D(2,2);
		Color c = new Color(1,1,0,0);
		selectionTexture.SetPixels(new Color[]{c,c,c,c});
		selectionTexture.filterMode = FilterMode.Point;
		selectionTexture.Apply();
	}
	
	private void CreateRoadTexture(){
		roadTexture = new Texture2D(2,2);
		roadTexture.SetPixels(new Color[]{Color.gray,Color.gray,Color.gray,Color.gray});
		roadTexture.filterMode = FilterMode.Point;
		roadTexture.Apply();
	
	}
	
	private void CreateDemolishTexture(){
		demolishTexture = new Texture2D(2,2);
		Color c = new Color(0.50f,0.25f,0.0f);
		demolishTexture.SetPixels(new Color[]{c,c,c,c});
		demolishTexture.filterMode = FilterMode.Point;
		demolishTexture.Apply();
	
	}
	
	private void CreateGrassTexture(){
		grassTexture = new Texture2D(2,2);
		grassTexture.SetPixels(new Color[]{Color.green,Color.green,Color.green,Color.green});
		grassTexture.filterMode = FilterMode.Point;
		grassTexture.Apply();
	
	}
	
	private void CreateWaterTexture(){
		waterTexture = new Texture2D(2,2);
		waterTexture.SetPixels(new Color[]{Color.blue,Color.blue,Color.blue,Color.blue});
		waterTexture.filterMode = FilterMode.Point;
		waterTexture.Apply();
	
	}
	
	private void Unselect(){
		if(selection != null){
			for(int i = 0; i<selection.Count;i++){
				selection[i].renderer.material = selectedTextures[i];
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
						selectedTextures = new List<Material>();
						AddSelection(hit.collider.gameObject);
						clicklean = true;
					}else{
						if(hit.collider.transform.root != null && hit.collider.transform.root.gameObject.name.Contains("ground")){
							selection = new List<GameObject>();
							selectedTextures = new List<Material>();
							AddSelection(hit.collider.transform.root.gameObject);
							clicklean = true;
						}
					}
				}
			}else{
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)){
					if(hit.collider.gameObject.name.Contains("ground")){
						if(!selection.Contains(hit.collider.gameObject)){
							AddSelection(hit.collider.gameObject);
						}
					}else{
						if(hit.collider.transform.root != null && hit.collider.transform.root.gameObject.name.Contains("ground")){
							if(!selection.Contains(hit.collider.transform.root.gameObject)){
								AddSelection(hit.collider.transform.root.gameObject);
							}
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
			CameraZoom(-Input.GetAxis ("Mouse ScrollWheel")*40);
		}
		
		
		if(Input.GetKeyUp(KeyCode.R)){
			List<GameObject> theSelection = selection;
			Unselect();
			MakeSelectionToRoad(theSelection);
			
		}
		
		if(Input.GetKeyUp(KeyCode.G)){
			List<GameObject> theSelection = selection;
			Unselect();
			MakeSelectionToGrass(theSelection);
			
		}
		
		if(Input.GetKeyUp(KeyCode.D)){
			List<GameObject> theSelection = selection;
			Unselect();
			MakeSelectionToDemolish(theSelection);
			
		}
		
		if(Input.GetKeyUp(KeyCode.W)){
			List<GameObject> theSelection = selection;
			Unselect();
			MakeSelectionToWater(theSelection);
			
		}
		
		
		if(Input.GetKeyUp(KeyCode.H)){
			List<GameObject> theSelection = selection;
			Unselect();
			BuildHouses(theSelection);
			
		}
		if(Input.GetKeyUp(KeyCode.F)){
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
		if(travelHere.y+amount > 85 && travelHere.y+amount < 500){
			travelHere.y+=amount;
		}
	}
	
	private void BuildHouses(List<GameObject> theSelection){
		foreach(GameObject g in theSelection){
			CreateHouse(g);
		}
	}
	
	private GameObject CreateHouse(GameObject gParent){
		GameObject house = GameObject.CreatePrimitive(PrimitiveType.Cube);
		house.name = "house";
		float lowY = 123456789;
		foreach(Vector3 v in gParent.GetComponent<MeshFilter>().mesh.vertices){
			if(lowY == 123456789){
				lowY = v.y;
			}else{
				if(lowY > v.y){
					lowY = v.y;
				}
			}
		}
		float hW = house.renderer.bounds.max.x-house.renderer.bounds.min.x;
		float gpW = gParent.renderer.bounds.max.x-gParent.renderer.bounds.min.x;
		float hNewScal = gpW/hW;
		Vector3 newScale = new Vector3(hW*hNewScal,hW*hNewScal,hW*hNewScal);
		house.transform.localScale = newScale*0.9f;
		Vector3 calcedPosition = gParent.transform.position;
		calcedPosition.y += (house.renderer.bounds.max.y-house.renderer.bounds.min.y)*0.5f;
		house.transform.position = calcedPosition;
		house.transform.parent = gParent.transform;
		
		return house;
	}
	
	private void LerpCameraPosition(){
		_transform.position = Vector3.Lerp(_transform.position, travelHere, Time.smoothDeltaTime*4.0f);
	}
	
	private void AddSelection(GameObject g){
		selection.Add(g);
		selectedTextures.Add(g.renderer.material);
		g.renderer.material = selectionMaterial;
	}
	
	private void MakeSelectionToRoad(List<GameObject> theSelection){
		foreach(GameObject g in theSelection){
			g.renderer.material = roadTMaterial;
		}
	}
	
	private void MakeSelectionToWater(List<GameObject> theSelection){
		foreach(GameObject g in theSelection){
			g.renderer.material = waterMaterial;
		}
	}
	
	private void MakeSelectionToDemolish(List<GameObject> theSelection){
		foreach(GameObject g in theSelection){
			g.renderer.material = demolishMaterial;
			for(int i = 0; i < g.transform.GetChildCount(); i++){
				Destroy(g.transform.GetChild(i).gameObject);
			}
		}
	}
	
	private void MakeSelectionToGrass(List<GameObject> theSelection){
		foreach(GameObject g in theSelection){
			g.renderer.material = grassMaterial;
		}
	}
}
