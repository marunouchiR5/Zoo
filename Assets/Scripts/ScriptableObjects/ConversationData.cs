using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConversationData", menuName = "Game Data/Conversation Data")]
public class ConversationData : ScriptableObject
{
    public List<ConversationElement> Elements;
}
