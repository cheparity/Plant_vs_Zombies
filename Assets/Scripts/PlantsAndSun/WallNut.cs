using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NutState
{
	HighHP,//4000-6000
	MiddleHP,//2000-4000
	LowHP,//0-2000
	Dead
}

public class WallNut : PlantBase
{
	private Animator anim;
	private NutState state;
	public override float MaxHP => 6000;
	private void Awake()
	{
		anim = GetComponent<Animator>();
		state = NutState.HighHP; //?????
	}
	private void Update()
	{
		if (2000<HP && HP <= 4000 && state!=NutState.MiddleHP) //?????
		{
			state = NutState.MiddleHP;
			StateToAnimation();
		}
		else if (0<HP && HP<=2000 && state!=NutState.LowHP) //?????
		{
			state = NutState.LowHP;
			StateToAnimation();

		}
		else if (HP <= 0 && state != NutState.Dead)
		{
			state=NutState.Dead;
			StateToAnimation();
		}
	}
	/// <summary>
	/// ????
	/// </summary>
	/// <param name="state"></param> Nut??
	private void StateToAnimation()
	{
		switch (state)
		{
			case NutState.HighHP:
				anim.Play("WallNut");
				anim.speed = 1;
				break;
			case NutState.MiddleHP:
				anim.Play("WallNut_State1");
				anim.speed = 1;
				break;
			case NutState.LowHP:
				anim.Play("WallNut_State2");
				anim.speed = 1;
				break;
			case NutState.Dead:
				Destroy(gameObject);
				break;
		}
	}
}
