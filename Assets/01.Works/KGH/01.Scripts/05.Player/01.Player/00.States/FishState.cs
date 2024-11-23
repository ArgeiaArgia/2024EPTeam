using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState : PlayerState
{
    public FishState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        defaultAnimationHash = Animator.StringToHash("Fishing");
    }
    public override void Enter()
    {
        base.Enter();
        Player.OnMiniGameStartEvent?.Invoke();
        Debug.Log("I AM JUST A FISH");
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;

    }
}
