using UnityEngine;
using UnityEngine.UI;

public class PointerFollower : MonoBehaviour
{
    [SerializeField] 
    private Image _followerImage;

    [SerializeField]
    private Canvas _canvas;

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);
        transform.position = _canvas.transform.TransformPoint(position);
    }

    public void SetData(Sprite sprite)
    {
        _followerImage.sprite = sprite;
    }

    public void Toggle(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
