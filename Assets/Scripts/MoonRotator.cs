using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotator : MonoBehaviour {

	public float orbitalPeriodInSeconds;
	public float inclinationAngle;

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
    private void Start()
	{
		transform.localRotation = Quaternion.Euler(0, 0, inclinationAngle);
	}

	private void Update () {
		float rotation = (360 / orbitalPeriodInSeconds) * Time.deltaTime * SolarMetrics.speed;
		transform.rotation *= Quaternion.Euler(0, rotation, 0);
	}
}
