﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RogoDigital.Lipsync;

[CustomEditor(typeof(LipSyncProject))]
public class LipSyncProjectSettings : Editor {
	private Texture2D logo;
	private LipSyncProject myTarget;

	private SerializedProperty phonemeSet;
	private SerializedProperty emotions;
	private SerializedProperty emotionColors;
	private SerializedProperty gestures;

	void OnEnable () {
		myTarget = (LipSyncProject)target;

		if (EditorGUIUtility.isProSkin) {
			logo = (Texture2D)EditorGUIUtility.Load("Rogo Digital/Lipsync/Dark/logo_component.png");
		} else {
			logo = (Texture2D)EditorGUIUtility.Load("Rogo Digital/Lipsync/Light/logo_component.png");
		}

		phonemeSet = serializedObject.FindProperty("phonemeSet");
		emotions = serializedObject.FindProperty("emotions");
		emotionColors = serializedObject.FindProperty("emotionColors");
		gestures = serializedObject.FindProperty("gestures");
	}

	public override void OnInspectorGUI () {

		serializedObject.Update();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Box(logo, GUIStyle.none);
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Box("Project Settings", EditorStyles.boldLabel);
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(20);
		EditorGUILayout.PropertyField(phonemeSet, new GUIContent("Phoneme Set"));
		GUILayout.Space(15);

		GUILayout.Box("Animation Emotions", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		Undo.RecordObject(myTarget, "Change Project Settings");
		EditorGUI.BeginDisabledGroup(true);
		for (int a = 0; a < myTarget.emotions.Length; a++) {
			Rect lineRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(25));
			if (a % 2 == 0) {
				GUI.Box(lineRect, "", (GUIStyle)"hostview");
			}
			GUILayout.Space(10);
			GUILayout.Box((a + 1).ToString(), EditorStyles.label);
			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			emotions.GetArrayElementAtIndex(a).stringValue = GUILayout.TextArea(emotions.GetArrayElementAtIndex(a).stringValue, EditorStyles.label, GUILayout.MinWidth(130));
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				emotions.GetArrayElementAtIndex(a).stringValue = Validate(a, myTarget.emotions);
			}
				

			EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Text);
			GUILayout.FlexibleSpace();
			emotionColors.GetArrayElementAtIndex(a).colorValue = EditorGUILayout.ColorField(emotionColors.GetArrayElementAtIndex(a).colorValue, GUILayout.MaxWidth(280));
			GUILayout.FlexibleSpace();
			GUI.backgroundColor = new Color(0.8f, 0.3f, 0.3f);
			if (GUILayout.Button("Delete", GUILayout.MaxWidth(70), GUILayout.Height(18))) {
				emotions.DeleteArrayElementAtIndex(a);
				emotionColors.DeleteArrayElementAtIndex(a);
				break;
			}
			GUI.backgroundColor = Color.white;
			GUILayout.Space(10);
			EditorGUILayout.EndHorizontal();
		}

		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add Emotion", GUILayout.MaxWidth(300), GUILayout.Height(25))) {
			emotions.arraySize++;
			emotionColors.arraySize++;

			emotions.GetArrayElementAtIndex(emotions.arraySize - 1).stringValue = "New Emotion";
			emotionColors.GetArrayElementAtIndex(emotionColors.arraySize - 1).colorValue = Color.white;

			serializedObject.ApplyModifiedProperties();
			emotions.GetArrayElementAtIndex(emotions.arraySize - 1).stringValue = Validate(emotions.arraySize - 1, myTarget.emotions);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(20);

		GUILayout.Box("Animation Gestures", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		for (int a = 0; a < myTarget.gestures.Count; a++) {
			Rect lineRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(25));
			if (a % 2 == 0) {
				GUI.Box(lineRect, "", (GUIStyle)"hostview");
			}
			GUILayout.Box((a + 1).ToString(), EditorStyles.label);
			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			gestures.GetArrayElementAtIndex(a).stringValue = GUILayout.TextArea(gestures.GetArrayElementAtIndex(a).stringValue, EditorStyles.label, GUILayout.MinWidth(130));
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				gestures.GetArrayElementAtIndex(a).stringValue = Validate(a, myTarget.gestures.ToArray());
			}

			EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Text);
			GUILayout.FlexibleSpace();
			GUI.backgroundColor = new Color(0.8f, 0.3f, 0.3f);
			if (GUILayout.Button("Delete", GUILayout.MaxWidth(70), GUILayout.Height(18))) {
				gestures.DeleteArrayElementAtIndex(a);
				break;
			}
			GUI.backgroundColor = Color.white;
			GUILayout.Space(10);
			EditorGUILayout.EndHorizontal();
		}

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add Gesture", GUILayout.MaxWidth(300), GUILayout.Height(25))) {
			gestures.arraySize++;
			gestures.GetArrayElementAtIndex(gestures.arraySize - 1).stringValue = "New Gesture";

			serializedObject.ApplyModifiedProperties();
			gestures.GetArrayElementAtIndex(gestures.arraySize - 1).stringValue = Validate(gestures.arraySize - 1, myTarget.gestures.ToArray());
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		EditorGUI.EndDisabledGroup();

		EditorGUILayout.HelpBox("Thank you for trying LipSync Lite! Upgrade to LipSync Pro to unlock additional features, such as AutoSync and Emotion markers!", MessageType.Info);

		if (GUILayout.Button("Get LipSync Pro")) {
			Application.OpenURL("http://u3d.as/cag");
		}
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Get LipSync Extensions")) {
			RDExtensionWindow.ShowWindow("LipSync_Lite");
		}
		if (GUILayout.Button("Forum Thread")) {
			Application.OpenURL("http://forum.unity3d.com/threads/released-lipsync-and-eye-controller-lipsyncing-and-facial-animation-tools.309324/");
		}
		if (GUILayout.Button("Website")) {
			Application.OpenURL("http://lipsync.rogodigital.com/");
		}
		EditorGUILayout.EndHorizontal();
		serializedObject.ApplyModifiedProperties();
	}

	[MenuItem("Edit/Project Settings/LipSync")]
	[MenuItem("Window/Rogo Digital/LipSync Lite/LipSync Project Settings", false, 12)]
	public static void ShowWindow () {
		string[] guids = AssetDatabase.FindAssets("ProjectSettings t:LipSyncProject");
		string path = "";

		if (guids.Length > 0) {
			path = AssetDatabase.GUIDToAssetPath(guids[0]);

			if (guids.Length > 1) Debug.LogWarning("LipSync: Multiple LipSyncProject files found. Only one will be used.");
		}

		LipSyncProject settings = (LipSyncProject)AssetDatabase.LoadAssetAtPath(path, typeof(LipSyncProject));

		if (settings == null) {
			settings = ScriptableObject.CreateInstance<LipSyncProject>();

			LipSyncProject newSettings = ScriptableObject.CreateInstance<LipSyncProject>();
			newSettings.emotions = new string[] { "default" };
			newSettings.emotionColors = new Color[] { new Color(1f, 0.7f, 0.1f) };

			EditorUtility.CopySerialized(newSettings, settings);
			AssetDatabase.CreateAsset(settings, "Assets/Rogo Digital/LipSync Lite/ProjectSettings.asset");
			AssetDatabase.Refresh();
			DestroyImmediate(newSettings);
		}
		Selection.activeObject = settings;
	}

	private string Validate (int index, string[] list) {
		return Validate(list[index], index, list);
	}

	private string Validate (string input, int index, string[] list) {
		string output = input;
		int dupCount = 0;

		for (int b = 0; b < list.Length; b++) {
			if ((input == list[b] && index != b) || (list[b].StartsWith(input + " (") && list[b].EndsWith(")"))) {
				dupCount++;
			}
		}

		if (dupCount > 0) {
			output += " (" + dupCount.ToString() + ")";
			output = Validate(output, index, list);
		}

		return output;
	}
}
