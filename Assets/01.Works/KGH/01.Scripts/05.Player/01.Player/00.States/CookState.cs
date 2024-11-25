using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookState : PlayerState
{
    public CookState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        defaultAnimationHash = Animator.StringToHash("Cook");
    }
    public override void Enter()
    {
        Player.OnMiniGameStartEvent?.Invoke();
        Player.InGameUI.ShowCookUI();
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;
    }
    public void FishSelected(ItemSO item)
    {
        Player.InGameUI.HideCookUI();
        Player.FishStartEvent?.Invoke();
        Player.AnimatorComponent.SetBool(defaultAnimationHash, true);
    }
    public override void Exit()
    {
        base.Exit();
        Player.InputReader.OnEscapeEvent -= StateMachine.ResetToIdleState;
        Player.OnMiniGameEndEvent?.Invoke();
        Player.InGameUI.HideCookUI();
    }
}
