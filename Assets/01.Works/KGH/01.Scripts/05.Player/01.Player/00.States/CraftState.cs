using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftState : PlayerState
{
    public CraftState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("CraftState Enter");
        Player.InGameUI.ShowLoadingUI(Player.transform.position + Vector3.down * Player.SpriteRendererComponent.bounds
            .size.y/2);
        Player.InGameUI.OnLoadingEnded.AddListener(StateMachine.ResetToIdleState);
    }

    public override void Exit()
    {
        Debug.Log("CraftState Exit");
        Player.InGameUI.OnLoadingEnded.RemoveListener(StateMachine.ResetToIdleState);
    }
}
