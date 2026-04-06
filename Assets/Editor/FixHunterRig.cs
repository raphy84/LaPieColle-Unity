using UnityEditor;
using UnityEngine;
using System.IO;

public class FixHunterRig : EditorWindow
{
    [MenuItem("Tools/Fix Hunter Rig (MiniGame)")]
    public static void FixHunterAvatarMapping()
    {
        string[] paths = new string[]
        {
            "Assets/Game/3D/Pawns/Hunter/Crouch Walk Forward.fbx",
            "Assets/Game/3D/Pawns/Hunter/Crouching.fbx",
            "Assets/Game/3D/Pawns/Hunter/Pontera.fbx"
        };

        foreach (string path in paths)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning("File not found: " + path);
                continue;
            }

            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null)
            {
                importer.animationType = ModelImporterAnimationType.Human;
                
                // We use SerializedObject to access the hidden m_HumanDescription to modify the mapping that gets auto-generated
                SerializedObject serializedObject = new SerializedObject(importer);
                SerializedProperty humanDescription = serializedObject.FindProperty("m_HumanDescription");
                SerializedProperty human = humanDescription.FindPropertyRelative("m_Human");

                // If m_Human is empty, Unity hasn't generated the explicit mapping block yet.
                // Reimport once as Humanoid to let Unity generate the default Avatar mapped data internally
                // Or we can manually force Jaw to be unmapped by supplying an explicit array
                // For simplicity, we just clear the Jaw mapping if it exists in the m_Human array
                bool modified = false;
                for (int i = 0; i < human.arraySize; i++)
                {
                    SerializedProperty bone = human.GetArrayElementAtIndex(i);
                    SerializedProperty humanName = bone.FindPropertyRelative("m_HumanName");
                    SerializedProperty boneName = bone.FindPropertyRelative("m_BoneName");

                    if (humanName.stringValue == "Jaw")
                    {
                        if (!string.IsNullOrEmpty(boneName.stringValue))
                        {
                            boneName.stringValue = ""; // Clear mapping
                            modified = true;
                        }
                    }
                }

                // Actually, if it's auto-generated, m_Human is empty. To fix it, we override the AvatarSetup to CreateFromThisModel 
                // and parse the generated bones. But the most reliable programmatic way is to uncheck Optimize or extract.
                // Unity 2020+ allows retrieving the HumanDescription:
                /* 
                // Unfortunately API doesn't let us modify HumanDescription easily so we use a dirty trick, reading the auto-mapped and removing Jaw:
                */
                // The issue is mixamorig:LeftEye mapped to both LeftEye and Jaw. Let's explicitly define Jaw as empty:
                if (human.arraySize == 0)
                {
                    // Let's create a minimal map overriding the Jaw
                    human.arraySize = 1;
                    SerializedProperty bone = human.GetArrayElementAtIndex(0);
                    bone.FindPropertyRelative("m_HumanName").stringValue = "Jaw";
                    bone.FindPropertyRelative("m_BoneName").stringValue = "";
                    modified = true;
                }

                if (modified)
                {
                    serializedObject.ApplyModifiedProperties();
                }

                importer.SaveAndReimport();
                Debug.Log($"Fixed rig for {path}");
            }
        }
        Debug.Log("Hunter Rig Fix complete. Please check the Avatar mapping in the FBX import settings (Jaw should be None).");
    }
}
