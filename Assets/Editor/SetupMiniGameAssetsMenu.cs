using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SetupMiniGameAssetsMenu : Editor
{
    [MenuItem("Tools/Setup MiniGame Models & Animator")]
    public static void SetupAssets()
    {
        // 1. Cration du BlendTree et de l'Animator Controller pour le Hunter
        string controllerPath = "Assets/Game/3D/Pawns/Hunter/AC_Hunter_MiniGame.controller";
        
        AnimatorController controller = null;
        if (!File.Exists(controllerPath))
        {
            controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            
            // Paramtres
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            controller.AddParameter("Attack", AnimatorControllerParameterType.Trigger);

            // Rcuprer les animations
            string pathWalk = "Assets/Game/3D/Pawns/Hunter/Crouch Walk Forward.fbx";
            string pathCrouch = "Assets/Game/3D/Pawns/Hunter/Crouching.fbx";
            string pathDash = "Assets/Game/3D/Pawns/Hunter/Pontera.fbx";

            AnimationClip clipWalk = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathWalk);
            AnimationClip clipCrouch = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathCrouch);
            AnimationClip clipDash = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathDash);

            var rootStateMachine = controller.layers[0].stateMachine;

            // Cration du BlendTree
            BlendTree blendTree;
            AnimatorState blendTreeState = controller.CreateBlendTreeInController("Locomotion", out blendTree);
            blendTree.blendType = BlendTreeType.Simple1D;
            blendTree.blendParameter = "Speed";

            // Ajouter les motions au blend tree
            if(clipCrouch != null) blendTree.AddChild(clipCrouch, 0f);
            if(clipWalk != null) blendTree.AddChild(clipWalk, 1f);

            // Cration de l'tat d'attaque
            AnimatorState attackState = rootStateMachine.AddState("Pontera");
            if(clipDash != null) attackState.motion = clipDash;

            // Transition
            AnimatorStateTransition transition = blendTreeState.AddTransition(attackState);
            transition.AddCondition(AnimatorConditionMode.If, 0, "Attack");
            transition.duration = 0.1f;
            
            // Retour possible vers locomation (optionnel, on pilote par C#)
            AnimatorStateTransition backTransition = attackState.AddTransition(blendTreeState);
            backTransition.hasExitTime = true;
            backTransition.exitTime = 1f;
            backTransition.duration = 0.1f;

            Debug.Log("Hunter Animator Controller gnr avec succs au niveau de Blend Tree.");
        }
        else
        {
            controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        }

        // 2. Assigner les modles dans la scne actuellement ouverte
        GameObject playerObj = GameObject.Find("MiniGamePlayer");
        if (playerObj != null)
        {
            // Retirer le mesh renderer temporaire si on avait mis un cube/capsule
            var mr = playerObj.GetComponent<MeshRenderer>();
            var mf = playerObj.GetComponent<MeshFilter>();
            if (mr) DestroyImmediate(mr);
            if (mf) DestroyImmediate(mf);

            // Rcuprer le script
            S_MiniGamePlayer scriptP = playerObj.GetComponent<S_MiniGamePlayer>();

            // Instancier le modle Gekko comme enfant
            if (playerObj.transform.Find("Gecko") == null)
            {
                GameObject geckoPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game/3D/Pawns/Gecko/Gecko.prefab");
                if (geckoPrefab != null)
                {
                    GameObject g = PrefabUtility.InstantiatePrefab(geckoPrefab, playerObj.transform) as GameObject;
                    g.name = "Gecko";
                }
            }
        }

        GameObject hunterObj = GameObject.Find("HunterAI");
        if (hunterObj != null)
        {
            var mr = hunterObj.GetComponent<MeshRenderer>();
            var mf = hunterObj.GetComponent<MeshFilter>();
            if (mr) DestroyImmediate(mr);
            if (mf) DestroyImmediate(mf);

            if (hunterObj.transform.Find("Hunter_Mesh") == null && hunterObj.transform.Find("Hunter") == null)
            {
                // Trouver le mche/fbx du hunter sans les anims
                string[] guids = AssetDatabase.FindAssets("Crouching t:Model", new[] { "Assets/Game/3D/Pawns/Hunter" });
                if (guids.Length > 0)
                {
                    GameObject hunterModel = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[0]));
                    GameObject h = PrefabUtility.InstantiatePrefab(hunterModel, hunterObj.transform) as GameObject;
                    h.name = "Hunter_Mesh";
                    
                    // Ajouter / Configurer l'Animator
                    Animator anim = h.GetComponent<Animator>();
                    if (anim == null) anim = h.AddComponent<Animator>();
                    anim.runtimeAnimatorController = controller;
                }
            }
        }
        
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("Assets (Gekko + Hunter BlendTree) assigns dans la MiniGameScene !");
    }
}
