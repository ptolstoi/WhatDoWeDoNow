﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private GameObject body;
	private Transform myTransform;
	private float rot;
	public float pickupdist = 0.5f;
	private Animator animator;

	private Vector2 movDirection;

	private int xm = 0;
	private int ym = 0;

	// Use this for initialization
	void Start () {
		myTransform = (Transform) GetComponent("Transform");
		animator = (Animator) GetComponent("Animator");
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody2D rigidBody = (Rigidbody2D)GetComponent("Rigidbody2D");
		
		if(Input.GetKey("up")){
			ym = 1;
			xm = 0;
			animator.SetInteger("Direction", 2);
			animator.SetBool("Moving", true);
		}
		else if(Input.GetKey("down")){
			ym = -1;
			xm = 0;
			animator.SetInteger("Direction", 0);
			animator.SetBool("Moving", true);
		}
		else{
			ym = 0;
			if(Input.GetKey("left")){
				xm = -1;
				animator.SetInteger("Direction", 1);
				animator.SetBool("Moving", true);
			}
			else if(Input.GetKey("right")){
				xm = +1;
				animator.SetInteger("Direction", 3);
				animator.SetBool("Moving", true);
			}
			else{
				xm = 0;
				animator.SetBool("Moving", false);
			}
		}

		//if(xm != 0 || ym != 0) rot = Mathf.Atan2(ym, xm)*Mathf.Rad2Deg;


		if(Input.GetKeyDown("space") && body == null){
			GameObject[] obj = GameObject.FindGameObjectsWithTag("Body");
			for(int i = 0; i < obj.Length; i++){
				Transform other = ((Transform) obj[i].GetComponent("Transform"));
				float magn = (other.position - myTransform.position).magnitude;
				if(magn < pickupdist){
					body = obj[i];
					DistanceJoint2D joint = gameObject.AddComponent<DistanceJoint2D>();
					joint.distance = magn;
					joint.connectedBody = (Rigidbody2D)body.GetComponent("Rigidbody2D");
					joint.maxDistanceOnly = true;
					break;
				}
			}
		}
		else if(Input.GetKeyUp("space") && body != null){
			GameObject.Destroy(gameObject.GetComponent("DistanceJoint2D"));
			body = null;
		}

		movDirection = new Vector2(xm, ym);
		//myTransform.rotation = Quaternion.Euler(0, 0, rot);
		movDirection *= 5;

		if(body != null){
			Transform other = (Transform) body.GetComponent("Transform");
			other.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(other.position.y - myTransform.position.y, other.position.x - myTransform.position.x) * Mathf.Rad2Deg);
		}

		rigidBody.velocity = movDirection;
	}
}