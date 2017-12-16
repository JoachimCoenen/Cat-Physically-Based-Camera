using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Cat.Common;
namespace Cat.PhysicallyBasedCamera {

	public sealed class RenderTextureContainer {
		private RenderTexture rt;

		public RenderTextureContainer() {
			this.rt = null;
		}

		public RenderTextureContainer(RenderTexture aRt) {
			this.rt = aRt;
		}

		~RenderTextureContainer() {
			if (this.rt != null) {
				this.rt.Release();
			}
			this.rt = null;
		}

		internal void setRT(RenderTexture aRt) {
			if (this.rt != null) {
				this.rt.Release();
			}
			this.rt = aRt;
		}

		public override string ToString() {
			return String.Format("({0})", rt);
		}

		public static implicit operator RenderTexture(RenderTextureContainer rtc) {
			return rtc.rt;
		}
		public static implicit operator RenderTargetIdentifier(RenderTextureContainer rtc) {
			return rtc.rt;
		}
	}
}
