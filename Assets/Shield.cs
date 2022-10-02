using DG.Tweening;
using UnityEngine;

using static GameData;

public class Shield : MonoBehaviour, IHittable
{
    [SerializeField] private ParticleSystem VFX;
    [SerializeField] private AudioClip hitClip;

    private Tween tween;

    private const float punchScale = 0.005f;

    public void GetHit(float damage)
    {
        VFX?.Play();

        tween.Goto(0, true);
        tween.Kill();

        tween = transform.DOPunchScale(Vector3.one * punchScale, .2f);

        _gameSystem.PlayShot(hitClip, 1f);
    }
}