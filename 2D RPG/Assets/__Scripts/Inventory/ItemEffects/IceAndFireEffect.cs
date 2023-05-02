using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_IceAndFire", menuName = "Inventory/Item Effect/Ice And Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform respawnPosition)
    {
        Player player = PlayerManager.Instance.player;

        bool thirdAttack = player.PrimaryAttack.ComboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, respawnPosition.transform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.FacingDir, 0);

            Destroy(newIceAndFire, 5f);
        }
    }
}
