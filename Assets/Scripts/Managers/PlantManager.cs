using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantTpye {
	SunFlower,
	PeaShooter,
	WalllNut,
	Spikeweed
};

public class PlantManager : MonoBehaviour {

	public static PlantManager Instance;
	private void Awake() {
		Instance = this;
	}
	
	/// <summary>
	/// ???????????
	/// </summary>
	/// <param name="type"></param>????
	/// <returns></returns>
	public GameObject GetPlantForType(PlantTpye type) {
		switch (type)
		{
			case PlantTpye.SunFlower:
				return GameManager.Instance.GameConf.SunFlower;
			case PlantTpye.PeaShooter:
				return GameManager.Instance.GameConf.PeaShooter;
			case PlantTpye.WalllNut:
				return GameManager.Instance.GameConf.WallNut;
			case PlantTpye.Spikeweed:
				return GameManager.Instance.GameConf.Spikeweed;
		}
		return null;
	}
}
