using UnityEngine;
using System.Collections;

public class FPCivilianArmsScript : MonoBehaviour {

	#region Fields

	//it'll be re-written to use arrays when weapon list grows bigger
	public GameObject popup;
	public Camera mainCameraGO;
	public GameObject mainCameraSnapObject;

	public GameObject bareArms;
	public SkinnedMeshRenderer[] bareArmsMeshes;
	public Material[] bareArmsMatLight;
	public Material[] bareArmsMatMedium;
	public Material[] bareArmsMatDark;

	public GameObject fingerlessGloves;
	public SkinnedMeshRenderer[] fingerlessGlovesMeshes;
	public Material[] fingerlessGlovesMat;

	public GameObject flannelShirt;
	public SkinnedMeshRenderer[] flannelShirtMeshes;
	public Material[] flannelShirtMat;

	public GameObject motorcycleGloves;
	public SkinnedMeshRenderer[] motorcycleGlovesMeshes;
	public Material[] motorcycleGlovesMat;

	public GameObject motorcycleSleeves;
	public SkinnedMeshRenderer[] motorcycleSleevesMeshes;
	public Material[] motorcycleSleevesMat;

	public GameObject sweaterSleeves;
	public SkinnedMeshRenderer[] sweaterSleevesMeshes;
	public Material[] sweaterSleevesMat;

	public GameObject tracksuitSleeves;
	public SkinnedMeshRenderer[] tracksuitSleevesMeshes;
	public Material[] tracksuitSleevesMat;

	public GameObject latexGloves;
	public SkinnedMeshRenderer[] latexGlovesMeshes;
	public Material[] latexGlovesMat;

	public GameObject pinstripeSleeves;
	public SkinnedMeshRenderer[] pinstripeSleevesMeshes;
	public Material[] pinstripeSleevesMat;

	public GameObject tuxedoGloves;
	public SkinnedMeshRenderer[] tuxedoGlovesMeshes;
	public Material[] tuxedoGlovesMat;

	public GameObject leatherGloves;
	public SkinnedMeshRenderer[] leatherGlovesMeshes;
	public Material[] leatherGlovesMat;


	public bool AAmode = false;
	public int gloveType = 0;
	public int maxGloves = 2;

	public float cameraNormalFov = 60f;
	public float cameraNearClipping = 0.01f;

	int selectedSleeves = 1;
	int selectedGloves = 1;

	bool isHidden = false;
	bool isHiddenManually = false;
	public bool isPlaying = true;
	public bool finishedPlaying = false;
	public bool isRunning = false;

	public float idleAfter = 4F;
	public float elapsedTime = 0F;
	//public GameObject selectedCameraSnapObject;
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 2F;
	public float sensitivityY = 2F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationY = 0F;

	string animationGrab 				= "FPArms_Unarmed_Grab";
	string animationGrabSmallObject 	= "FPArms_Unarmed_Grab_SmallObject";
	string animationGrabMediumObject	= "FPArms_Unarmed_Grab_MediumObject";
	string animationIdle 				= "FPArms_Unarmed_Idle";
	string animationJump 				= "FPArms_Unarmed_Jump";
	string animationPunch 				= "FPArms_Unarmed_Punch";
	string animationPushDoor 			= "FPArms_Unarmed_Push-Door";
	string animationSprint 				= "FPArms_Unarmed_Sprint";
	string animationThrow 				= "FPArms_Unarmed_Throw";
	string animationWalk 				= "FPHands_Male_Walk";

	#endregion

	void Start() 
	{
		elapsedTime = Time.time;

		selectedSleeves = 1;
		selectedGloves = 1;

		mainCameraGO = Camera.main;//Camera.main;

		turnAllOff();
		turnOnSelected();

		mainCameraSnapObject.transform.position = mainCameraGO.transform.position;
		mainCameraSnapObject.transform.rotation = mainCameraGO.transform.rotation;
		mainCameraGO.nearClipPlane = cameraNearClipping;
	}

	void FixedUpdate()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = mainCameraGO.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			mainCameraGO.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			mainCameraGO.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			mainCameraGO.transform.localEulerAngles = new Vector3(-rotationY, mainCameraGO.transform.localEulerAngles.y, 0);
		}
	}

	void Update() 
	{
		mainCameraSnapObject.transform.position = mainCameraGO.transform.position;
		mainCameraSnapObject.transform.rotation = mainCameraGO.transform.rotation;

//		if (!isPlaying)
//			playAnimation (AvailableAnimations.Idle);

		if ((isPlaying) && (!finishedPlaying))
		{
			elapsedTime += Time.deltaTime;

			if (elapsedTime > idleAfter)
				isPlaying = false;
		}

		if (Input.anyKey)					
		{		
			isPlaying = true;	
			elapsedTime = 0F;	
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (popup.transform.gameObject.activeSelf == true)
			{
				popup.transform.gameObject.SetActive(false);
				isHiddenManually = true;
			} 
			else if (popup.transform.gameObject.activeSelf == false)
			{
				isHiddenManually = false;
				popup.transform.gameObject.SetActive(true);
			}
		}

		if (Input.GetKeyDown("1"))			
		{		
			selectedSleeves = 1;	
			switch_weapon();		
		}
		if (Input.GetKeyDown("2"))			
		{		
			selectedSleeves = 2;	
			switch_weapon();		
		}
		if (Input.GetKeyDown("3"))			
		{		
			selectedSleeves = 3;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("4")) 
		{		
			selectedSleeves = 4;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("5")) 
		{		
			selectedSleeves = 5;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("q")) 
		{		
			selectedGloves = 1;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("w")) 
		{		
			selectedGloves = 2;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("e")) 
		{		
			selectedGloves = 3;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("r")) 
		{		
			selectedGloves = 4;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("t")) 
		{		
			selectedGloves = 5;	
			switch_weapon();		
		}		
		if (Input.GetKeyDown("y")) 
		{		
			selectedGloves = 6;	
			switch_weapon();		
		}		

		if (Input.GetKeyDown(KeyCode.LeftShift))			
			playAnimation (AvailableAnimations.Walk);
		
		if (Input.GetKeyDown("z"))			
			isRunning = true;	

		if (Input.GetKeyUp("z"))			
			isRunning = false;	

		if (isRunning == true) 
			playAnimation (AvailableAnimations.Sprint);	
		else if ((isRunning == false) && (!isPlaying)) 	
			playAnimation (AvailableAnimations.Idle);


		if (Input.GetKeyDown("x"))			
			playAnimation (AvailableAnimations.PushDoor);
		
		if (Input.GetKeyDown("c"))			
			playAnimation (AvailableAnimations.Throw);

		if (Input.GetKeyDown("v"))			
			playAnimation (AvailableAnimations.Grab);

		if (Input.GetKeyDown("b"))			
			playAnimation (AvailableAnimations.GrabSmallObject);

		if (Input.GetKeyDown("n"))			
			playAnimation (AvailableAnimations.GrabMediumObject);

		if (Input.GetKeyUp(KeyCode.Space))	
			playAnimation (AvailableAnimations.Jump);

		if (Input.GetKeyDown("a"))			
			changeSleevesColor();
		
		if (Input.GetKeyDown("s"))			
			changeSkin();

		if ((Input.GetMouseButtonDown(1)) || (Input.GetMouseButtonDown(0)) || (Input.GetMouseButtonDown(2)))
		{		
			playAnimation (AvailableAnimations.Punch);
		
			if (popup.transform.gameObject.activeSelf == true)
			{	
				popup.transform.gameObject.SetActive(false);	
				isHiddenManually = true;	
			}
		}
	}

	public void turnAllOn()
	{
		foreach (Transform child in mainCameraSnapObject.transform) {
			child.gameObject.SetActive (true);
		}
	}

	public void turnAllOff()
	{
		foreach (Transform child in mainCameraSnapObject.transform) {
			child.gameObject.SetActive (false);
		}
	}

	public void turnOnSelected ()
	{	
		switch (selectedSleeves) 
		{
			case 1 : 
				flannelShirt.SetActive (true);
				break;
			case 2:
				motorcycleSleeves.SetActive (true);
				break;
			case 3:
				sweaterSleeves.SetActive (true);
				break;
			case 4:
				tracksuitSleeves.SetActive (true);
				break;
			case 5:
				pinstripeSleeves.SetActive (true);
				break;
			default :
				break;
		}

		switch (selectedGloves) 
		{
			case 1 : 
				bareArms.SetActive (true);
				break;
			case 2:
				fingerlessGloves.SetActive (true);
				break;
			case 3:
				motorcycleGloves.SetActive (true);
				break;
			case 4:
				latexGloves.SetActive (true);
				break;
			case 5:
				tuxedoGloves.SetActive (true);
				break;
			case 6:
				leatherGloves.SetActive (true);
				break;
			default :
				break;
		}
	}
		
	int currentSkin = 0;

	public void changeSkin()
	{
		turnAllOn();

		if (currentSkin == 0) 
		{
			foreach (SkinnedMeshRenderer mesh in bareArmsMeshes) {
				mesh.material = bareArmsMatMedium [0];
			}
			currentSkin = 1;
		}
		else if (currentSkin == 1)
		{
			foreach (SkinnedMeshRenderer mesh in bareArmsMeshes) {
				mesh.material = bareArmsMatDark [0];
			}
			currentSkin = 2;
		}	
		else 
		{
			foreach (SkinnedMeshRenderer mesh in bareArmsMeshes) {
				mesh.material = bareArmsMatLight [0];
			}
			currentSkin = 0;
		}			
		turnAllOff();
		turnOnSelected();
	}

	int curFlannelMaterial = 0;
	int curTracksuitMaterial = 0;
	int curSweaterMaterial = 0;

	void changeSleevesColor ()
	{
		if (flannelShirt.activeInHierarchy) {
			curFlannelMaterial++;
			if (curFlannelMaterial >= flannelShirtMat.Length)
				curFlannelMaterial = 0;
			
			foreach (SkinnedMeshRenderer mesh in flannelShirtMeshes) {
				mesh.material = flannelShirtMat [curFlannelMaterial];
			}
		}

		if (tracksuitSleeves.activeInHierarchy) {
			curTracksuitMaterial++;
			if (curTracksuitMaterial >= tracksuitSleevesMat.Length)
				curTracksuitMaterial = 0;

			foreach (SkinnedMeshRenderer mesh in tracksuitSleevesMeshes) {
				mesh.material = tracksuitSleevesMat [curTracksuitMaterial];
			}
		}

		if (sweaterSleeves.activeInHierarchy) {
			curSweaterMaterial++;
			if (curSweaterMaterial >= sweaterSleevesMat.Length)
				curSweaterMaterial = 0;

			foreach (SkinnedMeshRenderer mesh in sweaterSleevesMeshes) {
				mesh.material = sweaterSleevesMat [curSweaterMaterial];
			}
		}
	}

	public void switch_weapon()
	{
//		Debug.Log ("SWITCH");

		turnAllOff();
	
		turnOnSelected();

		mainCameraSnapObject.transform.position = mainCameraGO.transform.position;
		mainCameraSnapObject.transform.rotation = mainCameraGO.transform.rotation;
	
		playAnimation (AvailableAnimations.Idle);
	}

	public void playAnimation (AvailableAnimations animToPlay)
	{
//		Debug.Log ("PLAY");

		string animToPlayName = "";

		switch (animToPlay) 
		{
			case AvailableAnimations.Idle:
				animToPlayName = animationIdle;
				break;
			case AvailableAnimations.Jump:
				animToPlayName = animationJump;
				idleAfter = 1.7f;
				break;
			case AvailableAnimations.Walk:
				animToPlayName = animationWalk;
				idleAfter = 0.5F;
				break;
			case AvailableAnimations.Sprint:
				animToPlayName = animationSprint;
				idleAfter = 0.5F;
				break;
			case AvailableAnimations.Grab:
				animToPlayName = animationGrab;
				break;
			case AvailableAnimations.GrabSmallObject:
				animToPlayName = animationGrabSmallObject;
				break;
			case AvailableAnimations.GrabMediumObject:
				animToPlayName = animationGrabMediumObject;
				break;
			case AvailableAnimations.Punch:
				animToPlayName = animationPunch;
				idleAfter = 1f;
				break;
			case AvailableAnimations.PushDoor:
				animToPlayName = animationPushDoor;
				idleAfter =  2.25f;
				break;
			case AvailableAnimations.Throw:
				animToPlayName = animationThrow;
				idleAfter = 1.5F;
				break;
			default :
				break;
		}

		switch (selectedSleeves) 
		{
			case 1:
				foreach (Transform child in flannelShirt.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 2:
			foreach (Transform child in motorcycleSleeves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 3:
				foreach (Transform child in sweaterSleeves.transform) {
						child.GetComponent<Animation>().Play (animToPlayName);
					}
				break;
			case 4:
				foreach (Transform child in tracksuitSleeves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 5:
				foreach (Transform child in pinstripeSleeves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			default :
				break;
		}

		switch (selectedGloves) 
		{
			case 1:
				foreach (Transform child in bareArms.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 2:
				foreach (Transform child in fingerlessGloves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 3:
				foreach (Transform child in motorcycleGloves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 4:
				foreach (Transform child in latexGloves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 5:
				foreach (Transform child in tuxedoGloves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			case 6:
				foreach (Transform child in leatherGloves.transform) {
					child.GetComponent<Animation>().Play (animToPlayName);
				}
				break;
			default :
				break;
		}

//		mainCameraGO.fieldOfView = cameraNormalFov;
	}

	public enum AvailableAnimations 
	{
		Idle,
		Jump,
		Walk,
		Sprint,
		Grab,
		GrabSmallObject,
		GrabMediumObject,
		Punch,
		PushDoor,
		Throw
	}
}