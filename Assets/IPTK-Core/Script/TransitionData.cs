using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum transitionEffectType
{
    none,
    animation,
    fade
}
public class TransitionData : ScriptableObject
{
    private int _transitionID;

    [Header("Transition General Stats")]
    [SerializeField]
    private SessionData _lastSession;

    [SerializeField]
    private SessionData _nextSession;

    [SerializeField]
    private transitionEffectType _effect;

    [SerializeField]
    private bool _Effect;

    [SerializeField]
    private string _importEffectAnimation;

    public SessionData lastSession => _lastSession;
    public SessionData nextSession => _nextSession;

    public void initialize(SessionData sessionData)
    {
        _lastSession = sessionData;
    }
    //public sessionData lastSession => _lastSession;
    //public sessionData nextSession => _nextSession;
    //public transitionEffectType effect => _effect;

}
