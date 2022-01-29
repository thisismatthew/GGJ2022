using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using TMPro;
public class BlobManager : MonoBehaviour
{
    //get it working offline then we'll work out how to post a data packet to the game manager;
    public TextMeshProUGUI TitleUI;
    public CinemachineVirtualCamera Cam;
    public GameObject PickerUI, PickingBackgroundUI;
    public SpriteRenderer Hat, Outfit;
    public List<Sprite> BlobHats, BlobOutfits;

    // Start is called before the first frame update
    void Start()
    {
        //first for blob player we want to make sure thier cam is in the right place and they have the right UI displaying
        if (PhotonNetwork.NickName == "Blob")
        {
            Cam.m_Priority = 10;
            PickerUI.SetActive(true);
            PickingBackgroundUI.SetActive(true);
            TitleUI.text = "<wiggle>Choose Your Outfit...";
        }
    }

    [PunRPC]
    public void UpdateBlobOutfit(int outfitIndex)
    {
        Outfit.sprite = BlobOutfits[outfitIndex];
    }

    [PunRPC]
    public void UpdateBlobHat(int hatIndex)
    {
        Hat.sprite = BlobHats[hatIndex];
    }

    public void OnBlobReady()
    {
        //create data packet to send to game manager with outfit details
        //signal that the hider only has a few moments to hide
        //put up a counter for both players. 
    }
}
