using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class ShadowManager : MonoBehaviour
{
    //get it working offline then we'll work out how to post a data packet to the game manager;

    public CinemachineVirtualCamera Cam;
    public GameObject ShadowUI;
    public PlayerMovement PlayerMover;
    public Color MovingClr, StoppedClr;

    public List<Sprite> ShadowHats;
    public List<Sprite> ShadowOutfits;

    public SpriteRenderer Hat,Outfit,Face;

    // Start is called before the first frame update
    void Start()
    {
        //first for blob player we want to make sure thier cam is in the right place and they have the right UI displaying
        if (PhotonNetwork.NickName == "Shadow")
        {
            Cam.m_Priority = 10;
            ShadowUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer[] sprites = PlayerMover.GetComponentsInChildren<SpriteRenderer>();
        if (PlayerMover.Moving)
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
    }
}
