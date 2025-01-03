using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "SpawnAvatarAction", menuName = "QR Actions/Spawn Avatar")]
public class SpawnAvatarAction : QRActionBase
{
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private GameObject avatarArabicPrefab;
    [SerializeField] private GameObject avatarWomanPrefab;
    [SerializeField] private GameObject avatarArabicWomanPrefab;
    [SerializeField] private GameObject welcomeOverlayPrefab;
    [SerializeField] private GameObject welcomeOverlayPrefabArabic;

    public override string QRCodeText => "Poem";
    private Language language = new Language();
    private Character character = new Character();

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            GameObject overlayObj = Instantiate(UserSystemManager.Language == Language.Dutch ? welcomeOverlayPrefab : welcomeOverlayPrefabArabic);
            overlayObj.GetComponent<StopStartCanvasHandler>().OnCloseClicked += SpawnAvatar;
            language = UserSystemManager.Language;
            character = UserSystemManager.Character;
        }
    }

    private void SpawnAvatar()
    {
        Camera camera = Camera.main;
        Vector3 userPosition = camera.transform.position;
        Vector3 rightDirection = camera.transform.right;
        Vector3 spawnPosition = userPosition + rightDirection * -2f;
        spawnPosition.y -= 1.7f;
        GameObject avatar = null;
        if (character == Character.Man)
        {
            if (language == Language.Dutch) { avatar = Instantiate(avatarPrefab, spawnPosition, Quaternion.identity); }
            else { avatar = Instantiate(avatarArabicPrefab, spawnPosition, Quaternion.identity); }
        }
        else
        {
            if (language == Language.Dutch) { avatar = Instantiate(avatarWomanPrefab, spawnPosition, Quaternion.identity); }
            else { avatar = Instantiate(avatarArabicWomanPrefab, spawnPosition, Quaternion.identity); }
        }

        if (avatar != null)
        {
            avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
        }
    }
}