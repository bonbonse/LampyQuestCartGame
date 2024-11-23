using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attribute", menuName = "Attribute", order = 51)]
[RequireComponent(typeof(Cart))]
public class Attribute : ScriptableObject
{
    public int changeHeroismAnswer1 = 0;
    public int changeHeroismAnswer2 = 0;
    public bool Dead = false;
}
