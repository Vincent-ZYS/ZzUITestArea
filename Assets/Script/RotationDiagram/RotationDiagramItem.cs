using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class RotationDiagramItem : MonoBehaviour,IDragHandler,IEndDragHandler
{
    public int posId;

    private float _animTime = 1.0f;

    private float _offsetX = 0f;

    private Action<float> _moveAction;

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
        rect.DOAnchorPos(Vector2.right * itemPosData.X, _animTime);
        rect.DOScale(Vector3.one*itemPosData.ScaleTimes,_animTime);
        //rect.DOSizeDelta(Vector3.one * itemPosData.ScaleTimes, _animTime,false);
        //rect.anchoredPosition = Vector2.right * itemPosData.X;
        //rect.localScale = Vector3.one*itemPosData.ScaleTimes;
        transform.SetSiblingIndex(itemPosData.Order);
    }

    public void SetParent(Transform parentTf)
    {
        transform.SetParent(parentTf);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void ChangePosId(int moveRorL,int itemCount)
    {
        int id = posId;
        id += moveRorL;
        if(id<0)
        {
            id += itemCount;
        }
        posId = id % itemCount;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _moveAction(_offsetX);
        _offsetX = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _offsetX += eventData.delta.x;
    }

    public void AddMoveListener(Action<float> onMove)
    {
        _moveAction = onMove;
    }
}
