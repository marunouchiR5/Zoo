using UnityEngine;

[CreateAssetMenu(fileName = "NewTask", menuName = "Game Data/Task")]
public class Task : ScriptableObject
{
    public string Name;
    public int Progress;
    public int ProgressGoal;
    public bool HasProgress;
}
