//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetaballCellCluster : MetaballCellClusterInterface {

    List<MetaballCell> _cells = new List<MetaballCell>();
    float _baseRadius;

    Vector3 _baseColor = Vector3.one;

    public float BaseRadius { 
        get { return _baseRadius; }
        set { _baseRadius = value; }
    }

    public void DoForeachCell(ForeachCellDeleg deleg)
    {
        foreach( MetaballCell c in _cells )
        {
            deleg(c);
        }
    }

    public MetaballCell GetCell(int index)
    {
        return _cells[index];
    }

    public int FindCell(MetaballCell cell)
    {
        for (int i = 0; i < _cells.Count; ++i)
        {
            if (_cells[i] == cell)
            {
                return i;
            }
        }

        return -1;
    }

    public int CellCount { get { return _cells.Count; } }

    public MetaballCell AddCell(Vector3 position, float minDistanceCoef = 1.0f, float ? radius = null, string tag = null)
    {
        MetaballCell cell = new MetaballCell();

        cell.baseColor = _baseColor;
        cell.radius = ( radius == null ) ? _baseRadius : radius.Value;
        cell.modelPosition = position;
        cell.tag = tag;

        bool bFail = false;
        DoForeachCell((c) =>
        {
            if ((cell.modelPosition - c.modelPosition).sqrMagnitude < cell.radius * cell.radius * minDistanceCoef * minDistanceCoef)
            {
                bFail = true;
            }
        });

        if (!bFail)
        {
            _cells.Add(cell);
        }

        return bFail ? null : cell;
    }

    public void RemoveCell(MetaballCell cell)
    {
        _cells.Remove(cell);
    }

    public void ClearCells()
    {
        _cells.Clear();
    }

    public string GetPositionsString()
    {
        string retval = "";

        foreach (MetaballCell cell in _cells)
        {
            retval += cell.modelPosition.ToString("F3");
            retval += ";";
        }

        if (retval[retval.Length - 1] == ';')
        {
            retval = retval.Substring(0, retval.Length - 1);
        }

        return retval;
    }

    public void ReadPositionsString(string positions)
    {
        ClearCells();

        string[] cells = positions.Split(';');

        if (cells.Length == 0)
        {
            throw new UnityException("invalid input positions data :" + positions);
        }

        for (int i = 0; i < cells.Length; ++i)
        {
            Vector3 pos;

            pos = ParseVector3(cells[i]);
            AddCell(pos, 0.0f);
        }
    }


    static Vector3 ParseVector3(string data)
    {
        int begin = data.IndexOf('(');
        int end = data.IndexOf(')');

        string content = data.Substring(begin + 1, end - begin - 1);

        string[] elements = content.Split(',');

        Vector3 retval = Vector3.zero;
        for (int i = 0; i < 3 && i < elements.Length; ++i)
        {
            retval[i] = float.Parse(elements[i]);
        }

        return retval;
    }
}
