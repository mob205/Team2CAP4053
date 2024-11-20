using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuNavigator : MonoBehaviour
{
    [field: SerializeField] public MenuNavigator Left { get; set; }
    [field: SerializeField] public MenuNavigator Right { get; set; }
    [field: SerializeField] public MenuNavigator Next { get; set; }
    [field: SerializeField] public MenuNavigator Back { get; set; }

    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;
    public UnityEvent OnBack;

    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
    }
    public void GoBack()
    {
        OnBack?.Invoke();
    }
    public void Click()
    {
        _button.onClick?.Invoke();
    }
    public void Select()
    {
        OnSelected?.Invoke();
    }
    public void Deselect()
    {
        OnDeselected?.Invoke();
    }
}