using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


/// <summary>
/// This class handles the Matcher Blitz editor window.
/// </summary>
public class MatcherBlitzEditor : EditorWindow
{

    private readonly List<EditorTab> tabs = new List<EditorTab>();

    private int selectedTabIndex = -1;
    private int prevSelectedTabIndex = -1;

    /// <summary>
    /// Static initialization of the editor window.
    /// </summary>
    [MenuItem("Tools/MatcherBlitz/Editor", false, 0)]
    private static void Init()
    {
        var window = GetWindow(typeof(MatcherBlitzEditor));
        window.titleContent = new GUIContent("MatcherBlitz Editor");
    }

    /// <summary>
    /// Unity's OnEnable method.
    /// </summary>
    private void OnEnable()
    {
        tabs.Add(new LevelEditorTab(this));
        selectedTabIndex = 0;
    }

    /// <summary>
    /// Unity's OnGUI method.
    /// </summary>
    private void OnGUI()
    {
        selectedTabIndex = GUILayout.Toolbar(selectedTabIndex,
            new[] { "Level editor" });
        if (selectedTabIndex >= 0 && selectedTabIndex < tabs.Count)
        {
            var selectedEditor = tabs[selectedTabIndex];
            if (selectedTabIndex != prevSelectedTabIndex)
            {
                selectedEditor.OnTabSelected();
                GUI.FocusControl(null);
            }
            selectedEditor.Draw();
            prevSelectedTabIndex = selectedTabIndex;
        }
    }
}

