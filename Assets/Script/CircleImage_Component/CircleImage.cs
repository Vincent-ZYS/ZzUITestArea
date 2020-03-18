using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CircleImage : Image
{
    [SerializeField]
    private float showPercent = 1;//show the percentage of the image fill amount
    [SerializeField]
    private int segments = 100;//how many segments do the cirle have

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        //clear the current image
        vh.Clear();

        //acquire the rect information
        float rectWidth = rectTransform.rect.width;
        float rectHeight = rectTransform.rect.height;
        int realSegments = (int)(segments * showPercent);

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
        //if want to make the center black shade, use the two line under
        //byte colorTemp = (byte)(255 * showPercent);
        //origin.color = new Color32(colorTemp, colorTemp, colorTemp, 255);
        //
        origin.color = color;
        origin.position = Vector2.zero;
        origin.position = new Vector2();
        origin.uv0 = new Vector2(origin.position.x * convertRatio.x + uvCenter.x, origin.position.y * convertRatio.y + uvCenter.y);
        vh.AddVert(origin);

        int vertexCount = realSegments + 1;//use the realSegment to control the percentage
        float currentRadian = 0;
        for(int i = 0; i < segments + 1; i++)//used vertexCount before
        {
            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;
            currentRadian += radian;

            UIVertex tempVertext = new UIVertex();
            if(i < vertexCount)
            {
                tempVertext.color = color;
            }else
            {
                tempVertext.color = new Color32(60, 60, 60, 255);
            }

            tempVertext.position = new Vector2(x, y);
            tempVertext.uv0 = new Vector2(tempVertext.position.x * convertRatio.x + uvCenter.x, tempVertext.position.y * convertRatio.y + uvCenter.y);
            vh.AddVert(tempVertext);
        }

        int id = 1;
        for(int i = 0; i < segments; i++)//use the realSegment to control the percentage//used realSegments before
        {
            vh.AddTriangle(id, 0, id + 1);
            //vh.AddTriangle(0, id + 1, id);
            id++;
        }
    }
}
