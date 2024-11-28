using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : PlayerState
{
    private float _sleepTime = 0;

    public SleepState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        defaultAnimationHash = Animator.StringToHash("Sleep");
    }

    public override void Enter()
    {
        base.Enter();
        Player.OnMiniGameStartEvent?.Invoke();
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;
        Player.InGameUI.ShowSleepLabel();

        _sleepTime = 0;
    }

    public override void Update()
    {
        base.Update();
        _sleepTime += Time.deltaTime;
        if (_sleepTime >= Player.SleepTerm)
        {
            Player.StatManager.StatValues[StatType.Tired] =
                Mathf.Clamp(Player.StatManager.StatValues[StatType.Tired] + Player.SleepGetOver, 0, 100);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Player.InputReader.OnEscapeEvent -= StateMachine.ResetToIdleState;
        Player.OnMiniGameEndEvent?.Invoke();
        Player.InGameUI.HideSleepLabel();
    }
}