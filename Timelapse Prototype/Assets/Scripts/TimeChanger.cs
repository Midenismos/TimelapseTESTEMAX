using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChanger : MonoBehaviour
{
    [SerializeField] public TimeChange timeChange;

    ////Changer ces variables pour équilibrage
    //[SerializeField]
    //private float Duration = 0.0f;

    //[SerializeField]
    //private float NewMultiplier = 0.0f;

    //Ne pas changer ces variables
    private TimeManager TimeManager;


    // Start is called before the first frame update
    void Start()
    {
        //Connecte l'objet au TimeManager
        TimeManager = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeTime()
    {
        //Initialise le timer du TimeManager
        //OLD, combined that in startTimechange TimeManager.GetComponent<TimeManager>().timer = Duration;
        //Change le multiplier de vitesse
        TimeManager.StartTimeChange(timeChange);
    }
}
