
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DepthPerception.EditorTools
{
 public static class Level01FinalizeCleanup
 {
  [MenuItem("Depth Perception/Level 01/Finalize Cleanup Retry")]
  public static void Apply()
  {
   int missing=0,markers=0,birds=0;
   foreach(var marker in Object.FindObjectsByType<PsychologicalMarker>(FindObjectsInactive.Include))
   {
    missing+=GameObjectUtility.RemoveMonoBehavioursWithMissingScript(marker.gameObject); foreach(var c in marker.GetComponents<MonoBehaviour>()){if(c!=null&&c.GetType().Assembly.GetName().Name.StartsWith("PipelineEvaluation")){Object.DestroyImmediate(c);missing++;}}
    var symbol=marker.transform.Find("Marker Symbol Glow");
    if(symbol==null)continue;
    var so=new SerializedObject(marker); var gp=so.FindProperty("symbolGlowRenderer"); if(gp!=null){gp.objectReferenceValue=symbol.GetComponent<Renderer>();so.ApplyModifiedPropertiesWithoutUndo();}
    markers++;
   }
   foreach(var r in Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include))
   {
    if(r.name.IndexOf("Bird",System.StringComparison.OrdinalIgnoreCase)<0)continue;
    Transform t=r.transform;while(t!=null){t.gameObject.SetActive(true);if(t.GetComponent<DepthBirdFlock>()!=null)break;t=t.parent;}birds++;
   }
   EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());EditorSceneManager.SaveOpenScenes();AssetDatabase.SaveAssets();
   Debug.Log("LEVEL01_FINALIZE missingRemoved="+missing+" markers="+markers+" birdRenderers="+birds);
  }
 }
}




