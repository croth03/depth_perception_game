#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using DepthPerception;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
internal static class FinalCriticalFix20261103
{
    private const string ScenePath = "Assets/DepthPerception/Scenes/Level01_Valley.unity";
    private const string AppliedMarker = "FINAL CRITICAL FIX 2026-11-03 APPLIED";
    private const string RockModelPath = "Assets/DepthPerception/Tripo/rock formation 01/tripo_convert_dd89129b-f771-4c28-8fe8-803149081289.fbx";
    private const string FigureModelPath = "Assets/DepthPerception/Tripo/ghostly humanoid 3d model/tripo_convert_3a90191e-edcf-48cf-9747-58e85f6e3668.fbx";
    private const string WalkClipPath = "Assets/DepthPerception/Animations/Player/TripoPlayer_Walk.anim";
    private const string FigureControllerPath = "Assets/DepthPerception/Animations/Figure/FigureApproach.controller";

    static FinalCriticalFix20261103()
    {
        EditorApplication.delayCall += ApplyWhenReady;
    }

    private static void ApplyWhenReady()
    {
        if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
        {
            EditorApplication.delayCall += ApplyWhenReady;
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        if (!scene.IsValid() || FindAll(scene).Any(g => g.name == AppliedMarker))
            return;

        int cloudLayers = FixClouds(scene);
        int treeSupports = FixTrees(scene);
        string geologicalObject = FixFloatingGeology(scene);
        int markerCount = FixMarkerGlow(scene);
        string walkClip = FixFigureAnimation(scene);
        int rockColliders = FixRockCollision(scene);

        var applied = new GameObject(AppliedMarker);
        applied.SetActive(false);
        EditorSceneManager.MoveGameObjectToScene(applied, scene);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log($"FINAL_CRITICAL_FIX_COMPLETE clouds={cloudLayers}; trees={treeSupports}; supported={treeSupports}; geology={geologicalObject}; markers={markerCount}; figureWalk={walkClip}; rockColliders={rockColliders}");
    }

    private static int FixClouds(Scene scene)
    {
        var positions = new Dictionary<string, Vector3>
        {
            ["Near Cloud Layer"] = new Vector3(0f, 15f, 104f),
            ["Far Cloud Layer"] = new Vector3(9f, 22f, 118f),
            ["High Cloud Layer"] = new Vector3(-8f, 30f, 132f)
        };

        int count = 0;
        foreach (GameObject layer in FindAll(scene).Where(g => positions.ContainsKey(g.name)))
        {
            layer.transform.position = positions[layer.name];
            layer.transform.rotation = Quaternion.identity;
            count++;
            foreach (Renderer renderer in layer.GetComponentsInChildren<Renderer>(true))
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }
        return count;
    }

    private static int FixTrees(Scene scene)
    {
        List<GameObject> trees = FindAll(scene)
            .Where(g => g.name == "Hallucination Echo Tree" || g.name == "Lost Echo Tree 01" || g.name == "Lost Echo Tree 02" || g.name == "Tree Silhouette 02")
            .Where(g => g.GetComponentInChildren<Renderer>(true) != null)
            .Take(6)
            .ToList();
        GameObject rockSource = AssetDatabase.LoadAssetAtPath<GameObject>(RockModelPath);
        int index = 0;
        foreach (GameObject tree in trees)
        {
            Bounds treeBounds = CombinedBounds(tree);
            tree.transform.position += Vector3.down * 0.22f;
            treeBounds = CombinedBounds(tree);
            if (rockSource == null)
                continue;

            GameObject support = (GameObject)PrefabUtility.InstantiatePrefab(rockSource, scene);
            support.name = $"Tree Support Rock - {tree.name} {++index:00}";
            support.transform.SetParent(tree.transform.parent, true);
            support.transform.rotation = Quaternion.Euler(4f + index * 2f, 29f * index, -3f);
            support.transform.localScale = Vector3.one;
            Bounds raw = CombinedBounds(support);
            float desiredWidth = Mathf.Clamp(treeBounds.size.x * 0.85f, 1.5f, 3.8f);
            float factor = desiredWidth / Mathf.Max(0.1f, Mathf.Max(raw.size.x, raw.size.z));
            support.transform.localScale = Vector3.one * factor;
            Bounds scaled = CombinedBounds(support);
            support.transform.position += new Vector3(
                treeBounds.center.x - scaled.center.x,
                treeBounds.min.y + 0.14f - scaled.max.y,
                treeBounds.center.z - scaled.center.z + (index % 2 == 0 ? 0.12f : -0.12f));
            foreach (Collider collider in support.GetComponentsInChildren<Collider>(true))
                UnityEngine.Object.DestroyImmediate(collider);
        }
        return index;
    }

    private static string FixFloatingGeology(Scene scene)
    {
        GameObject rock = FindAll(scene).FirstOrDefault(g => g.name == "Lost Echo Rock 01");
        if (rock == null)
            return "not found";
        rock.transform.position += new Vector3(-0.8f, -4.25f, 3.2f);
        rock.transform.rotation = Quaternion.Euler(7f, 24f, -11f);
        return rock.name;
    }

    private static int FixMarkerGlow(Scene scene)
    {
        int count = 0;
        foreach (PsychologicalMarker marker in FindAll(scene).SelectMany(g => g.GetComponents<PsychologicalMarker>()))
        {
            Transform overlay = marker.transform.Find("Marker Symbol Glow");
            if (overlay == null)
                continue;
            Renderer glow = overlay.GetComponent<Renderer>();
            Renderer stone = marker.GetComponentsInChildren<Renderer>(true).FirstOrDefault(r => r != glow);
            if (glow == null || stone == null)
                continue;

            Bounds b = stone.bounds;
            overlay.position = new Vector3(b.center.x, b.center.y + b.size.y * 0.04f, b.min.z - 0.012f);
            overlay.rotation = Quaternion.Euler(0f, 180f, 0f);
            float scale = Mathf.Clamp(Mathf.Min(b.size.x * 0.43f, b.size.y * 0.30f), 0.42f, 1.15f);
            overlay.localScale = Vector3.one * scale;
            glow.enabled = false;

            SerializedObject serialized = new SerializedObject(marker);
            serialized.FindProperty("symbolGlowRenderer").objectReferenceValue = glow;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            count++;
        }
        return count;
    }

    private static string FixFigureAnimation(Scene scene)
    {
        GameObject figure = FindAll(scene).FirstOrDefault(g => g.name == "Production Ghost Figure");
        AnimationClip walk = AssetDatabase.LoadAssetAtPath<AnimationClip>(WalkClipPath);
        if (figure == null || walk == null)
            return "unavailable";

        if (!AssetDatabase.IsValidFolder("Assets/DepthPerception/Animations/Figure"))
            AssetDatabase.CreateFolder("Assets/DepthPerception/Animations", "Figure");
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(FigureControllerPath);
        if (controller == null)
            controller = AnimatorController.CreateAnimatorControllerAtPathWithClip(FigureControllerPath, walk);

        Transform visual = figure.transform.Find("Ghostly Humanoid Visual");
        if (visual == null)
            visual = figure.GetComponentInChildren<SkinnedMeshRenderer>(true)?.transform.parent;
        Animator animator = visual != null ? visual.GetComponent<Animator>() : null;
        if (animator == null && visual != null)
            animator = visual.gameObject.AddComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = controller;
            animator.avatar = AssetDatabase.LoadAllAssetsAtPath(FigureModelPath).OfType<Avatar>().FirstOrDefault(a => a.isValid && a.isHuman);
            animator.applyRootMotion = false;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }

        FigureApproachWalkDriver driver = figure.GetComponent<FigureApproachWalkDriver>();
        if (driver == null)
            driver = figure.AddComponent<FigureApproachWalkDriver>();
        SerializedObject driverSerialized = new SerializedObject(driver);
        driverSerialized.FindProperty("figureAnimator").objectReferenceValue = animator;
        driverSerialized.ApplyModifiedPropertiesWithoutUndo();

        foreach (LineRenderer line in figure.GetComponentsInChildren<LineRenderer>(true))
            line.enabled = false;
        return walk.name;
    }

    private static int FixRockCollision(Scene scene)
    {
        GameObject existingRoot = FindAll(scene).FirstOrDefault(g => g.name == "GAMEPLAY COLLISION - Critical Rock Fix");
        if (existingRoot != null)
            UnityEngine.Object.DestroyImmediate(existingRoot);
        var collisionRoot = new GameObject("GAMEPLAY COLLISION - Critical Rock Fix");
        EditorSceneManager.MoveGameObjectToScene(collisionRoot, scene);

        string[] preferredNames = { "Rock Formation 08", "Rock Formation 09", "Rock Formation 10", "Rock Formation 11", "Rock Formation 12", "Rock Formation 13", "Rock Formation 14", "Rock Formation 15", "Passage Rock A", "Passage Rock B", "Passage Rock C", "Hallucination Echo Rock" };
        List<GameObject> candidates = FindAll(scene)
            .Where(g => preferredNames.Contains(g.name))
            .Where(g => g.GetComponentInChildren<Renderer>(true) != null)
            .Where(g => !g.GetComponentsInChildren<Collider>(true).Any(c => !c.isTrigger))
            .Select(g => new { Object = g, Bounds = CombinedBounds(g) })
            .Where(x => x.Bounds.size.x <= 8f && x.Bounds.size.z <= 8f && x.Bounds.min.y < 7.5f && x.Bounds.max.y > -0.5f)
            .OrderBy(x => x.Bounds.center.z)
            .ThenBy(x => x.Bounds.center.x)
            .Take(10)
            .Select(x => x.Object)
            .ToList();

        int count = 0;
        foreach (GameObject rock in candidates)
        {
            Bounds b = CombinedBounds(rock);
            var proxy = new GameObject($"Rock Collider - {rock.name} {++count:00}");
            proxy.transform.SetParent(collisionRoot.transform, false);
            proxy.transform.position = b.center;
            BoxCollider box = proxy.AddComponent<BoxCollider>();
            float yScale = b.size.y < 1.2f ? 0.62f : 0.84f;
            box.size = new Vector3(Mathf.Max(0.35f, b.size.x * 0.82f), Mathf.Max(0.25f, b.size.y * yScale), Mathf.Max(0.35f, b.size.z * 0.82f));
            box.center = Vector3.zero;
        }
        return count;
    }

    private static IEnumerable<GameObject> FindAll(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                yield return child.gameObject;
    }

    private static Bounds CombinedBounds(GameObject root)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true).Where(r => r.enabled).ToArray();
        if (renderers.Length == 0)
            return new Bounds(root.transform.position, Vector3.one);
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);
        return bounds;
    }
}
#endif
