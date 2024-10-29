using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour { 
    private Dictionary<int, Action> actions = new Dictionary<int, Action>();
    private List<Action> waitingToAdd = new List<Action>();
    private List<int> watingToDelete = new List<int>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        foreach (Action ac in waitingToAdd)
        {
            actions[ac.GetInstanceID()] = ac;
        }
        waitingToAdd.Clear();

        foreach (KeyValuePair<int, Action> kv in actions)
        {
            Action ac = kv.Value;
            if (ac.destroy)
            {
                watingToDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        foreach (int key in watingToDelete)
        {
            Action ac = actions[key];
            actions.Remove(key);
            Destroy(ac);
        }
        watingToDelete.Clear();
    }
    public void RunAction(GameObject gameObject, Action action, IActionCallback manager)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingToAdd.Add(action);
        action.Start();
    }
}