using UnityEngine;


[CreateAssetMenu(fileName = "SpawnAvatarActionStop3", menuName = "QR Actions/Spawn Avatar in stop3")]
public class SpawnAvatarActionStop3 : QRActionBase
{
    [SerializeField] private GameObject avatarPrefab;
    public override string QRCodeText => "AvatarStop3";

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