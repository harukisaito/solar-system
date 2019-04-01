using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotator : MonoBehaviour {

	[SerializeField] private float orbitalPeriodInDays;
	[SerializeField] private float inclinationAngle;

	[SerializeField] private List<MoonRotator> moonRotators;

	private float speed = 1f;
	private float trailLength = 1f;
	private float timePassed = 0f;

	private int yearCounter;

	public float Speed {
		get { return speed; }
		set {
			speed = value;
			if(trailRenderer != null) {
				SetTrailLength(value, trailLength);
			}
		} 
	}

	public float TrailLength {
		set {
			trailLength = value;
			if(trailRenderer != null) {
				SetTrailLength(speed, trailLength);
			}
		}
	}

	public List<MoonRotator> MoonRotators {
		get { return moonRotators; }
		set { moonRotators = value; }
	}

	private TrailRenderer trailRenderer;

	private float orbitalPeriodInSeconds;

	private void Start()
	{
		orbitalPeriodInSeconds = orbitalPeriodInDays * 24 * 3600;
		trailRenderer = GetComponentInChildren<TrailRenderer>();
		SetTrailLength(speed, trailLength);
		transform.localRotation = Quaternion.Euler(0, 0, inclinationAngle);
	}

	private void Update () {
		float rotation = (360 / orbitalPeriodInSeconds) * Time.deltaTime * speed;
		timePassed += (360 / orbitalPeriodInSeconds) * Time.deltaTime * speed;
		if(timePassed % 365 >= 1 && gameObject.name == "Earth") {
			timePassed -= 365;
			yearCounter ++;
			SolarSystemController.Instance.ButtonController.Texts[0].text = "Year Counter: " + yearCounter;
		}
		transform.rotation *= Quaternion.Euler(0, rotation, 0);
	}

	private void SetTrailLength(float speed, float trailLength) {
		trailRenderer.time = (orbitalPeriodInSeconds / speed) * trailLength;
	}
}
