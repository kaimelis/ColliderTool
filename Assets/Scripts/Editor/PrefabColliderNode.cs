using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PrefabColliderNode
    {
        private readonly Component m_component;
        
        private Collider m_collider;
        private MeshCollider m_meshCollider;
        
        private string[] m_options = new string[] {"Mesh", "Box", "Sphere"};
        private int m_selectedIndex = 0;
        private bool m_hasConvexEnabled = false;
        
        public PrefabColliderNode(Component textComponent)
        {
            m_component = textComponent;
            if (m_component == null) return;

            m_collider = m_component.GetComponent<Collider>();
            
            if (m_collider.GetType() == typeof(MeshCollider))
            {
                m_meshCollider = (MeshCollider)m_collider;
                m_hasConvexEnabled = m_meshCollider.convex;
            }
        }

        public void OnGUI(int indentationMultiply, int indent)
        {
            if (m_component == null) return;
            GUILayout.BeginHorizontal();
            
            GUILayout.Space((indentationMultiply + 1) * indent);
            EditorGUILayout.ObjectField(m_component, m_component.GetType(), false);
            EditorGUILayout.ObjectField(m_collider, m_collider.GetType(), false);
            
             if (Event.current.type.Equals(EventType.MouseUp) && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
             {
                 Selection.activeGameObject = m_component.gameObject;
             }
             
             if (m_meshCollider != null && !m_hasConvexEnabled)
             {
                 if (GUILayout.Button("Enable Convex", GUILayout.Width(EditorWindowConstants.ELEMENTS_WIDTH), GUILayout.Height(EditorWindowConstants.BUTTON_HEIGHT)))
                 {
                     EnableConvexInCollider();
                 }
             }
             
             if (m_collider == null)
             {
                 GUI.skin.button.fontSize = EditorWindowConstants.BUTTON_FONT_SIZE;
                 //have option to choose which collider ya want to add
                 m_selectedIndex = EditorGUILayout.Popup(m_selectedIndex, m_options);
                 if (GUILayout.Button("Add Collider", GUILayout.Width(EditorWindowConstants.ELEMENTS_WIDTH), GUILayout.Height(EditorWindowConstants.BUTTON_HEIGHT)))
                 {
                     TryAddColliderToPrefab();
                 }
                 GUI.skin.button.fontSize = EditorWindowConstants.DEFAULT_FONT_SIZE;
             }
             
             GUILayout.EndHorizontal();
        }

        public void AddCollidersToAll()
        {
            if (m_collider != null) return;
            TryAddColliderToPrefab();
        }


        private void EnableConvexInCollider()
        {
            m_meshCollider.convex = m_hasConvexEnabled = true;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void TryAddColliderToPrefab()
        {
            switch (m_selectedIndex)
            {
                case 0:
                    m_meshCollider = m_component.gameObject.AddComponent<MeshCollider>();
                    m_meshCollider.convex = m_hasConvexEnabled = true;
                    m_collider = m_meshCollider;
                    break;
                case 1:
                    m_collider = m_component.gameObject.AddComponent<BoxCollider>();
                    break;
                case 2:
                    m_collider = m_component.gameObject.AddComponent<SphereCollider>();
                    break;
                default:
                    m_meshCollider = m_component.gameObject.AddComponent<MeshCollider>();
                    m_meshCollider.convex = m_hasConvexEnabled = true;
                    m_collider = m_meshCollider;
                    break;
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
