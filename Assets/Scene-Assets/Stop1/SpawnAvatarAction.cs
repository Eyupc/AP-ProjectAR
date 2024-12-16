using UnityEngine;


[CreateAssetMenu(fileName = "SpawnAvatarAction", menuName = "QR Actions/Spawn Avatar")]
public class SpawnAvatarAction : QRActionBase
{
    [SerializeField] private GameObject avatarPrefab;
    public override string QRCodeText => "Avatar";

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 userPosition = mainCamera.transform.position;
            Vector3 rightDirection = mainCamera.transform.right;
            Vector3 spawnPosition = userPosition + rightDirection * -2f;
            spawnPosition.y -= 1.7f;

            GameObject avatar = Instantiate(avatarPrefab, spawnPosition, Quaternion.identity);
            avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
        }
    }
}