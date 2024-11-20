using UnityEngine;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField] private MenuNavigator _left;
    [SerializeField] private MenuNavigator _right;
    [SerializeField] private MenuNavigator _next;

    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
    }
    public void Click()
    {
        _button.onClick.Invoke();
    }
}