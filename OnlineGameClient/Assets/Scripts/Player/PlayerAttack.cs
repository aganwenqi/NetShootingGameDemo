using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    // Use this for initialization
    public GameObject arrowPrefab;
    private Animator anim;
    private Transform leftHandTrans;

    private Vector3 shootDir;

    private PlayManager playerMng;
	void Start () {
        anim = GetComponent<Animator>();
        leftHandTrans = transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");

    }
	
	// Update is called once per frame
	void Update () {
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        //{
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray,out hit);
                if (isCollider)
                {
                    Vector3 targetPoint = hit.point;
                    targetPoint.y = transform.position.y;
                    shootDir = targetPoint - transform.position;
                    transform.rotation = Quaternion.LookRotation(shootDir);
                    anim.SetTrigger("Attack");
                    Invoke("Shoot",0.2f);
                }
            }
        //}
	}
    public void SetPlayerMng(PlayManager playerMng)
    {
        this.playerMng = playerMng;
    }
    private void Shoot()
    {
        playerMng.Shoot(arrowPrefab, leftHandTrans.position, Quaternion.LookRotation(shootDir));
        
    }
}
