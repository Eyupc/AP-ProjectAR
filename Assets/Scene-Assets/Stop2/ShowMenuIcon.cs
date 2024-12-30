using UnityEngine;

[CreateAssetMenu(fileName = "ShowMenuIcon", menuName = "QR Actions/Show Menu Icon")]
public class ShowMenuIcon : QRActionBase
{
    [SerializeField] private GameObject menuIconCanvasPrefab;
    [SerializeField] private GameObject welcomeOverlayPrefab;
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private GameObject avatarArabicPrefab;
    private Language language = new Language();
    private Character character = new Character();
    public override string QRCodeText => "Restaurant";

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        if (menuIconCanvasPrefab != null && welcomeOverlayPrefab != null && avatarPrefab != null && avatarArabicPrefab != null)
        {
            GameObject overlayObj = Instantiate(welcomeOverlayPrefab);
            overlayObj.GetComponent<StopStartCanvasHandler>().OnCloseClicked += SpawnMenuIcon;
        }
        else
        {
            Debug.LogError($"{nameof(ShowMenuIcon)}: Menu Icon Canvas Prefab is not assigned or avatar prefab is empty!");
        }
    }
    private void SpawnMenuIcon()
    {
        var menuObj = Instantiate(menuIconCanvasPrefab);
        menuObj.SetActive(true);
        SpawnAvatar();
    }

    private void SpawnAvatar() 
    {
        language = UserSystemManager.Language;
        character = UserSystemManager.Character;
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 userPosition = mainCamera.transform.position;
            Vector3 rightDirection = mainCamera.transform.right;
            Vector3 spawnPosition = userPosition + rightDirection * -2f;
            spawnPosition.y -= 1.7f;
            GameObject avatar = null;
            if (language == Language.Dutch) { avatar = Instantiate(avatarPrefab, spawnPosition, Quaternion.identity); }
            else { avatar = Instantiate(avatarArabicPrefab, spawnPosition, Quaternion.identity); }

            if (avatar != null)
            {
                avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
            }
        }
    }
}