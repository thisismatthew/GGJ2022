using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameStates
{
    OutfitPicking,
    ShadowHiding,
    BlobSeeking,
    NewRoomSetup,
    End,
}

public class GameManager : MonoBehaviour
{
    public BlobManager bm;
    public ShadowManager sm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
