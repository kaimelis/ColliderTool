using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    public class PrefabColliderGroup
    {
        public bool FoldOutEnable { get; set; } = false;
        private readonly GameObject m_prefab;
        private readonly List<PrefabColliderNode> m_prefabNodes;

        public PrefabColliderGroup(GameObject prefab, List<PrefabColliderNode> prefabNodes)
        {
            m_prefab = prefab;
            m_prefabNodes = prefabNodes;
        }

        public void OnGUI(int indent)
        {
            if (m_prefab == null) return;

            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);

            Rect foldoutRect = GUILayoutUtility.GetLastRect();
            foldoutRect.width = 30;
            foldoutRect.height = 20;
            foldoutRect.position += new Vector2(indent, 0);

            GUILayout.Space(indent);
            FoldOutEnable = EditorGUI.Foldout(foldoutRect, FoldOutEnable, "");
            EditorGUILayout.ObjectField(m_prefab, typeof(GameObject), false, GUILayout.ExpandWidth(true));

            GUILayout.EndHorizontal();

            if (!FoldOutEnable) return;
            EditorGUI.BeginDisabledGroup(false);
            foreach (PrefabColliderNode prefabNode in m_prefabNodes)
            {
                prefabNode.OnGUI(2, indent);
            }

            EditorGUI.EndDisabledGroup();
        }

        public void AddCollidersToAll()
        {
            foreach (PrefabColliderNode prefabNode in m_prefabNodes)
            {
                prefabNode.AddCollidersToAll();
            }
        }
    }
}
