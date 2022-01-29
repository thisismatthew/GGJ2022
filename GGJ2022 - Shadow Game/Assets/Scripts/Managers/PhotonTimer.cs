using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Threading.Tasks;

public class PhotonTimer : MonoBehaviour
{
    bool ticking = false;
    int timerIncrementValue;
    double startTime;
    double timer;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    private TextMeshProUGUI tmp;
    public int countdown = 0;
    public bool TimesUp = false;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            ticking = true;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            ticking = true;
        }
    }


    private void Update()
    {
        if (!ticking) return;
        timerIncrementValue = (int)(PhotonNetwork.Time - startTime);
        if (timerIncrementValue < countdown)
        {
            tmp.text = (countdown - timerIncrementValue).ToString();
        }
        else
        {
            tmp.text = "";
            TimesUp = true;
        }
            
    }

    public void SetTimer(int duration)
    {
        TimesUp = false;
        countdown = timerIncrementValue + duration;
    }
    //lets set the timer to run forever, and we'll just update for our increment. 

}
