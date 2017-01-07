using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class ParticlePDBVisualization : PDBVisualization
	{
		public string emitterPrefabName = "DefaultParticleEmitter";

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
				scale =  1f / (2f * transform.parent.localScale.x);
			}
			else 
			{
				Debug.LogWarning("Cannot render PDB. There is no ParticleSystem prefab called " + emitterPrefabName + " in Resources");
			}
		}

		public override void Render ()
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
				if (atom.index % Mathf.Ceil(100f / resolution) == 0)
				{
					EmitAtom(atom);
				}
			}
		}

		void EmitAtom (PDBAtom atomData)
		{
			ParticleSystem.EmitParams particle = new ParticleSystem.EmitParams();

			particle.position = scale * atomData.localPosition;
			particle.velocity = Vector3.zero;
			particle.startLifetime = Mathf.Infinity;
			particle.startColor = palette.ColorForElement(atomData.elementType);
			particle.startSize = scale * atomSize * MoleculeUtility.SizeForElement(atomData.elementType);
			particle.randomSeed = (uint)atomData.index;

			emitter.Emit(particle, 1);
		}

		public override void StartMorph (int goalStructure, float duration)
		{
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[emitter.particleCount];
			int n = emitter.GetParticles(particles);
			int index;
			for (int i = 0; i < n; i++)
			{
				index = (int)particles[i].randomSeed;
				if (index < structures[goalStructure].atoms.Count)
				{
					particles[i].velocity = scale * (structures[goalStructure].atoms[index].localPosition - particles[i].position / scale) / duration;
				}
			}
			emitter.SetParticles(particles, n);
		}

		protected override void StopMorph ()
		{
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[emitter.particleCount];
			int n = emitter.GetParticles(particles);
			for (int i = 0; i < n; i++)
			{
				particles[i].velocity = Vector3.zero;
			}
			emitter.SetParticles(particles, n);
		}

		float lastTime;
		float morphDuration;
		float switchTime;

		protected override void TestMorph ()
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
	}
}
