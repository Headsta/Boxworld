using UnityEngine;
using System.Collections;

public class CameraTraveler : MonoBehaviour {

    private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    private RotationAxes axes = RotationAxes.MouseXAndY;
    private float sensitivityX = 15F;
    private float sensitivityY = 15F;
    private float minimumX = -360F;
    private float maximumX = 360F;
    private float minimumY = -60F;
    private float maximumY = 60F;
   	private float rotationX = 0F;
  	private float rotationY = 0F;
    Quaternion originalRotation;
	
	Light stroalKastare;
	float speed = 10.0f;
		
	private void Start(){
        stroalKastare = gameObject.AddComponent<Light>();
		stroalKastare.range = 100;
		stroalKastare.intensity = 0.4181818f;
        
		if(rigidbody){	
			rigidbody.freezeRotation = true;
		}
		originalRotation = transform.localRotation;
    }
	
	private void Update (){
		MovementLogic();
		RotationLogic();
    }
	
	private void MovementLogic(){
		if(Input.GetKey(KeyCode.A)){
			transform.Translate(Vector3.left*(Time.smoothDeltaTime*speed), Space.Self);
		}
		if(Input.GetKey(KeyCode.D)){
			transform.Translate(Vector3.right*(Time.smoothDeltaTime*speed), Space.Self);
		}
		if(Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.forward*(Time.smoothDeltaTime*speed), Space.Self);
		}
		if(Input.GetKey(KeyCode.S)){
			transform.Translate(Vector3.back*(Time.smoothDeltaTime*speed), Space.Self);
		}
	}
	
	private void RotationLogic(){
		 if (axes == RotationAxes.MouseXAndY){
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            	rotationX = ClampAngle (rotationX, minimumX, maximumX);
            	rotationY = ClampAngle (rotationY, minimumY, maximumY);
            		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
            		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }else if (axes == RotationAxes.MouseX){
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = ClampAngle (rotationX, minimumX, maximumX);
            	Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }else{
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle (rotationY, minimumY, maximumY);
           		Quaternion yQuaternion = Quaternion.AngleAxis (-rotationY, Vector3.right);
            transform.localRotation = originalRotation * yQuaternion;
        }
	}

    public static float ClampAngle (float angle, float min, float max){
        if (angle < -360F)angle += 360F;
        if (angle > 360F)angle -= 360F;
        return Mathf.Clamp (angle, min, max);
    }
}