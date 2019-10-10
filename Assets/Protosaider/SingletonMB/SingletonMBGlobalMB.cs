using System.Collections;
using System.Collections.Generic;
using Protosaider.Utils;
using UnityEngine;

namespace Protosaider
{
	public class SingletonMBGlobalMB : GlobalMonoBehaviour
	{
		private static SingletonMBGlobalMB s_instance;

		internal static SingletonMBGlobalMB Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = Create<SingletonMBGlobalMB>($"{typeof(SingletonMBGlobalMB)}");
					Debug.Log($"{typeof(SingletonMBGlobalMB)} was created");
				}

				return s_instance;
			}
		}
	}
}