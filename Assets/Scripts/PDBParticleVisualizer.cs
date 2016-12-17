using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class PDBParticleVisualizer : MonoBehaviour 
	{
		public bool renderOnStart = true;
		public string emitterPrefabName = "DefaultParticleEmitter";
		public PDBAsset pdbData;

		public float resolution = 0.5f;
		public float atomSize = 10f;
		public MoleculePalette palette;

		ParticleSystem _emitter;
		ParticleSystem emitter 
		{
			get
			{
				if (_emitter == null)
				{
					InitEmitter();
				}
				return _emitter;
			}
		}

		void InitEmitter ()
		{
			GameObject emitterPrefab = Resources.Load(emitterPrefabName) as GameObject;
			if (emitterPrefab != null) 
			{
				_emitter = (Instantiate(emitterPrefab, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<ParticleSystem>();
				_emitter.transform.SetParent(transform);
				_emitter.transform.position = transform.position;
				_emitter.transform.rotation = transform.rotation;
				_emitter.transform.localScale = Vector3.one;
			}
			else 
			{
				Debug.LogWarning("Cannot render PDB. There is no ParticleSystem prefab called " + emitterPrefabName + " in Resources");
			}
		}

		void Start ()
		{
			if (renderOnStart)
			{
				Render();
			}
		}

		public void Render ()
		{
			if (pdbData != null && emitter != null)
			{
				EmitAtoms();
			}
		}

		void EmitAtoms ()
		{
			foreach (PDBAtom atom in pdbData.atoms)
			{
				if (atom.index % Mathf.Ceil(1 / resolution) == 0)
				{
					EmitAtom(atom);
				}
			}
		}

		void EmitAtom (PDBAtom atomData)
		{
			ParticleSystem.EmitParams particle = new ParticleSystem.EmitParams();

			particle.position = atomData.localPosition;
			particle.velocity = Vector3.zero;
			particle.startLifetime = Mathf.Infinity;
			particle.startColor = palette.ColorForElement(atomData.elementType);
			particle.startSize = atomSize * SizeForElement(atomData.elementType);
			particle.randomSeed = (uint)atomData.index;

			emitter.Emit(particle, 1);
		}

		// Single bond covalent atomic radii in angstroms
		float SizeForElement (Element element)
		{
			switch (element)
			{
			case Element.C :
				return 0.77f;

			case Element.N :
				return 0.75f;

			case Element.O :
				return 0.73f;

			case Element.S :
				return 0.102f;

			case Element.P :
				return 0.106f;

			case Element.H :
				return 0.38f;

			default :
				return 0.1f;
			}
		}

		public void Morph ()
		{

		}

		void UnMorph ()
		{

		}
	}

	[CreateAssetMenu(fileName = "MoleculePalette", menuName = "ScienceTools/Colors/Molecule Palette", order = 1)]
	[System.Serializable]
	public class MoleculePalette : ScriptableObject
	{
		public List<AtomColor> atomColors = new List<AtomColor>();
		
		public Color ColorForElement (Element element)
		{
			return atomColors.Find( a => a.element == element ).color;
		}
	}

	[System.Serializable]
	public class AtomColor
	{
		public Element element;
		public Color color;
	}
}
