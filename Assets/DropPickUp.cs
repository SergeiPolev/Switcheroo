using UnityEngine;

using static GameData;

public class DropPickUp : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private AudioClip powerUpClip;

    private void Awake()
    {
        GetComponent<OnTriggerEnterComponent>().OnEnter += PickUp;
    }
    private void PickUp(Transform other, Transform @object)
    {
        if (other.CompareTag("Drop"))
        {
            var pickUp = other.GetComponent<Drop>();

            if (pickUp != null)
            {
                ChoosePickUp(pickUp.DropType);

                _gameSystem.PlayShot(powerUpClip, 1f);
            }

            Destroy(other.gameObject);
        }
    }
    private void ChoosePickUp(DropType dropType)
    {
        switch (dropType)
        {
            case DropType.Health:

                playerHealth.Heal((int) (playerHealth.MaxPoints / .33f));
                break;

            case DropType.Rate:

                playerShooting.SetModifier(0.7f, 3f);
                break;

            case DropType.CircleShot:

                playerShooting.ShootFromEachSide();
                break;

            case DropType.Switcheroo:

                _gameSystem.Switcheroo();
                break;
        }
    }
}