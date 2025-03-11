using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualizationNode : MonoBehaviour
{
    private const string heightFormat = "Height : {0}";
    private const string keyFormat = "Key : {0}";
    private const string valueFormat = "Value : {0}";

    private int key;
    private int value;
    private int height;

    public int Key
    {
        get
        {
            return key;
        }
        set
        {
            key = value;
            keyText.text = string.Format(keyFormat, key);
        }
    }

    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            valueText.text = string.Format(valueFormat, this.value);
        }
    }
    public int Height
    {
        get
        {
            return height;
        }
        set
        {
            height = value;
            heightText.text = string.Format(heightFormat, height);
        }
    }

    [field: SerializeField]
    public float LeftWidth { get; set; } = 0f;
    [field: SerializeField]
    public float RightWidth { get; set; } = 0f;

    public float FullWidth => LeftWidth + RightWidth;

    [SerializeField]
    private TextMeshPro heightText;
    [SerializeField]
    private TextMeshPro keyText;
    [SerializeField]
    private TextMeshPro valueText;
}
