using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoonManager : MonoBehaviour {

	// time it takes to rotate around the sun
	[SerializeField] private float orbitalPeriodInDays;
	// the angle relative to the suns rotational axis;
	[SerializeField] private float inclinationAngle;

	// time it takes to complete one rotation around its axis
	[SerializeField] private float minutesToCompleteRotation;
	// the tilt of the axis relative to the suns axis
	[SerializeField] private float axialTilt;
	[SerializeField] private MoonLabelController moonLabel;
 

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
			if(moonLabel == null) {
				SetupLabel();
			}
			moonLabel.Index = index;
		}
	}

	public Planet PlanetIndex {
		get {
			return planetIndex;
		}
		set {
			planetIndex = value;
			// if(moonLabel == null) {
			// 	SetupLabel();
			// }
			moonLabel.PlanetIndex = planetIndex;
			moonLabel.SetupMoonLabel(this);
		}
	}

	public MoonRotator MoonRotator {
		get {
			return moonRotator;
		}
		set {
			moonRotator = value;
		}
	}

	public GameObject MoonParent {
		get {
			return moonParent;
		}
		set {
			moonParent = value;
			if(moonRotator == null) {
				SetupRotationCenter();
			}
			moonLabel.transform.SetParent(moonRotator.transform);
		}
	}

	public float Radius {
		get {
			return radius;
		}
		set {
			radius = value;
			float diameter = radius * 2;
			transform.localScale = new Vector3(
				diameter,
				diameter,
				diameter
			);
			if(moonLabel != null) {
				moonLabel.Radius = radius;
			}
		}
	}

	public float DistanceToPlanet {
		get {
			return distanceToPlanet;
		}
		set {
			distanceToPlanet = value;
			transform.localPosition = new Vector3(
				value,
				transform.localPosition.y,
				transform.localPosition.z
			);
			if(moonLabel != null) {
				moonLabel.DistanceToPlanet = distanceToPlanet;
			}
			ClearTrail();
		}
	}

	public float OriginalDistanceToPlanet {
		get {
			return originalDistance;
		}
	}

	public float OriginalRadius {
		get {
			return originalRadius;
		}
	}
		public float OrbitalPeriodInDays {
		get {
			return orbitalPeriodInDays;
		}
		set {
			orbitalPeriodInDays = value;
		}
	}

	public float InclinationAngle {
		get {
			return inclinationAngle;
		}
		set {
			inclinationAngle = value;
		}
	}

	public float MinutesToCompleteRotation {
		get {
			return minutesToCompleteRotation;
		}
		set {
			minutesToCompleteRotation = value;
		}
	}

	public float AxialTilt {
		get {
			return axialTilt;
		}
		set {
			axialTilt = value;
		}
	}
	
	private MoonRotator moonRotator;
	private TrailRenderer trailRenderer; 
	private MeshRenderer meshRenderer;
	private Planet planetIndex;
	private GameObject moonParent;

	private float radius;
	private float originalRadius;
	private float distanceToPlanet;
	private float originalDistance;

	private int index;

	private float angle;
	private float orbitalPeriodInSeconds;


	private void Start()
	{
		if(moonRotator == null) {
			SetupRotationCenter();
		}
 		trailRenderer = GetComponent<TrailRenderer>();
		meshRenderer = GetComponent<MeshRenderer>();
		trailRenderer.material = UIManager.Instance.TrailMaterial;
		trailRenderer.time = orbitalPeriodInSeconds;

		angle = 360 / (minutesToCompleteRotation * 60f);
		transform.localRotation = Quaternion.Euler(0, 0, axialTilt);

		originalRadius = radius;
		originalDistance = distanceToPlanet;
	}

	private void Update()
	{
		float x = angle * Time.deltaTime * SolarMetrics.speed;
		transform.Rotate(0, x, 0);
	}

	private void SetupRotationCenter() {
		GameObject rotatorGameObject = new GameObject();
		rotatorGameObject.name = name;
		moonRotator = rotatorGameObject.AddComponent<MoonRotator>();
		transform.SetParent(moonRotator.transform);
		moonRotator.transform.SetParent(moonParent.transform);

		orbitalPeriodInSeconds = orbitalPeriodInDays * 24 * 3600;
		moonRotator.orbitalPeriodInSeconds = orbitalPeriodInSeconds;
		moonRotator.inclinationAngle = inclinationAngle;
	}

	private void SetupLabel() {
		GameObject moonLabelGameObject = new GameObject();
		moonLabelGameObject.name = "Moon Label";
		moonLabel = moonLabelGameObject.AddComponent<MoonLabelController>();
	}

	public void Show(bool enabled) {
		trailRenderer.enabled = enabled;
		meshRenderer.enabled = enabled;
	}

	public void ShowLabel(bool enabled) {
		moonLabel.Show(enabled);
	}

	public void ClearTrail() {
		if(trailRenderer == null) {
			trailRenderer = GetComponent<TrailRenderer>();
		}
		trailRenderer.Clear();
	}

	public void SetTrailLength() {
		trailRenderer.time = orbitalPeriodInSeconds / SolarMetrics.speed;
	}

	public void SetLabelSize(State currentState) {
		moonLabel.SetSize(currentState);
	}
}
