using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyForTime : MonoBehaviour {

    public float destroyTime;
	void Start () {
        Destroy(this.gameObject,destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
