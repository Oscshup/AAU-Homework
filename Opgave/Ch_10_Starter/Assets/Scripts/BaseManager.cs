using UnityEngine;

public abstract class BaseManager
{
    // 2
    protected string _state = "Manager is not initialized...";
    public abstract string State { get; set; }
    // 3
    public abstract void Initialize();
}
