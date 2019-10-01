using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonSerializer : MonoBehaviour {
	public static JsonSerializer Instance;

	private string jsonPath;

	private List<PlanetJson> planetJsons = new List<PlanetJson>();


	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	public string ReadJson(string type, int index) {
		jsonPath = Application.dataPath + "/JSON";
		return File.ReadAllText(jsonPath + "/" + type + index + ".json");
	}
}
