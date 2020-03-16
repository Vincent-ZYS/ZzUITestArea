using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CircleImage : Image
{
    private int segments;//how many segments do the cirle have

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        segments = 100;

        //clear the current image
        vh.Clear();

        //acquire the rect information
        float rectWidth = rectTransform.rect.width;
        float rectHeight = rectTransform.rect.height;

        //acquire the current sprite uv information
        Vector4 uv = overrideSprite!=null?DataUtility.GetOuterUV(overrideSprite):Vector4.zero;
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        Vector2 uvCenter = new Vector2(uvWidth * 0.5f, uvHeight * 0.5f);
        Vector2 convertRatio = new Vector2(uvWidth / rectWidth, uvHeight / rectHeight);

        //calculate the radian and radius of the circle image
        float radian = (2 * Mathf.PI) / segments;
        float radius = rectWidth * 0.5f;

        UIVertex origin = new UIVertex();
        origin.color = color;
        origin.position = Vector3.zero;
        origin.uv0 = new Vector2(origin.position.x * convertRatio.x + uvCenter.x, origin.position.y * convertRatio.y + uvCenter.y);
        vh.AddVert(origin);

        int vertexCount = segments + 1;
        float currentRadian = 0;
        for(int i = 0; i < vertexCount; i++)
        {
            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;
            currentRadian += radian;

            UIVertex tempVertext = new UIVertex();
            tempVertext.color = color;
            tempVertext.position = new Vector2(x, y);
            tempVertext.uv0 = new Vector2(tempVertext.position.x * convertRatio.x + uvCenter.x, tempVertext.position.y * convertRatio.y + uvCenter.y);
            vh.AddVert(tempVertext);
        }

        int id = 1;
        for(int i = 0; i < segments; i++)
        {
            vh.AddTriangle(id, 0, id + 1);
            //vh.AddTriangle(0, id + 1, id);
            id++;
        }
    }
}
