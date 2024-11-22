using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cart", menuName = "New cart", order = 51)]
public class Cart : ScriptableObject
{
    public string question;
    public string answerVariant1;
    public bool needUpdateCartsVariant1 = false;
    public string answerVariant2;
    public bool needUpdateCartsVariant2 = false;
    public List<Cart> nextCartsVariant1;
    public List<Cart> nextCartsVariant2;
    public string nickname = "Событие";
    public Sprite ava;
    public bool isShowOnce = false;
}
