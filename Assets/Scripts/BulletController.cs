using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviourPunCallbacks
{
    [Header("Bullet Info")]
    public int damageQuantity = 1;
    //public float speed; //Velocidad de la bala

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + transform.forward * speed * Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {

                other.GetComponent<PlayerController>().photonView.RPC("TakeDamage", RpcTarget.All, damageQuantity);
         

        }
        gameObject.SetActive(false);
    }

}
