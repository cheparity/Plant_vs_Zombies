using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_DeadBody : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();

	}



	// Update is called once per frame
	void Update()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			animator.speed = 0;
			Destroy(gameObject);
		}
	}

}
