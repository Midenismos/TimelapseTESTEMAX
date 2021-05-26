using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableRotatingCube : Rewindable
{
    [SerializeField] private RotatingCube rotatingCube = null;
    public override void StartRewind()
    {
        base.StartRewind();
    }

    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        rotatingCube.Rotate(-deltaGameTime);
    }
}
