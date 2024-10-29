using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    public int gameState = 0;  //0：游戏中  1：赢  -1：输
    public bool click = true;
    public bool moving = false;

    // Use this for initialization
    void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;

    }

    // Update is called once per frame
    void OnGUI()
    {
        if (gameState == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 40, 50, 100, 50), "You Win!");
        }
        else if (gameState == -1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 40, 50, 100, 50), "You Lose!");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 40, 300, 70, 30), "Restart"))
        {
            action.restart();
        }
    }

    void OnMouseDown()
    {
        if (click&&!SSDirector.getInstance().moving)
        {
            if (gameObject.name == "boat")
            {
                action.moveBoat();
            }
            else
            {
                string[] name = { "Devil", "Priest" };
                int index = gameObject.name[gameObject.name.Length - 1] - '0';
                for (int i = 0; i < 2; i++)
                {
                    if (gameObject.name.Substring(0, gameObject.name.Length - 1) == name[i])
                    {
                        index += 3 * i;
                    }
                }
                action.moveCharacter(index);
            }
        }

    }

}