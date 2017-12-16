using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.IO;
using Cat.PhysicallyBasedCamera;
using Cat.CommonEditor;

namespace Cat.PhysicallyBasedCameraEditor {
	
	public class ModelFactory {

		[MenuItem("Assets/Create/Physically Based Camera Lens", priority = 202)]
		static void MenuCreateLensModel() {
			var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateLensModel>(), "New Physically Based Camera Lens.asset", icon, null);
		}

		[MenuItem("Assets/Create/Physically Based Camera Body", priority = 201)]
		static void MenuCreateCameraBodyModel() {
			var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateCameraBodyModel>(), "New Physically Based Camera Body.asset", icon, null);
		}

		internal static LensModel CreateLensModelAtPath(string path) {
			var model = ScriptableObject.CreateInstance<LensModel>();
			model.name = Path.GetFileName(path);
			AssetDatabase.CreateAsset(model, path);
			return model;
		}

		internal static CameraBodyModel CreateCameraBodyModelAtPath(string path) {
			var model = ScriptableObject.CreateInstance<CameraBodyModel>();
			model.name = Path.GetFileName(path);
			AssetDatabase.CreateAsset(model, path);
			return model;
		}
	}

	class DoCreateLensModel : EndNameEditAction {
		public override void Action(int instanceId, string pathName, string resourceFile) {
			LensModel model = ModelFactory.CreateLensModelAtPath(pathName);
			ProjectWindowUtil.ShowCreatedAsset(model);
		}
	}

	class DoCreateCameraBodyModel : EndNameEditAction {
		public override void Action(int instanceId, string pathName, string resourceFile) {
			CameraBodyModel model = ModelFactory.CreateCameraBodyModelAtPath(pathName);
			ProjectWindowUtil.ShowCreatedAsset(model);
		}
	}
}
