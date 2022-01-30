using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShadowController : MonoBehaviour
{
    // Start is called before the first frame update
    public float CharacterSpeed;
    private float horizontal, vertical;
    private Vector2 inputDir;
    private Rigidbody2D rb;
    public bool Moving;
    private PhotonView _photonView;

    //public Color c_moving, c_stopped;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _photonView = PhotonView.Get(this);
        if (PhotonNetwork.NickName != "Shadow") return;
        Debug.Log("Transfer Ownership to:" + PhotonNetwork.LocalPlayer);
        _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PhotonNetwork.NickName != "Shadow") return;
        

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        inputDir = new Vector2(horizontal, vertical).normalized;
        if (inputDir != Vector2.zero)
        {
            Moving = true;
            rb.velocity = inputDir * CharacterSpeed;
        }
        else
        {
            Moving = false;
            rb.velocity = Vector2.zero;
        }
    }
}
