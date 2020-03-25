﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationDiagram2DMg : MonoBehaviour
{
    public Vector2 ItemSize;
    public Sprite[] ItemSprite;
    public float itemOffset;
    public float maxSize;
    public float minSize;
    private List<RotationDiagramItem> _itemList = new List<RotationDiagramItem>();
    private List<ItemPosData> _posDataList = new List<ItemPosData>();

    private void Start()
    {
        CreateItem();
    }

    private GameObject CreateTemplate()
    {
        GameObject item = new GameObject("Template");
        item.AddComponent<RectTransform>().sizeDelta = ItemSize;
        item.AddComponent<Image>();
        item.AddComponent<RotationDiagramItem>();
        return item;
    }

    private void CreateItem()
    {
        GameObject template = CreateTemplate();
        RotationDiagramItem itemTemp = null;
        foreach(Sprite sprite in ItemSprite)
        {
            itemTemp = Instantiate(template).GetComponent<RotationDiagramItem>();
            itemTemp.SetParent(transform);
            itemTemp.SetSprite(sprite);
            _itemList.Add(itemTemp);
        }
        Destroy(template);
    }

    private void CalculateData()
    {
        float length = (ItemSize.x + itemOffset) * _itemList.Count;
        float ratioOffset = 1 / (float)_itemList.Count;
    }

    private float GetX(float ratio, float length)
    {
        if(ratio > 1 || ratio < 0)
        {
            Debug.Log("The oral ratio is Error!");
            return 0f;
        }
        if (ratio >= 0f && ratio < 0.25f)
        {
            return ratio * length;
        }
        else if (ratio >= 0.25f && ratio <= 0.75f)
        {
            return length * (0.5f - ratio);
        }
        else
        {
            return length * (ratio - 1);
        }
    }

    private float GetScaleTimes(float ratio, float maxSz, float minSz)
    {
        float scaleOffset = (maxSz - minSz) / 0.5f;
        if(ratio > 1 || ratio < 0 )
        {
            Debug.Log("The oral ratio is Error!");
            return 0f;
        }
        if(ratio < 0.5f)
        {
            return maxSz - ratio * scaleOffset;
        }else
        {
            return maxSz - (1f - ratio) * scaleOffset;
        }
    }

    public struct ItemPosData
    {
        private float X;
        private float ScaleTimes;
    }
}