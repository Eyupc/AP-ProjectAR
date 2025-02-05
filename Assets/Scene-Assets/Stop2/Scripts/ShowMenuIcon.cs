using UnityEngine;

[CreateAssetMenu(fileName = "ShowMenuIcon", menuName = "QR Actions/Show Menu Icon & Spawn Avatar")]
public class ShowMenuIcon : QRActionBase
{
    [SerializeField] private GameObject menuIconCanvasPrefab;
    [SerializeField] private GameObject welcomeOverlayPrefab;
    [SerializeField] private GameObject welcomeOverlayPrefabArabic;
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private GameObject avatarArabicPrefab;
    [SerializeField] private GameObject avatarWomanPrefab;
    [SerializeField] private GameObject avatarWomanArabicPrefab;
    private Language language = new Language();
    private Character character = new Character();
    public override string QRCodeText => "Restaurant";

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        if (menuIconCanvasPrefab != null && welcomeOverlayPrefab != null && avatarPrefab != null && avatarArabicPrefab != null)
        {
            GameObject overlayObj = Instantiate(UserSystemManager.Language == Language.Dutch ? welcomeOverlayPrefab : welcomeOverlayPrefabArabic);
            overlayObj.GetComponent<StopStartCanvasHandler>().OnCloseClicked += SpawnMenuIcon;
        }
        else
        {
            Debug.LogError($"{nameof(ShowMenuIcon)}: Menu Icon Canvas Prefab is not assigned or avatar prefab is empty!");
        }
    }
    private void SpawnMenuIcon()
    {
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
            if (language == Language.Dutch)
            {
                if (character == Character.Man) { avatar = Instantiate(avatarPrefab, spawnPosition, Quaternion.identity); }
                else { avatar = Instantiate(avatarWomanPrefab, spawnPosition, Quaternion.identity); }
            }
            else
            {
                if (character == Character.Man) { avatar = Instantiate(avatarArabicPrefab, spawnPosition, Quaternion.identity); }
                else { avatar = Instantiate(avatarWomanArabicPrefab, spawnPosition, Quaternion.identity); }
            }

            avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
            avatar.GetComponent<PlayPoem>().OnPoemEnd += () =>
            {
                var menuObj = Instantiate(menuIconCanvasPrefab);
                menuObj.SetActive(true);
            };
            if (avatar != null)
            {
                avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
            }
        }
    }
}