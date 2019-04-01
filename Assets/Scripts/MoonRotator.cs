using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotator : MonoBehaviour {

	[SerializeField] private float orbitalPeriodInDays;
    [SerializeField] private float inclinationAngle;

	private float radius;
	private float speed = 1;
	private float distance = 1;
	private float trailLength = 1;

	public float Speed {
		get { return speed; }
		set {
			speed = value;
			SetMoonSpeed(value);
			if(trailRenderer != null) {
				SetTrailLength(value, trailLength);
			}
		} 
	}

	public float Distance {
		get {return distance;}
		set {
			distance = value;
			SetDistance(value);
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

	private TrailRenderer trailRenderer;

	private float orbitalPeriodInSeconds;

    private void Start()
	{
		radius = transform.localPosition.x;
		orbitalPeriodInSeconds = orbitalPeriodInDays * 24 * 3600;
		trailRenderer = GetComponentInChildren<TrailRenderer>();
		SetTrailLength(speed, trailLength);
		transform.localRotation = Quaternion.Euler(0, 0, inclinationAngle);
	}

	private void Update () {
		transform.rotation *= Quaternion.Euler(0, (360 / orbitalPeriodInSeconds) * Time.deltaTime * speed, 0);
	}

	private void SetTrailLength(float speed, float trailLength) {
		trailRenderer.time = (orbitalPeriodInSeconds / speed) * trailLength;
	}

	private void SetMoonSpeed(float speed) {
		this.speed = speed;
	}

	private void SetDistance(float distance)
	{
		transform.localPosition = new Vector3(
			radius * (1 / distance), 
			transform.localPosition.y, 
			transform.localPosition.z
		);
	}

	public void ClearTrail() {
		trailRenderer.Clear();
	}
}
