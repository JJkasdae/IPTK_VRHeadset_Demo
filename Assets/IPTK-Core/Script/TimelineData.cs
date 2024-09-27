using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineData : ScriptableObject
{
    [SerializeField]
    private TransitionData[] _transitionData;

    public TransitionData[] transitionData => _transitionData;
}
