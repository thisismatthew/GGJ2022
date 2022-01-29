using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading.Tasks;


public enum GameStates
{
    OutfitPicking,
    ShadowHiding,
    BlobSeeking,
    NewRoomSetup,
    BlobVictory,
    ShadowVictory,
}

public class GameManager : MonoBehaviour
{
    public BlobManager bm;
    public ShadowManager sm;
    private PhotonView _photonView;
    public PhotonTimer timer;
    // Start is called before the first frame update
    void Start()
    {
        _photonView = PhotonView.Get(this);
        Physics2D.IgnoreCollision(bm.controller.GetComponent<CapsuleCollider2D>(), sm.Controller.GetComponent<CapsuleCollider2D>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public async void EnterGameState(GameStates newState)
    { 
        if (newState == GameStates.OutfitPicking)
        {
            bm.controller.gameObject.SetActive(false);
            Task picking = bm.WaitForPick();
            await picking;
            //go to next state for everyone.
            //disable blob movement and hide blob body
            _photonView.RPC("EnterGameState",RpcTarget.All, GameStates.ShadowHiding);
        }
        if (newState == GameStates.ShadowHiding)
        {
            //start dialogue task for blob
            //start dialogue task for shadow
            Task countdown = timer.Timer(20);
            await countdown; //once timer is up 
            _photonView.RPC("EnterGameState", RpcTarget.All, GameStates.BlobSeeking);

        }
        if (newState == GameStates.BlobSeeking)
        {
            //start dialogue for blob&shadow
            //lock the shadows movement & enter the blob
            sm.Controller.enabled = false;
            bm.controller.gameObject.SetActive(false);
            Task countdown = timer.Timer(20);
            await countdown; //once timer is up 
            if (bm.controller.Detector.OnShadow)
            {
                _photonView.RPC("EnterGameState", RpcTarget.All, GameStates.BlobVictory);
            }
            else
            {
                _photonView.RPC("EnterGameState", RpcTarget.All, GameStates.ShadowVictory);
            }
        }
        if (newState == GameStates.ShadowVictory)
        {

        }
        if (newState == GameStates.BlobVictory)
        {

        }

    }

}
