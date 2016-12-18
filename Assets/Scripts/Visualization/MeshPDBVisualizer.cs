using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class MeshPDBVisualizer : PDBVisualizer 
	{
		public Material material;

		Metaballs metaballs;
		float[][] blobs;

		public override void Render ()
		{
			MakeTestBlobs();
			metaballs = new Metaballs(gameObject, material, blobs);
		}

		void MakeBlobs ()
		{

		}

		void MakeTestBlobs ()
		{
			blobs=new float[5][];
			blobs[0]=new float[]{.16f,.26f,.16f,.13f};
			blobs[1]=new float[]{.13f,-.134f,.35f,.12f};
			blobs[2]=new float[]{-.18f,.125f,-.25f,.16f};
			blobs[3]=new float[]{-.13f,.23f,.255f,.13f};		
			blobs[4]=new float[]{-.18f,.125f,.35f,.12f};
		}

		public override void StartMorph (int goalStructure, float duration)
		{
			
		}

		protected override void StopMorph ()
		{
			
		}

		protected override void TestMorph ()
		{
			TestUpdateBlobs();
			metaballs.UpdateMetaballs(blobs);
		}

		void TestUpdateBlobs ()
		{
			blobs[0][0]=.12f+.12f*(float)Mathf.Sin((float)Time.time*.50f);
			blobs[0][2]=.06f+.23f*(float)Mathf.Cos((float)Time.time*.2f);
			blobs[1][0]=.12f+.12f*(float)Mathf.Sin((float)Time.time*.2f);
			blobs[1][2]=-.23f+.10f*(float)Mathf.Cos((float)Time.time*1f);
			blobs[2][1]=-.03f+.24f*(float)Mathf.Sin((float)Time.time*.35f);
			blobs[3][1]=.126f+.10f*(float)Mathf.Cos((float)Time.time*.1f);
			blobs[4][0]=.206f+.1f*(float)Mathf.Cos((float)Time.time*.5f);
			blobs[4][1]=.056f+.2f*(float)Mathf.Sin((float)Time.time*.3f);
			blobs[4][2]=.25f+.08f*(float)Mathf.Cos((float)Time.time*.2f);
		}
	}
}
