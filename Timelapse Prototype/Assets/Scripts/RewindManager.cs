using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public event Action OnRewindStopped = null;

    private Rewindable[] rewindables = null;
    private float rewindDuration = 0;
    private float rewindSpeed = 0;
    private float rewindTimeCounter = 0;
    private bool isRewinding = false;

    // Start is called before the first frame update
    void Start()
    {
        rewindables = FindObjectsOfType<Rewindable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRewinding)
        {
            float deltaGameTime = Time.deltaTime * rewindSpeed;
            float lastCounter = rewindTimeCounter;
            rewindTimeCounter += deltaGameTime;

            if(rewindTimeCounter >= rewindDuration)
            {
                deltaGameTime = rewindDuration - lastCounter;
                rewindTimeCounter = rewindDuration;
                RewindRewindables(deltaGameTime);
                EndRewind();
            }
            else
            {
                RewindRewindables(deltaGameTime);
            }
        }
        else
        {
            RecordRewindables();
        }
    }

    private void RecordRewindables()
    {
        for (int i = 0; i < rewindables.Length; i++)
        {
            rewindables[i].Record();
        }
    }

    private void RewindRewindables(float deltaGameTime)
    {
        for (int i = 0; i < rewindables.Length; i++)
        {
            rewindables[i].Rewind(deltaGameTime, rewindTimeCounter);
        }
    }

    public void StartRewind(float rewindSpeed, float rewindDuration)
    {
        this.rewindSpeed = rewindSpeed;
        this.rewindDuration = rewindDuration;

        rewindTimeCounter = 0;

        for (int i = 0; i < rewindables.Length; i++)
        {
            rewindables[i].StartRewind();
        }

        isRewinding = true;
    }

    public float EndRewind()
    {
        for (int i = 0; i < rewindables.Length; i++)
        {
            rewindables[i].EndRewind();
        }

        isRewinding = false;

        OnRewindStopped?.Invoke();

        return rewindDuration - rewindTimeCounter;
    }

    public void AddDuration(float addedDuration)
    {
        rewindDuration += addedDuration;
    }

    public void ChangeSpeed(float newSpeed)
    {
        rewindSpeed = newSpeed;
    }
}
