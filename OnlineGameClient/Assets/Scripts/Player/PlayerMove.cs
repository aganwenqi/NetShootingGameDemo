﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    // Use this for initialization
    public float forward = 0;
    private float speed = 3;
    private Animator anim;
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == false) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);//移动
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));//旋转
            float res = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            forward = res;
            anim.SetFloat("Forward", res);//动画
        }
	}
}
