using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState : PlayerState
{
    public FishState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Player.OnMiniGameStartEvent?.Invoke();
        Debug.Log("I AM JUST A FISH");
    }
}
