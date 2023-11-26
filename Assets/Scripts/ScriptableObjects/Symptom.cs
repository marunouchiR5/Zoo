using UnityEngine;

[CreateAssetMenu(fileName = "NewSymptom", menuName = "Game Data/Symptom")]
public class Symptom : ScriptableObject
{
    public string Name;

    public Symptom(string name)
    {
        Name = name;
    }

    // Add methods related to symptoms if needed
}
