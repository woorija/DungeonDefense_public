using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Image mana_image;
    [SerializeField] Text mana_text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mana_text.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mana_text.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        //마나 UI관리
        if (mana_image.fillAmount != GameManager.Instance.Money_manager.mana)
        {
            mana_image.fillAmount = Mathf.Lerp(mana_image.fillAmount, GameManager.Instance.Money_manager.mana / GameManager.Instance.Money_manager.max_mana, Time.deltaTime * 4); //부드러운 증감
        }
    }
    public void TextUpdate(float _mana, int _maxmana)
    {
        mana_text.text = string.Format(_mana + " / " + _maxmana);
    }
}
