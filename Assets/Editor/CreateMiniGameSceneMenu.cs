using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupMiniGameMenu : Editor
{
    [MenuItem("Tools/Generate MiniGame Scene")]
    public static void GenerateScene()
    {
        string scenePath = "Assets/Game/Scenes/MiniGameScene.unity";
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // -- Terrain --
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(3, 1, 3);
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if(mat != null) mat.color = new Color(0.2f, 0.2f, 0.2f);
        floor.GetComponent<Renderer>().sharedMaterial = mat != null ? mat : new Material(Shader.Find("Standard"));

        // -- Game Manager --
        GameObject managerGO = new GameObject("MiniGameManager");
        var managerGroup = managerGO.AddComponent<S_MiniGameManager>();
        
        string[] guids = AssetDatabase.FindAssets("t:SO_PlayerDatas");
        if(guids.Length > 0 && managerGroup != null)
        {
            SO_PlayerDatas data = AssetDatabase.LoadAssetAtPath<SO_PlayerDatas>(AssetDatabase.GUIDToAssetPath(guids[0]));
            var prop = typeof(S_MiniGameManager).GetField("_playerDatas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if(prop != null) prop.SetValue(managerGroup, data);
        }

        // -- Player --
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "MiniGamePlayer";
        player.transform.position = new Vector3(-10, 1, 0);
        player.tag = "Player";
        var playerScript = player.AddComponent<S_MiniGamePlayer>();
        if(playerScript != null && managerGroup != null)
        {
            var propPlayer = typeof(S_MiniGamePlayer).GetField("gameManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if(propPlayer != null) propPlayer.SetValue(playerScript, managerGroup);
        }

        // -- Hunter --
        GameObject hunter = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hunter.name = "HunterAI";
        hunter.transform.position = new Vector3(0, 1, 0);
        hunter.GetComponent<Renderer>().sharedMaterial.color = Color.red;

        var hunterScript = hunter.AddComponent<S_MiniGameHunterAI>();
        if(hunterScript != null && managerGroup != null)
        {
            var propTran = typeof(S_MiniGameHunterAI).GetField("playerTransform", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if(propTran != null) propTran.SetValue(hunterScript, player.transform);
            
            var propMgr = typeof(S_MiniGameHunterAI).GetField("miniGameManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if(propMgr != null) propMgr.SetValue(hunterScript, managerGroup);
        }

        // -- Finish Line --
        GameObject finish = GameObject.CreatePrimitive(PrimitiveType.Cube);
        finish.name = "GoalArea";
        finish.transform.position = new Vector3(10, 1, 0);
        finish.transform.localScale = new Vector3(2, 2, 10);
        finish.tag = "Finish";
        finish.GetComponent<Collider>().isTrigger = true;
        finish.GetComponent<Renderer>().sharedMaterial.color = Color.green;

        // -- Camera --
        Camera cam = Camera.main;
        if(cam != null)
        {
            cam.transform.position = new Vector3(0, 15, -10);
            cam.transform.rotation = Quaternion.Euler(60, 0, 0);
        }

        EditorSceneManager.SaveScene(newScene, scenePath);
        Debug.Log("MiniGameScene generated at: " + scenePath);

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        bool exists = false;
        foreach (var s in scenes)
        {
            if (s.path == scenePath) { exists = true; break; }
        }
        if (!exists)
        {
            var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            System.Array.Copy(scenes, newScenes, scenes.Length);
            newScenes[scenes.Length] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = newScenes;
        }
    }
}
