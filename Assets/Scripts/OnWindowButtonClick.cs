using UnityEngine;

public class OnWindowButtonClick : MonoBehaviour
{
	public void OnButtonClick() => GameLoop.Instance.SwitchToStateGame();
}
