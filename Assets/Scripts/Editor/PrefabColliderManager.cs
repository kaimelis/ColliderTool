using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PrefabColliderManager
    {
        private readonly List<PrefabColliderGroup> m_prefabGroups;

        public PrefabColliderManager()
        {
            m_prefabGroups = LoadAllMeshPrefabs();
        }
        
        public void OnGUI(int indent)
        {
            foreach (PrefabColliderGroup prefabGroup in m_prefabGroups)
            {
                prefabGroup.OnGUI(indent);
            }
        }
        
        public void ExpandAll(bool value)
        {
            foreach (PrefabColliderGroup prefabGroup in m_prefabGroups)
            {
                prefabGroup.FoldOutEnable = value;
            }
        }

        public void AddCollidersToAll()
        {
            foreach (PrefabColliderGroup prefabGroup in m_prefabGroups)
            {
                prefabGroup.AddCollidersToAll();
            }
        }


        private List<PrefabColliderGroup> LoadAllMeshPrefabs()
        {
            List<PrefabColliderGroup> prefabGroups = new List<PrefabColliderGroup>();
            DirectoryInfo dirInfo = new DirectoryInfo(EditorWindowConstants.PREFABS_PATH);
            FileInfo[] filesInfo = dirInfo.GetFiles("*.prefab", SearchOption.AllDirectories);
            foreach (FileInfo fileInfo in filesInfo)
            {
                string fullPath = fileInfo.FullName.Replace(@"\", "/");
                string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

                if (prefab != null)
                {
                    List<Component> prefabComponents = GetAllChildrenOfMeshTypes(prefab.transform);

                    if (prefabComponents.Count > 0)
                    {
                        List<PrefabColliderNode> prefabNodes = new List<PrefabColliderNode>();
                        foreach (Component component in prefabComponents)
                        {
                            prefabNodes.Add(new PrefabColliderNode(component));
                        }
                        prefabGroups.Add(new PrefabColliderGroup(prefab,prefabNodes));
                    }
                }
            }
            

            return prefabGroups;
        }
        
        private List<Component> GetAllChildrenOfMeshTypes(Transform obj)
        {
            List<Component> components = new List<Component>();
            ProcessChild(obj, ref components);

            return components;
        }

        private void ProcessChild(Transform obj, ref List<Component> components)
        {

            Component component = obj.GetComponent<MeshRenderer>();
            if (component != null)
            {
                components.Add(component);
            }
            

            foreach (Transform child in obj)
            {
                ProcessChild(child, ref components);
            }
        }
        
        

    }
}
