using UnityEngine;


[CreateAssetMenu(fileName = "SpawnAvatarAction3", menuName = "QR Actions/Spawn Avatar 3")]
public class SpawnAvatarAction3 : QRActionBase
{
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private GameObject avatarArabicPrefab;
    [SerializeField] private GameObject avatarWomanPrefab;
    [SerializeField] private GameObject avatarWomanArabicPrefab;
    private Language language = new Language();
    private Character character = new Character();
    public override string QRCodeText => "AleppoSoap";

    public override void Execute(Vector3 position, Quaternion rotation)
    {
        Camera mainCamera = Camera.main;
        language = UserSystemManager.Language;
        character = UserSystemManager.Character;
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

            if (avatar != null)
            {
                avatar.transform.LookAt(new Vector3(userPosition.x, avatar.transform.position.y, userPosition.z));
            }
        }
    }
}