using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ColliderEditorWIndow : EditorWindow
    {
        private bool m_showPrefabsTexts = true;
        private Vector2 m_scrollPos = Vector2.zero;
        private Texture2D m_refreshIcon;

        private PrefabColliderManager m_prefabColliderManager;
        
        
        [MenuItem("Custom Tool/Collider Inspector", priority = 10315)]
        private static void Init()
        {
            ColliderEditorWIndow window = (ColliderEditorWIndow)EditorWindow.GetWindow(typeof(ColliderEditorWIndow));
            window.titleContent = new GUIContent("Collider Inspector", EditorGUIUtility.FindTexture("BuildSettings.Web.Small"));
            window.minSize =  new Vector2(215f, 110f);
            window.Show();
        }
        
        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            RefreshPrefabList();
            m_refreshIcon = EditorGUIUtility.FindTexture("LookDevResetEnv");
        }

        private void OnDisable()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        private void OnGUI()
        {
            if (GUILayout.Button("Add Colliders To All prefabs", GUILayout.Height(EditorWindowConstants.BUTTON_HEIGHT)))
            {
                //maybe do a pop up to be sure they say yes?
                bool answer = EditorUtility.DisplayDialog("Are you sure?","Are you sure you want to add colliders to all prefabs?","YES", "NO");
                if (!answer) return;
                
                m_prefabColliderManager.AddCollidersToAll();
            }

            m_scrollPos = GUILayout.BeginScrollView(m_scrollPos);
            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorWindowConstants.FOLDOUT_INDENTATION);
            GUI.skin.button.fontSize = EditorWindowConstants.BUTTON_FONT_SIZE;
            if (GUILayout.Button(new GUIContent(" Refresh Prefabs", m_refreshIcon), GUILayout.Height(EditorWindowConstants.BUTTON_HEIGHT)))
            {
                RefreshPrefabList();
            }
            if (GUILayout.Button("Expand All", GUILayout.Height(EditorWindowConstants.BUTTON_HEIGHT)))
            {
                m_prefabColliderManager.ExpandAll(true);
            }
            if (GUILayout.Button("Collapse All", GUILayout.Height(EditorWindowConstants.BUTTON_HEIGHT)))
            {
                m_prefabColliderManager.ExpandAll(false);
            }
            GUI.skin.button.fontSize = EditorWindowConstants.DEFAULT_FONT_SIZE;

            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            m_prefabColliderManager.OnGUI(EditorWindowConstants.FOLDOUT_INDENTATION);
            
           EditorGUILayout.Space();
           GUILayout.EndScrollView();
        }

        private void RefreshPrefabList()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            m_prefabColliderManager = new PrefabColliderManager();
        }
    }
}
