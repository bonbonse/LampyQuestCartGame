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
    // уникальные карты, которые уже были
    private List<Cart> usedUniqueCarts = new List<Cart>();

    // возможные следующие карты
    private List<Cart> possibleNextCarts = new List<Cart>();
    // полный список действующих карт
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
         * Получаем следующую карту по selectedVariant
         * Если нужно обновить possible - обновляем и return
         * Если их там много - Random
         * Если там ни одной - берём из possible
         * Если в possible ни одной - заполняем possible allCarts и берём ещё раз
         * 
         * в possibleCarts не должны быть все карты уникальными. Неуникальные карты не должны быть тупиковыми
         * 
    */
    public void NextCart(AnswerVariants selectedVariant)
    {
        // берём карты из выбора ответа
        Cart nextCart;
        List<Cart> nextCarts = GetNextCarts(selectedVariant);
        // нужно обновлять карты, если поставлена галочка "обновления"
        bool hasUpdateAllCarts = HasUpdateAllCarts(selectedVariant);
        if (nextCarts.Count == 0)
        {
            // если в выборе нет следующих карт, удаляем из possible и берём новые карты
            // UpdateCarts();
            DropThisCartFromPossibleCartList();
            nextCart = GetRandomCart(possibleNextCarts);
        }
        else
        {
            // карты выбираются из possible набора
            if (hasUpdateAllCarts)
            {
                // обновляем все карты - ничего не удаляем
                UpdateCarts(nextCarts);
            }
            else
            {
                // если не обновляем карту, то нужно удалить карту из действующего списка
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
            // есть возможные карты
            if (possibleNextCarts.Contains(currentCart))
            {
                // эта карта входит в список карт - тогда удаляем её
                possibleNextCarts.Remove(currentCart);
                if (possibleNextCarts.Count - 1 == 0)
                {
                    // обновляем карты до полного списка
                    ClonePossibleCarts(allPossibleNextCarts);
                }
            }
        }
        if (possibleNextCarts.Count == 0)
        {
            // если после удаления не осталось карт - обновляем possible до all
            // @deprecated в теории, здесь оказаться не должны
            Debug.LogWarning("Карточки будут повторяться!");
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
            if (carts[i].isShowOnce)
            {
                if (!MeetingUniquaCartFirstTime(carts[i])){
                    // встречается не в первый раз уникальная карта
                    continue;
                }
            }
            possibleNextCarts.Add(carts[i]);
        }
    }
    // проверяет, была ли уникальная карта использована
    private bool MeetingUniquaCartFirstTime(Cart cart)
    {
        if (usedUniqueCarts.Find(c => c.question == cart.question))
        {
            // впервые
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
