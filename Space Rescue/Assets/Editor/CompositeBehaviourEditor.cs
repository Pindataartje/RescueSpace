using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompositeBehaviour))]
public class CompositeBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Setup
        CompositeBehaviour cb = (CompositeBehaviour)target;

        // Check for behaviors
        if (cb.behaviors == null || cb.behaviors.Length == 0)
        {
            EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
        }
        else
        {
            // Create a horizontal layout for behaviors and weights
            EditorGUILayout.LabelField("Behaviors", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Weights", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < cb.behaviors.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                cb.behaviors[i] = (RobotBehaviour)EditorGUILayout.ObjectField(cb.behaviors[i], typeof(RobotBehaviour), false);
                cb.weights[i] = EditorGUILayout.FloatField(cb.weights[i]);
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb);
            }
        }

        // Add Behavior Button
        if (GUILayout.Button("Add Behavior"))
        {
            AddBehavior(cb);
            EditorUtility.SetDirty(cb);
        }

        // Remove Behavior Button
        if (cb.behaviors != null && cb.behaviors.Length > 0)
        {
            if (GUILayout.Button("Remove Behavior"))
            {
                RemoveBehavior(cb);
                EditorUtility.SetDirty(cb);
            }
        }
    }

    void AddBehavior(CompositeBehaviour cb)
    {
        int oldCount = (cb.behaviors != null) ? cb.behaviors.Length : 0;
        RobotBehaviour[] newBehaviors = new RobotBehaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];

        if (cb.behaviors != null)
        {
            for (int i = 0; i < oldCount; i++)
            {
                newBehaviors[i] = cb.behaviors[i];
                newWeights[i] = cb.weights[i];
            }
        }

        newWeights[oldCount] = 1f; // Default weight for the new behavior
        cb.behaviors = newBehaviors;
        cb.weights = newWeights;
    }

    void RemoveBehavior(CompositeBehaviour cb)
    {
        int oldCount = cb.behaviors.Length;
        if (oldCount == 1)
        {
            cb.behaviors = null;
            cb.weights = null;
            return;
        }

        RobotBehaviour[] newBehaviors = new RobotBehaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];

        // Copy all but the last element
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviors[i] = cb.behaviors[i];
            newWeights[i] = cb.weights[i];
        }

        cb.behaviors = newBehaviors;
        cb.weights = newWeights;
    }
}