using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    private Vector3 offset = new Vector3(12.49579f , 16.47141f, -12.32857f);
    private float smoothing = 2;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position,targetPosition, smoothing * Time.deltaTime);
        transform.LookAt(target);
	}
}
