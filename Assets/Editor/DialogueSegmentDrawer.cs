using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(DialogueSegment))]
public class DialogueSegmentDrawer : PropertyDrawer
{
    private static Character[] cachedCharacters;
    private static string[] characterNames;

    private const string CHARACTER_FOLDER = "Assets/SO/Characters";

    private void CacheCharacters()
    {
        if (cachedCharacters != null) return;

        string[] guids = AssetDatabase.FindAssets(
            "t:Character",
            new[] { CHARACTER_FOLDER }
        );

        cachedCharacters = guids
            .Select(guid =>
                AssetDatabase.LoadAssetAtPath<Character>(
                    AssetDatabase.GUIDToAssetPath(guid)))
            .Where(c => c != null)
            .ToArray();

        characterNames = cachedCharacters
            .Select(c =>
                string.IsNullOrEmpty(c.CharacterName)
                    ? c.name
                    : c.CharacterName
            )
            .ToArray();
    }

    private static void ClearCache()
    {
        cachedCharacters = null;
        characterNames = null;
    }

    static DialogueSegmentDrawer()
    {
        EditorApplication.projectChanged += ClearCache;
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CacheCharacters();
        EditorGUI.BeginProperty(position, label, property);

        var characterProp = property.FindPropertyRelative("character");
        var dialogueProp = property.FindPropertyRelative("ActorDialogue");

        float line = EditorGUIUtility.singleLineHeight;
        float space = 4f;
        float y = position.y;

        // Character dropdown
        Rect charRect = new Rect(position.x, y, position.width, line);

        int currentIndex = Mathf.Max(
            0,
            System.Array.IndexOf(cachedCharacters, characterProp.objectReferenceValue)
        );

        if (cachedCharacters.Length == 0)
        {
            EditorGUI.LabelField(charRect, "Character", "No Characters Found");
        }
        else
        {
            int newIndex = EditorGUI.Popup(
                charRect,
                "Character",
                currentIndex,
                characterNames
            );

            characterProp.objectReferenceValue = cachedCharacters[newIndex];
        }

        y += line + space;

        // Actor Dialogue label
        Rect labelRect = new Rect(position.x, y, position.width, line);
        EditorGUI.LabelField(labelRect, "Actor Dialogue");
        y += line;

        // Actor Dialogue TextArea
        int minLines = 3;
        // int maxLines = 6;

        float textHeight = EditorStyles.textArea.lineHeight * minLines;
        Rect textRect = new Rect(position.x, y, position.width, textHeight);

        EditorGUI.BeginChangeCheck();
        string newText = EditorGUI.TextArea(textRect, dialogueProp.stringValue);
        if (EditorGUI.EndChangeCheck())
        {
            dialogueProp.stringValue = newText;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float space = 4f;

        int minLines = 3;
        float textHeight = EditorStyles.textArea.lineHeight * minLines;

        return
            line + space +          // Character
            line +                  // Label
            textHeight;             // TextArea
    }
}
