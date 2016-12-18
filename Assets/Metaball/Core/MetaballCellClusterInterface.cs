//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public delegate void ForeachCellDeleg(MetaballCell c);

public interface MetaballCellClusterInterface
{
    float BaseRadius { get; }

    void DoForeachCell(ForeachCellDeleg deleg);

    int CellCount { get; }
}
