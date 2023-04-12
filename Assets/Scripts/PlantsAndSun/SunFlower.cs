using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : PlantBase {
	public override float MaxHP {
		get {
			return 300; //最大生命值为300;
		}
	}


	/// <summary>
	/// 创建阳光
	/// </summary>
	private void CreatSun() {
		//实例化阳光（生成阳光对象），获取阳光身上的Sun组件 （？？？？）
		Sun sun = Instantiate<GameObject>(GameManager.Instance.GameConf.Sun, transform.position, Quaternion.identity, transform).GetComponent<Sun>();

		//阳光跳跃动画
		sun.JumpAnimation();

	}

	protected override void OnInitForPlace() {
		hp = MaxHP;
		InvokeRepeating("CreatSun", 7.5f, 24);//7.5s之后生产第一个阳光,之后每24s生产一个

	}
}
