using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookState : PlayerState
{
    public CookState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }
    public override void Enter()
    {
        Player.OnMiniGameStartEvent?.Invoke();
        Player.InGameUI.ShowCookUI();
    }
    public void FishSelected(ItemSO item)
    {
        Player.InGameUI.HideCookUI();
        Player.FishStartEvent?.Invoke();
    }
    public override void Exit()
    {
        Player.OnMiniGameEndEvent?.Invoke();
        Player.InGameUI.HideCookUI();
    }
}
