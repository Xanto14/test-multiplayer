using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class GameManageOnline : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshProUGUI countdownText;

    public int maxLaps = 3;
    public List<Player> finishedPlayers = new List<Player>();

    [SerializeField]
    private float countdownDuration = 3f;

    private float currentCountdownValue;
    public TextMeshProUGUI timerText;
    public float startTime;
    public bool raceStarted;
    private float elapsedTime;
    private bool timerStarted = false;

    public bool IsPlaying { get { return isRaceStarted; } }

    private bool isRaceStarted = false;

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        //LapCounter.OnPlayerFinishedLap += HandlePlayerFinishedLap;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        //LapCounter.OnPlayerFinishedLap -= HandlePlayerFinishedLap;
    }

    public void StartRace()
    {
        StartCoroutine(CountdownCoroutine());
        
    }
    void HandlePlayerFinishedLap(Player player, int lap)
    {
        if (lap >= maxLaps)
        {
            photonView.RPC("RPCPlayerFinishedRace", RpcTarget.MasterClient, player, elapsedTime);
        }
    }

    [PunRPC]
    void RPCPlayerFinishedRace(Player player, float finalTime)
    {
        finishedPlayers.Add(player);

        if (finishedPlayers.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            finishedPlayers.Sort((p1, p2) => GetPlayerTime(p1).CompareTo(GetPlayerTime(p2)));

            Debug.Log("Race finished!");
            for (int i = 0; i < finishedPlayers.Count; i++)
            {
                Debug.Log("Player " + finishedPlayers[i].NickName + " finished in " + GetPlayerTime(finishedPlayers[i]).ToString("0.00") + " seconds.");
            }
        }
    }

    float GetPlayerTime(Player player)
    {
        float playerTime = -1f;
        for (int i = 0; i < LapCounter.Instance.lapTimes.Count; i++)
        {
            if (LapCounter.Instance.lapTimes[i].player == player)
            {
                playerTime = LapCounter.Instance.lapTimes[i].time;
                break;
            }
        }
        if (playerTime < 0f)
        {
            Debug.LogError("Could not find time for player " + player.NickName);
        }
        return playerTime;
    }
    public void OnClickLoadScene(int scene)
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    private IEnumerator CountdownCoroutine()
    {
        currentCountdownValue = countdownDuration;

        while (currentCountdownValue > 0)
        {
            yield return new WaitForSeconds(1f);
            currentCountdownValue--;
            countdownText.text = currentCountdownValue.ToString();
        }

        isRaceStarted = true;
        countdownText.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCStartRace", RpcTarget.AllBuffered);
        }
    }
    


    

    

   

    [PunRPC]
    public void RPCStartRace()
    {
        raceStarted = true;
        startTime = (float)PhotonNetwork.Time;
    }

    private void Awake()
    {
        StartRace();
    }

    void Update()
    {
        if (isRaceStarted)
        {
            //// Get the current timestamp from PhotonNetwork.Time
            //double currentTime = PhotonNetwork.Time;

            //// Convert the timestamp to a DateTime object
            //DateTime dateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(currentTime);

            //// Convert the DateTime object to a string
            //string timestamp = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            //// Print the timestamp to the console
            //Debug.Log("Current timestamp: " + timestamp);
            //elapsedTime = (float)PhotonNetwork.Time - startTime;
            //string minutes = ((int)elapsedTime / 60).ToString("00");
            //string seconds = (elapsedTime % 60).ToString("00.00");
            //timerText.text = minutes + ":" + seconds;

            if (!timerStarted)
            {
                timerText.GetComponent<Chronometer>().StartChronometer();
                timerStarted = true;

            }
        }
    }
}
