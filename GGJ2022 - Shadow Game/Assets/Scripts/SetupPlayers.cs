using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SetupPlayers : MonoBehaviour
{
    public GameObject shadowPrefab, blobPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NickName == "Shadow") { shadowPrefab.SetActive(true); }
        if (PhotonNetwork.NickName == "Blob") { blobPrefab.SetActive(true); }

    }

}
