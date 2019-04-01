using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour {

	[SerializeField] float minutesToCompleteRotation;
	[SerializeField] float axialTilt;
	[SerializeField] List<MoonManager> moons;
	[SerializeField] List<NameLabel> moonLabels;
	[SerializeField] NameLabelButton planetLabel;

	private float speed = 1;
	private float size = 1;
	private float divider = 1;

	private Vector3 originalSize;
	private float originalDistance;

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

	public float DistanceDivider {
		get {return divider;}
		set {
			divider = value;
			SetDistance(value);
		}
	}

	public List<MoonManager> Moons {
		get { return moons; }
		set { moons = value; }
	}

	public List<NameLabel> MoonLabels {
		get { return moonLabels; }
		set { moonLabels = value; }
	}

	public Vector3 OriginalSize {
		get { return originalSize; }
	}

	public NameLabelButton PlanetLabel {
		get { return planetLabel; }
		set { planetLabel = value; }
	}

	private float angle;

	private void Awake()
	{
		originalSize = transform.localScale;
		originalDistance = transform.localPosition.x;
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

	private void SetDistance(float divider)
	{
		transform.localPosition = new Vector3(
			originalDistance * (1 / divider), 
			transform.localPosition.y, 
			transform.localPosition.z
		);
	}
}
