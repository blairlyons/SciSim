//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkinnedMetaballCell : MetaballCell, MetaballCellClusterInterface
{ 
    public SkinnedMetaballCell parent;
    public List<SkinnedMetaballCell> children = new List<SkinnedMetaballCell>();

    public List<SkinnedMetaballCell> links = new List<SkinnedMetaballCell>();

    public int distanceFromRoot;

    public float BaseRadius
    {
        get { return radius; }
    }

    public bool IsRoot
    {
        get { return parent == null; }
    }

    public bool IsTerminal
    {
        get { return children.Count == 0; }
    }

    public bool IsBranch
    {
        get { return IsRoot || (children.Count > 1); }
    }

    public SkinnedMetaballCell Root
    {
        get
        {
            if (IsRoot) return this;
            else return parent.Root;
        }
    }
    
    public int CellCount
    {
        get
        {
            int retval = 1;

            foreach (SkinnedMetaballCell c in children)
            {
                retval += c.CellCount;
            }

            return retval;
        }
    }

    public delegate void ForeachSkinnedCellDeleg(SkinnedMetaballCell c);

    public void DoForeachSkinnedCell(ForeachSkinnedCellDeleg deleg)
    {
        deleg(this);

        foreach (SkinnedMetaballCell c in children)
        {
            c.DoForeachSkinnedCell(deleg);
        }
    }

    public void DoForeachCell(ForeachCellDeleg deleg)
    {
        deleg(this);

        foreach (SkinnedMetaballCell c in children)
        {
            c.DoForeachCell(deleg);
        }
    }

    public int DistanceFromBranch
    {
        get
        {
            if (IsBranch)
            {
                return 0;
            }
            int fromLast = DistanceFromLastBranch;
            int toNext = DistanceToNextBranch;

            return Mathf.Min(fromLast, toNext);
        }
    }

    public int DistanceFromLastLink
    {
        get
        {
            if (IsRoot || children.Count > 1 || links.Count > 0)
            {
                return 0;
            }
            else
            {
                return parent.DistanceFromLastLink + 1;
            }
        }
    }

    int DistanceFromLastBranch
    {
        get
        {
            if (IsBranch) return 0;

            return 1 + parent.DistanceFromLastBranch;
        }
    }

    int DistanceToNextBranch
    {
        get
        {
            if (IsBranch) return 0;

            int nearest = int.MaxValue;
            foreach (SkinnedMetaballCell c in children)
            {
                int tmp = c.DistanceToNextBranch;
                if (tmp < nearest)
                {
                    nearest = tmp;
                }
            }

            return nearest;
        }
    }

    public SkinnedMetaballCell AddChild(Vector3 position, float in_radius, float minDistanceCoef = 1.0f)
    {
        SkinnedMetaballCell child = new SkinnedMetaballCell();

        child.baseColor = baseColor;
   //     child.resource = 0.0f;
        child.radius = in_radius;
        child.distanceFromRoot = distanceFromRoot + 1;
        child.modelPosition = position;

        child.parent = this;
        children.Add(child);

        // collision test
        bool bFail = false;
        Root.DoForeachSkinnedCell((c) =>
        {
            if (c != child)
            {
                if ((child.modelPosition - c.modelPosition).sqrMagnitude < child.radius * child.radius * minDistanceCoef * minDistanceCoef)
                {
                    bFail = true;
                }
            }
        });

        if (bFail)
        {
            children.Remove(child);
            return null;
        }

        child.CalcRotation();

        return child;
    }

    // assume all positions and parent rotation already calculated
    public void CalcRotation()
    {
        if (IsRoot)
        {
            modelRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up);
        }
        else
        {
            Vector3 vecFrom = parent.modelRotation * Vector3.forward;
            Vector3 vecTo = modelPosition - parent.modelPosition;

            modelRotation = Quaternion.FromToRotation(vecFrom, vecTo) * parent.modelRotation;
        }
    }

    public string GetStringExpression()
    {
        string retval = "";
        
        retval += modelPosition.ToString("F3");
        retval += ";";

        foreach (SkinnedMetaballCell cell in children)
        {
            retval += cell.GetStringExpression();
            retval += ";";
        }

        if (retval[retval.Length - 1] == ';')
        {
            retval = retval.Substring(0, retval.Length - 1);
        }

        return retval;
    }

    public static SkinnedMetaballCell ConstructFromString(string data, float radius)
    {
        string[] cells = data.Split(';');

        if (cells.Length == 0)
        {
            throw new UnityException("invalid input data :"+data);
        }

        // first one is root
        SkinnedMetaballCell rootCell = new SkinnedMetaballCell();
        rootCell.parent = null;
        //_rootCell.directionFromParent = -1;
        rootCell.modelPosition = ParseVector3(cells[0]);
        rootCell.radius = radius;
  //      rootCell.resource = 100.0f;
        rootCell.baseColor = Vector3.zero;

        rootCell.CalcRotation();

        for (int i = 1; i < cells.Length; ++i)
        {
            Vector3 pos;

            pos = ParseVector3(cells[i]);
            rootCell.AddChild(pos, radius, 0.0f);
        }

        return rootCell;
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
