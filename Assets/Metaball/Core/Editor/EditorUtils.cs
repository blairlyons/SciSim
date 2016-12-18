//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class EditorUtils
{
    [MenuItem("Metaball/Save Prefab")]
    public static void SaveMetaballAsPrefab()
    {
        SaveMetaball(false);
    }

    [MenuItem("Metaball/Save Mesh")]
    public static void SaveMetaballMesh()
    {
        SaveMetaball(true);
    }

    static void SaveMetaball( bool bMeshOnly )
    {
        Object [] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);

        string defaultPath = "Assets";
        if( selection != null && selection.Length > 0 )
        {
            defaultPath = AssetDatabase.GetAssetPath(selection[0]);
        }

        GameObject go = Selection.activeGameObject;
        ImplicitSurfaceMeshCreaterBase seed = Utils.FindComponentInParents<ImplicitSurfaceMeshCreaterBase>(go.transform);

        string path = EditorUtility.SaveFilePanel("select folder and input filename", defaultPath, seed.gameObject.name, "");
        string assetsRoot = Application.dataPath;
        if (path.StartsWith(assetsRoot))
        {
            path = path.Remove(0, assetsRoot.Length);
        }

        path = "Assets" + path;

        string meshPath = path + "Mesh.asset";// folder + "/" + seed.gameObject.name + "Mesh.asset";

        Mesh newMesh = Object.Instantiate<Mesh>(seed.Mesh);
        AssetDatabase.CreateAsset( newMesh, meshPath);


        if (!bMeshOnly)
        {
            string prefabPath = path + ".prefab";            
            GameObject newPrefab = PrefabUtility.CreatePrefab(prefabPath, seed.gameObject, ReplacePrefabOptions.ReplaceNameBased);
            newPrefab.GetComponent<ImplicitSurfaceMeshCreaterBase>().Mesh = newMesh;

            Selection.activeObject = newPrefab;
        }
        else
        {
            Selection.activeObject = newMesh;
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Metaball/RebuildMesh %#r")]
    public static void RebuildMetaballMesh()
    {
        GameObject go = Selection.activeGameObject;
        ImplicitSurfaceMeshCreaterBase generator = Utils.FindComponentInParents<ImplicitSurfaceMeshCreaterBase>(go.transform);

        if (generator != null)
        {
            generator.CreateMesh();
        }
    }

    [MenuItem("Metaball/Save Prefab", true)]
    [MenuItem("Metaball/Save Mesh", true)]
    [MenuItem("Metaball/RebuildMesh %#r", true)]
    public static bool IsMetaballSeedSelected()
    {
        GameObject go = Selection.activeGameObject;
        return go != null && Utils.FindComponentInParents<ImplicitSurfaceMeshCreaterBase>(go.transform) != null;
    }


    [MenuItem("Metaball/DrawWithBrush %#e", true)]
    public static bool IsIMBrushSelected()
    {
        GameObject go = Selection.activeGameObject;
        return go != null && go.GetComponent<IMBrush>() != null;
    }

    [MenuItem("Metaball/DrawWithBrush %#e")]
    public static void DrawWithBrush()
    {
        GameObject go = Selection.activeGameObject;
        IMBrush brush = go.GetComponent<IMBrush>();

        brush.Draw();
    }

    [MenuItem("Metaball/CreateChild %#e")]
    public static void CreateChildNode()
    {
        GameObject go = Selection.activeGameObject;

        float baseRadius = 1.0f;

        MetaballSeedBase seed = Utils.FindComponentInParents<MetaballSeedBase>(go.transform);

        if (seed != null)
        {
            baseRadius = seed.baseRadius;
        }

        GameObject child = new GameObject("MetaballNode");
        child.transform.parent = go.transform;
        child.transform.localPosition = Vector3.zero;
        child.transform.localScale = Vector3.one;
        child.transform.localRotation = Quaternion.identity;

        MetaballNode newNode = child.AddComponent<MetaballNode>();
        newNode.baseRadius = baseRadius;

        Selection.activeGameObject = child;
    }
    [MenuItem("Metaball/CreateChild %#e", true)]
    public static bool IsMetaballNodeSelected()
    {
        GameObject go = Selection.activeGameObject;
        return go != null && go.GetComponent<MetaballNode>() != null;
    }
}
