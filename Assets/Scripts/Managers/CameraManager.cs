using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
	#region variable
	public static CameraManager Instance;
	private Vector3 leftPosition = new Vector3(-0.41f, 0.14f, -10f);//摄像机的初始位置
	private Vector3 rightPosition = new Vector3(3.97f, 0.14f, -10f);//摄像机的初始位置
	public float moveSpeed;

	public float PreviewTime = 5;

	#endregion
	#region unity
	// Start is called before the first frame update
	private void Awake()
	{
		Instance = this;
		transform.position = leftPosition; //设置摄像机的初始位置
	}
	#endregion



	#region methods


	public void MoveCameraForStart()
	{
		StartCoroutine(MoveCamera());
	}

	/// <summary>
	/// 从当前位置移动到目标位置,用协程套协程的方式
	/// </summary>
	IEnumerator MoveCamera()
	{
		IEnumerator enum1 = DoMove(rightPosition);
		while (enum1.MoveNext())
			yield return enum1.Current;                     // 以协程的方式执行第一个协程的内容， 但是不启动，具体启动在外部

		IEnumerator enum2 = DoMove(leftPosition);           // 以协程的方式执行第二个协程的内容， 同上
		while (enum2.MoveNext())
			yield return enum2.Current;
	}

	IEnumerator MoveCamera(float duration)
	{
		IEnumerator enum1 = DoMove(rightPosition, duration / 2);
		while (enum1.MoveNext())
			yield return enum1.Current;

		IEnumerator enum2 = DoMove(leftPosition, duration / 2);
		while (enum2.MoveNext())
			yield return enum2.Current;
	}


	/// <summary>
	/// 从当前位置移动到目标位置（协程）
	/// </summary>
	/// <param name="targetPos"></param>目标位置
	/// <returns></returns>
	IEnumerator DoMove(Vector3 targetPos)
	{
		while (Vector2.Distance(targetPos, transform.position) > 0.1f) //只要现在的位置,离目标位置不够近
		{
			yield return new WaitForSeconds(0.01f); //延时  // 我帮你写一个时间的 // 稍等我一下
			transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed); //移动
		}
	}

	IEnumerator DoMove(Vector3 targetPos, float duration)
	{
		float startTime = Time.time;

		Vector3 startPoint = transform.position;
		while (Vector2.Distance(targetPos, transform.position) > 0.1f)
		{
			yield return new WaitForEndOfFrame();
			float ratio = Mathf.Clamp01(Time.time - startTime / duration);		 // 把值保证在 0~1
			transform.position = Vector3.Lerp(startPoint, targetPos, ratio);       // 挑时间估计很好用的吧 要不要试试？
		}
	}


	#endregion
}
