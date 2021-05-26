using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CandyMachine : MonoBehaviour
{
    public GameObject timeFood;
    public float duration;
    public GameObject spawnPosition;
    private Vector3 pos;
    public ParticleSystem particle;
    public GameObject text;
    public string type;


    // Start is called before the first frame update
    void Start()
    {
        pos = spawnPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<TextMeshPro>().text = type + " " + duration+"s";

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (Input.GetKeyDown("a"))
            {
                GameObject prefab = Instantiate(timeFood);
                prefab.transform.position = pos;
                prefab.GetComponent<TimeChanger>().timeChange.duration = duration;
                prefab.GetComponentInChildren<TextMeshPro>().text = type + " " + duration+"s";
                particle.Play();

            }
            if (Input.GetKeyDown("y"))
            {
                duration += 1;
            }
            if (Input.GetKeyDown("t"))
            {
                duration -= 1;
            }
        }
    }
}
