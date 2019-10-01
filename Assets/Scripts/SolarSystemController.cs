using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SolarSystemController : MonoBehaviour {

	public static SolarSystemController Instance;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else {
			Destroy(gameObject);
		}

		currentState = State.Normal;
	}

	[SerializeField] private SunManager sun;
	[SerializeField] private PlanetManager[] planets;


	public SunManager Sun {
		get {
			return sun;
		}
		set {
			sun = value;
		}
	}

	public PlanetManager[] Planets {
		get {
			return planets;
		}
		set {
			planets = value;
		}
	}

	public State currentState;

	private PlanetJson planetJson;
	private PlanetManager GetPreviousPlanet(PlanetManager currentPlanet) {
		if(currentPlanet.Index == 0) {
			return null;
		}
		return planets[currentPlanet.Index - 1];
	}

	private void Start() {
		CreatePlanets();
	}

	private void CreatePlanets() {
		planets = new PlanetManager[9];
		for(int i = 0; i < 9; i++) {
			string json = JsonSerializer.Instance.ReadJson("planet", i);
			planetJson = JsonUtility.FromJson<PlanetJson>(json);
			GameObject planetGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			planetGameObject.AddComponent<TrailRenderer>();
			planetGameObject.GetComponent<MeshRenderer>().sharedMaterial = 
				MaterialAssigner.Instance.PlanetMaterials[i];
			PlanetManager planet = planetGameObject.AddComponent<PlanetManager>();

			planetGameObject.name = planetJson.planetName;
			planet.Index = planetJson.index;
			planet.Radius = planetJson.radius;
			planet.DistanceToSun = planetJson.distanceToSun;
			planet.OrbitalPeriodInDays = planetJson.orbitalPeriodInDays;
			planet.InclinationAngle = planetJson.inclinationAngle;
			planet.MinutesToCompleteRotation = planetJson.minutesToCompleteRotation;
			planet.AxialTilt = planetJson.axialTilt;

			planets[i] = planet;
			CreateMoons(planet);
		}
		EventManager.Instance.Subscribe();
	}

	private void CreateMoons(PlanetManager planet) {
		string json = JsonSerializer.Instance.ReadJson("moons", planet.Index);
		MoonJson[] moons = JsonHelper.FromJson<MoonJson>(json);
		planet.Moons = new List<MoonManager>();

		if(moons.Length == 0) {
			planet.Setup();
		}
		else {
			GameObject moonParent = new GameObject();
			moonParent.name = "Moons";
			
			for(int i = 0; i < moons.Length; i++) {
				GameObject moonGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				moonGameObject.AddComponent<TrailRenderer>();
				// moonGameObject.GetComponent<MeshRenderer>().sharedMaterial = ;
				MoonManager moon = moonGameObject.AddComponent<MoonManager>();

				moon.name = moons[i].moonName;
				moon.Index = moons[i].index;
				moon.PlanetIndex = moons[i].planetIndex;
				moon.Radius = moons[i].radius;
				moon.DistanceToPlanet = moons[i].distanceToPlanet;
				moon.OrbitalPeriodInDays = moons[i].orbitalPeriodInDays;
				moon.InclinationAngle = moons[i].inclinationAngle;
				moon.MinutesToCompleteRotation = moons[i].minutesToCompleteRotation;
				moon.AxialTilt = moons[i].axialTilt;
				moon.MoonParent = moonParent;

				moon.MoonParent.transform.SetParent(planet.PlanetRotator.transform);
				planet.Moons.Add(moon);
			}
			moonParent.transform.localRotation = Quaternion.identity;
		}
	}

	public void SetRelativeScale() {
		currentState = State.RelativeScale;
		sun.Radius = sun.OriginalRadius * 100;
		for(int i = 0; i < planets.Length; i++) {
			PlanetManager currentPlanet = planets[i];
			currentPlanet.Radius = currentPlanet.OriginalRadius * 100; 
			float previousDistanceToSun = 
				GetPreviousPlanet(currentPlanet) == null ? 
					sun.Radius : 
					GetPreviousPlanet(currentPlanet).DistanceToSun + 
					GetPreviousPlanet(currentPlanet).Radius;
			currentPlanet.DistanceToSun = 
				previousDistanceToSun + 
				currentPlanet.Radius + i * SolarMetrics.offset;
			currentPlanet.ClearTrail();
			currentPlanet.ShowMoons(false);
			currentPlanet.ShowMoonLabels(false);
		}
	}

	public void SetRelativeDistance() {
		currentState = State.RelativeDistance;
		sun.Radius = 10;
		foreach(var planet in planets) {
			planet.Radius = 10;
			planet.DistanceToSun = planet.OriginalDistanceToSun * 0.05f;
			planet.ClearTrail();
			planet.ShowMoons(true);
			planet.ShowMoonLabels(true);
			planet.SetRelativeDistance();
			planet.SetLabelSize(currentState);
		}
	}

	public void SetNormal() {
		currentState = State.Normal;
		sun.Reset();
		foreach(var planet in planets) {
			planet.Reset();
			planet.ShowMoons(true);
			planet.ShowMoonLabels(false);
			planet.SetRelativeDistance();
		}
		if(PlanetFollower.Instance.LookingAtPlanet) {
			PlanetFollower.Instance.Expand();
		}
	}

	public void SetInclinationAngle(bool activate) {
		foreach(var planet in planets) {
			planet.SetInclinationAngle(activate);
			planet.ClearTrail();
			foreach(var moon in planet.Moons) {
				moon.ClearTrail();
			}
		}
	}
}
