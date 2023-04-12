using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Head : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
		animator = GetComponent<Animator>();

	}



	// Update is called once per frame
	void Update()
    {
        //如果播放完毕
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            //播放速度为0
            animator.speed = 0;
            //2s后销毁自身
            Destroy(gameObject, 1f);
        }
    }

}
