using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolarSystemController : MonoBehaviour {

	[SerializeField] private List<PlanetRotator> planetPivots;
	[SerializeField] private List<PlanetManager> planets;
	[SerializeField] private List<OrbitCamera> cameraPlanets;
	[SerializeField] private List<Text> planetLabels;
	[SerializeField] private SunController sun;
	[SerializeField] private ButtonController buttonController;
	[SerializeField] private PlanetFollower planetCamera;

	private float speed = 1;
	private float size = 1;
	private float distance = 1;

	private bool noTrail;
	private bool noLabels;
	private bool lookingAtPlanet;
	private bool moonLabelsActive;

	private int currentIndex;

	private Vector3 originalValues;

	public static SolarSystemController Instance;


	public float Speed {
		get { return speed; }
		set {
			speed = Mathf.Pow(1.5f, value);
			SetSpeed(speed);
		}
	}

	public float PlanetSize {
		get { return size; }
		set {
			size = value;
			SetSize(value);
		} 
	}

	public float PlanetDistance {
		get { return distance; }
		set {
			distance = value;
			SetDistance(value);
		} 
	}

	public float TrailLength {
		set {
			if(planetPivots != null) {
				foreach(var planet in planetPivots) {
					planet.TrailLength = value;
					foreach(var moon in planet.MoonRotators) {
						moon.TrailLength = value;
					}
				}
			}
		}
	}

	public float LabelFontSize {
		set {
			foreach(var planet in planets) {
				planet.PlanetLabel.FontSize = value;
				foreach(var moonLabel in planet.MoonLabels) {
					moonLabel.FontSize = value;
				}
			}
		}
	}

	public bool LookingAtPlanet {
		get { return lookingAtPlanet; }
		set {
			lookingAtPlanet = value; 
		}
	}

	public List<PlanetManager> PlanetManagers {
		get { return planets; }
	}

	public List<PlanetRotator> PlanetRotators {
		get { return planetPivots; }
	}

	public List<OrbitCamera> CameraPlanets {
		get { return cameraPlanets; }
	}

	public ButtonController ButtonController {
		get { return buttonController; }
		set { buttonController = value; }
	}
	public List<Text> PlanetLabels {
		get { return planetLabels; }
		set { planetLabels = value; }
	}

	public PlanetFollower PlanetCamera {
		get { return planetCamera; }
		set { planetCamera = value; }
	}


	private void Awake() {
		if(Instance == null) {
			Instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	
	private void Start() {
		SetAllPlanetValues();
		EnableLabels();
	}

	public void SetAllPlanetValues() {
		SetSpeed(speed);
		SetDistance(distance);
		SetSize(size);
	}

	private void SetSpeed(float speed) {
		SetRotatorSpeed(speed);
		SetRotationSpeed(speed);
	}

	private void SetRotatorSpeed(float speed) {
		foreach(var rotator in planetPivots) {
			rotator.Speed = speed == 0 ? 1 : speed;
			foreach(var moonRotator in rotator.MoonRotators) {
				moonRotator.Speed = speed == 0 ? 1 : speed;
			}
		}

	}

	private void SetRotationSpeed(float speed) {
		foreach(var planet in planets) {
			planet.Speed = speed == 0 ? 1 : speed;
			foreach(var moon in planet.Moons) {
				moon.Speed = speed == 0 ? 1 : speed;
			}
		}
	}

	private void SetDistance(float distance) {
		SetMoonRotatorDistance(distance);
		SetPlanetDistance(distance);
		SetCameraPivotDistance(distance);
		ClearMoonTrail();
	}

	private void SetMoonRotatorDistance(float distance) {
		foreach(var rotator in planetPivots) {
			foreach(var moonRotator in rotator.MoonRotators) {
				moonRotator.Distance = distance == 0 ? 1 : distance;
			}
		}
	}

	public void ClearMoonTrail() {
		foreach(var planet in planets) {
			foreach(var moon in planet.Moons) {
				moon.GetComponent<TrailRenderer>().Clear();
			}
		}
	}

	private void SetPlanetDistance(float distance) {
		foreach(var planet in planets) {
			planet.DistanceDivider = distance == 0 ? 1 : distance;
			planet.GetComponent<TrailRenderer>().Clear();
		}
	}

	private void SetCameraPivotDistance(float distance) {
		foreach(var camPivot in cameraPlanets) {
			if(camPivot.name != "Sun Camera Pivot") {
				camPivot.Distance = distance;
			}
		}
	}

	private void SetSize(float size) {
		SetSunSize(size);
		SetPlanetAndMoonSize(size);
	}

	private void SetSunSize(float size) {
		sun.Size = size == 0 ? 1 : size;
	}

	private void SetPlanetAndMoonSize(float size) {
		foreach(var planet in planets) {
			if(planet.transform.localScale.x < 99) {
				planet.Size = size == 0 ? 1 : size;
			}
			foreach(var moon in planet.Moons) {
				moon.Size = size == 0 ? size : size;
			}
		}
	}

	public void EnableMoonLabels(int index, bool enabled) { 
		currentIndex = index;
		if(planets[index].MoonLabels != null) {
			for(int i = 0; i < planets[index].MoonLabels.Count; i++) {
				planets[index].MoonLabels[i].MoonLabel.enabled = enabled;
			}
			moonLabelsActive = enabled;
		}
	}

	public void EnableDisableTrail() {
		if(!noTrail) {
			foreach(var planet in planets) {
				TrailRenderer trailRenderer = planet.GetComponent<TrailRenderer>();
				trailRenderer.enabled = false;
				trailRenderer.Clear();
				foreach(var moon in planet.Moons) {
					TrailRenderer trailRender = moon.GetComponent<TrailRenderer>();
					trailRender.enabled = false;
					trailRender.Clear();
				}
			}
			noTrail = true;
		}
		else {
			foreach(var planet in planets) {
				planet.GetComponent<TrailRenderer>().enabled = true;
				foreach(var moon in planet.Moons) {
					moon.GetComponent<TrailRenderer>().enabled = true;
				}
			}
			noTrail = false;
		}
	}

	public void EnableLabels() {
		if(noLabels) {
			foreach(var planet in planets) {
				planet.PlanetLabel.Text.enabled = false;
				foreach(var moonLabel in planet.MoonLabels) {
					moonLabel.MoonLabel.enabled = false;
				}
			}
			noLabels = false;
		}
		else {
			foreach(var planet in planets) {
				planet.PlanetLabel.Text.enabled = true;
			}
			EnableMoonLabels(currentIndex, moonLabelsActive);
			noLabels = true;
		}
	}


	public void EnableMoveButton(bool enabled) {
		buttonController.EnableMoveButton(enabled);
	}

	public void EnableAllPlanetButtons(bool enabled)
	{
		buttonController.EnableAllPlanetButtons(enabled);
	}
}
