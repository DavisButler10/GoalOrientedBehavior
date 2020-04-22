using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal 
{
    public string name;
    public float value;

    public Goal(string goalName, float goalValue)
    {
        name = goalName;
        value = goalValue;
    }

    public float getDiscontentment(float newValue)
    {
        return newValue * newValue;
    }
}
