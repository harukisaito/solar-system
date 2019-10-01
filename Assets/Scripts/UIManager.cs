using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;
	private void Awake() {
		if(Instance == null) {
			Instance = this;
		}
		else {
			Destroy(gameObject);
		}

		FillDictionary();
	}
	
	[SerializeField] private Button[] planetButtons;
	[SerializeField] private Text[] mercuryMoonLabels, 
		venusMoonLabels, earthMoonLabels, 
		marsMoonLabels, jupiterMoonLabels, saturnMoonLabels, 
		uranusMoonLabels, neptuneMoonLabels, plutoMoonLabels;
	[SerializeField] private Material trailMaterial;



	public EventHandler SettingSpeed;

	public bool InclinationAngleActive {
		get {
			return inclinationAngleActive;
		}
		set {
			inclinationAngleActive = value;
			SolarSystemController.Instance.SetInclinationAngle(inclinationAngleActive);
		}
	}

	public Button[] PlanetButtons {
		get {
			return planetButtons;
		}
		set {
			planetButtons = value;
		}
	}

	public Dictionary<Planet, Text[]> MoonLabels {
		get {
			return moonLabels;
		}
		set {
			moonLabels = value;
		}
	}

	public Text[] MercuryMoonLabels {
		get {
			return mercuryMoonLabels;
		}
		set {
			mercuryMoonLabels = value;
		}
	}
	public Text[] VenusMoonLabels {
		get {
			return venusMoonLabels;
		}
		set {
			venusMoonLabels = value;
		}
	}
	public Text[] EarthMoonLabels {
		get {
			return earthMoonLabels;
		}
		set {
			earthMoonLabels = value;
		}
	}
	public Text[] MarsMoonLabels {
		get {
			return marsMoonLabels;
		}
		set {
			marsMoonLabels = value;
		}
	}
	public Text[] JupiterMoonLabels {
		get {
			return jupiterMoonLabels;
		}
		set {
			jupiterMoonLabels = value;
		}
	}
	public Text[] SaturnMoonLabels {
		get {
			return saturnMoonLabels;
		}
		set {
			saturnMoonLabels = value;
		}
	}
	public Text[] UranusMoonLabels {
		get {
			return uranusMoonLabels;
		}
		set {
			uranusMoonLabels = value;
		}
	}
	public Text[] NeptuneMoonLabels {
		get {
			return neptuneMoonLabels;
		}
		set {
			neptuneMoonLabels = value;
		}
	}
	public Text[] PlutoMoonLabels {
		get {
			return plutoMoonLabels;
		}
		set {
			plutoMoonLabels = value;
		}
	}

	public Material TrailMaterial {
		get {
			return trailMaterial;
		}
	}

	private Dictionary<Planet, Text[]> moonLabels = new Dictionary<Planet, Text[]>();
	private bool inclinationAngleActive;


	public void SetSpeed(float speed) {
		SolarMetrics.speed = Mathf.Pow(1.5f, speed);
		OnSettingSpeed();
	}

	public void SetDistance(float distance) {
		SolarMetrics.offset = distance;
	}

	public void SetRelativeScale() {
		SolarSystemController.Instance.SetRelativeScale();
	}

	public void SetRelativeDistance() {
		SolarSystemController.Instance.SetRelativeDistance();
	}

	public void SetNormal() {
		SolarSystemController.Instance.SetNormal();
	}

	public void AngleView() {
		PlanetFollower.Instance.AngledView();
	}

	public void MoveToPlanet(int index) {
		PlanetFollower.Instance.MoveToPlanet(index);
	}

	protected virtual void OnSettingSpeed() {
		if(SettingSpeed == null) {
			return;
		}
		SettingSpeed(this, EventArgs.Empty);
	}

	private void FillDictionary() {
		moonLabels.Add(Planet.Mercury, mercuryMoonLabels);
		moonLabels.Add(Planet.Venus, venusMoonLabels);
		moonLabels.Add(Planet.Earth, earthMoonLabels);
		moonLabels.Add(Planet.Mars, marsMoonLabels);
		moonLabels.Add(Planet.Jupiter, jupiterMoonLabels);
		moonLabels.Add(Planet.Saturn, saturnMoonLabels);
		moonLabels.Add(Planet.Uranus, uranusMoonLabels);
		moonLabels.Add(Planet.Neptune, neptuneMoonLabels);
		moonLabels.Add(Planet.Pluto, plutoMoonLabels);
	}
}
