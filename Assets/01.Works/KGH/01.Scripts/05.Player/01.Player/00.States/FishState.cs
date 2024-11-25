using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState : PlayerState
{
    private bool _isWaiting;
    private bool _isPulling;

    int pullHash = Animator.StringToHash("Pull");
    int biteHash = Animator.StringToHash("Bite");

    public FishState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        defaultAnimationHash = Animator.StringToHash("Fishing");
    }

    public override void Enter()
    {
        base.Enter();
        Player.OnMiniGameStartEvent?.Invoke();
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;
        _isWaiting = true;
        _isPulling = false;
        Player.CoroutineStarter(Waiting());
    }

    private IEnumerator Waiting()
    {
        if (!_isWaiting || _isPulling || StateMachine.CurrentState != this) yield break;
        var percentage = Random.Range(0, 100);
        yield return new WaitForSeconds(1f);
        if (percentage >= Player.CatchPercentage)
        {
            _isWaiting = false;
            Player.CoroutineStopper(Waiting());
            Player.CoroutineStarter(FishHit());
        }
        else
        {
            Player.CoroutineStarter(Waiting());
        }
    }

    private IEnumerator FishHit()
    {
        if(StateMachine.CurrentState != this) yield break;
        Player.AnimatorComponent.SetBool(defaultAnimationHash, false);
        Player.AnimatorComponent.SetBool(biteHash, true);
        Player.InputReader.OnEscapeEvent -= StateMachine.ResetToIdleState;
        Player.InputReader.OnTriggerEvent += FishCaught;
        Player.InGameUI.ShowFishLabel();
        yield return new WaitForSeconds(Player.FishWaitingTime);
        if(_isPulling) yield break;
        Player.InGameUI.HideFishLabel();
        Player.InputReader.OnTriggerEvent -= FishCaught;
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;
        _isWaiting = true;
        Player.AnimatorComponent.SetBool(defaultAnimationHash, true);
        Player.AnimatorComponent.SetBool(biteHash, false);
        Player.CoroutineStarter(Waiting());
    }

    private void FishCaught()
    {
        if(StateMachine.CurrentState != this) return;
        _isPulling = true;
        Player.InGameUI.HideFishLabel();
        Player.CoroutineStopper(FishHit());
        Player.CoroutineStopper(Waiting());
        Player.AnimatorComponent.SetBool(defaultAnimationHash, false);
        Player.AnimatorComponent.SetBool(biteHash, false);
        Player.AnimatorComponent.SetBool(pullHash, true);
        Player.InputReader.OnTriggerEvent -= FishCaught;
        Player.FishMiniGameUI.EnableUI();
    }

    public override void Exit()
    {
        base.Exit();
        Player.AnimatorComponent.SetBool(biteHash, false);
        Player.AnimatorComponent.SetBool(pullHash, false);
        Player.OnMiniGameEndEvent?.Invoke();
        Player.InputReader.OnEscapeEvent -= StateMachine.ResetToIdleState;
        Player.InputReader.OnTriggerEvent -= FishCaught;
        Player.InGameUI.HideFishLabel();
        Player.CoroutineStopper(Waiting());
        Player.CoroutineStopper(FishHit());
        _isPulling = false;
        _isWaiting = false;
    }
}