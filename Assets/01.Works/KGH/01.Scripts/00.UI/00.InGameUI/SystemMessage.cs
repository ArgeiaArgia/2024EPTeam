using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SystemMessage : ToolkitParents
{
    private ScrollView _systemMessageContainer;
    private Label _systemMessageLabel;

    protected override void OnEnable()
    {
        base.OnEnable();
        _systemMessageContainer = root.Q<ScrollView>("SystemMessageContainer");
        _systemMessageLabel = root.Q<Label>("SystemMessage");
        _systemMessageLabel.text = "좌클릭으로 이동, \n우클릭 혹은 더블클릭으로 행동, \n행동 중 ESC키로 취소합니다.\nEsc를 꾹 누르면 게임이 꺼집니다.......\n";
    }

    public void OnItemAdded(ItemSO item)
    {
        if (item == null)
        {
            _systemMessageLabel.text += "이런! 아무것도 못 얻었습니다";
            return;
        }

        _systemMessageLabel.text += $"{item.itemName}을/를 획득하였습니다.\n";
        ;
        _systemMessageContainer.scrollOffset =
            _systemMessageLabel.layout.max - _systemMessageContainer.contentViewport.layout.size;
    }

    public void ShowMessage(string text)
    {
        _systemMessageLabel.text += text + "\n";
        _systemMessageContainer.scrollOffset =
            _systemMessageLabel.layout.max - _systemMessageContainer.contentViewport.layout.size;
    }

    public void ShowMessages(string[] texts)
    {
        StopAllCoroutines();
        if (texts == null) return;
        StartCoroutine(ShowMessageWithTerm(texts));
    }

    IEnumerator ShowMessageWithTerm(string[] texts)
    {
        foreach (var text in texts)
        {
            _systemMessageLabel.text += text + "\n";
            _systemMessageContainer.scrollOffset =
                _systemMessageLabel.layout.max - _systemMessageContainer.contentViewport.layout.size;
            yield return new WaitForSeconds(1f);
        }
    }
}