using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect", order = 51)]
public class Effect : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite icon;
}
