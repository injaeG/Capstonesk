using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CarDriverAI1))]
public class CarDriverAIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CarDriverAI1 carDriverAI1 = (CarDriverAI1)target;

        if (GUILayout.Button("Respawn On Road"))
        {
            carDriverAI1.RespawnOnRoad();
        }
    }
}
