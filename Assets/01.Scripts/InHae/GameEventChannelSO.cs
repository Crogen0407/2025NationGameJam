using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    //empty class
}

[CreateAssetMenu (menuName = "SO/Events/EventChannel")]
public class GameEventChannelSO : ScriptableObject
{
    private Dictionary<Type, Action<GameEvent>> _events = new();
    private Dictionary<Delegate, Action<GameEvent>> _lookUp = new();
    
    public void AddListener<T>(Action<T> handler) where T : GameEvent
    {
        if (_lookUp.ContainsKey(handler) == false) //이미 구독하는 매서드는 중복 제거
        {
            Action<GameEvent> CastHandler = (evt) => handler(evt as T);
            _lookUp[handler] = CastHandler;

            Type evtType = typeof(T);
            if(_events.ContainsKey(evtType))
            {
                _events[evtType] += CastHandler;
            }else
            {
                _events[evtType] = CastHandler;
            }
        }
    }

    public void RemoveListener<T>(Action<T> handler) where T : GameEvent
    {
        Type evtType = typeof(T);
        if(_lookUp.TryGetValue(handler, out Action<GameEvent> action))
        {
            if(_events.TryGetValue(evtType, out Action<GameEvent> internalAction))
            {
                internalAction -= action;
                if (internalAction == null)
                    _events.Remove(evtType);
                else
                    _events[evtType] = internalAction;
            }
        }
    }

    public void RaiseEvent(GameEvent evt)
    {
        if(_events.TryGetValue(evt.GetType(), out Action<GameEvent> handlers))
        {
            handlers?.Invoke(evt);
        }
    }

    public void Clear()
    {
        _events.Clear();
        _lookUp.Clear();
    }
}
