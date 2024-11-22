using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KeyInput : MonoBehaviour
{
    public List<Image> keyTiles = new List<Image>();
    private TextMeshProUGUI keyText;
    public void KeyRamdom()
    {
        try
        {
            if (keyTiles == null)
                throw new ArgumentNullException(nameof(keyTiles));
        }
        catch (ArgumentNullException ex)
        {
            return;
        }

        foreach (var VARIABLE in keyTiles)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        int ran = Random.Range(0, keyTiles.Count);
        keyTiles[ran].gameObject.SetActive(true);
        keyText = keyTiles[ran].GetComponentInChildren<TextMeshProUGUI>();
        CookManager.instance.key = keyText.text;
    }
}
