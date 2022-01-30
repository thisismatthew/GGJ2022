using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;

public class ShadowManager : MonoBehaviour
{
    //get it working offline then we'll work out how to post a data packet to the game manager;

    public CinemachineVirtualCamera Cam;
    public GameObject ShadowUI;
    public ShadowController Controller;
    public Color MovingClr, StoppedClr;

    private int currentHatIndex, currentOutfitIndex;
    public int newHatIndex, newOutfitIndex;

    public List<Sprite> ShadowHats;
    public List<Sprite> ShadowOutfits;

    private PhotonView _photonView;

    public SpriteRenderer Hat, Outfit, Face;
    public Image DialogueHat;

    // Start is called before the first frame update
    void Start()
    {
        _photonView = PhotonView.Get(this);
        //first for blob player we want to make sure thier cam is in the right place and they have the right UI displaying
        if (PhotonNetwork.NickName == "Shadow")
        {
            Cam.m_Priority = 10;
            Cam.LookAt = Controller.gameObject.transform;
            Cam.Follow = Controller.gameObject.transform;
            //ShadowUI.SetActive(true);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer[] sprites = Controller.GetComponentsInChildren<SpriteRenderer>();
        if (Controller.Moving)
        {
             foreach(SpriteRenderer spr in sprites)
             {
               spr.color = MovingClr;
             }
        }
        else
        {
            foreach (SpriteRenderer spr in sprites)
            {
                spr.color = StoppedClr;
            }
        }
        Face.color = StoppedClr;

        CheckForOutfitUpdate();
    }

    public void CheckForOutfitUpdate()
    {
        if (currentHatIndex != newHatIndex)
        {
            currentHatIndex = newHatIndex;
            _photonView.RPC("UpdateShadowHat", RpcTarget.All, currentHatIndex);
        }

        if (currentOutfitIndex != newOutfitIndex)
        {
            currentOutfitIndex = newOutfitIndex;
            _photonView.RPC("UpdateShadowOutfit", RpcTarget.All, currentOutfitIndex);
        }
    }

    [PunRPC]
    public void UpdateShadowOutfit(int outfitIndex)
    {
        Outfit.sprite = ShadowOutfits[outfitIndex];
    }

    [PunRPC]
    public void UpdateShadowHat(int hatIndex)
    {
        Hat.sprite = ShadowHats[hatIndex];
        DialogueHat.sprite = ShadowHats[hatIndex];
    }
}
