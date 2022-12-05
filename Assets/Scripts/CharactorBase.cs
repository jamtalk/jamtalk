using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public abstract class CharactorBase : MonoBehaviour
{
    public SkeletonGraphic charactor;
    public eCharactorMotion eMotion;

    protected string skeletonName;
    protected string skeletonType;
    protected string skeletonPath;

    private void Awake()
    {
        GetSkeletonPath();
    }

    private void GetSkeletonPath()
    {
        var temp =  eMotion.ToString().Split('_');

        if(eMotion.ToString().Contains("Bambam"))
        {
            skeletonName = "BamBam";
            skeletonType = eMotion.ToString();
        }
        else
        {
            skeletonName = temp[0];
            skeletonType = temp[1];
        }

        skeletonPath = string.Format("SpineAnimations/{0}/{1}/{2}_SkeletonData"
            , skeletonName, skeletonType, eMotion.ToString());

        SkeletonChange();
    }

    /// <summary>
    /// SkeletonGraphic Asset data Change 
    /// </summary>
    void SkeletonChange()
    {
        var skeletonDataAsset = Resources.Load<SkeletonDataAsset>(skeletonPath);
        charactor.skeletonDataAsset = skeletonDataAsset;
        charactor.Initialize(true);
    }

    /// <summary>
    /// SkeletonGraphic Animation Change
    /// </summary>
    void DetailChange()
    {
        charactor.AnimationState.SetAnimation(1, "2_hi", false);
        charactor.AnimationState.SetEmptyAnimation(0, 0);
        charactor.unscaledTime = false;
    }
}
