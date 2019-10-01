using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetFollower : MonoBehaviour {

	public static PlanetFollower Instance;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else {
			Destroy(gameObject);
		}
		AngledView();
	}


	public bool AngleView {
		get { return angleView; } 
		set { angleView = value; }
	}

	public bool LookingAtPlanet {
		get {
			return lookingAtPlanet;
		}
		set {
			lookingAtPlanet = value;
		}
	}
	private Vector3 offset = new Vector3(0, 0, 0);
	private Vector3 planetObservationPosition = new Vector3(0, 0, -100);
	private Vector3 angledViewPosition = new Vector3(0, 0, -300);

	private Transform planetCameraPivot;


	private float zoom = 7.8f;
	private bool angleView;
	private bool lookingAtPlanet;
	private PlanetManager previousPlanet;


	private void SetOffset(float zoomLevel) {
		if(transform.localPosition == offset) {
			if(lookingAtPlanet) {
				offset = SetOffsets(planetObservationPosition, zoomLevel);
			}
			else {
				SetOffsets(angledViewPosition, zoomLevel);
				offset = SetOffsets(angledViewPosition, zoomLevel);
			}
		}
		else {
			if(lookingAtPlanet) {
				offset = SetOffsets(planetObservationPosition, zoomLevel);
			}
			else {
				if(angleView) {
					offset = SetOffsets(transform.localPosition, zoomLevel);
				}
				else {
					offset = SetOffsets(transform.localPosition, zoomLevel);
				}
			}
		}
		transform.localPosition = Vector3.zero + offset;
	}

	private Vector3 SetOffsets(Vector3 position, float zoomLevel) {
		if(lookingAtPlanet || angleView) {
			Vector3 offset = new Vector3(
				position.x,
				position.y,
				-zoomLevel * position.z
				// * (position.z == 0 ? 1 : position.z)
			);
			return offset;
		}
		else {
			Vector3 offset = new Vector3(
				position.x,
				zoomLevel / 7.8f * position.y,
				position.z
			);
			return offset;
		}
	}

	private void LateUpdate() {
		// if(currentPlanet!= null && !lookingAtPlanet) {
		// 	Quaternion targetRotation = 
		// 		Quaternion.LookRotation(
		// 			planetCameraPivot.transform.position - transform.position
		// 		);
		// 	transform.rotation = 
		// 		Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
		// }
		// if(currentPlanet != null && lookingAtPlanet) {
		// 	transform.LookAt(planetCameraPivot.transform.localPosition);
		// }
		// else {
		// 	// if(!lookingAtPlanet) {
		// 	transform.LookAt(Vector3.zero);
		// 	// }
		// }
		if(lookingAtPlanet) {
			transform.LookAt(planetCameraPivot);
		}
		else {
			transform.LookAt(Vector3.zero);
		}
		if(Input.mouseScrollDelta.y > 0) {
			zoom += 0.1f;
			SetOffset(zoom);
		}
		else if(Input.mouseScrollDelta.y < 0) {
			zoom -= 0.1f;
			SetOffset(zoom);
		}
	}

	public void AngledView() {
		UnFollowPlanet();
		transform.localPosition = angledViewPosition;
		angleView = true;
		lookingAtPlanet = false;
		SetOffset(zoom);
	}

	private void UnFollowPlanet() {
		transform.parent = 
			SolarSystemController.Instance.Sun.sunPivot.transform;
	}

	private void ShowPlanet(PlanetManager planet) {
		lookingAtPlanet = true;
		planetCameraPivot = planet.CameraPivot.transform;
		transform.SetParent(planetCameraPivot);
		StartCoroutine(GoToPlanet(SetOffsets(planetObservationPosition, zoom), 2f));
		if(SolarSystemController.Instance.currentState == State.Normal) {
			planet.ShowMoonLabels(true);
			StartCoroutine(ExpandPlanet(planet, 1f));
		}
	}

	private void HidePlanet(PlanetManager planet) {
		StartCoroutine(ShrinkPlanet(planet, 1f));
	}

	public void MoveToPlanet(int index) {
		PlanetManager planet = SolarSystemController.Instance.Planets[index];
		if(previousPlanet == planet) {
			return;
		}
		if(previousPlanet != planet) {
			ShowPlanet(planet);
			if(previousPlanet != null) {
				HidePlanet(previousPlanet);
			}
		}
		previousPlanet = planet;
	}

	public void Expand() {
		previousPlanet.ShowMoonLabels(true);
		StartCoroutine(ExpandPlanet(previousPlanet, 1f));
	}


	private IEnumerator GoToPlanet(Vector3 desiredPosition, float time) {
		float elapsedTime = 0;

		while (elapsedTime < time) {
			transform.localPosition = 
				Vector3.Lerp(
					transform.localPosition, 
					desiredPosition, 
					SmoothCurve(elapsedTime, time)
				);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator ExpandPlanet(PlanetManager planet, float time) {
		float elapsedTime = 0;

		while (elapsedTime < time)
		{
			planet.Radius = Mathf.Lerp(
				planet.Radius,
				20,
				SmoothCurve(elapsedTime, time)
			);

			float scaleAmount = 20 / planet.OriginalRadius;

			foreach(var moon in planet.Moons) {
				moon.Radius = Mathf.Lerp(
					moon.Radius,
					moon.OriginalRadius * scaleAmount,
					SmoothCurve(elapsedTime, time)
				);
				moon.DistanceToPlanet = Mathf.Lerp(
					moon.DistanceToPlanet,
					moon.OriginalDistanceToPlanet * scaleAmount,
					SmoothCurve(elapsedTime, time)
				);
				moon.ClearTrail();
			}
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator ShrinkPlanet(PlanetManager planet, float time) {
		float elapsedTime = 0;

		while (elapsedTime < time)
		{
			planet.Radius = Mathf.Lerp(
				planet.Radius,
				planet.OriginalRadius,
				SmoothCurve(elapsedTime, time)
			);
			foreach(var moon in planet.Moons) {
				moon.Radius = Mathf.Lerp(
					moon.Radius,
					moon.OriginalRadius,
					SmoothCurve(elapsedTime, time)
				);
				moon.DistanceToPlanet = Mathf.Lerp(
					moon.DistanceToPlanet,
					moon.OriginalDistanceToPlanet,
					SmoothCurve(elapsedTime, time)
				);
				moon.ClearTrail();
				moon.ShowLabel(false);
			}
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	private float SmoothCurve(float value, float time) {
		return ((value * value * value * 
		(value  * (6f * value - 15f) + 10f)) / 
		(time * time * time * 
		(time * (6f * time - 15f) + 10f)));
	}

}
