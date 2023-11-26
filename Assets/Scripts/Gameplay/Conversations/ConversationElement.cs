using System.Collections.Generic;

[System.Serializable]
public class ConversationElement
{
    public string Speaker;
    public string Content;

    // Decision related properties
    public List<DecisionOption> Options;

    // Constructor for standard dialogue line
    public ConversationElement(string speaker, string content)
    {
        this.Speaker = speaker;
        this.Content = content;
    }

    // Constructor for decision point
    public ConversationElement(string speaker, string content, List<DecisionOption> options)
    {
        this.Speaker = speaker;
        this.Content = content;
        this.Options = options;
    }
}
