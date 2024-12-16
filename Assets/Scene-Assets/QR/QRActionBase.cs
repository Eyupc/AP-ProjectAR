using UnityEngine;
public abstract class QRActionBase : ScriptableObject
{
    public abstract string QRCodeText { get; }
    public abstract void Execute(Vector3 position, Quaternion rotation);
}