using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class GridCellUI : MonoBehaviour, IDropHandler
{
	[SerializeField]
	protected Image CellIconImage;

	public Int32 CellIndex { get; private set; }

	private void OnEnable() => UpdateCellUI();

	protected virtual void Start()
	{
		CellIndex = transform.GetSiblingIndex();
		UpdateCellUI();
	}

	protected virtual void EnableCellUI(Boolean isEnabled) => CellIconImage.enabled = isEnabled;
	public abstract void OnDrop(PointerEventData eventData);
	public abstract void UpdateCellUI();

	public abstract Boolean IsOnPointerDownExecutionAllowed();

}