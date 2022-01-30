using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Febucci.UI;
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
            DialogueBlocks.Add(e.name, e.blocks);
        }
        textAnimatorPlayer.textAnimator.onEvent += OnEvent;
    }
    public void StartDialogueEvent(string eventName)
    {
        eventActive = true;
        currentEvent = eventName;
        /*if (!DialogueBlocks.ContainsKey(eventName))
        {
            Debug.LogError("no dialogue event for key: " + eventName);
        }*/
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
                eventIndex++;
                Task waiter = WaitForInputOrSeconds(2f);
                await waiter;
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

}
