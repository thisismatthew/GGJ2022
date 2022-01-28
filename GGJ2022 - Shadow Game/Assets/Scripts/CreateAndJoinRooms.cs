using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    [SerializeField]
    //public Player player;
    public int Number;


    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {

        /*
        // We're setting which players which here, for the moment we'll just do it by seeing how many people are in the rooms. 
        */
        Debug.Log("You are Player number: " + PhotonNetwork.PlayerList.Length);
        if (PhotonNetwork.PlayerList.Length == 1){PhotonNetwork.NickName = "Blob";}
        if (PhotonNetwork.PlayerList.Length == 2) { PhotonNetwork.NickName = "Shadow"; }

        //player = this.player;
        //Debug.Log("Actor Number:" + player.ActorNumber);
        PhotonNetwork.LoadLevel("Game");
    }
}
