using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
	float downTargetY;
	float perishTime = 5;


	#region unity
	// Update is called once per frame
	void Update()
	{
		//如果超过了目的地，则return
		if (transform.position.y <= downTargetY)
		{
			Invoke("Perish", perishTime); //过5秒之后销毁（对向日葵生成的花依然生效）
			return;
		}
		//如果没有到目的地，则继续translate
		transform.Translate(Vector2.down * Time.deltaTime);
	}
	/// <summary>
	/// 鼠标点击阳光时，增加阳光数量（GameManager）之后，销毁自身
	/// </summary>
	private void OnMouseDown()
	{
		PlayerManager.Instance.SunNum += 25;
		Vector3 uiPos = Camera.main.ScreenToWorldPoint(UIManager.Instance.GetSunNumTextPos());
		uiPos.z = 0; //将z坐标改成0，可能会出错
		StartCoroutine(DoFly(uiPos));
	}
	#endregion

	#region methods

	private IEnumerator DoFly(Vector3 pos)//协程，向着pos方向飞行
	{
		Vector3 direction = (pos - transform.position).normalized;
		while (Vector3.Distance(pos, transform.position) > 0.5f)
		{
			yield return new WaitForSeconds(0.01f);
			transform.Translate(direction);
		}
		Perish();//销毁阳光
	}

	/// <summary>
	/// 销毁自身
	/// </summary>
	private void Perish()
	{
		Destroy(gameObject);
	}

	/// <summary>
	/// 阳光从天空中初始化的方法
	/// 设置成了public，是为了能够给SkySunManager用到初始化阳光的方法
	/// </summary>
	public void InitForSky(float creatPosX, float creatPosY, float downTargetPosY)
	{
		downTargetY = downTargetPosY;
		transform.position = new Vector2(creatPosX, creatPosY);
	}

	/// <summary>
	/// 协程实现阳光跳跃动画
	/// </summary>
	public void JumpAnimation()
	{
		StartCoroutine(DoJump());
	}

	private IEnumerator DoJump()
	{

		bool isLeft = Random.Range(0, 2) == 0; //随机向左或向右，如果为0则做，为1则右
		Vector3 originalPos = transform.position;

		//接下来是跳跃动画
		//每个动作之间延时，会让动画更流畅
		//否则，就会一下就到目标位置

		//这是直线做法，也可以试试抛物线
		if (isLeft) //如果是左边
		{

			while (transform.position.y <= originalPos.y + 0.18)//只要是大于0.9则停止跳
			{
				yield return new WaitForSeconds(0.01f); //停顿一段时间
				transform.Translate(new Vector3(-0.01f, 0.05f, 0)); //位移，向左上跳
			}
			//接下来下落
			while (transform.position.y >= originalPos.y)//只要是大于初始位置，则向下降落
			{
				yield return new WaitForSeconds(0.04f); //停顿一段时间
				transform.Translate(new Vector3(0, -0.06f, 0)); //位移，向下落
			}
		}
		else //如果是右边，逻辑相反
		{
			yield return new WaitForSeconds(0.04f); //停顿一段时间
			while (transform.position.y <= originalPos.y + 0.18)//只要是大于0.9则停止跳
			{
				yield return new WaitForSeconds(0.01f); //停顿一段时间
				transform.Translate(new Vector3(0.01f, 0.05f, 0)); //位移，向右上跳
			}
			//接下来下落
			while (transform.position.y >= originalPos.y)//只要是大于初始位置，则向下降落
			{
				yield return new WaitForSeconds(0.04f); //停顿一段时间
				transform.Translate(new Vector3(0, -0.06f, 0)); //位移，向下落
			}
		}

	}

	#endregion


}
