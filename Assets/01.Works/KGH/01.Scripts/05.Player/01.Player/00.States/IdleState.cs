using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Player.RigidbodyComponent.velocity = Vector2.zero;
        Player.OnMouseDownEvent += HandleMousseDownEvent;
    }

    public override void Exit()
    {
        base.Exit();
        Player.OnMouseDownEvent -= HandleMousseDownEvent;
    }

    private void HandleMousseDownEvent(Vector2 targetPos)
    {
        Debug.Log(targetPos);
        Player.TargetPosition = targetPos;
        StateMachine.ChangeState<MoveState>();
    }
}
