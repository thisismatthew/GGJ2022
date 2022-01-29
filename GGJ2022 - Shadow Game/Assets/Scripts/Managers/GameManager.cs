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
        _photonView.RPC("EnterGameState", RpcTarget.All, GameStates.OutfitPicking);
    }
    private void Update()
    {
        Physics2D.IgnoreCollision(bm.controller.GetComponent<CapsuleCollider2D>(), sm.Controller.GetComponent<CapsuleCollider2D>(), true);
    }


    [PunRPC]
    public async void EnterGameState(GameStates newState)
    { 
        if (newState == GameStates.OutfitPicking)
        {
            
            //bm.controller.gameObject.SetActive(false);
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
            timer.SetTimer(10);
            Task countdown = WaitForTimer();
            await countdown; //once timer is up 
            _photonView.RPC("EnterGameState", RpcTarget.All, GameStates.BlobSeeking);
        }
        if (newState == GameStates.BlobSeeking)
        {
            //start dialogue for blob&shadow
            //lock the shadows movement & enter the blob
            bm.PickerUI.SetActive(false);
            bm.PickingBackgroundUI.SetActive(false);
            sm.Controller.enabled = false;
            bm.controller.gameObject.SetActive(true);
            timer.SetTimer(20);
            Task countdown = WaitForTimer();
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
            Debug.Log("ShadowWins");
        }
        if (newState == GameStates.BlobVictory)
        {
            Debug.Log("blobwins");
        }

    }

    public async Task WaitForTimer()
    {
        while (!timer.TimesUp) await Task.Yield();
    }
}
