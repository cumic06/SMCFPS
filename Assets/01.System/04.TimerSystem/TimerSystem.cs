using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerSystem : MonoSigleton<TimerSystem>
{
    private HashSet<TimerAgent> timerAgentHash = new HashSet<TimerAgent>();
    private HashSet<TimerAgent> destroyAgentHash = new HashSet<TimerAgent>();

    private void Update()
    {
        TimerManager();
    }

    private void TimerManager()
    {
        if (timerAgentHash.Count > 0)
        {
            UpdateAction();
            DestroyAction();
        }
    }

    public void AddTimer(TimerAgent agent)
    {
        timerAgentHash.Add(agent);
    }

    public void DeleteTimer(TimerAgent agent)
    {
        timerAgentHash.Remove(agent);
    }

    private void UpdateAction()
    {
        foreach (var timerAgent in timerAgentHash)
        {
            timerAgent.UpdateAction?.Invoke(timerAgent);
            timerAgent.AddTime(Time.deltaTime);
            if (timerAgent.IsEndTime)
            {
                destroyAgentHash.Add(timerAgent);
            }
        }
    }

    private void DestroyAction()
    {
        foreach (var destroyAgent in destroyAgentHash)
        {
            timerAgentHash.Remove(destroyAgent);
            destroyAgent.EndAction?.Invoke(destroyAgent);
        }
        destroyAgentHash.Clear();
    }
}

public class TimerAgent
{
    public float CurrentTime;

    public float TimerTime;

    public Action<TimerAgent> UpdateAction;
    public Action<TimerAgent> EndAction;

    public bool IsEndTime => CurrentTime >= TimerTime;

    public TimerAgent(float timerTime, Action<TimerAgent> updateAction, Action<TimerAgent> endAction)
    {
        TimerTime = timerTime;
        UpdateAction = updateAction;
        EndAction = endAction;
    }

    public void AddTime(float time)
    {
        if (!IsEndTime)
        {
            CurrentTime += time;
        }
    }
}
