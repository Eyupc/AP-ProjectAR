using System.Collections.Generic;
using UnityEngine;
public class QRActionRegistry : MonoBehaviour
{
    [SerializeField] private List<QRActionBase> registeredActions;
    private Dictionary<string, QRActionBase> actionLookup = new Dictionary<string, QRActionBase>();

    private void Awake()
    {
        foreach (var action in registeredActions)
        {
            actionLookup[action.QRCodeText] = action;
        }
    }

    public bool TryGetAction(string qrText, out QRActionBase action)
    {
        return actionLookup.TryGetValue(qrText, out action);
    }
}