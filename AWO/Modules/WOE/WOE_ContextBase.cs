﻿using LevelGeneration;
using System;

namespace AWO.Modules.WOE;

internal abstract class WOE_ContextBase
{
    public abstract eWardenObjectiveType TargetType { get; }
    public abstract Type DataType { get; }
    protected LG_LayerType Layer { get; private set; }
    protected int ChainIndex { get; private set; }

    public void Setup(LG_LayerType layer, int chainIndex)
    {
        Layer = layer;
        ChainIndex = chainIndex;
    }

    public virtual void OnSetup()
    {

    }

    public virtual void OnBuildDone()
    {

    }

    public virtual void OnBuildDoneLate()
    {

    }

    public virtual void OnLevelCleanup()
    {

    }
}
