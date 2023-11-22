using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Image manaImage;
    [SerializeField] Text manaText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        manaText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manaText.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        //마나 UI관리
        if (manaImage.fillAmount != GameManager.Instance.moneyManager.mana)
        {
            manaImage.fillAmount = Mathf.Lerp(manaImage.fillAmount, GameManager.Instance.moneyManager.mana / GameManager.Instance.moneyManager.maxMana, Time.deltaTime * 4); //부드러운 증감
        }
    }
    public void TextUpdate(float _mana, int _maxMana)
    {
        manaText.text = string.Format(_mana + " / " + _maxMana);
    }
}
