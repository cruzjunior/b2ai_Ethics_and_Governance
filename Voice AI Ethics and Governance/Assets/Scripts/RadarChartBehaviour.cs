using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class RadarChartBehaviour : MonoBehaviour
{
    public RadarPolygon radarPolygon;
    // Start is called before the first frame update
    private Dictionary<string, float> conflictValuesDict = new Dictionary<string, float>();
    private Dictionary<string, int> chosenValuesDict = new Dictionary<string, int>();
    void Start()
    {
        chosenValuesDict = StoryManager.Instance.GetChosenValues();
        conflictValuesDict = StoryManager.Instance.GetConflictValues();
        radarPolygon.value = new float[conflictValuesDict.Count];

        int i = 0;
        foreach (KeyValuePair<string, int> entry in chosenValuesDict)
        {
            if (conflictValuesDict.ContainsKey(entry.Key))
            {
                conflictValuesDict[entry.Key] /= entry.Value;
            }
        }

        foreach (KeyValuePair<string, float> entry in conflictValuesDict)
        {
            Debug.Log(entry.Key + " " + entry.Value);
            if(entry.Value == 0)
            {
                radarPolygon.value[i] = 0.05f;
                i++;
                continue;
            }
            radarPolygon.value[i] = entry.Value;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
