using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameFieldCellsData", menuName = "Custom/GameFieldCellsData")]
public class GameFieldCellsData : ScriptableObject
{
	//private readonly Dictionary<EMarkerType, Color> _markers = new Dictionary<EMarkerType, Color>(Enum.GetValues(typeof(EMarkerType)).Length);
	[SerializeField]
	private MarkerData _markerTypeOne;
	public MarkerData MarkerTypeOne => _markerTypeOne;

	[SerializeField]
	private MarkerData _markerTypeTwo;
	public MarkerData MarkerTypeTwo => _markerTypeTwo;

	[SerializeField]
	private MarkerData _markerTypeThree;
	public MarkerData MarkerTypeThree => _markerTypeThree;

	[SerializeField]
	private BlockageData _blockage;
	public BlockageData Blockage => _blockage;

    public readonly Int32 MarkerEachTypeCount = 5;
}