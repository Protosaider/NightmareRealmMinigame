using UnityEngine;

public class CellData : ScriptableObject
{
	[SerializeField]
	private Sprite _icon = null;
	public Sprite Icon => _icon;
}