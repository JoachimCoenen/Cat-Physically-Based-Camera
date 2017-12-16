using System;
using UnityEngine;
using Cat.Common;

namespace Cat.PhysicallyBasedCamera {

	[Serializable]
	public class CameraBodyModel : CameraBodyModelBase {

		// TODO: JCO@@@ Add a list of  pre-sets for m_FilmDimensions to choose fom in the Editor.
		public Vector2 sensorDimensions = new Vector2(36, 24); // in [mm], 36mm × 24mm is the 35mm format (yes, really!)
		public int sensorISO = 100; // in [mm], 36mm × 24mm is the 35mm format (yes, really!)
		public int fps = 30;

		public int shutterSpeed;




		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
