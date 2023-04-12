using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAnimation : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        Invoke("Show", 5f); //5s之后调用show方法
    }
    private void Update()
    {
        
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) //如果播放完了
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 显示自身
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true); //设置成true
        animator.Play("Ready", 0, 0) ; //开始播放
    }
}
