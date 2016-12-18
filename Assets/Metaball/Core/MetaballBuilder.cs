//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetaballBuilder
{
	class MB3DCubeVector
	{
		public sbyte [] e = new sbyte[3];
		public sbyte x
		{
			get { return e[0]; }
			set { e [0] = value; }
		}
		public sbyte y
		{
			get { return e [1]; }
			set { e [1] = value; }
		}
		public sbyte z
		{
			get { return e [2]; }
			set { e [2] = value; }
		}
		public sbyte axisIdx=-1;

		public MB3DCubeVector()
		{
		}

		public MB3DCubeVector( sbyte x_, sbyte y_, sbyte z_ )
		{
			x=x_; y=y_; z=z_; axisIdx = -1;
			CalcAxis();
		}

		public static MB3DCubeVector operator+( MB3DCubeVector lh, MB3DCubeVector rh )
		{
			return new MB3DCubeVector ( (sbyte)(lh.x+rh.x), (sbyte)(lh.y+rh.y), (sbyte)(lh.z+rh.z) );
		}
		
		void CalcAxis()
		{
			for( sbyte i=0; i<3; ++i )
			{
				if( e[i] != 0 )
				{
					if( axisIdx==-1 )
					{
						axisIdx = i;
					}
					else
					{
						axisIdx = -1;
						break;
					}
				}
			}
		}
		
		public void SetAxisVector( sbyte axisIdx_, sbyte value )
		{
			x=y=z=0;
			if( value == 0 )
			{
				axisIdx = -1;
			}
			else
			{
				axisIdx = axisIdx_;
				e[axisIdx] = value;
			}
		}
		
		public static MB3DCubeVector operator*( MB3DCubeVector lh, sbyte rh )
		{
			return new MB3DCubeVector( (sbyte)(lh.x*rh), (sbyte)(lh.y*rh), (sbyte)(lh.z*rh) );
		}
	}
    
    class MB3DCubeInOut
	{
		// z, y, x
		public sbyte[,,] bFill = new sbyte[2,2,2];
		public int inCount;
		
		public MB3DCubeInOut()
		{
		}
		public MB3DCubeInOut( sbyte patternIdx )
		{
			sbyte [] bInOut = new sbyte[8];
			for( int i=0; i<8; ++i )
			{
				bInOut[i] = (sbyte)( ( patternIdx>>i ) & 1);
			}
			
			Init( bInOut[0], bInOut[1], bInOut[2], bInOut[3], bInOut[4], bInOut[5], bInOut[6], bInOut[7] );
		}
		public MB3DCubeInOut( sbyte a0, sbyte a1, sbyte a2, sbyte a3, sbyte a4, sbyte a5, sbyte a6, sbyte a7 )
		{
			Init( a0, a1, a2, a3, a4, a5, a6, a7 );
		}
		
		void Init( sbyte a0, sbyte a1, sbyte a2, sbyte a3, sbyte a4, sbyte a5, sbyte a6, sbyte a7 )
		{
			bFill[0,0,0] = a0;
			bFill[0,0,1] = a1;
			bFill[0,1,0] = a2;
			bFill[0,1,1] = a3;
			bFill[1,0,0] = a4;
			bFill[1,0,1] = a5;
			bFill[1,1,0] = a6;
			bFill[1,1,1] = a7;
			
			inCount = a0+a1+a2+a3+a4+a5+a6+a7;
		}
		
		public sbyte At( MB3DCubeVector point )
		{
			return bFill[point.z,point.y,point.x];
		}
	}
    
	struct MB3DCubePrimitivePattern
	{
		public MB3DCubeInOut InOut;
		
		public int [] IndexBuf;
        public int [] IndexBufAlter;
        public int IndexCount
        {
            get
            {
                if (IndexBuf != null)
                {
                    return IndexBuf.Length;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int IndexCountAlter
        {
            get
            {
                if (IndexBufAlter != null)
                {
                    return IndexBufAlter.Length;
                }
                else
                {
                    return 0;
                }
            }
        }
	}
    
	class MB3DPatternMatchingInfo
    {
        // result, 0~14
        public int PrimaryPatternIndex;

        // 1: flip in/out
        public bool bFlipInOut;
        // 2: origin( point zero )
        public MB3DCubeVector Origin = new MB3DCubeVector();
        // 3: axis.
        public MB3DCubeVector[] Axis = new MB3DCubeVector[3];


		public void Match( MB3DCubeInOut src )
        {
	        // in count
	        PrimaryPatternIndex=-1;
	
	        bFlipInOut = src.inCount > 4;

	        for( int i=0; i<MB3D_PATTERN_COUNT; ++i )
	        {
		        MB3DCubeInOut tgt = __primitivePatterns[i].InOut;

		        if( bFlipInOut )
		        {
			        if( (8-src.inCount) != tgt.inCount )
			        {
				        continue;
			        }
		        }
		        else
		        {
			        if( src.inCount != tgt.inCount )
			        {
				        continue;
			        }
		        }

		        sbyte [] AxisDir = new sbyte[3];
		        for( Origin.x=0; Origin.x<2; ++Origin.x )
		        {
			        AxisDir[0] = (sbyte)( ( Origin.x != 0 ) ? -1 : 1 );
			        for( Origin.y=0; Origin.y<2; ++Origin.y )
			        {
				        AxisDir[1] = (sbyte)( ( Origin.y != 0 ) ? -1 : 1 );
				        for( Origin.z=0; Origin.z<2; ++Origin.z )
				        {
					        AxisDir[2] = (sbyte)( ( Origin.z != 0 ) ? -1 : 1 );

					        sbyte AxisOrder = (sbyte)( ( ( ( Origin.x + Origin.y + Origin.z ) % 2 ) != 0 ) ? 2 : 1 );
					
					        for( sbyte StartingAxis=0; StartingAxis<3; ++StartingAxis )
					        {
						        // ここで座標軸が決定
						        // 原点 : bStartingPeak[3]
						        // 軸1 : StartingAxis
						        // 軸2 : StartingAxis+AxisOrder;
						        // 軸3 : StartingAxis+AxisOrder+AxisOrder;
                                Axis[0] = new MB3DCubeVector();
                                Axis[1] = new MB3DCubeVector();
                                Axis[2] = new MB3DCubeVector();
						        Axis[0].SetAxisVector( StartingAxis, AxisDir[StartingAxis] );
						        Axis[1].SetAxisVector( (sbyte)( ( StartingAxis+AxisOrder ) % 3 ), (sbyte)( AxisDir[( StartingAxis+AxisOrder ) % 3] ) );
						        Axis[2].SetAxisVector( (sbyte)( ( StartingAxis+AxisOrder+AxisOrder ) % 3 ), (sbyte)( AxisDir[( StartingAxis+AxisOrder+AxisOrder ) % 3] ) );

						        // start matching
						        bool bMatch = true;
						        for( sbyte a0=0; a0<2; ++a0 )
						        {
							        for( sbyte a1=0; a1<2; ++a1 )
							        {
								        for( sbyte a2=0; a2<2; ++a2 )
								        {
									        MB3DCubeVector point = SampleVertex( new MB3DCubeVector(a0,a1,a2) );

									        if( ( bFlipInOut != ( src.At( point ) == tgt.bFill[a2,a1,a0] ) ) )
									        {
									        }
									        else
									        {
										        bMatch = false;
									        }
								        }
							        }
						        }

						        if( bMatch )
						        {
							        PrimaryPatternIndex = i;
							
							        return;
						        }
					        }
				        }
			        }
		        }
	        }
        }
        
        public int[] GetTargetPrimitiveIndexBuffer()
        {
            return (bFlipInOut && __primitivePatterns[PrimaryPatternIndex].IndexCountAlter > 0) ? __primitivePatterns[PrimaryPatternIndex].IndexBufAlter : __primitivePatterns[PrimaryPatternIndex].IndexBuf;
        }
		
		public MB3DCubeVector SampleVertex( MB3DCubeVector primaryPos )
        {
            return Origin + Axis[0] * primaryPos.x + Axis[1] * primaryPos.y + Axis[2] * primaryPos.z;
        }

		public void SampleSegment( sbyte primarySegmentID, out sbyte out_axis, out sbyte out_a_1, out sbyte out_a_2 )
        {
            sbyte primary_axis = (sbyte)( primarySegmentID / 4 );
            sbyte primary_a_1 = (sbyte)( primarySegmentID % 2 );
            sbyte primary_a_2 = (sbyte)( (primarySegmentID / 2) % 2 );

            out_axis = Axis[primary_axis].axisIdx;

            MB3DCubeVector pos = Origin + Axis[(primary_axis + 1) % 3] * primary_a_1 + Axis[(primary_axis + 2) % 3] * primary_a_2;

            sbyte primary_a_1_idx = (sbyte)( (out_axis + 1) % 3 );
            sbyte primary_a_2_idx = (sbyte)( (out_axis + 2) % 3 );
            out_a_1 = pos.e[primary_a_1_idx];
            out_a_2 = pos.e[primary_a_2_idx];
        }
		
	}

	class MB3DCubePattern
    {
        public int PatternIdx;
        public MB3DPatternMatchingInfo MatchingInfo = new MB3DPatternMatchingInfo();
        public int[] IndexBuf = new int[15];

        public void Init(int patternIdx)
        {
	        PatternIdx = patternIdx;

	        MB3DCubeInOut inOut = new MB3DCubeInOut( (sbyte)patternIdx);

	        MatchingInfo.Match( inOut );
	
	        // init index buffer
            int[] targetIdxBuffer = MatchingInfo.GetTargetPrimitiveIndexBuffer();// (MatchingInfo.bFlipInOut && __primitivePatterns[MatchingInfo.PrimaryPatternIndex].IndexCountAlter > 0) ? __primitivePatterns[MatchingInfo.PrimaryPatternIndex].IndexBufAlter : __primitivePatterns[MatchingInfo.PrimaryPatternIndex].IndexBuf;
            for (int i = 0; i < targetIdxBuffer.Length; ++i)
	        {
		        sbyte axis, a_1, a_2;
                MatchingInfo.SampleSegment((sbyte)(targetIdxBuffer[i]), out axis, out a_1, out a_2);

		        // reverse order if flip
                int targetIdx = MatchingInfo.bFlipInOut ? (targetIdxBuffer.Length- i - 1) : i;
		        IndexBuf[targetIdx] = axis*4+a_2*2+a_1;
	        }
        }
	};
    
	static bool __bCubePatternsInitialized=false;

	static MB3DCubePattern [] __cubePatterns = new MB3DCubePattern[256];

    static void __InitCubePatterns()
    {
        for (int i = 0; i < 256; ++i)
        {
            __cubePatterns[i] = new MB3DCubePattern();
            __cubePatterns[i].Init((sbyte)i);
        }
        __bCubePatternsInitialized = true;
    }
    
	const int MB3D_PATTERN_COUNT = 15;
	static MB3DCubePrimitivePattern [] __primitivePatterns = new MB3DCubePrimitivePattern[MB3D_PATTERN_COUNT]{

		// 0 points
        // #0
        new MB3DCubePrimitivePattern()
		{
			InOut = new MB3DCubeInOut(0, 0, 0, 0, 0, 0, 0, 0),
			IndexBuf = new int[]{}
		},
		
		// 1 points
        // #1
        new MB3DCubePrimitivePattern()
		{
			InOut = new MB3DCubeInOut(1, 0, 0, 0, 0, 0, 0, 0),
			IndexBuf = new int[]{0,4,8}
		},

		// 2 points
        // #2
        new MB3DCubePrimitivePattern()
		{
			//02
			InOut = new MB3DCubeInOut(1, 0, 1, 0, 0, 0, 0, 0),
			IndexBuf = new int[]{1,10,0,8,0,10}
		},
        // #3
        new MB3DCubePrimitivePattern()
		{
			//06
			InOut = new MB3DCubeInOut(1, 0, 0, 0, 0, 0, 1, 0),
			IndexBuf = new int[]{0,4,8,3,5,10},
            IndexBufAlter = new int[]{0,3,8,5,8,3,0,4,3,10,3,4}
		},
        // #4
        new MB3DCubePrimitivePattern()
		{
			//07
			InOut = new MB3DCubeInOut(1, 0, 0, 0, 0, 0, 0, 1),
			IndexBuf = new int[]{0,4,8,3,11,7}
		},

		// 3 points
        // #5
        new MB3DCubePrimitivePattern()
		{
			//123
			InOut = new MB3DCubeInOut(0, 1, 1, 1, 0, 0, 0, 0),
			IndexBuf = new int[]{10,4,0,10,0,9,10,9,11}
		},
        // #6
        new MB3DCubePrimitivePattern()
		{
			//027
			InOut = new MB3DCubeInOut(1, 0, 1, 0, 0, 0, 0, 1),
			IndexBuf = new int[]{1,10,0,8,0,10,3,11,7},
            IndexBufAlter = new int[]{3,10,7,10,8,7,8,0,7,0,1,7,1,11,7}
		},
        // #7
        new MB3DCubePrimitivePattern()
		{
			//247
			InOut = new MB3DCubeInOut(0, 0, 1, 0, 1, 0, 0, 1),
			IndexBuf = new int[]{10,4,1,2,8,5,3,11,7}
		},

		// 4 points
        // #8
        new MB3DCubePrimitivePattern()
		{
			//0123
			InOut = new MB3DCubeInOut(1, 1, 1, 1, 0, 0, 0, 0),
			IndexBuf = new int[]{10,8,11,9,11,8}
		},        
        // #9
        new MB3DCubePrimitivePattern()
		{
			//0135
			InOut = new MB3DCubeInOut(1, 1, 0, 1, 0, 1, 0, 0),
			IndexBuf = new int[]{2,7,8,8,7,4,4,7,11,4,11,1}
		},
        // #10
        new MB3DCubePrimitivePattern()
		{
			//0347
			InOut = new MB3DCubeInOut(1, 0, 0, 1, 1, 0, 0, 1),
			IndexBuf = new int[]{2,0,5,4,5,0,3,1,7,6,7,1}
		},
        // #11
        new MB3DCubePrimitivePattern()
		{
			//0137
			InOut = new MB3DCubeInOut(1, 1, 0, 1, 0, 0, 0, 1),
			IndexBuf = new int[]{8,9,4,4,9,3,7,3,9,1,4,3}
		},
        // #12
        new MB3DCubePrimitivePattern()
		{
			//1234
			InOut = new MB3DCubeInOut(0, 1, 1, 1, 1, 0, 0, 0),
			IndexBuf = new int[]{2,8,5,10,4,0,10,0,9,10,9,11}
		},
        // #13
        new MB3DCubePrimitivePattern()
		{
			//0356
			InOut = new MB3DCubeInOut(1, 0, 0, 1, 0, 1, 1, 0),
			IndexBuf = new int[]{0,4,8,3,5,10,2,7,9,11,1,6}
		},
        // #14
        new MB3DCubePrimitivePattern()
		{
			//1235
			InOut = new MB3DCubeInOut(0, 1, 1, 1, 0, 1, 0, 0),
			IndexBuf = new int[]{2,4,0,2,11,4,7,11,2,10,4,11}
		},
	};

    static MetaballBuilder()
    {
        __InitCubePatterns();
    }
    
    static float CalcPower( Vector3 relativePos, float radius, float density )
    {
        float rate = relativePos.magnitude / (radius);
        if( rate > 1.0f )
        {
            return 0.0f;
        }
	    return density * Mathf.Max( (1.0f-rate)*(1.0f-rate), 0.0f );
    }

    const int _maxGridCellCount = 1000000;
    const int _maxVertexCount = 300000;

    public static int MaxGridCellCount
    {
        get { return _maxGridCellCount;  }
    }
    public static int MaxVertexCount
    {
        get { return _maxVertexCount; }
    }

    static MetaballBuilder _instance;
    public static MetaballBuilder Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MetaballBuilder();
            }
            return _instance;
        }
    }

    public class MetaballPointInfo
    {
        public Vector3 center;
        public float radius;
        public float density;
    }

    public string CreateMesh(MetaballCellClusterInterface rootCell, Transform root, float powerThreshold, float gridCellSize, Vector3 uDir, Vector3 vDir, Vector3 uvOffset, out Mesh out_mesh, MetaballCellObject cellObjPrefab = null, bool bReverse = false, Bounds ? fixedBounds = null, bool bAutoGridSize = false, float autoGridQuarity = 0.2f)
    {
        Mesh mesh = new Mesh();

        Bounds bounds;
        MetaballPointInfo[] points;

        AnalyzeCellCluster(rootCell, root, out bounds, out points, cellObjPrefab);
        //float cellSize = rootCell.BaseRadius * 0.3f;
        //float powerThreshold = 0.4f;

        if( fixedBounds != null )
        {
            bounds = fixedBounds.Value;
        }

        if( bAutoGridSize )
        {
            int targetCellCount = (int)(_maxGridCellCount * Mathf.Clamp01(autoGridQuarity));
            gridCellSize = Mathf.Pow(bounds.size.x * bounds.size.y * bounds.size.z / targetCellCount, 1.0f / 3.0f);
        }

        float approximateCellCount = (int)(bounds.size.x / gridCellSize) * (int)(bounds.size.y / gridCellSize) * (int)(bounds.size.z / gridCellSize);
        if (approximateCellCount > _maxGridCellCount)
        {
            out_mesh = mesh;

            return "Too many grid cells for building mesh (" + approximateCellCount.ToString() + " > " + _maxGridCellCount.ToString() + " )." + System.Environment.NewLine
                + "Make the area smaller or set larger (MetaballSeedBase.gridSize)";
        }

        BuildMetaballMesh(mesh, bounds.center, bounds.extents, gridCellSize, points, powerThreshold, bReverse, uDir, vDir, uvOffset);

        out_mesh = mesh;

        return null;
    }
    public string CreateMeshWithSkeleton(SkinnedMetaballCell rootCell, Transform root, float powerThreshold, float gridCellSize, Vector3 uDir, Vector3 vDir, Vector3 uvOffset, out Mesh out_mesh, out Transform[] out_bones, MetaballCellObject cellObjPrefab = null, bool bReverse = false, Bounds? fixedBounds = null, bool bAutoGridSize = false, float autoGridQuarity = 0.2f)
    {
        Mesh mesh = new Mesh();

        Bounds bounds;
        Transform [] bones;
        Matrix4x4 [] bindPoses;
        MetaballPointInfo [] points;

        AnalyzeCellClusterWithSkeleton(rootCell, root, out bounds, out bones, out bindPoses, out points, cellObjPrefab);
      //  float gridCellSize = rootCell.radius * Mathf.Lerp(0.3f, 0.1f, Mathf.Clamp01(detailLevel));

        if (fixedBounds != null)
        {
            bounds = fixedBounds.Value;
        }

        if (bAutoGridSize)
        {
            int targetCellCount = (int)(_maxGridCellCount * Mathf.Clamp01(autoGridQuarity));
            gridCellSize = Mathf.Pow(bounds.size.x * bounds.size.y * bounds.size.z / targetCellCount, 1.0f / 3.0f);
        }

        mesh.bindposes = bindPoses;

        float approximateCellCount = (int)(bounds.size.x / gridCellSize) * (int)(bounds.size.y / gridCellSize) * (int)(bounds.size.z / gridCellSize);
        if (approximateCellCount > _maxGridCellCount)
        {
            out_mesh = mesh;
            out_bones = bones;

            return "Too many grid cells for building mesh (" + approximateCellCount.ToString() + " > " + _maxGridCellCount.ToString() + " )." + System.Environment.NewLine
                + "Make the area smaller or set larger (MetaballSeedBase.gridSize)";
        }

        BuildMetaballMesh(mesh, bounds.center, bounds.extents, gridCellSize, points, powerThreshold, bReverse, uDir, vDir, uvOffset);

        // set boneweights
        out_mesh = mesh;
        out_bones = bones;

        return null;
    }

    void AnalyzeCellCluster(MetaballCellClusterInterface cellCluster, Transform root, out Bounds bounds, out MetaballPointInfo[] ballPoints, MetaballCellObject cellObjPrefab = null)
    {
        int cellCount = cellCluster.CellCount;

        Bounds tmpBounds = new Bounds(Vector3.zero, Vector3.zero);

        MetaballPointInfo[] tmpBallPoints = new MetaballPointInfo[cellCount];

        int cellIdx = 0;
        cellCluster.DoForeachCell((c) =>
        {
            // update bounds
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (c.modelPosition[i] - c.radius < tmpBounds.min[i])
                    {
                        Vector3 tmp = tmpBounds.min;
                        tmp[i] = c.modelPosition[i] - c.radius;
                        tmpBounds.min = tmp;
                    }
                    if (c.modelPosition[i] + c.radius > tmpBounds.max[i])
                    {
                        Vector3 tmp = tmpBounds.max;
                        tmp[i] = c.modelPosition[i] + c.radius;
                        tmpBounds.max = tmp;
                    }
                }
            }

            // update skeleton
            {
//                Vector3 pos = c.modelPosition;
                GameObject myBoneObj = null;

                if (cellObjPrefab)
                {
                    myBoneObj = (GameObject)GameObject.Instantiate(cellObjPrefab.gameObject);
                    myBoneObj.GetComponent<MetaballCellObject>().Setup(c);
                }
                else
                {
                    myBoneObj = new GameObject();
                }
                if (!string.IsNullOrEmpty(c.tag))
                {
                    myBoneObj.name = c.tag + "_Bone";
                }
                else
                {
                    myBoneObj.name = "Bone";
                }

                Transform myBone = myBoneObj.transform;

                {
                    myBone.parent = root;
                    myBone.localPosition = c.modelPosition;
                    myBone.localRotation = c.modelRotation;// Quaternion.identity;
                    myBone.localScale = Vector3.one;
                }
            }

            // update ball points
            {
                MetaballPointInfo point = new MetaballPointInfo();
                point.center = c.modelPosition;
                point.radius = c.radius;
                point.density = c.density;// 1.0f;
                tmpBallPoints[cellIdx] = point;
            }

            ++cellIdx;
        });

        bounds = tmpBounds;
        ballPoints = tmpBallPoints;
    }

    void AnalyzeCellClusterWithSkeleton( SkinnedMetaballCell rootCell, Transform root, out Bounds bounds, out Transform [] bones, out Matrix4x4 [] bindPoses, out MetaballPointInfo[] ballPoints, MetaballCellObject cellObjPrefab=null)
    {
        int cellCount = rootCell.CellCount;

        Transform[] tmpBones = new Transform[cellCount];
        Matrix4x4[] tmpBindPoses = new Matrix4x4[cellCount];
        Bounds tmpBounds = new Bounds(Vector3.zero, Vector3.zero);

        MetaballPointInfo[] tmpBallPoints = new MetaballPointInfo[cellCount];

        Dictionary<SkinnedMetaballCell, int> boneDictionary = new Dictionary<SkinnedMetaballCell, int>();

        int cellIdx = 0;
        rootCell.DoForeachSkinnedCell((c) =>
        {
            // update bounds
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (c.modelPosition[i] - c.radius < tmpBounds.min[i])
                    {
                        Vector3 tmp = tmpBounds.min;
                        tmp[i] = c.modelPosition[i] - c.radius;
                        tmpBounds.min = tmp;
                    }
                    if (c.modelPosition[i] + c.radius > tmpBounds.max[i])
                    {
                        Vector3 tmp = tmpBounds.max;
                        tmp[i] = c.modelPosition[i] + c.radius;
                        tmpBounds.max = tmp;
                    }
                }
            }

            // update skeleton
            {
//                Vector3 pos = c.modelPosition;
                GameObject myBoneObj = null;

                if (cellObjPrefab)
                {
                    myBoneObj = (GameObject)GameObject.Instantiate(cellObjPrefab.gameObject);
                    myBoneObj.GetComponent<MetaballCellObject>().Setup(c);
                }
                else
                {
                    myBoneObj = new GameObject();
                }

                if (!string.IsNullOrEmpty(c.tag))
                {
                    myBoneObj.name = c.tag + "_Bone";
                }
                else
                {
                    myBoneObj.name = "Bone";
                }

                Transform myBone = myBoneObj.transform;

                if (c.IsRoot)
                {
                    myBone.parent = root;
                    myBone.localPosition = Vector3.zero;
                    myBone.localRotation = c.modelRotation;// Quaternion.identity;
                    myBone.localScale = Vector3.one;
                }
                else
                {
                    Transform parentBone = tmpBones[boneDictionary[c.parent]];

                    myBone.parent = root;
                    myBone.localPosition = c.parent.modelPosition;
                    //myBone.localRotation = Quaternion.LookRotation(c.position - c.parent.position);
                    myBone.localRotation = c.modelRotation;
                    myBone.localScale = Vector3.one;

                    myBone.parent = parentBone;
                }

                tmpBones[cellIdx] = myBone;
                tmpBindPoses[cellIdx] = tmpBones[cellIdx].worldToLocalMatrix * root.localToWorldMatrix;

                boneDictionary.Add(c, cellIdx);
            }

            // update ball points
            {
                MetaballPointInfo point = new MetaballPointInfo();
                point.center = c.modelPosition;
                point.radius = c.radius;
                point.density = c.density;// 1.0f;
                tmpBallPoints[cellIdx] = point;
            }

            ++cellIdx;
        });
                
        bounds = tmpBounds;
        bones = tmpBones;
        bindPoses = tmpBindPoses;
        ballPoints = tmpBallPoints;
    }


    public Mesh CreateImplicitSurfaceMesh( int countX, int countY, int countZ, Vector3 [] positionMap, float [] powerMap, bool bReverse, float threshold, Vector3 uDir, Vector3 vDir, Vector3 uvOffset )
    {
        if (!__bCubePatternsInitialized)
        {
            __InitCubePatterns();
        }
        /*
        int resolutionX = countX;
        int resolutionY = countY;
        int resolutionZ = countZ;
        */
        int cellCount = countX * countY * countZ;

        Vector3[] gradientMap = new Vector3[cellCount];
        int[] pointMap = new int[cellCount * 3];
        bool[] inOutMap = new bool[cellCount];

        int gridStrideY = countX;
        int gridStrideZ = countX * countY;

        int pointDirStride = countX * countY * countZ;
                
        for (int i = 0; i < cellCount * 3; ++i)
        {
            pointMap[i] = -1;
        }

        const float powerEpsilon = 0.001f;
        for (int i = 0; i < cellCount; ++i)
        {
            float p = powerMap[i] - threshold;
            inOutMap[i] = p >= 0.0f;

            if (inOutMap[i])
            {
                if (p < powerEpsilon)
                {
                    powerMap[i] = threshold + powerEpsilon;
                }
            }
        }

        // calc gradient map
        for (int z = 1; z < countZ - 1; ++z)
        {
            for (int y = 1; y < countY - 1; ++y)
            {
                for (int x = 1; x < countX - 1; ++x)
                {
                    int idx = x + y * gridStrideY + z * gridStrideZ;

                    Vector3 gradient;
                    gradient.x = powerMap[idx + 1] - powerMap[idx - 1];
                    gradient.y = powerMap[idx + gridStrideY] - powerMap[idx - gridStrideY];
                    gradient.z = powerMap[idx + gridStrideZ] - powerMap[idx - gridStrideZ];

                    if (gradient.sqrMagnitude > 0.001f)
                    {
                        gradient.Normalize();
                    }

                    gradientMap[idx] = gradient;
                }
            }
        }

        int vertexCount = 0;
        // create vertices
        List<Vector3> positionList = new List<Vector3>();
        List<Vector3> normalList = new List<Vector3>();
//        List<BoneWeight> boneWeightList = new List<BoneWeight>();
        List<Vector2> uvList = new List<Vector2>();

        for (int z = 0; z < countZ && vertexCount < _maxVertexCount - 1; ++z)
        {
            for (int y = 0; y < countY && vertexCount < _maxVertexCount - 1; ++y)
            {
                for (int x = 0; x < countX && vertexCount < _maxVertexCount - 1; ++x)
                {
                    for (int dir = 0; dir < 3 && vertexCount < _maxVertexCount - 1; ++dir)
                    {
                        int dx = dir == 0 ? 1 : 0;
                        int dy = dir == 1 ? 1 : 0;
                        int dz = dir == 2 ? 1 : 0;

                        if (z + dz < countZ && y + dy < countY && x + dx < countX)
                        {
                            int idx0 = x + y * gridStrideY + z * gridStrideZ;
                            int idx1 = (x + dx) + (y + dy) * gridStrideY + (z + dz) * gridStrideZ;
                            float p0 = powerMap[idx0];
                            float p1 = powerMap[idx1];
                            if ((p0 - threshold) * (p1 - threshold) < 0.0f)
                            {
                                float alpha = (threshold - p0) / (p1 - p0);

                                Vector3 position = positionMap[idx1] * alpha + positionMap[idx0] * (1.0f - alpha);
                                positionList.Add(position);

                                Vector3 positionWithUVOffset = position + uvOffset;
                                uvList.Add(new Vector2(Vector3.Dot(positionWithUVOffset, uDir), Vector3.Dot(positionWithUVOffset, vDir)));
                                //vertexBuffer[vertexCount].Color = FLinearColor(1.0f,1.0f,1.0f);
                                Vector3 tmpNormal = -(gradientMap[idx1] * alpha + gradientMap[idx0] * (1.0f - alpha)).normalized;
                                normalList.Add(bReverse ? -tmpNormal : tmpNormal);

                                pointMap[dir * pointDirStride + idx0] = vertexCount;

                                vertexCount++;
                            }
                        }
                    }
                }
            }
        }

        int[] primaryPatternCounter = new int[MB3D_PATTERN_COUNT];

        // create indices
        int indexCount = 0;
        List<int> indexList = new List<int>();
        if (vertexCount > 3)
        {
            for (int z = 0; z < countZ - 1; ++z)
            {
                for (int y = 0; y < countY - 1; ++y)
                {
                    for (int x = 0; x < countX - 1; ++x)
                    {
                        // inoutbuf->pattern idx
                        byte inOutBits = 0;
                        for (int zoff = 0; zoff < 2; ++zoff)
                        {
                            for (int yoff = 0; yoff < 2; ++yoff)
                            {
                                for (int xoff = 0; xoff < 2; ++xoff)
                                {
                                    if (inOutMap[(x + xoff) + (y + yoff) * gridStrideY + (z + zoff) * gridStrideZ])
                                    {
                                        inOutBits |= (byte)(1 << (zoff * 4 + yoff * 2 + xoff));
                                    }
                                }
                            }
                        }

                        /// create local vertex map
                        int[] localVertexMap = new int[12];
                        for (int dir = 0; dir < 3; ++dir)
                        {
                            for (int a_1 = 0; a_1 < 2; ++a_1)
                            {
                                for (int a_2 = 0; a_2 < 2; ++a_2)
                                {
                                    int p_x, p_y, p_z;
                                    switch (dir)
                                    {
                                        case 0:
                                            p_x = x;
                                            p_y = y + a_1;
                                            p_z = z + a_2;
                                            break;
                                        case 1:
                                            p_x = x + a_2;
                                            p_y = y;
                                            p_z = z + a_1;
                                            break;
                                        case 2:
                                            p_x = x + a_1;
                                            p_y = y + a_2;
                                            p_z = z;
                                            break;
                                        default:
                                            p_x = p_y = p_z = -1;
                                            break;
                                    }
                                    int localIndex = dir * 4 + a_2 * 2 + a_1;
                                    localVertexMap[localIndex] = pointMap[dir * pointDirStride + p_x + p_y * gridStrideY + p_z * gridStrideZ];
                                }
                            }
                        }

                        int primaryPatternIdx = __cubePatterns[inOutBits].MatchingInfo.PrimaryPatternIndex;
                        primaryPatternCounter[primaryPatternIdx]++;

                        bool bErase = false;

                        if (!bErase)
                        {
                            for (int idx = 0; idx < __cubePatterns[inOutBits].MatchingInfo.GetTargetPrimitiveIndexBuffer().Length; ++idx)
                            {
                                /*
                                // error case
                                if (localVertexMap[__cubePatterns[inOutBits].IndexBuf[idx]] < 0)
                                {
                                    Debug.Log("(x,y,z)=" + x + "," + y + "," + z);
                                    Debug.Log("resolution=" + gridResolutionX + "," + gridResolutionY + "," + gridResolutionZ);
                                    {
                                        string tmp = "";
                                        for (int i = 0; i < 12; ++i)
                                        {
                                            tmp += (localVertexMap[i].ToString() + ",");
                                        }
                                        Debug.Log("localvtxmap=" + tmp);
                                    }
                                    Debug.Log("inout=" + System.Convert.ToString(inOutBits, 2));
                                    Debug.Log("idx=" + idx);
                                    Debug.Log("primaryPatternIdx=" + __cubePatterns[inOutBits].MatchingInfo.PrimaryPatternIndex);
                                    Debug.Log("indexCount=" + __primitivePatterns[__cubePatterns[inOutBits].MatchingInfo.PrimaryPatternIndex].IndexCount);

                                    {
                                        string tmp = "";
                                        for (int zoffset = 0; zoffset < 2; ++zoffset)
                                        {
                                            for (int yoffset = 0; yoffset < 2; ++yoffset)
                                            {
                                                for (int xoffset = 0; xoffset < 2; ++xoffset)
                                                {
                                                    int mapIdx = x + xoffset + (y + yoffset) * gridStrideY + (z + zoffset) * gridStrideZ;
                                                    tmp += (powerMap[mapIdx].ToString() + ",");
                                                }
                                            }
                                        }
                                        Debug.Log("powerMap=" + tmp);
                                    }
                                    throw new UnityException("vertex error");
                                }
                                */
                                indexList.Add(localVertexMap[__cubePatterns[inOutBits].IndexBuf[idx]]);
                                indexCount++;
                            }
                        }
                    }
                }
            }
        }

        Mesh mesh = new Mesh();

        mesh.vertices = positionList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.normals = normalList.ToArray();
        if (!bReverse)
        {
            mesh.triangles = indexList.ToArray();
        }
        else
        {
            indexList.Reverse();
            mesh.triangles = indexList.ToArray();
        }
        
        return mesh;
    }

    void BuildMetaballMesh( Mesh mesh, Vector3 center, Vector3 extent, float cellSize, MetaballPointInfo [] points, float powerThreshold, bool bReverse, Vector3 uDir, Vector3 vDir, Vector3 uvOffset)
    {         
	    if(!__bCubePatternsInitialized)
	    {
		    __InitCubePatterns();
	    }
        
        int halfResolutionX = (int)Mathf.CeilToInt(extent.x/cellSize)+1;
        int halfResolutionY = (int)Mathf.CeilToInt(extent.y/cellSize)+1;
        int halfResolutionZ = (int)Mathf.CeilToInt(extent.z/cellSize)+1;

        int resolutionX = halfResolutionX*2;
        int resolutionY = halfResolutionY*2;
        int resolutionZ = halfResolutionZ*2;

        int gridResolutionX = resolutionX;
        int gridResolutionY = resolutionY;
        int gridResolutionZ = resolutionZ;

        Vector3 actualExtent = new Vector3(halfResolutionX*cellSize, halfResolutionY*cellSize, halfResolutionZ*cellSize );

	    Vector3 gridOrigin = center - actualExtent;

        float [] powerMap = new float[resolutionX*resolutionY*resolutionZ];
        Vector3 [] positionMap = new Vector3[resolutionX*resolutionY*resolutionZ];
        Vector3[] gradientMap = new Vector3[resolutionX * resolutionY * resolutionZ];
        int [] pointMap = new int[resolutionX*resolutionY*resolutionZ*3];
        bool [] inOutMap = new bool[resolutionX*resolutionY*resolutionZ];

        BoneWeight[] boneWeightMap = new BoneWeight[resolutionX * resolutionY * resolutionZ];
	
	    int gridStrideY = gridResolutionX;
	    int gridStrideZ = gridResolutionX*gridResolutionY;

	    int pointDirStride = gridResolutionX*gridResolutionY*gridResolutionZ;

	    for( int z=0; z<gridResolutionZ; ++z )
	    {
		    for( int y=0; y<gridResolutionY; ++y )
		    {
			    for( int x=0; x<gridResolutionX; ++x )
			    {
				    positionMap[x+y*gridStrideY+z*gridStrideZ] = gridOrigin + new Vector3(cellSize*x, cellSize*y, cellSize*z);
			    }
		    }
	    }
	
	    for( int i=0; i<3*gridResolutionZ*gridResolutionY*gridResolutionX; ++i )
	    {
		    pointMap[i] = -1;
	    }

        int pointIdx = 0;
        foreach( MetaballPointInfo pointInfo in points )
        {
            int minCellX = (int)Mathf.Floor((pointInfo.center.x - center.x - pointInfo.radius) / cellSize) + halfResolutionX;
            int minCellY = (int)Mathf.Floor((pointInfo.center.y - center.y - pointInfo.radius) / cellSize) + halfResolutionY;
            int minCellZ = (int)Mathf.Floor((pointInfo.center.z - center.z - pointInfo.radius) / cellSize) + halfResolutionZ;

            minCellX = Mathf.Max(0, minCellX);
            minCellY = Mathf.Max(0, minCellY);
            minCellZ = Mathf.Max(0, minCellZ);
         
            int maxCellX = (int)Mathf.Ceil((pointInfo.center.x - center.x + pointInfo.radius) / cellSize) + halfResolutionX;
            int maxCellY = (int)Mathf.Ceil((pointInfo.center.y - center.y + pointInfo.radius) / cellSize) + halfResolutionY;
            int maxCellZ = (int)Mathf.Ceil((pointInfo.center.z - center.z + pointInfo.radius) / cellSize) + halfResolutionZ;

            maxCellX = Mathf.Min(gridResolutionX - 1, maxCellX);
            maxCellY = Mathf.Min(gridResolutionY - 1, maxCellY);
            maxCellZ = Mathf.Min(gridResolutionZ - 1, maxCellZ);

            for( int cellZ = minCellZ; cellZ <= maxCellZ; ++cellZ )
            {
                for( int cellY = minCellY; cellY <= maxCellY; ++cellY )
                {
                    for( int cellX = minCellX; cellX <= maxCellX; ++cellX )
                    {
						Vector3 cellPos = positionMap[cellX + cellY*gridStrideY + cellZ*gridStrideZ];
                        
                        float power = CalcPower(cellPos - pointInfo.center, pointInfo.radius, pointInfo.density);

						powerMap[cellX + cellY*gridStrideY + cellZ*gridStrideZ] += power;

                        if (power > 0.0f)
                        {
                            BoneWeight bw = boneWeightMap[cellX + cellY * gridStrideY + cellZ * gridStrideZ];
                            if (bw.weight0 < power || bw.weight1 < power)
                            {
                                if (bw.weight0 < bw.weight1)
                                {
                                    bw.weight0 = power;
                                    bw.boneIndex0 = pointIdx;
                                }
                                else
                                {
                                    bw.weight1 = power;
                                    bw.boneIndex1 = pointIdx;
                                }
                            }
                            boneWeightMap[cellX + cellY * gridStrideY + cellZ * gridStrideZ] = bw;
                        }
                    }
                }
            }
            ++pointIdx;
        }
	
	    // calc in/out
	    float threshold = powerThreshold;
        for( int i=0; i<gridResolutionX*gridResolutionY*gridResolutionZ; ++i )
        {
            inOutMap[i] = powerMap[i] >= threshold;

            if (inOutMap[i])
            {
                float powerEpsilon = 0.001f;
                if ( Mathf.Abs( powerMap[i] - threshold ) < powerEpsilon)
                {
                    if (powerMap[i] - threshold >= 0)
                    {
                        powerMap[i] = threshold + powerEpsilon;
                    }
                    else
                    {
                        powerMap[i] = threshold - powerEpsilon;
                    }
                }
            }
        }

	    // gradient
	    for( int z=1; z<gridResolutionZ-1; ++z )
	    {
		    for( int y=1; y<gridResolutionY-1; ++y )
		    {
			    for( int x=1; x<gridResolutionX-1; ++x )
			    {
				    gradientMap[x+y*gridStrideY+z*gridStrideZ].x = powerMap[(x+1)+y*gridStrideY+z*gridStrideZ]-powerMap[(x-1)+y*gridStrideY+z*gridStrideZ];
				    gradientMap[x+y*gridStrideY+z*gridStrideZ].y = powerMap[x+(y+1)*gridStrideY+z*gridStrideZ]-powerMap[x+(y-1)*gridStrideY+z*gridStrideZ];
				    gradientMap[x+y*gridStrideY+z*gridStrideZ].z = powerMap[x+y*gridStrideY+(z+1)*gridStrideZ]-powerMap[x+y*gridStrideY+(z-1)*gridStrideZ];

                    if (gradientMap[x + y * gridStrideY + z * gridStrideZ].sqrMagnitude > 0.001f)
                    {
                        gradientMap[x + y * gridStrideY + z * gridStrideZ].Normalize();
                    }
			    }
		    }
	    }

        int vertexCount = 0;
	    // create vertices
        List<Vector3> positionList = new List<Vector3>();
        List<Vector3> normalList = new List<Vector3>();
        List<BoneWeight> boneWeightList = new List<BoneWeight>();
        List<Vector2> uvList = new List<Vector2>();

	    for( int z=0; z<gridResolutionZ && vertexCount < _maxVertexCount-1; ++z )
	    {
		    for( int y=0; y<gridResolutionY && vertexCount < _maxVertexCount-1; ++y )
		    {
			    for( int x=0; x<gridResolutionX && vertexCount < _maxVertexCount-1; ++x )
			    {
				    for( int dir=0; dir<3 && vertexCount < _maxVertexCount-1; ++dir )
				    {
					    int dx = dir==0 ? 1 : 0;
					    int dy = dir==1 ? 1 : 0;
					    int dz = dir==2 ? 1 : 0;

					    if( z+dz < gridResolutionZ && y+dy < gridResolutionY && x+dx < gridResolutionX )
					    {
						    int idx0 = x+y*gridStrideY+z*gridStrideZ;
						    int idx1 = (x+dx)+(y+dy)*gridStrideY+(z+dz)*gridStrideZ;
						    float p0 = powerMap[idx0];
						    float p1 = powerMap[idx1];
						    if( ( p0 - threshold ) * ( p1 - threshold ) < 0.0f )
						    {
							    float alpha = ( threshold - p0 ) / ( p1 - p0 );

                                Vector3 position = positionMap[idx1]*alpha + positionMap[idx0]*(1.0f-alpha);
                                positionList.Add( position );

                                Vector3 positionWithUVOffset = position + uvOffset;
                                uvList.Add(new Vector2(Vector3.Dot(positionWithUVOffset, uDir), Vector3.Dot(positionWithUVOffset, vDir)));
							    //vertexBuffer[vertexCount].Color = FLinearColor(1.0f,1.0f,1.0f);
                                Vector3 tmpNormal = -(gradientMap[idx1] * alpha + gradientMap[idx0] * (1.0f - alpha)).normalized;
                                normalList.Add(bReverse ? -tmpNormal : tmpNormal);

                                BoneWeight sourceBW;
                                if (p0 > p1)
                                {
                                    sourceBW = boneWeightMap[idx0];
                                }
                                else
                                {
                                    sourceBW = boneWeightMap[idx1];
                                }

                                BoneWeight bw = new BoneWeight();
                                float weightSum = sourceBW.weight0+sourceBW.weight1;
                                if (weightSum > 0.0f)
                                {
                                    if (sourceBW.weight0 > sourceBW.weight1)
                                    {
                                        bw.weight0 = sourceBW.weight0 / weightSum;
                                        bw.boneIndex0 = sourceBW.boneIndex0;
                                        bw.weight1 = sourceBW.weight1 / weightSum;
                                        bw.boneIndex1 = sourceBW.boneIndex1;
                                    }
                                    else
                                    {
                                        bw.weight0 = sourceBW.weight1 / weightSum;
                                        bw.boneIndex0 = sourceBW.boneIndex1;
                                        bw.weight1 = sourceBW.weight0 / weightSum;
                                        bw.boneIndex1 = sourceBW.boneIndex0;
                                    }
                                }
                                else
                                {
                                    Debug.LogError("invalid boneweight");
                                }
                                boneWeightList.Add(bw);
//                            TODO : add to boneWeightList

							    pointMap[dir*pointDirStride+idx0] = vertexCount;

							    vertexCount++;
						    }
					    }
				    }
			    }
		    }
	    }

        int[] primaryPatternCounter = new int[MB3D_PATTERN_COUNT];

	    // create indices
        int indexCount = 0;
        List<int> indexList = new List<int>();
	    if( vertexCount > 3 )
	    {
		    for( int z=0; z<gridResolutionZ-1; ++z )
		    {
			    for( int y=0; y<gridResolutionY-1; ++y )
			    {
				    for( int x=0; x<gridResolutionX-1; ++x )
				    {
					    // inoutbuf->pattern idx
					    byte inOutBits=0;
					    for( int zoff=0; zoff<2; ++zoff )
					    {
						    for( int yoff=0; yoff<2; ++yoff )
						    {
							    for( int xoff=0; xoff<2; ++xoff )
							    {
								    if( inOutMap[(x+xoff)+(y+yoff)*gridStrideY+(z+zoff)*gridStrideZ] )
								    {
									    inOutBits |= (byte)( 1<<(zoff*4+yoff*2+xoff) );
								    }
							    }
						    }
					    }

					    /// create local vertex map
					    int [] localVertexMap = new int[12];
					    for( int dir=0; dir<3; ++dir )
					    {
						    for( int a_1=0; a_1<2; ++a_1 )
						    {
							    for( int a_2=0; a_2<2; ++a_2 )
							    {
								    int p_x, p_y, p_z;
								    switch( dir )
								    {
								    case 0:
									    p_x = x;
									    p_y = y+a_1;
									    p_z = z+a_2;
									    break;
								    case 1:
									    p_x = x+a_2;
									    p_y = y;
									    p_z = z+a_1;
									    break;
								    case 2:
									    p_x = x+a_1;
									    p_y = y+a_2;
									    p_z = z;
									    break;
								    default:
									    p_x=p_y=p_z=-1;
									    break;
								    }
								    int localIndex = dir*4+a_2*2+a_1;
								    localVertexMap[localIndex] = pointMap[dir*pointDirStride + p_x + p_y*gridStrideY + p_z*gridStrideZ];
							    }
						    }
					    }

                        int primaryPatternIdx = __cubePatterns[inOutBits].MatchingInfo.PrimaryPatternIndex;
                        primaryPatternCounter[primaryPatternIdx]++;

                        bool bErase = false;
                        
                        if (!bErase)
                        {
                            for( int idx=0; idx<__cubePatterns[inOutBits].MatchingInfo.GetTargetPrimitiveIndexBuffer().Length; ++idx )
                            {
                                // error case
                                if (localVertexMap[__cubePatterns[inOutBits].IndexBuf[idx]] < 0)
                                {
                                    Debug.Log("(x,y,z)=" + x + "," + y + "," + z);
                                    Debug.Log("resolution=" + gridResolutionX + "," + gridResolutionY + "," + gridResolutionZ);
                                    {
                                        string tmp = "";
                                        for (int i = 0; i < 12; ++i)
                                        {
                                            tmp += (localVertexMap[i].ToString() + ",");
                                        }
                                        Debug.Log("localvtxmap=" + tmp);
                                    }
                                    Debug.Log("inout=" + System.Convert.ToString(inOutBits, 2));
                                    Debug.Log("idx=" + idx);
                                    Debug.Log("primaryPatternIdx=" + __cubePatterns[inOutBits].MatchingInfo.PrimaryPatternIndex);
                                    Debug.Log("indexCount=" + __primitivePatterns[__cubePatterns[inOutBits].MatchingInfo.PrimaryPatternIndex].IndexCount);

                                    {
                                        string tmp = "";
                                        for (int zoffset = 0; zoffset < 2; ++zoffset)
                                        {
                                            for (int yoffset = 0; yoffset < 2; ++yoffset)
                                            {
                                                for (int xoffset = 0; xoffset < 2; ++xoffset)
                                                {
                                                    int mapIdx = x + xoffset + (y + yoffset) * gridStrideY + (z + zoffset) * gridStrideZ;
                                                    tmp += (powerMap[mapIdx].ToString() + ",");
                                                }
                                            }
                                        }
                                        Debug.Log("powerMap=" + tmp);
                                    }
                                    throw new UnityException("vertex error");
                                }
                                indexList.Add(localVertexMap[__cubePatterns[inOutBits].IndexBuf[idx]]);
                                indexCount++;
                            }
                        }
				    }
			    }
		    }
	    }

        mesh.vertices = positionList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.normals = normalList.ToArray();
        if (!bReverse)
        {
            mesh.triangles = indexList.ToArray();
        }
        else
        {
            indexList.Reverse();
            mesh.triangles = indexList.ToArray();
        }
        mesh.boneWeights = boneWeightList.ToArray();
    }
}
