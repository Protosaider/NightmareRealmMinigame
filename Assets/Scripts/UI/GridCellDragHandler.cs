using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class GridCellDragHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	//[SerializeField]
	//private Canvas _dynamicCanvas; //Create clone for drag and drop canvas and set current item as Faded

	private CanvasGroup _canvasGroup;
	private Transform _originalParent;
	private Boolean _isHovering;
	private Boolean _isDragging;

	[SerializeField]
	protected GridCellUI gridCellUI;
	public GridCellUI GridCellUI => gridCellUI;

	private Transform _transform;

	private void Start()
	{
		_transform = transform;
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnDisable()
	{
		if (_isHovering)
			_isHovering = false;
	}

	public void OnPointerEnter(PointerEventData eventData) => _isHovering = true;
	public void OnPointerExit(PointerEventData eventData) => _isHovering = false;

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (!GridCellUI.IsOnPointerDownExecutionAllowed())
			{
				_isDragging = false;
				return;
			}

            var parent = _transform.parent;
			_originalParent = parent;
			_transform.SetParent(parent.parent);
			_canvasGroup.blocksRaycasts = false;
			_isDragging = !_isDragging;
		}
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (_isDragging)
			{
				_transform.SetParent(_originalParent);
				_transform.localPosition = Vector3.zero;
				_canvasGroup.blocksRaycasts = true;
				_isDragging = !_isDragging;
            }
        }
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (!_isDragging)
			{
				eventData.pointerDrag = null;
				return;
            }
            transform.position = Input.mousePosition;
        }
    }
}