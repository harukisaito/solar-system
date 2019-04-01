using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour {

	private float size;
	private Vector3 originalSize;
	private Renderer sunRenderer;
	float floor = 0.9f;
 	float ceiling = 5.0f;
	public float Size {
		get { return size; }
		set {
			size = value;
			SetSize(value);
		}
	}

	private void Awake() {
		originalSize = transform.localScale;
        sunRenderer = GetComponent<Renderer> ();
		StartCoroutine(Rotate());
	}

	private void SetSize(float size) {
		transform.localScale = originalSize * size;
	}

	void Update () {
		Material mat = sunRenderer.material;

		float emission = floor + Mathf.PingPong (Time.time, ceiling - floor);
		Color baseColor = Color.white;

		Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);

		mat.SetColor ("_EmissionColor", finalColor);
	}

	private IEnumerator Rotate() {
		Quaternion randomRotation = Random.rotation;

		while(transform.localRotation != randomRotation) {
			transform.localRotation = Quaternion.LerpUnclamped(transform.localRotation, randomRotation, Time.deltaTime * 0.1f);
			yield return null;
		}
		if(transform.localRotation == randomRotation) {
			StartCoroutine(Rotate());
		}
	}
}
