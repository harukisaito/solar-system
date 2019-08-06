using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetFollower : MonoBehaviour {

	private Vector3 offset = new Vector3(0, 0, 0);
	private Vector3 planetObservationPosition = new Vector3(0, 0, -100);
	private Vector3 topDownPosition = new Vector3(0, 1500, 0);
	private Vector3 topDownAngle = new Vector3(90, 0, 0);
	private Vector3 angledViewPosition = new Vector3(0, 300, -300);
	private Vector3 angledViewAngle = new Vector3(30, 0, 0);

	private Vector3[] moonDesiredScales; 

	private PlanetManager previousPlanet;
	private PlanetManager currentPlanet;

	private OrbitCamera planetCameraPivot;

	private Camera cam;

	private float zoom = 7.8f;
	private bool angledView;
	private bool lookAtPlanet;
	private int currentIndex = int.MaxValue;
	private int previousIndex = int.MaxValue;
	

	public float Zoom {
		set {
			zoom = value;
			SetOffset(value);
		}
	}

	public bool AngleView {
		get { return angledView; } 
		set { angledView = value; }
	}

	public int CurrentIndex {
		get { return currentIndex; }
	}

	private void Awake() {
		transform.localPosition = topDownPosition;
		transform.localRotation = Quaternion.Euler(topDownAngle);
		Camera.main.fieldOfView = 45;
	}

	private void SetOffset(float zoomLevel) {
		if(transform.localPosition == offset) {
			if(SolarSystemController.Instance.LookingAtPlanet) {
				offset = SetOffsets(planetObservationPosition, zoomLevel);
			}
			else {
				if(angledView) {
					SetOffsets(topDownPosition, zoomLevel);
					offset = SetOffsets(angledViewPosition, zoomLevel);
				}
				else {
					SetOffsets(angledViewPosition, zoomLevel);
					offset = SetOffsets(topDownPosition, zoomLevel);
				}
			}
		}
		else {
			if(SolarSystemController.Instance.LookingAtPlanet) {
				offset = SetOffsets(planetObservationPosition, zoomLevel);
			}
			else {
				if(angledView) {
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
		if(SolarSystemController.Instance.LookingAtPlanet || angledView) {
			Vector3 offset = new Vector3(
				position.x,
				position.y,
				-zoomLevel * (position.z == 0 ? 1 : position.z)
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
		if(currentPlanet!= null && !lookAtPlanet) {
			Quaternion targetRotation = Quaternion.LookRotation(planetCameraPivot.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
		}
		if(currentPlanet != null && lookAtPlanet) {
			transform.LookAt(planetCameraPivot.transform.localPosition);
		}
		else {
			if(!SolarSystemController.Instance.LookingAtPlanet)
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

	public void TopDownView() {
		UnFollowPlanet();
		transform.localPosition = topDownPosition;
		transform.localRotation = Quaternion.Euler(topDownAngle);
		angledView = false;
		SolarSystemController.Instance.LookingAtPlanet = false;
		SetOffset(zoom);
	}

	public void AngledView() {
		UnFollowPlanet();
		transform.localPosition = angledViewPosition;
		transform.localRotation = Quaternion.Euler(angledViewAngle);
		angledView = true;
		SolarSystemController.Instance.LookingAtPlanet = false;
		SetOffset(zoom);
	}

	private void UnFollowPlanet() {
		cam = GetComponent<Camera>();
		planetCameraPivot = SolarSystemController.Instance.CameraPlanets[9];
		transform.parent = planetCameraPivot.transform;
	}

	public void ShowHidePlanet(int index) {
		if(currentIndex == index) {
			ResetPlanet(currentPlanet);
			SolarSystemController.Instance.EnableMoonLabels(currentIndex == int.MaxValue ? index : currentIndex, false);
			currentIndex = int.MaxValue;
			previousIndex = int.MaxValue;
		}
		else {
			ShowPlanet(index);
		}
	}


	private void ResetPlanet(PlanetManager p) {
		HidePlanet(p);
		previousPlanet = null;
		currentPlanet = null;
		SolarSystemController.Instance.EnableMoveButton(false);
	}

	private void HidePlanet(PlanetManager planet) {
		StartCoroutine(ShrinkPlanet(planet, 1.5f));
	}

	private void ShowPlanet(int index) {
		if(currentPlanet != null) {
			previousPlanet = currentPlanet;
			previousIndex = currentIndex;
			HidePlanet(previousPlanet);
		}

		currentIndex = index;
		currentPlanet = SolarSystemController.Instance.PlanetManagers[currentIndex];

		SolarSystemController.Instance.EnableMoveButton(true);
		if(previousPlanet == null) {
			previousPlanet = currentPlanet;
		}
		Vector3 desiredSize = Vector3.one * 100f;
		if(currentPlanet.Moons != null) {
			int moonCount = currentPlanet.Moons.Count;
			moonDesiredScales = new Vector3[moonCount];

			for(int i = 0; i < currentPlanet.Moons.Count; i++) {
				float ratio = currentPlanet.transform.localScale.x / currentPlanet.Moons[i].transform.localScale.x;
				moonDesiredScales[i] = desiredSize / ratio;
			}
		}

		StartCoroutine(ExpandPlanet(desiredSize, moonDesiredScales, 1.5f));
	}

	public void MoveToPlanet() {
		lookAtPlanet = false;
		angledView = false;
		SolarSystemController.Instance.LookingAtPlanet = true;
		planetCameraPivot = SolarSystemController.Instance.CameraPlanets[currentIndex];
		transform.parent = planetCameraPivot.transform;
		StartCoroutine(GoToPlanet(SetOffsets(planetObservationPosition, zoom), 2f));
	}

	private IEnumerator ExpandPlanet(Vector3 desiredScale, Vector3[] moonDesiredScales, float time) {
		SolarSystemController.Instance.EnableAllPlanetButtons(false);
		float elapsedTime = 0;

		Vector3 planetSize = currentPlanet.transform.localScale;

		float ratio = desiredScale.x / planetSize.x;

		while (elapsedTime < time)
		{
			currentPlanet.transform.localScale = Vector3.Slerp(
				currentPlanet.transform.localScale,
				desiredScale, 
				((elapsedTime * elapsedTime * elapsedTime * (elapsedTime  * (6f * elapsedTime - 15f) + 10f)) / 
				(time * time * time * (time * (6f * time - 15f) + 10f))));
				for(int i = 0; i < currentPlanet.Moons.Count; i++) {
					currentPlanet.Moons[i].transform.localScale = Vector3.Slerp(
						currentPlanet.Moons[i].transform.localScale, 
						moonDesiredScales[i], 
						((elapsedTime * elapsedTime * elapsedTime * (elapsedTime  * (6f * elapsedTime - 15f) + 10f)) / 
						(time * time * time * (time * (6f * time - 15f) + 10f))));
				currentPlanet.Moons[i].SetDistance(
					distance: ratio * 0.00005f, 
					puffer: true
				);
				SolarSystemController.Instance.PlanetRotators[currentIndex].MoonRotators[i].ClearTrail();
			}
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		SolarSystemController.Instance.EnableMoonLabels(currentIndex, true);
		SolarSystemController.Instance.EnableAllPlanetButtons(true);
	}

	private IEnumerator ShrinkPlanet(PlanetManager planet, float time) {
		float elapsedTime = 0;
		SolarSystemController.Instance.EnableAllPlanetButtons(false);
		SolarSystemController.Instance.EnableMoonLabels(currentIndex, false);

		int index = previousIndex == int.MaxValue ? currentIndex : previousIndex;
		while (elapsedTime < time)
		{
			planet.transform.localScale = Vector3.Slerp(
				planet.transform.localScale, 
				planet.OriginalSize, 
				((elapsedTime * elapsedTime * elapsedTime * (elapsedTime  * (6f * elapsedTime - 15f) + 10f)) / 
				(time * time * time * (time * (6f * time - 15f) + 10f))));
				if(planet.Moons != null) {
					for(int i = 0; i < planet.Moons.Count; i++) {
						planet.Moons[i].transform.localScale = Vector3.Slerp(
							planet.Moons[i].transform.localScale, 
							planet.Moons[i].OriginalSize,
							((elapsedTime * elapsedTime * elapsedTime * (elapsedTime  * (6f * elapsedTime - 15f) + 10f)) / 
							(time * time * time * (time * (6f * time - 15f) + 10f))));
						planet.Moons[i].SetDistance(
							distance: 1, 
							puffer: false
						); // set back to original radius
					
						SolarSystemController.Instance.PlanetRotators[index].MoonRotators[i].ClearTrail();
					}
				}
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		SolarSystemController.Instance.EnableAllPlanetButtons(true);
	}

	private IEnumerator GoToPlanet(Vector3 desiredPosition, float time) {
		float elapsedTime = 0;

		while (elapsedTime < time) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition, ((elapsedTime * elapsedTime * elapsedTime * (elapsedTime  * (6f * elapsedTime - 15f) + 10f)) / 
				(time * time * time * (time * (6f * time - 15f) + 10f))));
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 100, ((elapsedTime * elapsedTime * elapsedTime * (elapsedTime  * (6f * elapsedTime - 15f) + 10f)) / 
				(time * time * time * (time * (6f * time - 15f) + 10f))));
			// if(elapsedTime > 1.5f) {
			// 	StartCoroutine(NormalizeFieldOfView(2));
			// }
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		StartCoroutine(NormalizeFieldOfView(2));
	}

	private IEnumerator NormalizeFieldOfView(float time) {
		float elapsedTime = 0;

		while (elapsedTime < time) {
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, ((elapsedTime * elapsedTime * elapsedTime * (elapsedTime  * (6f * elapsedTime - 15f) + 10f)) / 
				(time * time * time * (time * (6f * time - 15f) + 10f))));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}
}
