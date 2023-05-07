using TMPro;
using UnityEngine;

public class Chronometer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timer;
    private void Awake()
    {
    }
    // The elapsed time in seconds
    private float elapsedTime = 0.0f;

    // Whether the chronometer is currently running
    private bool isRunning = false;

    // Start the chronometer
    public void StartChronometer()
    {
        isRunning = true;
    }

    // Stop the chronometer
    public void StopChronometer()
    {
        isRunning = false;
    }

    // Reset the chronometer
    public void ResetChronometer()
    {
        elapsedTime = 0.0f;
    }

    // Update the chronometer once per frame
    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            timer.text = "Time: " + FormatTime(elapsedTime);
        }
    }

    // Display the elapsed time as a string
    //void OnGUI()
    //{
    //    timer.text="Time: " + FormatTime(elapsedTime);
    //}

    // Format the time as a string (mm:ss:ms)
    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60.0f);
        int seconds = (int)(time % 60.0f);
        int milliseconds = (int)((time * 1000.0f) % 1000.0f);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}