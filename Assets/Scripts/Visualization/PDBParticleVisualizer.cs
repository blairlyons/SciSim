using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class PDBParticleVisualizer : MonoBehaviour 
	{
		public bool renderOnStart = true;
		public string emitterPrefabName = "DefaultParticleEmitter";
		public int currentStructure = 0;
		public PDBAsset[] structures = new PDBAsset[1];

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

		void Update ()
		{
			TestMorph();
		}

		public void Render ()
		{
			if (structures.Length > 0 && structures[currentStructure] != null && emitter != null)
			{
				EmitAtoms();
			}
		}

		void EmitAtoms ()
		{
			foreach (PDBAtom atom in structures[currentStructure].atoms)
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

		bool morphing;
		bool warned;

		public void Morph (int goalStructure, float duration)
		{
			if (goalStructure < structures.Length && structures[goalStructure] != null)
			{
				if (!warned && structures[currentStructure].atoms.Count != structures[goalStructure].atoms.Count)
				{
					Debug.LogWarning(name + " trying to morph to a structure with a different number of atoms!");
					warned = true;
				}

				ParticleSystem.Particle[] particles = new ParticleSystem.Particle[emitter.particleCount];
				int n = emitter.GetParticles(particles);
				int index;
				for (int i = 0; i < n; i++)
				{
					index = (int)particles[i].randomSeed;
					if (index < structures[goalStructure].atoms.Count)
					{
						particles[i].velocity = (structures[goalStructure].atoms[index].localPosition - particles[i].position) / duration;
					}
				}
				emitter.SetParticles(particles, n);
				morphing = true;
				currentStructure = goalStructure;

				Invoke("EndMorph", duration);
			}
		}

		void EndMorph ()
		{
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[emitter.particleCount];
			int n = emitter.GetParticles(particles);
			for (int i = 0; i < n; i++)
			{
				particles[i].velocity = Vector3.zero;
			}
			emitter.SetParticles(particles, n);
			morphing = false;
		}

		float lastTime;
		float morphDuration;
		float switchTime;

		void TestMorph ()
		{
			if (Time.time - lastTime > switchTime)
			{
				if (morphing)
				{
					CancelInvoke("EndMorph");
				}

				int goal = currentStructure + 1;
				if (goal >= structures.Length) { goal = 0; }

				if (Random.Range(0, 1f) > 0.9f) //react
				{
					morphDuration = Random.Range(0.3f, 0.5f);
					switchTime = morphDuration + 0.2f;
				}
				else //jitter
				{
					morphDuration = Random.Range(1.5f, 2.5f);
					switchTime = Random.Range(0.1f, 0.2f);
				}

				Morph(goal, morphDuration);
				lastTime = Time.time;
			}
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
				return 1f;
			}
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
