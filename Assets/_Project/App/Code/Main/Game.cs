using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game instance;

    public static float speedText = 1;
    public float fastSpeedText = 0.05f;
    public float slowSpeedText = 0.1f;

    [SerializeField]
    private Cart currentCart;
    [SerializeField]
    private List<Cart> defaultNextCarts = new List<Cart>();
    // ���������� �����, ������� ��� ����
    private List<Cart> usedUniqueCarts = new List<Cart>();

    // ��������� ��������� �����
    private List<Cart> possibleNextCarts = new List<Cart>();
    // ������ ������ ����������� ����
    private List<Cart> allPossibleNextCarts = new List<Cart>();

    private void Start()
    {
        instance = this;
        speedText = slowSpeedText;
        AnswerButton.isDisabled = false;

        allPossibleNextCarts = defaultNextCarts;
        ClonePossibleCarts(allPossibleNextCarts);
        UI.instance.Init();
        UI.instance.UpdateUI(currentCart);
    }

    public void Answer(AnswerVariants selectedVariant)
    {
        Attribute attribute = currentCart.attribute;
        if (attribute != null)
        {
            int changeValue = GetEffectValue(selectedVariant, attribute);
            EffectManager.instance.ChangeHeroism(changeValue);
            Debug.Log(EffectManager.instance.GetHeroism());
            if (attribute.Dead)
            {
                SceneManager.LoadScene("Menu");
            }
        }
        NextCart(selectedVariant);
    }

    private int GetEffectValue(AnswerVariants selectedVariant, Attribute attribute)
    {
        return selectedVariant == AnswerVariants.Variant1 ? attribute.changeHeroismAnswer1 : attribute.changeHeroismAnswer2;
    }

    /*
         * �������� ��������� ����� �� selectedVariant
         * ���� ����� �������� possible - ��������� � return
         * ���� �� ��� ����� - Random
         * ���� ��� �� ����� - ���� �� possible
         * ���� � possible �� ����� - ��������� possible allCarts � ���� ��� ���
         * 
         * � possibleCarts �� ������ ���� ��� ����� �����������. ������������ ����� �� ������ ���� ����������
         * 
    */
    public void NextCart(AnswerVariants selectedVariant)
    {
        // ���� ����� �� ������ ������
        Cart nextCart;
        List<Cart> nextCarts = GetNextCarts(selectedVariant);
        // ����� ��������� �����, ���� ���������� ������� "����������"
        bool hasUpdateAllCarts = HasUpdateAllCarts(selectedVariant);
        if (nextCarts.Count == 0)
        {
            // ���� � ������ ��� ��������� ����, ������� �� possible � ���� ����� �����
            // UpdateCarts();
            DropThisCartFromPossibleCartList();
            nextCart = GetRandomCart(possibleNextCarts);
        }
        else
        {
            // ����� ���������� �� possible ������
            if (hasUpdateAllCarts)
            {
                // ��������� ��� ����� - ������ �� �������
                UpdateCarts(nextCarts);
            }
            else
            {
                // ���� �� ��������� �����, �� ����� ������� ����� �� ������������ ������
                DropThisCartFromPossibleCartList();
            }
            nextCart = GetRandomCart(nextCarts);
        }
        currentCart = nextCart;
        UI.instance.UpdateUI(currentCart);
    }

    private bool HasUpdateAllCarts(AnswerVariants selectedVariant)
    {
        if (selectedVariant == AnswerVariants.Variant1)
        {
            return currentCart.needUpdateCartsVariant1;
        }
        else
        {
            return currentCart.needUpdateCartsVariant2;
        }
    }

    private List<Cart> GetNextCarts(AnswerVariants selectedVariant)
    {
        List<Cart> carts = selectedVariant == AnswerVariants.Variant1
            ? currentCart.nextCartsVariant1
            : currentCart.nextCartsVariant2;
        return carts;
    }
    private Cart GetRandomCart(List<Cart> carts)
    {
        Cart result;
        if (carts.Count == 1)
        {
            result = carts[0];
        }
        int randomCartNumber = Random.Range(0, carts.Count);
        if (carts[randomCartNumber].isShowOnce)
        {
            usedUniqueCarts.Add(carts[randomCartNumber]);
        }
        return carts[randomCartNumber];
    }
    private void DropThisCartFromPossibleCartList()
    {
        if (possibleNextCarts.Count > 0)
        {
            // ���� ��������� �����
            if (possibleNextCarts.Contains(currentCart))
            {
                // ��� ����� ������ � ������ ���� - ����� ������� �
                possibleNextCarts.Remove(currentCart);
                if (possibleNextCarts.Count - 1 == 0)
                {
                    // ��������� ����� �� ������� ������
                    ClonePossibleCarts(allPossibleNextCarts);
                }
            }
        }
        if (possibleNextCarts.Count == 0)
        {
            // ���� ����� �������� �� �������� ���� - ��������� possible �� all
            // @deprecated � ������, ����� ��������� �� ������
            Debug.LogWarning("�������� ����� �����������!");
            UpdateCarts(allPossibleNextCarts);
        }
    }
    private void UpdateCarts(List<Cart> carts)
    {
        allPossibleNextCarts = carts;
        ClonePossibleCarts(allPossibleNextCarts);
    }
    // �������� all � possible
    private void ClonePossibleCarts(List<Cart> carts)
    {
        possibleNextCarts.Clear();
        for (int i = 0; i < carts.Count; i++)
        {
            if (carts[i].isShowOnce)
            {
                if (!MeetingUniquaCartFirstTime(carts[i])){
                    // ����������� �� � ������ ��� ���������� �����
                    continue;
                }
            }
            possibleNextCarts.Add(carts[i]);
        }
    }
    // ���������, ���� �� ���������� ����� ������������
    private bool MeetingUniquaCartFirstTime(Cart cart)
    {
        if (usedUniqueCarts.Find(c => c.question == cart.question))
        {
            // �������
            return false;
        }
        return true;
    }

    public void UpdateSpeedText(bool isSpeedUp)
    {
        if (isSpeedUp)
        {
            speedText = fastSpeedText;
        }
        else
        {
            speedText = slowSpeedText;
        }
    }
}
