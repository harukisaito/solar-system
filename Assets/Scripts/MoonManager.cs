using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonManager : MonoBehaviour {

	[SerializeField] float minutesToCompleteRotation;
	[SerializeField] float axialTilt;


	private float speed = 1;
	private float size = 1;
	private float distance = 1;
	private float radius;

	private Vector3 originalSize;

	public float Speed {
		get {return speed;}
		set {
			speed = value;
		}
	}

	public float Size {
		get {return size;}
		set {
			size = value;
			SetSize(value);
		}
	}

	public float Distance {
		get { return distance; }
		set {
			distance = value;
			SetDistance(value);
		}
	}

	public float Radius {
		get { return radius; }
	}

	public Vector3 OriginalSize {
		get { return originalSize; }
	}

	private float angle;

	private void Awake()
	{
		originalSize = transform.localScale;
		radius = transform.localPosition.x;
	}

	private void Start()
	{
		angle = 360 / (minutesToCompleteRotation * 60f);
		transform.localRotation = Quaternion.Euler(0, 0, axialTilt);
	}

	private void Update()
	{
		float x = angle * Time.deltaTime * speed;
		transform.Rotate(0, x, 0);
	}

	private void SetSize(float size)
	{
		transform.localScale = originalSize * size;
	}

	private void SetDistance(float distance) {
		transform.localPosition = new Vector3(
			radius * (1 / distance), 
			transform.localPosition.y, 
			transform.localPosition.z
		);
	}

	public void SetDistance(float distance, bool puffer) {
		transform.localPosition = new Vector3(
			radius * (1 / distance), 
			transform.localPosition.y, 
			transform.localPosition.z
		);

		Vector3 pufferDistance = new Vector3(100, 0, 0);

		if(puffer) {
			transform.localPosition += pufferDistance;
		}
	}
}
