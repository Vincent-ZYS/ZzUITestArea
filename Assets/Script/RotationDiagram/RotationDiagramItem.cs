using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationDiagramItem : MonoBehaviour
{
    private Image _image;

    private Image image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();
            return _image;
        }
    }
    private RectTransform _rect;

    private RectTransform rect
    {
        get
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }

    public void SetSelfData(ItemPosData itemPosData)
    {
        rect.anchoredPosition = Vector2.right * itemPosData.X;
        rect.localScale = Vector3.one*itemPosData.ScaleTimes;
    }

    public void SetParent(Transform parentTf)
    {
        transform.SetParent(parentTf);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
