using System;
using UnityEngine;
using Cat.Common;

namespace Cat.PhysicallyBasedCamera {

	[Serializable]
	public abstract class CameraSettingsBase<Body, Lens> 
		where Body : CameraBodyModelBase 
		where Lens : LensModelBase {

		public Body body;
		public Lens lens;

		public virtual bool isValid {
			get { return (lens != null && body != null); }
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
