using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookState : PlayerState
{
    public CookState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }
    public override void Enter()
    {
        Player.OnMiniGameStartEvent?.Invoke();
    }
}
