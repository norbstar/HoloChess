using UnityEngine;

using TMPro;

public class PointManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI textUI;

    public string Text { get { return textUI.text; } set  { textUI.text = value; } }
}