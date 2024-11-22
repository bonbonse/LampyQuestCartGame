using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;

    [SerializeField]
    private Cart currentCart;
    [SerializeField]
    private List<Cart> defaultNextCarts = new List<Cart>();

    // ��������� ��������� �����
    private List<Cart> possibleNextCarts = new List<Cart>();
    // ������ ������ ����������� ����
    private List<Cart> allPossibleNextCarts = new List<Cart>();

    private void Start()
    {
        instance = this;

        allPossibleNextCarts = defaultNextCarts;
        ClonePossibleCarts(allPossibleNextCarts);
        UI.instance.Init();
        UI.instance.UpdateUI(currentCart);
    }

    /*
         * �������� ��������� ����� �� selectedVariant
         * ���� ����� �������� possible - ��������� � return
         * ���� �� ��� ����� - Random
         * ���� ��� �� ����� - ���� �� possible
         * ���� � possible �� ����� - ��������� possible allCarts � ���� ��� ���
         * ���� � allCarts �� ����� - 
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
        Debug.Log(possibleNextCarts.Count);
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
            possibleNextCarts.Add(carts[i]);
        }
    }

}
