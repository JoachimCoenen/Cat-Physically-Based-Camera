using System;
using UnityEngine;
using Cat.Common;

namespace Cat.PhysicallyBasedCamera {

	public class LensModel : LensModelBase {

		public int m_ColorFilter;
		public int m_LenseGuard;

		public float m_fStopMin = 0;
		public float m_fStopMax = 10;

		public float focalLength = 35;


		public float m_ZoomMin = 0;
		public float m_ZoomMax = 10;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
