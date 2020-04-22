using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    Goal[] mGoals;
    Action[] mActions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 2.0f;

    void Start()
    {
        mGoals = new Goal[3];
        mGoals[0] = new Goal("Eat", 4);
        mGoals[1] = new Goal("Sleep", 3);
        mGoals[2] = new Goal("Bathroom", 3);

        mActions = new Action[6];
        mActions[0] = new Action("eat some raw food");
        mActions[0].targetGoals.Add(new Goal("Eat", -3f));
        mActions[0].targetGoals.Add(new Goal("Sleep", +2f));
        mActions[0].targetGoals.Add(new Goal("Bathroom", +1f));

        mActions[1] = new Action("eat a snack");
        mActions[1].targetGoals.Add(new Goal("Eat", -2f));
        mActions[1].targetGoals.Add(new Goal("Sleep", -1f));
        mActions[1].targetGoals.Add(new Goal("Bathroom", +1f));

        mActions[2] = new Action("sleep in the bed");
        mActions[2].targetGoals.Add(new Goal("Eat", +2f));
        mActions[2].targetGoals.Add(new Goal("Sleep", -4f));
        mActions[2].targetGoals.Add(new Goal("Bathroom", +2f));

        mActions[3] = new Action("sleep on the sofa");
        mActions[3].targetGoals.Add(new Goal("Eat", +1f));
        mActions[3].targetGoals.Add(new Goal("Sleep", -2f));
        mActions[3].targetGoals.Add(new Goal("Bathroom", +1f));

        mActions[4] = new Action("drink a soda");
        mActions[4].targetGoals.Add(new Goal("Eat", -1f));
        mActions[4].targetGoals.Add(new Goal("Sleep", -2f));
        mActions[4].targetGoals.Add(new Goal("Bathroom", +3f));

        mActions[5] = new Action("visit the bathroom");
        mActions[5].targetGoals.Add(new Goal("Eat", 0f));
        mActions[5].targetGoals.Add(new Goal("Sleep", 0f));
        mActions[5].targetGoals.Add(new Goal("Bathroom", -4f));

        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Eat", +4f));
        mChangeOverTime.targetGoals.Add(new Goal("Sleep", +1f));
        mChangeOverTime.targetGoals.Add(new Goal("Bathroom", +2f));

        InvokeRepeating("Tick", 0f, TICK_LENGTH);
    }

    void Tick()
    {
        foreach (Goal goal in mGoals)
        {
            goal.value += mChangeOverTime.getGoalChange(goal);
            goal.value = Mathf.Max(goal.value, 0);
            
        }
        Debug.Log("Tick.");
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Action bestThingToDo = chooseAction(mActions, mGoals);
            Debug.Log("I think I will " + bestThingToDo.name);

            foreach (Goal goal in mGoals)
            {
                goal.value += bestThingToDo.getGoalChange(goal);
                goal.value = Mathf.Max(goal.value, 0);
                Debug.Log(goal.name + ": " + goal.value);
            }
        }
    }

    Action chooseAction(Action[] actions, Goal[] goals)
    {
        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = discontentment(action, goals);
            
            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }

        return bestAction;
    }

    float discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0f;

        foreach (Goal goal in goals)
        {
            float newValue = goal.value + action.getGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);

            discontentment += goal.getDiscontentment(newValue);
        }

        return discontentment;
    }
}
