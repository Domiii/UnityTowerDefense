//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//
//[CustomEditor(typeof(WavePathTileBuilder))]
//public class WavePathEditor : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		DrawDefaultInspector();
//		
//		WavePathTileBuilder builder = (WavePathTileBuilder)target;
//		if(GUILayout.Button("Build Mesh"))
//		{
//			builder.BuildMesh();
//		}
//	}
//}