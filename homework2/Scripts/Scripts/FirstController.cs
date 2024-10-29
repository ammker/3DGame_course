using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene Controller
/// Usage: host on a gameobject in the scene   
/// responsiablities:
///   acted as a scene manager for scheduling actors.log something ...
///   interact with the director and players
/// </summary>
public class FirstController : MonoBehaviour, ISceneController, IUserAction {

	public CharacterController[] characters;
	public BoatController boat;
	public BankController leftBank, rightBank;

	UserGUI userGUI;
	UserGUI []clickGUI=new UserGUI [7];
	//public bool moving=false;

	private CCActionManager myActionManager;
	public Judgment judgment;

	// the first scripts
	void Awake () {
		SSDirector director = SSDirector.getInstance ();
		director.setFPS (60);
		director.currentSceneController = this;
		director.currentSceneController.LoadResources ();
		userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
		userGUI.gameState = 0;
	}
	 
	// loading resources for the first scence
	public void LoadResources () {
		judgment = new Judgment();
		myActionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
		boat = new BoatController();
		boat.boat = Instantiate(Resources.Load("Prefabs/Boat"), new Vector3(-4, -2, 0), Quaternion.identity) as GameObject;
		clickGUI[6]=boat.boat.AddComponent<UserGUI>() as UserGUI;
		boat.setName("boat");
		//boat.move = boat.boat.AddComponent(typeof(Move)) as Move;

		leftBank = new BankController("left");
		leftBank.bank = Instantiate(Resources.Load("Prefabs/Bank"), new Vector3(-15, -1.5f, 0), Quaternion.identity) as GameObject;
		leftBank.setName("LeftBank");
		rightBank = new BankController("right");
		rightBank.bank = Instantiate(Resources.Load("Prefabs/Bank"), new Vector3(15, -1.5f, 0), Quaternion.identity) as GameObject;
		rightBank.setName("RightBank");
		string []name = { "Devil", "Priest" };
		characters = new CharacterController[6];
		for (int i = 0; i < 2; i++)
		{
			for(int j = 0; j < 3; j++)
            {
			characters[i * 3 + j] = new CharacterController(name[i], i * 3 + j);
			characters[i * 3 + j].character = Instantiate(Resources.Load("Prefabs/"+name[i]), Vector3.zero, Quaternion.identity) as GameObject;
			characters[i * 3 + j].setName(name[i] + j);
			characters[i * 3 + j].character.transform.position = new Vector3((float)(-20 + (i * 3 + j) * 2), 2f, 0);
			leftBank.putACharacter(characters[i*3+j]);

			clickGUI[i * 3 + j]=characters[i * 3 + j].character.AddComponent<UserGUI>() as UserGUI;
			//characters[i * 3 + j].move = characters[i * 3 + j].character.AddComponent(typeof(Move)) as Move;
            }
			
		}
	}
	/*
	public bool checkLose()
	{
		int countDevilLeft = leftBank.getNumDevil(), countPriestLeft = leftBank.getNumPriest();
		int countDevilRight = rightBank.getNumDevil(), countPriestRight = rightBank.getNumPriest();
		int[] personOnBoat = boat.getPersonOnBoat();   //返回下标
		int d = 0, p = 0;
		for (int i = 0; i < 2; i++)
		{
			if (personOnBoat[i] < 3 && personOnBoat[i] >= 0) d++; 
			else if (personOnBoat[i] >= 3) p++;
		}
		if (boat.getState() == 1)
		{
			countDevilLeft += d;
			countPriestLeft += p;
		}
		else if (boat.getState() == 2)
		{
			countDevilRight += d;
			countPriestRight += p;
		}
		if ((countDevilLeft > countPriestLeft && countPriestLeft != 0) || (countDevilRight > countPriestRight && countPriestRight != 0))
		{
			return true;
		}
		return false;
	}

	public bool checkWin()
	{
		if (boat.getNumEmptyPos() == 2 && (leftBank.getNumDevil() + leftBank.getNumPriest() == 0) && (rightBank.getNumDevil() + rightBank.getNumPriest() == 6))
			return true;
		return false;
	}
	*/
	#region IUserAction implementation
	public void moveBoat()
    {
		if (boat.getNumEmptyPos() == 2) return;       //没人在船上，船不动
		int[] personOnBoat = boat.getPersonOnBoat();  //得到在船上的人的编号
		//boat.moveToAnotherBank();
		myActionManager.moveBoat(boat);
		for (int i = 0; i < 2; i++)
		{
			if (personOnBoat[i] == -1) continue;
			//characters[personOnBoat[i]].moveOnBoat((boat.getState() == 1 ? 2 : 1), i);   //先移动船再移动人，船的state先变，让char得到移动后的state
			myActionManager.moveCharacter(characters[personOnBoat[i]], characters[personOnBoat[i]].getDestinationOnBoat((boat.getState() == 1 ? 2 : 1), i));
		}

		if (judgment.checkLose(boat, leftBank, rightBank) == true)
		{
			for(int i = 0; i < 7; i++)
			{
				clickGUI[i].click = false;

			}
			userGUI.gameState = -1;
			return;
		}
	}
	public void moveCharacter(int index)
    {
		if (characters[index].getState() == 1 && boat.getState() == 1 && boat.getNumEmptyPos() > 0)   //人在左岸，船在左岸，船有空位
		{
			characters[index].setPositionOnBoat();
			myActionManager.moveCharacter(characters[index], boat.getEmptyPos());  ;
			leftBank.removeACharacter(characters[index]);
			boat.putACharacter(characters[index]);
		}
		else if (characters[index].getState() == 2 && boat.getState() == 2 && boat.getNumEmptyPos() > 0)   //人在右岸，船在右岸，船有空位
		{
			characters[index].setPositionOnBoat();
			myActionManager.moveCharacter(characters[index], boat.getEmptyPos());   
			rightBank.removeACharacter(characters[index]);
			boat.putACharacter(characters[index]);
		}
		else if (characters[index].getState() == 0 && boat.getState() == 1)       //人在船上，船靠左岸
		{
			characters[index].setPositionOnLeftBank();
			myActionManager.moveCharacter(characters[index], characters[index].getPosOnLeftBank());   
			boat.removeACharacter(characters[index]);
			leftBank.putACharacter(characters[index]);
		}
		else if (characters[index].getState() == 0 && boat.getState() == 2)       //人在船上，船靠右岸
		{
			characters[index].setPositionOnRightBank();
			myActionManager.moveCharacter(characters[index], characters[index].getPosOnRightBank()); 
			boat.removeACharacter(characters[index]);
			rightBank.putACharacter(characters[index]);
		}

		if (judgment.checkWin(boat, leftBank, rightBank) == true)
		{
			for (int i = 0; i < 7; i++)
			{
				clickGUI[i].click = false;

			}
			userGUI.gameState = 1;
			return;
		}
	}
	public void restart()
    {
		boat.reset();
		leftBank.reset();
		rightBank.reset();
		for (int i = 0; i < 6; i++)
		{
			characters[i].reset();
		}
		userGUI.gameState = 0;
		for (int i = 0; i < 7; i++)
		{
			clickGUI[i].click = true;

		}
	}
	#endregion


	// Use this for initialization
	void Start () {
		//give advice first
	}
	
		// Update is called once per frame
	void Update () {
		/*
		for (int i = 0; i < 7; i++)
		{
			clickGUI[i].moving = moving;

		}
		*/
	}

}
