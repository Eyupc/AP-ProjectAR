using UnityEngine;

[CreateAssetMenu(fileName = "ShowMenuIcon", menuName = "QR Actions/Show Menu Icon")]
public class ShowMenuIcon : QRActionBase
{
    [SerializeField] private GameObject menuIconCanvasPrefab;
    public override string QRCodeText => "Restaurant";

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        if (menuIconCanvasPrefab != null)
        {

            var menuObj = Instantiate(menuIconCanvasPrefab);
            menuObj.SetActive(true);

        }
        else
        {
            Debug.LogError($"{nameof(ShowMenuIcon)}: Menu Icon Canvas Prefab is not assigned!");
        }
    }
}