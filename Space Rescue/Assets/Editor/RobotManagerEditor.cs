using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RobotManager))]
public class RobotManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RobotManager squadManager = (RobotManager)target;

        // Check if the squad manager has squads
        if (squadManager.RobotsInSquad.Count == 0)
        {
            EditorGUILayout.LabelField("No squads available.");
            return;
        }

        // Iterate through each squad
        for (int i = 0; i < squadManager.RobotsInSquad.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Squad {i + 1}", EditorStyles.boldLabel);

            // Check if the current squad has robots
            if (squadManager.RobotsInSquad[i].Count == 0)
            {
                EditorGUILayout.LabelField("No robots in this squad.");
            }
            else
            {
                // Iterate through each RobotAI in the current squad
                for (int j = 0; j < squadManager.RobotsInSquad[i].Count; j++)
                {
                    RobotAI robotAI = squadManager.RobotsInSquad[i][j];

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Robot {j + 1}: Name = {robotAI.name}");
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        // Save any changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(squadManager);
        }
    }
}