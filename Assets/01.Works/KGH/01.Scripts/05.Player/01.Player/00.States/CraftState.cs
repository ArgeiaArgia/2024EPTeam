using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftState : PlayerState
{
    public CraftState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        defaultAnimationHash = Animator.StringToHash("Craft");
    }

    public override void Enter()
    {
        base.Enter();
        Player.InGameUI.ShowLoadingUI(Player.transform.position + Vector3.down * Player.SpriteRendererComponent.bounds
            .size.y / 2);
        Player.InGameUI.OnLoadingEnded.AddListener(StateMachine.ResetToIdleState);
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;
    }

    public override void Exit()
    {
        base.Exit();
        Player.InputReader.OnEscapeEvent -= StateMachine.ResetToIdleState;
        Player.InGameUI.OnLoadingEnded.RemoveListener(StateMachine.ResetToIdleState);
    }
}