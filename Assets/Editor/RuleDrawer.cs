//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//
//namespace SciSim
//{
//	[CustomPropertyDrawer( typeof(ConditionNode) )]
//	public class ConditionNodeDrawer : PropertyDrawer 
//	{
//		float propertyHeight;
//		float padding = 16f;
//
//		public override float GetPropertyHeight (SerializedProperty property, GUIContent label) 
//		{
//			return propertyHeight;
//		}
//
//		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
//		{
//			EditorGUI.BeginProperty(position, label, property);
//
//			Rect newPosition = position;
//			newPosition.height = padding;
//
//			EditorGUI.BeginChangeCheck();
//
//			SerializedProperty type = property.FindPropertyRelative("type");
//			SerializedProperty condition = property.FindPropertyRelative("condition");
//
//			EditorGUI.PropertyField(newPosition, type, true);
//			newPosition.y += EditorGUI.GetPropertyHeight(type);
//
//			if (EditorGUI.EndChangeCheck())
//			{
//				SetConditionType( (ConditionType)type.enumValueIndex, property );
//			}
//				
//			EditorGUI.PropertyField(newPosition, condition, true);
//			newPosition.y += EditorGUI.GetPropertyHeight(condition);
//
//			EditorGUI.EndProperty();
//			propertyHeight = newPosition.y - position.y + padding;
//		}
//
//		void SetConditionType (ConditionType type, SerializedProperty property)
//		{
//			string propertyPath = property.propertyPath;
//			List<RuleNode> nodes = (property.serializedObject.targetObject as Rule).nodes;
//			Condition newCondition = Condition.GetConditionForType( type );
//
//			//get node index
//			int i = propertyPath.IndexOf( "[" );
//			string n = propertyPath.Substring( i + 1, 1 );
//			int nodeIndex = 0;
//			int.TryParse( n, out nodeIndex );
//
//			//get condition index
//			i = propertyPath.LastIndexOf( "[" );
//			n = propertyPath.Substring( i + 1, 1 );
//			int conditionIndex = 0;
//			int.TryParse( n, out conditionIndex );
//
//			Condition oldCondition = nodes[nodeIndex].conditions[conditionIndex].condition;
//			if (oldCondition != null) 
//			{
//				Object.Destroy( oldCondition );
//			}
//			nodes[nodeIndex].conditions[conditionIndex].condition = newCondition;
//
//			//save condition asset
//			if (!System.IO.Directory.Exists( Application.dataPath + "/Behaviors" )) 
//			{
//				AssetDatabase.CreateFolder( "Assets", "Behaviors" );
//			}
//			if (!System.IO.Directory.Exists( Application.dataPath + "/Behaviors/Conditions" )) 
//			{
//				AssetDatabase.CreateFolder( "Assets/Behaviors", "Conditions" );
//			}
//			AssetDatabase.CreateAsset( newCondition, "Assets/Behaviors/Conditions/" + System.Guid.NewGuid() );
//			AssetDatabase.SaveAssets();
//			AssetDatabase.Refresh();
//		}
//	}
//}