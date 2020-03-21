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

    private List<Vector3> _vertexList = new List<Vector3>();

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
        //origin.position = Vector2.zero;
        Vector2 orignPos = new Vector2((0.5f - rectTransform.pivot.x)*rectWidth,(0.5f - rectTransform.pivot.y)*rectHeight);
        Vector2 vertPos = Vector2.zero;
        origin.position = orignPos;
        origin.uv0 = new Vector2(vertPos.x * convertRatio.x + uvCenter.x, vertPos.y * convertRatio.y + uvCenter.y); 
        vh.AddVert(origin);

        int vertexCount = realSegments + 1;//use the realSegment to control the percentage
        float currentRadian = 0;
        Vector2 tempPos;
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
            tempPos = new Vector2(x, y);
            tempVertext.position = tempPos + orignPos;
            tempVertext.uv0 = new Vector2(tempPos.x * convertRatio.x + uvCenter.x, tempPos.y * convertRatio.y + uvCenter.y);
            _vertexList.Add(tempPos + orignPos);
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

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);
        return GetCrpssPointNum(localPoint, _vertexList) % 2 == 1;
    }

    private int GetCrpssPointNum(Vector2 localPoint, List<Vector3> vertexList)
    {
        Vector3 vertex1 = new Vector3();
        Vector3 vertex2 = new Vector3();
        int verCount = vertexList.Count;
        int pointNum = 0;

        for(int i = 0; i < verCount; i++)
        {
            vertex1 = vertexList[i];
            vertex2 = vertexList[(i + 1) % verCount];
            if(isRayInRange(localPoint,vertex1,vertex2))
            {
                if (localPoint.x < GetX(vertex1,vertex2,localPoint.y))
                {
                    pointNum++;
                }
            }
        }
        return pointNum;
    }

    private bool isRayInRange(Vector2 localPoint,Vector3 vertex1,Vector3 vertex2)
    {
        if(vertex1.y > vertex2.y)
        {
            return localPoint.y < vertex1.y && localPoint.y > vertex2.y;
        }else
        {
            return localPoint.y < vertex2.y && localPoint.y > vertex1.y;
        }
    }

    private float GetX(Vector3 vertex1,Vector3 vertex2,float y)
    {
        float k = (vertex1.y - vertex2.y) / (vertex1.x - vertex2.x);
        return vertex1.x + (y - vertex1.y) / k;
    }

    //private UIVertex GetUIVertex(Color32 col, Vector3 pos, Vector2 uvPos, Vector2 uvCenter, Vector2 uvScale)
    //{
    //    UIVertex tempVertex = new UIVertex();
    //    tempVertex.color = col;
    //    tempVertex.position = pos;
    //    tempVertex.uv0 = new Vector2(uvPos.x * uvScale.x + uvCenter.x, uvPos.y * uvScale.y + uvCenter.y);
    //    return tempVertex;
    //}
}
