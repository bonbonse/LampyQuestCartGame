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

    // возможные следующие карты
    private List<Cart> possibleNextCarts = new List<Cart>();
    // полный список действующих карт
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
         * ѕолучаем следующую карту по selectedVariant
         * ≈сли нужно обновить possible - обновл€ем и return
         * ≈сли их там много - Random
         * ≈сли там ни одной - берЄм из possible
         * ≈сли в possible ни одной - заполн€ем possible allCarts и берЄм ещЄ раз
         * ≈сли в allCarts ни одной - 
         * 
    */
    public void NextCart(AnswerVariants selectedVariant)
    {
        // берЄм карты из выбора ответа
        Cart nextCart;
        List<Cart> nextCarts = GetNextCarts(selectedVariant);
        // нужно обновл€ть карты, если поставлена галочка "обновлени€"
        bool hasUpdateAllCarts = HasUpdateAllCarts(selectedVariant);
        if (nextCarts.Count == 0)
        {
            // если в выборе нет следующих карт, удал€ем из possible и берЄм новые карты
            // UpdateCarts();
            DropThisCartFromPossibleCartList();
            nextCart = GetRandomCart(possibleNextCarts);
        }
        else
        {

            if (hasUpdateAllCarts)
            {
                // обновл€ем все карты - ничего не удал€ем
                UpdateCarts(nextCarts);
            }
            else
            {
                // если не обновл€ем карту, то нужно удалить карту из действующего списка
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
            // есть возможные карты
            if (possibleNextCarts.Contains(currentCart))
            {
                // эта карта входит в список карт - тогда удал€ем еЄ
                possibleNextCarts.Remove(currentCart);
                if (possibleNextCarts.Count - 1 == 0)
                {
                    // обновл€ем карты до полного списка
                    ClonePossibleCarts(allPossibleNextCarts);
                }
            }
        }
        if (possibleNextCarts.Count == 0)
        {
            // если после удалени€ не осталось карт - обновл€ем possible до all
            // @deprecated в теории, здесь оказатьс€ не должны
            Debug.LogWarning(" арточки будут повтор€тьс€!");
            UpdateCarts(allPossibleNextCarts);
        }
    }
    private void UpdateCarts(List<Cart> carts)
    {
        allPossibleNextCarts = carts;
        ClonePossibleCarts(allPossibleNextCarts);
    }
    // копирует all в possible
    private void ClonePossibleCarts(List<Cart> carts)
    {
        possibleNextCarts.Clear();
        for (int i = 0; i < carts.Count; i++)
        {
            possibleNextCarts.Add(carts[i]);
        }
    }

}
