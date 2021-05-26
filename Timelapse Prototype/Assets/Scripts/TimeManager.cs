using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeChangeType
{
    REWIND,
    STOP,
    SLOW,
    ACCELERATE
};

[Serializable]
public struct TimeChange
{
    public TimeChangeType type;
    public float duration;
    public float speed;
}

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float startingMultiplier = 1;
    [SerializeField] private float timeLerpDuration = 1;

    [SerializeField] private RewindManager rewindManager = null;
    
    public float multiplier;
    public float timer = 0f;

    private bool hasTimeChange = false;
    private TimeChange currentTimeChange;

    private bool hasStandartTimeChange = false;

    private bool mustResumeCurrentTimeChange = false;

    private Coroutine TimeLerpCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        multiplier = startingMultiplier;
        rewindManager.OnRewindStopped += RewindStopped;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStandartTimeChange)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                EndStandartTimeChange();
            }
        }
    }

    //Commence le changement de temps (appellé par un TimeChanger)
    public void StartTimeChange(TimeChange timeChange)
    {
        if (!hasTimeChange)
        {
            if (timeChange.type == TimeChangeType.REWIND)
            {
                StartRewind(timeChange.speed, timeChange.duration);
            }
            else
            {
                StartStandartTimeChange(timeChange.speed, timeChange.duration);
            }
            currentTimeChange = timeChange;
        }
        else
        {
            if (currentTimeChange.type == TimeChangeType.SLOW)
            {
                if (timeChange.type == TimeChangeType.REWIND)
                {
                    EndStandartTimeChange(false);
                    StartRewind(timeChange.speed, timer + timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.STOP)
                {
                    PauseCurrentTimeChange();
                    StartStandartTimeChange(timeChange.speed, timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.ACCELERATE)
                {
                    EndStandartTimeChange();
                }
                else if (timeChange.type == TimeChangeType.SLOW)
                {
                    timer += timeChange.duration;
                }
            }
            else if (currentTimeChange.type == TimeChangeType.ACCELERATE)
            {
                if (timeChange.type == TimeChangeType.REWIND)
                {
                    EndStandartTimeChange(false);
                    StartRewind(timeChange.speed, timer + timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.STOP)
                {
                    PauseCurrentTimeChange();
                    StartStandartTimeChange(timeChange.speed, timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.SLOW)
                {
                    EndStandartTimeChange();
                }
                else if (timeChange.type == TimeChangeType.ACCELERATE)
                {
                    timer += timeChange.duration;
                }
            } else if(currentTimeChange.type == TimeChangeType.REWIND)
            {
                if(timeChange.type == TimeChangeType.REWIND)
                {
                    rewindManager.AddDuration(timeChange.duration);
                } else if(timeChange.type == TimeChangeType.STOP)
                {
                    PauseCurrentTimeChange();
                    StartStandartTimeChange(timeChange.speed, timeChange.duration);
                } else if(timeChange.type == TimeChangeType.SLOW || timeChange.type == TimeChangeType.ACCELERATE)
                {
                    rewindManager.AddDuration(timeChange.duration);
                    rewindManager.ChangeSpeed(timeChange.speed);
                }
            } else if(currentTimeChange.type == TimeChangeType.STOP)
            {
                if (timeChange.type == TimeChangeType.REWIND)
                {
                    currentTimeChange = timeChange;
                    mustResumeCurrentTimeChange = true;
                } else if(timeChange.type == TimeChangeType.STOP)
                {
                    timer += timeChange.duration;
                } else if(timeChange.type == TimeChangeType.SLOW || timeChange.type == TimeChangeType.ACCELERATE)
                {
                    currentTimeChange = timeChange;
                    mustResumeCurrentTimeChange = true;
                }
            }
        }
    }

    private void StartStandartTimeChange(float speed, float duration)
    {
        timer = duration;
        hasTimeChange = true;
        hasStandartTimeChange = true;

        StartTimeLerp(speed);
    }
    public void EndStandartTimeChange(bool executeTimeLerp = true)
    {
        hasTimeChange = false;
        hasStandartTimeChange = false;

        if(mustResumeCurrentTimeChange)
        {
            if(currentTimeChange.type == TimeChangeType.SLOW || currentTimeChange.type == TimeChangeType.ACCELERATE)
            {
                StartStandartTimeChange(currentTimeChange.speed, currentTimeChange.duration);
            } else
            {
                StartRewind(currentTimeChange.speed, currentTimeChange.duration);
            }

            mustResumeCurrentTimeChange = false;
            return;
        }

        if(executeTimeLerp)
        {
            StartTimeLerp(1);
        }
    }

    private void StartRewind(float speed, float duration)
    {
        multiplier = 0;
        hasTimeChange = true;
        rewindManager.StartRewind(speed, duration);
    }

    private void RewindStopped()
    {
        multiplier = 1;
        hasTimeChange = false;
    }

    private void StartTimeLerp(float speed)
    {
        if (TimeLerpCoroutine != null)
        {
            StopCoroutine(TimeLerpCoroutine);
        }
        TimeLerpCoroutine = StartCoroutine(TimeLerp(multiplier, speed, timeLerpDuration));
    }

    private IEnumerator TimeLerp(float oldMultiplier, float newMultiplier, float duration)
    {
        float timeCounter = 0;

        while (timeCounter < duration)
        {
            timeCounter += Time.deltaTime;
            multiplier = Mathf.Lerp(oldMultiplier, newMultiplier, timeCounter / duration);
            yield return null;
        }
        multiplier = newMultiplier;
    }

    private void PauseCurrentTimeChange()
    {
        if(currentTimeChange.type == TimeChangeType.SLOW  || currentTimeChange.type == TimeChangeType.ACCELERATE)
        {
            EndStandartTimeChange(false);
            currentTimeChange.duration = timer;
        } else if (currentTimeChange.type == TimeChangeType.REWIND)
        {
            currentTimeChange.duration = rewindManager.EndRewind();
        }
        mustResumeCurrentTimeChange = true;

    }
}
