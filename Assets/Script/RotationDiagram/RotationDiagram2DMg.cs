using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        CalculateData();
        SetItemData();
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
            itemTemp.AddMoveListener(MoveXChange);
            _itemList.Add(itemTemp);
        }
        Destroy(template);
    }

    private void MoveXChange(float offsetX)
    {
        int leftOrRightMove = offsetX > 0 ? 1 : -1;
        foreach(RotationDiagramItem item in _itemList)
        {
            item.ChangePosId(leftOrRightMove, _itemList.Count);
        }
        for(int i = 0; i < _posDataList.Count; i++)
        {
            _itemList[i].SetSelfData(_posDataList[_itemList[i].posId]);
        }
    }

    private void CalculateData()
    {
        List<ItemData> itemDatas = new List<ItemData>();

        float length = (ItemSize.x + itemOffset) * _itemList.Count;
        float ratioOffset = 1.0f / (float)_itemList.Count;
        float ratio = 0f;
        for(int i = 0; i < _itemList.Count; i++)
        {
            ItemData itemData = new ItemData();
            itemData.PosId = i;
            _itemList[i].posId = i;
            itemDatas.Add(itemData);

            ItemPosData itemPosData = new ItemPosData();
            itemPosData.X = GetX(ratio, length);
            itemPosData.ScaleTimes = GetScaleTimes(ratio, maxSize, minSize);
            ratio += ratioOffset;
            _posDataList.Add(itemPosData);
        }
        itemDatas = itemDatas.OrderBy(n => _posDataList[n.PosId].ScaleTimes).ToList();//ascending the order by scale times
        for(int i = 0; i < itemDatas.Count; i++)
        {
            _posDataList[itemDatas[i].PosId].Order = i;
        }
    }

    private void SetItemData()
    {
        for(int i = 0; i < _posDataList.Count; i++)
        {
            _itemList[i].SetSelfData(_posDataList[i]);
        }
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
}

public class ItemPosData
{
    public float X;
    public float ScaleTimes;
    public int Order;
}

public struct ItemData
{
    public int PosId;
    public int OrderId;
}
