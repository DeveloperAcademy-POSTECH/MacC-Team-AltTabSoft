using System;
using UnityEngine;
using System.Collections;

namespace deko
{

public class FlyCamera : MonoBehaviour
{
	public float mSpeed = 100.0f;
	public float mShiftAdd = 250.0f;
	public float mShiftLimit = 1000.0f;
	public float mMouseSensitivity = 0.25f;

	private Vector3 mMouseBefore = new Vector3(255, 255, 255);


	void Start()
	{
		mMouseBefore = Input.mousePosition;
	}

	void Update ()
	{
		if (Input.GetMouseButton(0))
		{
			mMouseBefore = Input.mousePosition - mMouseBefore ;
			mMouseBefore = new Vector3(-mMouseBefore.y * mMouseSensitivity, mMouseBefore.x * mMouseSensitivity, 0 );
			mMouseBefore = new Vector3(transform.eulerAngles.x + mMouseBefore.x , transform.eulerAngles.y + mMouseBefore.y, 0);
			transform.eulerAngles = mMouseBefore;
			mMouseBefore =  Input.mousePosition;

			float f = 0.0f;
			Vector3 p = GetKeyInput();
			float total_run = 1.0f;

			if (Input.GetKey(KeyCode.LeftShift))
			{
				total_run += Time.deltaTime;
				p  = p * total_run * mShiftAdd;
				p.x = Mathf.Clamp(p.x, -mShiftLimit, mShiftLimit);
				p.y = Mathf.Clamp(p.y, -mShiftLimit, mShiftLimit);
				p.z = Mathf.Clamp(p.z, -mShiftLimit, mShiftLimit);
			}
			else
			{
				total_run = Mathf.Clamp(total_run * 0.5f, 1f, 1000f);
				p = p * mSpeed;
			}

			p = p * Time.deltaTime;
			Vector3 new_position = transform.position;

			if (Input.GetKey(KeyCode.Space))
			{
				transform.Translate(p);
				new_position.x = transform.position.x;
				new_position.z = transform.position.z;
				transform.position = new_position;
			}
			else
			{
				transform.Translate(p);
			}
		}
		mMouseBefore = Input.mousePosition;
	}

	private Vector3 GetKeyInput()
	{
		Vector3 ret_v = new Vector3();
		if (Input.GetKey (KeyCode.W))
			ret_v += new Vector3(0, 0 , 1);
		if (Input.GetKey (KeyCode.S))
			ret_v += new Vector3(0, 0, -1);
		if (Input.GetKey (KeyCode.A))
			ret_v += new Vector3(-1, 0, 0);
		if (Input.GetKey (KeyCode.D))
			ret_v += new Vector3(1, 0, 0);
		if (Input.GetKey (KeyCode.Q))
			ret_v += new Vector3(0, -1, 0);
		if (Input.GetKey (KeyCode.E))
			ret_v += new Vector3(0, 1, 0);
		return ret_v;
	}
}

}