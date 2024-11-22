using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Player Player;
    protected PlayerStateMachine StateMachine;

    protected int defaultAnimationHash;
    
    protected PlayerState(Player player, PlayerStateMachine stateMachine)
    {
        Player = player;
        StateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        Player.AnimatorComponent.SetBool(defaultAnimationHash, true);
    }

    public virtual void Exit()
    {
        Player.AnimatorComponent.SetBool(defaultAnimationHash, false);
    }

    public virtual void Update()
    {
    }

    public virtual void FixedUpdate()
    {
    }
}
