using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolsManager : MonoBehaviour
{
    public static ToolsManager instance;

    void Awake() => instance = (instance == null) ? this : instance;

    [Header("Ferramentas")]
    public List<Tool> tools = new List<Tool>();
}

[System.Serializable]
public class Tool{
    public Tools tool;
    public int animation;
    public UnityEvent event_;
}
