using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class Arrow : MonoBehaviour {

    // Use this for initialization
    public RoleType roleType;
    private Rigidbody rgd;
    public int speed = 5;
    public bool isLocal = false;
    public GameObject explosionEffect;
	void Start () {
        rgd = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        rgd.MovePosition(transform.position+ transform.forward * speed * Time.deltaTime);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameFacade.Instance.PlayNormalSound(AUdioManager.Sound_ShootPerson);
            if(isLocal)
            {
                bool playerIsLocal = other.GetComponent<PlayerInfo>().isLocal;
                if (isLocal != playerIsLocal)
                {
                    GameFacade.Instance.SendAttack(Random.Range(10,20));
                }
            }
        }
        else
        {
            GameFacade.Instance.PlayNormalSound(AUdioManager.Sound_Miss);
        }
        explosionEffect = Instantiate(explosionEffect,transform.position,transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
