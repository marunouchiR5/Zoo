using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

public class RuleSetLoader : MonoBehaviour
{
    [MenuItem("Assets/Create/RuleSets")]
    public static void CreateRuleSets()
    {
        string path = "RuleTexts";
        TextAsset[] ruleTexts = Resources.LoadAll<TextAsset>(path);

        string assetPath = "Assets/Resources/GameData/RuleSets/";

        foreach (var textAsset in ruleTexts)
        {
            RuleSet newRuleSet = ScriptableObject.CreateInstance<RuleSet>();
            newRuleSet.Elements = ParseTextToConversationElements(textAsset.text);
            newRuleSet.SetName = textAsset.name;

            string fullPath = AssetDatabase.GenerateUniqueAssetPath(assetPath + newRuleSet.SetName + ".asset");
            AssetDatabase.CreateAsset(newRuleSet, fullPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static List<ConversationElement> ParseTextToConversationElements(string text)
    {
        var paragraphs = text.Split('\n');
        List<ConversationElement> elements = new List<ConversationElement>();

        foreach (var paragraph in paragraphs)
        {
            if (!string.IsNullOrWhiteSpace(paragraph))
            {
                bool startsWithOrdinal = Regex.IsMatch(paragraph.Trim(), @"^\d+\.");
                string speaker = startsWithOrdinal ? Regex.Match(paragraph.Trim(), @"^\d+").Value : "";
                string contentToParse = startsWithOrdinal ? Regex.Replace(paragraph.Trim(), @"^\d+\.\s*", "") : paragraph;

                // Split the paragraph into sentences, avoiding splits after ordinal numbers
                var sentences = Regex.Split(contentToParse, @"(?<!\d)\. ");
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i].Trim();
                    if (!string.IsNullOrWhiteSpace(sentence))
                    {
                        // Re-add the dot at the end of each sentence except the last one in a paragraph
                        if (i < sentences.Length - 1)
                        {
                            sentence += ".";
                        }

                        // Create a new ConversationElement for each sentence
                        var element = new ConversationElement(speaker, sentence);
                        elements.Add(element);
                    }
                }
            }
        }

        return elements;
    }
}
