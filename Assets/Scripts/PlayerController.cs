using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks
{
    Rigidbody rig;
    Animator anim;
    public float speed; //Velocidad máxima del movimiento
    public float rotationSpeed; //Velocidad de giro del ratón
    public GameObject bala; //Bala del arma
    public float bulletSpeed = 100f;
    public GameObject posCanyon; //Posicion del cañon del arma
    private ExitGames.Client.Photon.Hashtable playerProperties;


    [Header("Player Info")]
    public int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        PhotonView photonView = PhotonView.Get(this);
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            //Si se pulsa teclas Horizontal o Vertical, movemos el personaje
            Move();

            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Disparo");
                //photonView.RPC("Shoot", RpcTarget.All);
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HUDController.instance.ShowPanelPlayers();
        }

        else if (Input.GetKeyUp(KeyCode.Tab)) 
        { 
            HUDController.instance.HidePanelPlayer(); 
        }

            
        

        
        void Shoot()
        {
            GameObject miBala = Instantiate(bala, posCanyon.transform.position, Quaternion.identity);
            //miBala.transform.forward = posCanyon.transform.forward * bulletSpeed;

            Rigidbody rbBala = miBala.GetComponent<Rigidbody>();
            rbBala.velocity = this.transform.forward * bulletSpeed;
        }

        void Move()
        {
            float movZ = Input.GetAxis("Vertical");
            float movX = Input.GetAxis("Horizontal");

            Vector3 velocidad = transform.forward * movZ * speed
                + transform.right * movX * speed
                + transform.up * rig.velocity.y;

            rig.velocity = velocidad;



            transform.Rotate(transform.up * Input.GetAxis("Mouse X")
                * rotationSpeed);

            velocidad = new Vector3(velocidad.x, 0, velocidad.z);

            anim.SetFloat("velocity", velocidad.magnitude);




        }

        [PunRPC]
        void TakeDamage(int damagePoints)
        {
            health = health - damagePoints;
            HUDController.instance.UpdateHealth(health);


            if (health <= 0)
            {
                PhotonNetwork.AutomaticallySyncScene = false;
                Destroy(gameObject); ;

                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene(3);
                }

                if (photonView.IsMine)
                {
                    playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
                    playerProperties["Health"] = health;
                    PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
                }



            }
        }

        /*
        private void SetCustomProperties( int health)
        {
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "vida", health } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
        */

    } 
}
