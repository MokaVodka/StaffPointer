using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

public class Timer : MonoBehaviour
{
    //All variables are declared and/or initialized at the top of the class.
    public bool canStart;

    public float currentTime;

    [SerializeField]
    private float startingTime;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if(canStart)
            Countdown();
    }

    //In the Countdown() we decrease variable currentTime by 1 each second.
    //Then convert it to string and display in the project.
    //To avoid the countdown decreasing to negative number, we ser currentTime to 0.
    void Countdown()
    {
        currentTime -= 1 * Time.deltaTime;

        countdownText.text = currentTime.ToString("0");

        if(currentTime <= 0)
            currentTime = 0;
    }

    public void Reset()
    {
        currentTime = startingTime;        
    }
}
