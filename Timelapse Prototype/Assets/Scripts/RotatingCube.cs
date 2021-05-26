using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour
{
    //Touchez à cette variable pour le LD
    [SerializeField]
    private float BaseSpeed = 5;

    //Ne pas toucher à ces variables
    public float multiplier = 1f;
    private GameObject TimeManager;

    // Start is called before the first frame update
    void Start()
    {
        //Connecte l'objet au TimeManager
        TimeManager = GameObject.Find("TimeManager");
    }

    // Update is called once per frame
    void Update()
    {
        //Change la vitesse du gameObject
        multiplier = TimeManager.GetComponent<TimeManager>().multiplier;

        //Tourne l'item en fonction du multiplier
        //OLD CODE : gameObject.transform.Rotate(0, BaseSpeed * multiplier, 0);
        Rotate(multiplier * Time.deltaTime);
    }

    public void Rotate(float deltaGameTime)
    {
        gameObject.transform.Rotate(0, BaseSpeed * deltaGameTime, 0);
    }
}
