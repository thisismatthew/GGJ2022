using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Threading.Tasks;

public class PhotonTimer : MonoBehaviour
{
    bool ticking = false;
    double timerIncrementValue;
    double startTime;
    double timer;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    private TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void StartTimer(double _timer)
    {
        timer = _timer;
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

    public async Task Timer(double _time) 
    {
        
        StartTimer(_time);
        while (ticking)
        {
            timerIncrementValue = PhotonNetwork.Time - startTime;
            tmp.text = "Countdown: " + (timer - (int)timerIncrementValue).ToString();

            await Task.Yield();

            if (timerIncrementValue >= timer)
            {
                tmp.text = "";
                ticking = false;
            }
        }

    }
}
