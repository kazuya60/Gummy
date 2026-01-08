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
            .Select(c => string.IsNullOrEmpty(c.CharacterName) ? c.name : c.CharacterName)
            .ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CacheCharacters();

        EditorGUI.BeginProperty(position, label, property);

        var characterProp = property.FindPropertyRelative("character");
        var dialogueProp = property.FindPropertyRelative("ActorDialogue");

        float line = EditorGUIUtility.singleLineHeight;
        float space = 4f;

        Rect charRect = new Rect(position.x, position.y, position.width, line);

        int currentIndex = Mathf.Max(0,
            System.Array.IndexOf(cachedCharacters, characterProp.objectReferenceValue));

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

        Rect dialogueRect = new Rect(
            position.x,
            position.y + line + space,
            position.width,
            EditorGUI.GetPropertyHeight(dialogueProp)
        );

        EditorGUI.LabelField(
    new Rect(dialogueRect.x, dialogueRect.y, dialogueRect.width, EditorGUIUtility.singleLineHeight),
    "Actor Dialogue"
);

Rect textAreaRect = new Rect(
    dialogueRect.x,
    dialogueRect.y + EditorGUIUtility.singleLineHeight,
    dialogueRect.width,
    dialogueRect.height - EditorGUIUtility.singleLineHeight
);

dialogueProp.stringValue = EditorGUI.TextArea(
    textAreaRect,
    dialogueProp.stringValue
);


        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
{
    return EditorGUIUtility.singleLineHeight * 2 +
           EditorGUIUtility.singleLineHeight * 3 + 6f;
}

}
