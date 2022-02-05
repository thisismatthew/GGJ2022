using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using TMPro;
using System.Threading.Tasks;
public class BlobManager : MonoBehaviour
{
    //get it working offline then we'll work out how to post a data packet to the game manager;
    public TextMeshProUGUI TitleUI;
    public CinemachineVirtualCamera Cam;
    public GameObject PickerUI, PickingBackgroundUI;
    public SpriteRenderer Hat, Outfit;
    public Image DialogueHead;
    private int currentHatIndex, currentOutfitIndex;
    public int newHatIndex, newOutfitIndex;
    public List<Sprite> BlobHats, BlobOutfits;
    private PhotonView _photonView;
    public BlobController controller;
    public bool ClothesReady = false;

    // Start is called before the first frame update
    void Start()
    {
        _photonView = PhotonView.Get(this);
        //first for blob player we want to make sure thier cam is in the right place and they have the right UI displaying
        if (PhotonNetwork.NickName == "Blob")
        {
            Cam.m_Priority = 10;
            PickerUI.SetActive(true);
            PickingBackgroundUI.SetActive(true);
            TitleUI.text = "<wiggle>Choose Your Outfit...";
        }
    }

    private void Update()
    {
        //when hatIndex changes we want to update all views with the new info
        if (currentHatIndex!= newHatIndex)
        {
            currentHatIndex = newHatIndex;
            _photonView.RPC("UpdateBlobHat", RpcTarget.All, currentHatIndex);
        }

        if (currentOutfitIndex != newOutfitIndex)
        {
            currentOutfitIndex = newOutfitIndex;
            _photonView.RPC("UpdateBlobOutfit", RpcTarget.All, currentOutfitIndex);
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
        DialogueHead.sprite = BlobHats[hatIndex];
    }

    //TODO Wait for dialogue event
    [PunRPC]
    public void OnBlobReady()
    {
        ClothesReady = true;
        _photonView.RPC("OnBlobReady", RpcTarget.Others);
    }

    public async Task WaitForPick()
    {
        while (!ClothesReady) await Task.Yield(); 
    }
}
