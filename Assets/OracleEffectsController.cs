using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OracleEffectsController : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 1.0f;
    [SerializeField] private float _fadeOutDuration = 1.0f;
    [SerializeField] private float _lingerDuration = 10f;

    [SerializeField] private AudioEvent _spawnSFX;
    [SerializeField] private AudioEvent _deathSFX;

    private AudioSource _audioSource;
    private ParticleSystem _particles;
    private void Start()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnSFX)
        {
            _spawnSFX.Play(_audioSource);
        }

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, _fadeInDuration).SetEase(Ease.OutSine);
    }

    private void OnKill()
    {
        transform.DOKill();
        transform.SetParent(null, true);
        _particles.Stop();
        transform.DOScale(Vector3.zero, _fadeOutDuration).SetEase(Ease.InElastic);

        if(_deathSFX)
        {
            _deathSFX.Play(_audioSource);
        }

        Invoke(nameof(FinalizeKill), _lingerDuration);
    }
    private void FinalizeKill()
    {
        Destroy(gameObject);
    }
}
