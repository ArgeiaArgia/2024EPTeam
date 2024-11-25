using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    private PlayerState _targetState;

    public PlayerState TargetState
    {
        get => _targetState ?? StateDictionary[typeof(IdleState)];
        private set => _targetState = value;
    }

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
        CurrentState.Exit();
        CurrentState = TargetState;
        CurrentState.Enter();
    }

    public void ResetToIdleState()
    {
        CurrentState.Exit();
        CurrentState = StateDictionary[typeof(IdleState)];
        SetTargetState<IdleState>();
        CurrentState.Enter();
    }

    public void SetTargetState<T>() where T : PlayerState
    {
        TargetState = StateDictionary[typeof(T)];
        if (CurrentState.GetType() == typeof(IdleState) && TargetState.GetType() != typeof(IdleState))
        {
            if (TargetState.GetType() == typeof(CraftState))
            {
                ChangeState<T>();
            }
            else if (_player.CheckIfPlayerNearPosition())
            {
                ChangeState<T>();
            }
            else
            {
                ChangeState<MoveState>();
            }
        }
    }
}