using UnityEngine;

[CreateAssetMenu(fileName = "ShowMenuIcon", menuName = "QR Actions/Show Menu Icon")]
public class ShowMenuIcon : QRActionBase
{
    [SerializeField] private GameObject menuIconCanvasPrefab;
    [SerializeField] private GameObject welcomeOverlayPrefab;
    public override string QRCodeText => "Restaurant";

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        if (menuIconCanvasPrefab != null && welcomeOverlayPrefab != null)
        {
            GameObject overlayObj = Instantiate(welcomeOverlayPrefab);
            overlayObj.GetComponent<StopStartCanvasHandler>().OnCloseClicked += SpawnMenuIcon;
        }
        else
        {
            Debug.LogError($"{nameof(ShowMenuIcon)}: Menu Icon Canvas Prefab is not assigned!");
        }
    }
    private void SpawnMenuIcon()
    {
        var menuObj = Instantiate(menuIconCanvasPrefab);
        menuObj.SetActive(true);
    }
}