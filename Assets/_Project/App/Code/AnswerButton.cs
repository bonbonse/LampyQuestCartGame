using UnityEngine;

public enum AnswerVariants
{
    Variant1, Variant2
}
public class AnswerButton : MonoBehaviour
{
    public AnswerVariants variant;
    public static bool isDisabled = false;
    public void ButtonClick()
    {
        if (!isDisabled)
        {
            Game.instance.NextCart(variant);
        }
    }
}
