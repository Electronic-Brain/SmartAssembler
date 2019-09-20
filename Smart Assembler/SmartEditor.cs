/* This script may not be reproduced or selling anywhere without prior written permission of Electronic Brain.
 * Project Developed by - 
 * Srejon Khan 
 * Game Programmer, Electronic Brain 
 * 
 * Ashikur Rahman 
 * Game Programmer,Electronic Brain
 * Email : support@electronicbrain.net 
 * 
 * WE LOVE OUR COMMUNITY MORE THAN ANYTHING. Don't hasitate to request for any plugins that you want. Mail us at support@electronicbrain.net
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SmartEditor : EditorWindow
{
    public Dictionary<string, string> assembleData = new Dictionary<string, string>
    {
        {"Animation", "*.anim"},
        {"Animator", "*.controller"},
        {"Audio Mixer", "*.mixer"},
        {"Flare", "*.flare"},
        {"Font", "*.ttf,*.otf"},
        {"Material", "*.mat"},
        {"Model", "*.fbx,*.dae,*.obj,*.3DS"},
        {"Prefab", "*.prefab"},
        {"Particle", "*.prefab"},
        {"Scene", "*.unity"},
        {"Script","*.cs"},
        {"Sound","*.wav,*.mp3"},
        {"Sprites","*.jpg,*.png,*.bmp,*.jpe,*.jpeg,*.ico,*.tif,*.tiff"}

    };
    string asmbFolderName;
    string asmbExtension;
    public Dictionary<string, string> prefabFilterData = new Dictionary<string, string>
    {
        {"Particle","ParticleSystem"}
    };
    string filterFolderName;
    string filterSpecial;

    [MenuItem("Electronic Brain/Smart Library/Smart Assembler")]
    public static void ShowWindow()
    {
        GetWindow<SmartEditor>(false, "Smart Assembler", true);
    }
    void OnGUI()
    {
        #region Header
        var headerStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        GUILayout.Label("Smart Assembler", headerStyle);
        GUILayout.Label("Smartly Assembly your Project", headerStyle);
        #endregion

        EditorGUILayout.BeginVertical();

        #region Assembly
        GUILayout.Label("Assembly", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical();
        if (assembleData != null)
        {
            List<string> assembleFolder = new List<string>();
            List<string> assembleExt = new List<string>();

            foreach (var keyValuePair in assembleData)
            {
                assembleFolder.Add(keyValuePair.Key);
                assembleExt.Add(keyValuePair.Value);
            }

            for (int i = 0; i < assembleData.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField("Folder", assembleFolder[i]);
                EditorGUILayout.TextField("Extenstion",assembleExt[i]);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    assembleData.Remove(assembleFolder[i]);
                }
                EditorGUILayout.EndHorizontal();  
            }                         

        }
        EditorGUILayout.EndVertical();
        GUILayout.Label("Add to Assembly", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        asmbFolderName = EditorGUILayout.TextField("Folder",asmbFolderName);
        asmbExtension = EditorGUILayout.TextField("Extension", asmbExtension); 
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Add"))
        {
            assembleData.Add(asmbFolderName, asmbExtension);
        }
        #endregion

        #region FilterAssembly
        GUILayout.Label("Filter Prefab", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical();
        if (assembleData != null)
        {
            List<string> filterAssembleFolder = new List<string>();
            List<string> filterAssembleExt = new List<string>();

            foreach (var keyValuePair in prefabFilterData)
            {
                filterAssembleFolder.Add(keyValuePair.Key);
                filterAssembleExt.Add(keyValuePair.Value);
            }

            for (int i = 0; i < prefabFilterData.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField("Folder", filterAssembleFolder[i]);
                EditorGUILayout.TextField("Special Identity", filterAssembleExt[i]);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    prefabFilterData.Remove(filterAssembleFolder[i]);
                }
                EditorGUILayout.EndHorizontal();
            }

        }
        EditorGUILayout.EndVertical();
        GUILayout.Label("Add to Filter", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        filterFolderName = EditorGUILayout.TextField("Folder", filterFolderName);
        filterSpecial = EditorGUILayout.TextField("Identity", filterSpecial);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Add Filter"))
        {
            prefabFilterData.Add(filterFolderName, filterSpecial);
        }
        #endregion

        #region Actions
        GUILayout.Label("Actions", EditorStyles.boldLabel);
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.red;
        if (GUILayout.Button("Assemble",style))
        {
            Assemble();
        }
        #endregion

        EditorGUILayout.EndVertical();
    }
            
    void Assemble()
    {
        List<string> assembleFolder = new List<string>();
        List<string> assembleExt = new List<string>();
        if (assembleData != null)
        {
            foreach (var keyValuePair in assembleData)
            {
                assembleFolder.Add(keyValuePair.Key);
                assembleExt.Add(keyValuePair.Value);
            }
            for (int i = 0; i < assembleFolder.Count; i++)
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/" + assembleFolder[i]);
            }
        }
        for (int i = 0; i < assembleExt.Count; i++)
        {
            if (assembleExt[i].Contains("prefab"))
            {
                List<string> filterFolder = new List<string>();
                List<string> filterSpecial = new List<string>();
                if(prefabFilterData != null)
                {
                    foreach (var keyValuePair in prefabFilterData)
                    {
                        filterFolder.Add(keyValuePair.Key);
                        filterSpecial.Add(keyValuePair.Value);
                    }
                }
                foreach (string file in System.IO.Directory.GetFiles(Application.dataPath, assembleExt[i]))
                {
                    string metaData = System.IO.File.ReadAllText(file);
                    for (int j = 0; j < prefabFilterData.Count; j++)
                    {
                        if (metaData.Contains(filterSpecial[j]))
                        {
                            System.IO.File.Move(file, file.Replace(Application.dataPath, Application.dataPath + "/" + filterFolder[j]));
                        }
                        else
                        {
                            System.IO.File.Move(file, file.Replace(Application.dataPath, Application.dataPath + "/Prefab"));
                        }
                    }
                }
                AssetDatabase.Refresh();
               // return;
            }
            else
            {
                foreach (string file in System.IO.Directory.GetFiles(Application.dataPath, "*.*").Where(s =>
                assembleExt[i].Contains(System.IO.Path.GetExtension(s).ToLower())))
                {
                    System.IO.File.Move(file, file.Replace(Application.dataPath, Application.dataPath + "/" + assembleFolder[i]));
                }
            }
        }
        AssetDatabase.Refresh();
    }
}
