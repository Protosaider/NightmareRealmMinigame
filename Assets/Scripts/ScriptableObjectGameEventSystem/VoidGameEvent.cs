using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidGameEvent", menuName = "Game Events/Void Event")]
public class VoidGameEvent : BaseGameEvent<SVoid>
{
	public void Invoke() => Invoke(Void.Null);
}
