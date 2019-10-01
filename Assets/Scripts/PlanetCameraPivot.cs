using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCameraPivot : MonoBehaviour {



	public float DistanceToSun {
		get {
			return distanceToSun;
		}
		set {
			distanceToSun = value;
			transform.localPosition = new Vector3(
				distanceToSun,
				transform.localPosition.y,
				transform.localPosition.z
			);
		}
	}

	private float distanceToSun;
	private float sensitivity = 2f;
	private bool cameraEnabled = true;
	private bool isDragging = false;

	private Vector3 theSpeed;
	private Vector3 avgSpeed;

	private PlanetFollower planetFollower;
	private void Start() {
		planetFollower = PlanetFollower.Instance;
	}

	private void LateUpdate() {
		if(cameraEnabled) {
			if(planetFollower.LookingAtPlanet || planetFollower.AngleView) {
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
				// if(planetFollower.LookingAtPlanet) {
				// 	transform.Rotate(Camera.main.transform.right * theSpeed.y * 1f, Space.World);
				// }
				// else {
				transform.Rotate(Camera.main.transform.right * theSpeed.y * 0.5f, Space.World);
				// }

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
}
