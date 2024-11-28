using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoCHul : MonoBehaviour
{
    [SerializeField] private endingkdfnlk endingkdfn;
    [SerializeField] private Credit Credit;

    public void ddd()
    {
        endingkdfn.OnAnimationEnd();
    }
    public void cre()
    {
        Credit.OnCredit();
    }
}
