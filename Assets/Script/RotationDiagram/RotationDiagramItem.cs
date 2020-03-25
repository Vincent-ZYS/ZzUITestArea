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

    public void SetParent(Transform parentTf)
    {
        transform.SetParent(parentTf);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
