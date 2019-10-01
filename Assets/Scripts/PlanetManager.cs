using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlanetManager : MonoBehaviour {

	// time it takes to rotate around the sun
	[SerializeField] private float orbitalPeriodInDays;
	// the angle relative to the suns rotational axis;
	[SerializeField] private float inclinationAngle;

	// time it takes to complete one rotation around its axis
	[SerializeField] private float minutesToCompleteRotation;
	// the tilt of the axis relative to the suns axis
	[SerializeField] private float axialTilt;

	[SerializeField] private List<MoonManager> moons;
	[SerializeField] private PlanetLabelController planetLabel;
	[SerializeField] private PlanetCameraPivot cameraPivot;
	

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
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
			if(planetLabel != null) {
				planetLabel.Radius = radius;
			}
		}
	}

	public float DistanceToSun {
		get {
			return distanceToSun;
		}
		set {
			distanceToSun = value;
			transform.localPosition = new Vector3(
				value,
				transform.localPosition.y,
				transform.localPosition.z
			);
			if(cameraPivot != null) {
				cameraPivot.DistanceToSun = distanceToSun;
			}
			if(planetLabel != null) {
				planetLabel.DistanceToSun = distanceToSun;
			}
			if(moons != null) {
				foreach(var moon in moons) {
					if(moon != null) {
						moon.MoonRotator.DistanceToSun = distanceToSun;
						moon.ClearTrail();
					}
				}
			}
		}
	}

	public float OriginalDistanceToSun {
		get {
			return originalDistance;
		}
	}

	public float OriginalRadius {
		get {
			return originalRadius;
		}
	}

	public PlanetLabelController Label {
		get {
			return planetLabel;
		}
		set {
			planetLabel = value;
		}
	}

	public PlanetCameraPivot CameraPivot {
		get {
			return cameraPivot;
		}
		set {
			cameraPivot = value;
		}
	}

	public PlanetRotator PlanetRotator {
		get {
			if(planetRotator == null) {
				Setup();
			}
			return planetRotator;
		}
	}
	public List<MoonManager> Moons {
		get {
			return moons;
		}
		set {
			moons = value;
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

	private PlanetRotator planetRotator;
	private TrailRenderer trailRenderer;

	private int index;

	private float radius;
	private float originalRadius;
	private float distanceToSun;
	private float originalDistance;
	private float angle;
	private float orbitalPeriodInSeconds;

	private void Start()
	{
		if(planetRotator == null) {
			Setup();
		}
		SetMoonRotatorPosition();
		originalDistance = distanceToSun;
		originalRadius = radius;

		trailRenderer = GetComponent<TrailRenderer>();
		trailRenderer.sharedMaterial = UIManager.Instance.TrailMaterial;
		trailRenderer.time = (orbitalPeriodInSeconds);
		angle = 360 / (minutesToCompleteRotation * 60f);
		transform.localRotation = Quaternion.Euler(0, 0, axialTilt);
	}

	private void Update()
	{
		float x = angle * Time.deltaTime * SolarMetrics.speed;
		transform.Rotate(0, x, 0);
	}

	public void Setup() {
		SetupRotationCenter();
		SetupCameraPivot();
		SetupLabel();
	}

	private void SetupRotationCenter() {
		GameObject rotatorGameObject = new GameObject();
		rotatorGameObject.name = name;
		planetRotator = rotatorGameObject.AddComponent<PlanetRotator>();
		transform.SetParent(planetRotator.transform);
		orbitalPeriodInSeconds = orbitalPeriodInDays * 24 * 3600;
		planetRotator.orbitalPeriodInSeconds = orbitalPeriodInSeconds;
		planetRotator.InclinationAngle = inclinationAngle;
	}

	private void SetupCameraPivot() {
		GameObject cameraPivotGameObject = new GameObject();
		cameraPivotGameObject.name = "Camera Pivot";
		cameraPivot = cameraPivotGameObject.AddComponent<PlanetCameraPivot>();
		cameraPivot.transform.SetParent(planetRotator.transform);
		cameraPivot.DistanceToSun = distanceToSun;
	}
	
	private void SetupLabel() {
		GameObject planetLabelGameObject = new GameObject();
		planetLabelGameObject.name = "Planet Label";
		planetLabel = planetLabelGameObject.AddComponent<PlanetLabelController>();
		planetLabel.transform.SetParent(planetRotator.transform);
		planetLabel.Radius = radius;
		planetLabel.Index = index;
	}

	private void SetMoonRotatorPosition() {
		foreach(var moon in moons) {
			moon.MoonRotator.DistanceToSun = distanceToSun;
			moon.ClearTrail();
		}
	}

	public void SetInclinationAngle(bool activate) {
		if(activate) {
			planetRotator.InclinationAngle = inclinationAngle;
		}
		else {
			planetRotator.InclinationAngle = 0;
		}
	}

	public void ClearTrail() {
		trailRenderer.Clear();
	}

	public void ShowMoons(bool enabled) {
		if(moons.Count == 0) {
			return;
		}
		foreach(var moon in moons) {
			moon.Show(enabled);
		}
	}

	public void ShowMoonLabels(bool enabled) {
		if(moons.Count == 0) {
			return;
		}
		foreach(var moon in moons) {
			moon.ShowLabel(enabled);
		}
	}

	public void SetRelativeDistance() {
		foreach(var moon in moons) {
			moon.MoonRotator.DistanceToSun = distanceToSun;
			moon.ClearTrail();
			if(SolarSystemController.Instance.currentState == State.RelativeDistance) {
				moon.Radius = 5;
				moon.DistanceToPlanet = radius + 
					moon.OriginalDistanceToPlanet * 10;
			}
			else if(SolarSystemController.Instance.currentState == State.Normal) {
				moon.Radius = moon.OriginalRadius;
				moon.DistanceToPlanet = moon.OriginalDistanceToPlanet;
			}
		}
	}

	public void Reset() {
		Radius = originalRadius;
		DistanceToSun = originalDistance;
		ClearTrail();
	}

	public void OnSettingSpeed(object o, EventArgs e) {
		trailRenderer.time = 
			orbitalPeriodInSeconds / SolarMetrics.speed;
		foreach(var moon in moons) {
			moon.SetTrailLength();
		}
	}

	public void SetLabelSize(State currentState) {
		planetLabel.SetLabelSize(currentState);
		foreach(var moon in moons) {
			moon.SetLabelSize(currentState);
		}
	}

}
