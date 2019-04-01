using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {

	private float sensitivity = 2f;
	private float distance;
	private float originalDistance;

	private bool cameraEnabled = true;
	private bool isDragging = false;

	private Vector3 theSpeed;
	private Vector3 avgSpeed;


	public float Distance {
		get { return distance; }
		set {
			distance = value; 
			SetDistance(value);
		}
	}
	public bool CameraEnabled {
		set {
			cameraEnabled = value;
		}
	}

	private void Awake() {
		originalDistance = transform.localPosition.x;
	}

	private void LateUpdate() {
		if(cameraEnabled) {
			if(SolarSystemController.Instance.LookingAtPlanet || SolarSystemController.Instance.PlanetCamera.AngleView) {
				if(Input.GetMouseButtonDown(0)) {
					isDragging = true;
				}
				if (Input.GetMouseButton(0) && isDragging) {
					theSpeed = new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0.0F);
					avgSpeed = Vector3.Lerp(avgSpeed, theSpeed, Time.deltaTime * 5);
				} else {
					if (isDragging) {
						theSpeed = avgSpeed;
						isDragging = false;
					}
					theSpeed = Vector3.Lerp(theSpeed, Vector3.zero, Time.deltaTime);
				}

				transform.Rotate(Camera.main.transform.up * theSpeed.x * sensitivity, Space.World);
				if(SolarSystemController.Instance.LookingAtPlanet) {
					transform.Rotate(Camera.main.transform.right * theSpeed.y * 1f, Space.World);
				}
				else {
					transform.Rotate(Camera.main.transform.right * theSpeed.y * 0.1f, Space.World);
				}

				Vector3 rotation = transform.localRotation.eulerAngles;

				float max = 70f;
				float min = 310f;

				rotation.z = Mathf.Clamp(rotation.z, 1, 3);
				transform.localRotation = Quaternion.Euler(rotation);
				if(rotation.x > max && rotation.x < 90) {
					rotation.x = max;
					transform.localRotation = Quaternion.Euler(rotation);
				}
				else if (380 - rotation.x > max && rotation.x < 360 && rotation.x > 300) {
					rotation.x = min;
					transform.localRotation = Quaternion.Euler(rotation);
				}
			}
		}
	}

	private void SetDistance(float distance) {
		transform.localPosition = new Vector3(
			originalDistance * (1 / distance), 
			transform.localPosition.y, 
			transform.localPosition.z
		);
	}
}
