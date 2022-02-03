using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public enum GameStates
{
    Initialising,
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
    public DialogueManager dm;
    private PhotonView _photonView;
    public PhotonTimer timer;
    public GameObject InitialisingUI;
    private List<Task> activeTasks;
    private bool isBlob = true;
    public GameStates currentState;
    private GameStates otherState = GameStates.Initialising;
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private bool synced = false;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().StopAll();
        activeTasks = new List<Task>();
        if (PhotonNetwork.NickName == "Shadow") isBlob = false;
        _photonView = PhotonView.Get(this);
        _photonView.RPC("EnterGameState", RpcTarget.All, GameStates.OutfitPicking);
    }
    private void Update()
    {
        if (otherState != GameStates.Initialising) InitialisingUI.SetActive(false);
        Physics2D.IgnoreCollision(bm.controller.GetComponent<CapsuleCollider2D>(), sm.Controller.GetComponent<CapsuleCollider2D>(), true);
    }


    [PunRPC]
    public async void EnterGameState(GameStates newState)
    {
        currentState = newState;
        _photonView.RPC("UpdateOtherOfGameState", RpcTarget.Others, currentState);
        await WaitForStateSync();

        if (newState == GameStates.OutfitPicking)
        {
            activeTasks.Clear();
            bm.controller.gameObject.SetActive(false);

            //starting dialogues - asyncronous
            if (isBlob)
            {
                FindObjectOfType<AudioManager>().Play("blob_pick");
                dm.EnableDialogue(true);
                dm.StartDialogueEvent("blobIntro"); //BLOB
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("shadow_pick");
                dm.EnableDialogue(false);
                dm.StartDialogueEvent("shadowIntro"); //SHADOW
            }
            await dm.WaitForEventComplete();
            dm.DisableDialogue();


            await bm.WaitForPick();
            //Post Picking Dialogues - asynchronous 
            if (isBlob)
            {
                dm.EnableDialogue(true);
                dm.StartDialogueEvent("blobDressed"); //BLOB
            }
            
            await dm.WaitForEventComplete();
            dm.DisableDialogue();

            //TODO - SYNCING STATES
            //when all tasks are done we send for the next state;
            _photonView.RPC("EnterGameState",RpcTarget.All, GameStates.ShadowHiding);
        }
        if (newState == GameStates.ShadowHiding)
        {
            activeTasks.Clear();
            if(!isBlob)
            {
                FindObjectOfType<AudioManager>().StopAll();
                FindObjectOfType<AudioManager>().Play("shadow_hide");
                dm.EnableDialogue(false);
                dm.StartDialogueEvent("shadowHiding"); //SHADOW
            }
            timer.SetTimer(10);
            activeTasks.Add(WaitForTimer());


            await Task.WhenAll(activeTasks);
            _photonView.RPC("EnterGameState", RpcTarget.All, GameStates.BlobSeeking);
        }
        if (newState == GameStates.BlobSeeking)
        {
            //lock the shadows movement & enter the blob
            bm.PickerUI.SetActive(false);
            bm.PickingBackgroundUI.SetActive(false);
            sm.Controller.enabled = false;


            //SYNCHRONOUS DIALOGUE
            dm.EnableDialogue(true);
            dm.StartDialogueEvent("blobEnters");
            if (isBlob)
            {
                FindObjectOfType<AudioManager>().StopAll();
                FindObjectOfType<AudioManager>().Play("blob_seek");
            }
            await dm.WaitForEventComplete();
            dm.DisableDialogue();

            dm.EnableDialogue(false);
            dm.StartDialogueEvent("shadowLaughs");
            await dm.WaitForEventComplete();
            dm.DisableDialogue();


            bm.controller.gameObject.SetActive(true);




            //StartHuntTimer
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

            //SYNCHRONOUS DIALOGUE
            dm.EnableDialogue(true);
            dm.StartDialogueEvent("blobLoses");
            await dm.WaitForEventComplete();
            dm.DisableDialogue();
            dm.EnableDialogue(false);
            dm.StartDialogueEvent("shadowLaughs");
            await dm.WaitForEventComplete();
            dm.DisableDialogue();

            SwapAndReload();
        }
        if (newState == GameStates.BlobVictory)
        {
            Debug.Log("blobwins");
            //blobwinDialogue
            //SYNCHRONOUS DIALOGUE
            dm.EnableDialogue(true);
            dm.StartDialogueEvent("blobWins");
            await dm.WaitForEventComplete();
            dm.DisableDialogue();
            dm.EnableDialogue(false);
            dm.StartDialogueEvent("shadowLoses");
            await dm.WaitForEventComplete();
            dm.DisableDialogue();

            SwapAndReload();
        }
    }


    public async Task WaitForStateSync()
    {
        //Debug.Log("WaitOnSync Called");
        while (otherState != currentState)
        {
            Debug.Log(PhotonNetwork.NickName + " is waiting to start " + currentState);
            await Task.Yield();
        }
        Debug.Log("Synced");
    }

    [PunRPC]
    private void UpdateOtherOfGameState(GameStates state)
    {
        Debug.Log("otherState Updated");
        otherState = state;
    }

    public void SwapAndReload()
    {
        if (isBlob)
        {
            PhotonNetwork.NickName = "Shadow";
        }
        else
        {
            PhotonNetwork.NickName = "Blob";
        }
        SceneManager.LoadScene("Game");
    }

    public async Task WaitForTimer()
    {
        while (!timer.TimesUp) await Task.Yield();
    }
}
