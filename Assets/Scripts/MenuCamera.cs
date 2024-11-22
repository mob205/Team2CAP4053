using DG.Tweening;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private float _tweenDuration;
    public void SetCameraLocation(Transform location)
    {
        transform.DOComplete();
        transform.DOMove(location.position, _tweenDuration).SetEase(Ease.InOutQuart);
        transform.DORotateQuaternion(location.rotation, _tweenDuration).SetEase(Ease.InOutQuart);
    }
}
