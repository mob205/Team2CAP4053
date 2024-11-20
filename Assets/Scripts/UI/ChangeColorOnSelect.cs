using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChangeColorOnSelect : MonoBehaviour
{
    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _selectFontColor;

    private Color _startColor;
    private Color _startFontColor;

    private Image _image;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _startColor = _image.color;
        _startFontColor = _text.color;
    }

    public void Select()
    {
        if(_image)
        {
            _image.color = _selectColor;
        }
        if(_text)
        {
            _text.color = _selectFontColor;
        }
    }
    public void Deselect()
    {
        if(_image)
        {
            _image.color = _startColor;
        }
        if(_text)
        {
            _text.color = _startFontColor;
        }
    }
}
