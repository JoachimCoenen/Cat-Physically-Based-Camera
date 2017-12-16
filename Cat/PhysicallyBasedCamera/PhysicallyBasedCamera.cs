using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using Cat.Common;

namespace Cat.PhysicallyBasedCamera {
	
	using Settings = CameraSettings;//<CameraBodyModel, LensModel>;

	[RequireComponent(typeof(Camera))]
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[DisallowMultipleComponent]
	[AddComponentMenu("Cat/Physically Based Camera")]
	public class PhysicallyBasedCamera : MonoBehaviour {

		//[SerializeField] public CamSettings settings = CamSettings.default;

		/*
		[Serializable]
		public struct Settings {

			private int m_ColorFilter;
			private int m_LenseGuard;

			[CustomLabelRange(0.1f, 22, "f-Stop f/n"), SerializeField]
			private float m_fStop;

			[Range(18, 200), SerializeField]
			private float m_Lense;

			[Range(1, 20), SerializeField]
			private float m_Zoom;

			[Range(0.185f, 100), SerializeField]
			private float m_FocusDistance;

			private int m_ShutterSpeed;
			private int m_Fps;
			private int m_Film;
			private int m_Posprocessing;

			//[Header("Tone Mapping")]
			//[Range(0, 1)]
			//public float strength;

			public float fStop {
				get { return m_fStop; }
				set { m_fStop = value; }
			}

			public float Lense {
				get { return m_Lense; }
				set { m_Lense = value; }
			}

			public float Zoom {
				get { return m_Zoom; }
				set { m_Zoom = value; }
			}

			public float FocusDistance {
				get { return m_FocusDistance; }
				set { m_FocusDistance = value; }
			}


			public static Settings defaultSettings { 
				get {
					return new Settings {
						m_ColorFilter = 0,
						m_LenseGuard = 0,

						m_fStop = 0.5f,
						m_Lense = 35,
						m_Zoom = 1,
						m_FocusDistance = 1,

						m_ShutterSpeed = 0,
						m_Fps = 0,
						m_Film = 0,
						m_Posprocessing = 0,

						//strength = 0.5f,
					};
				}
			}
		}
		*/

		[SerializeField]
		//[Inlined]
		private Settings m_Settings = Settings.defaultSettings;
		public Settings settings {
			get { return m_Settings; }
			set { 
				m_Settings = value;
				OnValidate();
			}
		}

		private Material m_Material = null;
		protected Material material {
			get {
				if (m_Material == null) {
					var shaderName = "Hidden/PhysicallyBasedCameraUberShader";
					var shader = Shader.Find(shaderName);
					if (shader == null) {
						this.enabled = false;
						throw new ArgumentException(String.Format("Shader not found: '{0}'", shaderName));
					}
					m_Material = new Material(shader);
					m_Material.hideFlags = HideFlags.DontSave;
				}
				return m_Material;
			} 
		}

		private Camera m_camera = null;
		internal new Camera camera {
			get {
				if (m_camera == null) {
					m_camera = GetComponent<Camera>();
					if (m_camera == null) {
						this.enabled = false;
						throw new ArgumentException(String.Format("PostProcessingManager requires a Camera component attached"));
					}
				}
				return m_camera;
			} 
		}

		private VectorInt2 m_cameraSize = new VectorInt2(1, 1);

		internal VectorInt2 cameraSize {
			get { return m_cameraSize; }
			private set {
				if (m_cameraSize != value) {
					m_cameraSize = value;
				}
			}
		}

		internal bool isSceneView { 
			get {
				#if UNITY_EDITOR 
				return UnityEditor.SceneView.currentDrawingSceneView != null && UnityEditor.SceneView.currentDrawingSceneView.camera == camera;
				#else
				return false;
				#endif 
			}
		}



		public float getPerspectiveFactor(float S1, float zoom, float f1) {
			float S2 = 1 / (1 / f1 - 1 / S1);
			float x = 1;
			//return x / (2 * f1 * zoom) * S2 / f1;
			return x / (2 * zoom * f1 * (1 - f1 / S1));
		}



		private void OnPreCull() {
			var cam = this.camera;
			var size = this.cameraSize;
			if (!settings.isValid) {
				return;
			}

			cameraSize = new VectorInt2(camera.pixelWidth, camera.pixelHeight);
			var filmDimensions = settings.body.sensorDimensions / 1000f;//new Vector2(0.036f * cam.aspect, 0.036f);

			if (!isSceneView) {
				float factor = getPerspectiveFactor(settings.focusDistance, settings.zoom, settings.lens.focalLength / 1000f); // Why dividing by 1000f ??

				var sensorAspect = filmDimensions.x / filmDimensions.y;
				if (cam.aspect < sensorAspect) {
					cam.fieldOfView = Mathf.Atan(filmDimensions.x * factor / cam.aspect) * 2 * Mathf.Rad2Deg;
				} else {
					cam.fieldOfView = Mathf.Atan(filmDimensions.y * factor) * 2 * Mathf.Rad2Deg;
				}

				material.SetVector("_MaskingSize", new Vector2(cam.aspect / sensorAspect, sensorAspect / cam.aspect));
				//cam.aspect / sensorAspect * x > 1
				//cam.projectionMatrix = GetPerspectiveProjectionMatrix(new Vector2(0.036f, 0.024f), cam);
				//cam.projectionMatrix = GetPerspectiveProjectionMatrix(new Vector2(0.036f * cam.aspect, 0.036f), cam);
			}

			//	UpdateCameraDepthBufferCameraEvent(cam.actualRenderingPath);

			//	foreach (var preCullFunc in m_preCullChain) {
			//		preCullFunc(cam, size);
			//	}
		}

		private void OnPreRender() {
			if (!settings.isValid) {
				return;
			}
			var cam = this.camera;
			var size = this.cameraSize;
			//	foreach (var preRenderFunc in m_preRenderChain) {
			//		preRenderFunc(cam, size);
			//	}
		}

		private void OnPostRender() {
			if (!settings.isValid) {
				return;
			}
			// This is here inorder to overcome a bug, where Camera.pixelWidth & Camera.pixelHeight 
			// return a slightly wrong value in the editor scene view.

			//Debug.LogFormat("aspect: {0}, size: {1}", camera.aspect, cameraSize);
			camera.ResetProjectionMatrix();

			//var cam = this.camera;
			//var size = this.cameraSize;
			//foreach (var postRenderFunc in m_postRenderChain) {
			//	postRenderFunc(cam, size);
			//}
		}


		//[ImageEffectTransformsToLDR]
		private void OnRenderImage(RenderTexture source, RenderTexture destination) {
			if (!settings.isValid) {
				Graphics.Blit(source, destination);
				return;
			}
				
			if (false && isSceneView) {
				Graphics.Blit(source, destination);
				return;
			}

			Graphics.Blit(source, destination, material, 0);
		}


		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}

		void OnValidate() {
			
		}
	}
	
}

