using UnityEngine;

[CreateAssetMenu(fileName = "SpawnAvatarAction", menuName = "QR Actions/Spawn Avatar")]
public class SpawnAvatarAction : QRActionBase
{
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private GameObject welcomeOverlayPrefab;
    public override string QRCodeText => "Poem";

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            GameObject overlayObj = Instantiate(welcomeOverlayPrefab);
            overlayObj.GetComponent<StopStartCanvasHandler>().OnCloseClicked += SpawnAvatar;

        }
    }

    private void SpawnAvatar()
    {
        Camera camera = Camera.main;
        Vector3 userPosition = camera.transform.position;
        Vector3 rightDirection = camera.transform.right;
        Vector3 spawnPosition = userPosition + rightDirection * -2f;
        spawnPosition.y -= 1.7f;
        GameObject avatar = Instantiate(avatarPrefab, spawnPosition, Quaternion.identity);
        avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
    }
}