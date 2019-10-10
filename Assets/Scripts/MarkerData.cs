using UnityEngine;

[CreateAssetMenu(fileName = "Marker", menuName = "Custom/Marker")]
public class MarkerData : CellData
{
	[SerializeField]
	private EMarkerType _markerType;
	public EMarkerType MarkerType => _markerType;
}