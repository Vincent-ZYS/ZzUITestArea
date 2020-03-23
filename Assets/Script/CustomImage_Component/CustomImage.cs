using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomImage : Image
{
    private PolygonCollider2D _polygon2D;

    private PolygonCollider2D polygon2D
    {
        get
        {
            if(_polygon2D == null)
            {
                _polygon2D = GetComponent<PolygonCollider2D>();
            }
            return _polygon2D;
        }
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 point;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);
        return polygon2D.OverlapPoint(point);
    }
}
