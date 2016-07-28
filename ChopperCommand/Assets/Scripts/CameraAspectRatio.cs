using UnityEngine;
using System.Collections;

public class CameraAspectRatio : MonoBehaviour {

	public float wantedAspectRatio = 1.5f;//960/640 which is the design size

	void Start()
	{

		float currentAspectRatio = 0.0f;
		currentAspectRatio = (float)Screen.width / Screen.height;

		if ((int)(currentAspectRatio * 100) / 100.0f == (int)(wantedAspectRatio * 100) / 100.0f) {
			Camera.main.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			return;

		}

		if (currentAspectRatio > wantedAspectRatio) {
			float inset = 1.0f - wantedAspectRatio/currentAspectRatio;
			Camera.main.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
		} else {
			float inset = 1.0f - currentAspectRatio/wantedAspectRatio;
			Camera.main.rect = new Rect(0.0f, inset/2, 1.0f, 1.0f-inset);
		}
	}
}
