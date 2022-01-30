using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Febucci.UI;
using UnityEngine.UI;
using System.Threading.Tasks;

[System.Serializable]
public struct DialogueEvent{
    public string name;
    public List<string> blocks;
}

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueHolder;
    private TextMeshProUGUI textMeshPro;
    private TextAnimatorPlayer textAnimatorPlayer;
    public DialogueEvent[] Events;
    public Image ShadowHead, BlobFace, DialogueUI;
    private Dictionary<string, List<string>> DialogueBlocks;
    private bool eventActive = false;
    public bool blockActive = false;
    private string currentEvent;
    private int eventIndex;

    private void Start()
    {
        textMeshPro = dialogueHolder.GetComponent<TextMeshProUGUI>();
        textAnimatorPlayer = dialogueHolder.GetComponent<TextAnimatorPlayer>();
        DialogueBlocks = new Dictionary<string, List<string>>();
        foreach(DialogueEvent e in Events)
        {
/*            for (int i = 0; i < e.blocks.Count - 1; i++)
            {
                e.blocks[i] += "<?end>";
            }*/
            DialogueBlocks.Add(e.name, e.blocks);
        }
        textAnimatorPlayer.textAnimator.onEvent += OnEvent;
    }
    public void StartDialogueEvent(string eventName)
    {
        eventActive = true;
        currentEvent = eventName;
        eventIndex = 0;

    }

    private void Update()
    {
        //if the event is active, and we have blocks remaing
        if (eventActive && eventIndex <= DialogueBlocks[currentEvent].Count-1)
        {
            //set the block to active and send the text to the typewriter
            if (!blockActive)
            {
                textMeshPro.text = DialogueBlocks[currentEvent][eventIndex];
                blockActive = true;
            }
        }
        else
        {

            //event is done lets cleanup
            eventActive = false;
            blockActive = false;
            
            textMeshPro.text = "";
        }
    }

    private async void OnEvent(string message)
    {
        //when we recieve the end message we'll call for the next block
        switch (message)
        {
            case "end":
                Task waiter = WaitForInputOrSeconds(2f);
                await waiter;
                eventIndex++;
                //start an async that waits for input
                blockActive = false;
                break;
        }
    }

    public async Task WaitForInputOrSeconds(float duration)
    {
        float end = Time.time + duration;
        while (Time.time < end && !Input.GetMouseButtonDown(0))
        {
            await Task.Yield();
        }
    }

    public async Task WaitForEventComplete()
    {
        while (eventActive) await Task.Yield();
    }

    public void EnableDialogue(bool blob)
    {
        DialogueUI.gameObject.SetActive(true);
        if (blob)
        {
            BlobFace.gameObject.SetActive(true);
        }
        else
        {
            ShadowHead.gameObject.SetActive(true);
        }
    }
    public void DisableDialogue()
    {
        DialogueUI.gameObject.SetActive(false);
        BlobFace.gameObject.SetActive(false);
        ShadowHead.gameObject.SetActive(false);
    }
}
