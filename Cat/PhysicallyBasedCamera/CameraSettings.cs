using System;
using UnityEngine;
using Cat.Common;

namespace Cat.PhysicallyBasedCamera {

	[Serializable]
	public struct CameraSettings/*<Body, Lens> 
		where Body : CameraBodyModelBase 
		where Lens : LensModelBase */{

		public CameraBodyModel body;
		public LensModel lens;

		public bool isValid {
			get { return (lens != null && body != null); }
		}
		
		public float fStop;

		public float zoom;

		public float focusDistance;
		public bool autoFocus;

		[Range(1, 359)]
		public float shutterAngle;


		public static CameraSettings defaultSettings {
			get {
				return new CameraSettings {
					fStop = 0.5f,
					zoom = 1f,
					focusDistance = 5f,
					autoFocus = true,
					shutterAngle = 180,
				};
			}
		}
	}
}
