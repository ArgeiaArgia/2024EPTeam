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
        base.Enter();
        Player.OnMiniGameStartEvent?.Invoke();
        Player.InGameUI.ShowCookUI();
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;
    }

    public void FishSelected(ItemSO item)
    {
        Player.InGameUI.HideCookUI();
        Player.FishStartEvent?.Invoke();
        Player.AnimatorComponent.SetBool(defaultAnimationHash, true);
        Player.StatManager.StatValues[StatType.Bored] =
            Mathf.Clamp(Player.StatManager.StatValues[StatType.Bored] + 1, 0, 100);
        Player.StatManager.StatValues[StatType.Tired] =
            Mathf.Clamp(Player.StatManager.StatValues[StatType.Tired] - 1, 0, 100);
    }

    public override void Exit()
    {
        base.Exit();
        Player.InputReader.OnEscapeEvent -= StateMachine.ResetToIdleState;
        Player.OnMiniGameEndEvent?.Invoke();
        Player.InGameUI.HideCookUI();
    }
}