using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

    private Rect camRect;

	void Start () {
		//camRect = this.GetComponent<Camera>().viewpointre
	}
	
	void LateUpdate () {
        this.transform.Translate(0.1f,0.0f,0.0f);
        //Debug.Log(this.transform.localPosition);
	}
}
