using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Controller : MonoBehaviour
{

	public float moveSpeed = 6;

	Rigidbody rigidbody;
	Camera viewCamera;
	Vector3 velocity;


    void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		viewCamera = Camera.main;
		
		
	}

	void Update()
	{
		
	}

	
}