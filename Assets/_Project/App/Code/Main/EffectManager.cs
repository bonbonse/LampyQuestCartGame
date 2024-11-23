using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private int heroism;

    private void Awake()
    {
        instance = this;
    }
    public int GetHeroism()
    {
        return heroism;
    }
    public void ChangeHeroism(int value)
    {
        heroism = value;
    }
}
