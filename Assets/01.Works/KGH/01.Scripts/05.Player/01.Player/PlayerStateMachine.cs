using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public PlayerState TargetState { get; private set; }
    private Dictionary<Type, PlayerState> StateDictionary = new Dictionary<Type, PlayerState>();
    
    private Player _player;
    
    public void Initialize(Type startState, Player player)
    {
        _player = player;
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();
    }
    public void AddState(PlayerState state)
    {
        StateDictionary.Add(state.GetType(), state);
    }
    public void ChangeState<T>() where T : PlayerState
    {
        CurrentState.Exit();
        CurrentState = StateDictionary[typeof(T)];
        CurrentState.Enter();
    }
    public void ChangeToTargetState()
    {
        if(TargetState == null) TargetState = StateDictionary[typeof(IdleState)];
        CurrentState.Exit();
        CurrentState = TargetState;
        CurrentState.Enter();
    }
    public void SetTargetState<T>() where T : PlayerState
    {
        TargetState = StateDictionary[typeof(T)];
    }
}