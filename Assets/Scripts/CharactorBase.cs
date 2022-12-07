using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class CharactorBase : MonoBehaviour
{
    public SkeletonGraphic charactor;
    public eCharactorMotion eMotion;
    public eCharactorDetail eDetail;

    private string skeletonName;
    private string skeletonType;
    private string skeletonPath;

    private void Awake()
    {
        MotionChange(eMotion, eDetail);
    }
    public void MotionChange(eCharactorMotion eMotion, eCharactorDetail eDetail)
    {
        this.eMotion = eMotion;
        this.eDetail = eDetail;

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

        DetailChange(eDetail, true);
    }

    /// <summary>
    /// SkeletonGraphic Animation Change
    /// </summary>
    public void DetailChange(eCharactorDetail eDetail, bool bLoof = false)
    {
        this.eDetail = eDetail;

        var detailValue = eDetail.ToString().Replace(eMotion.ToString() + "_", string.Empty);

        charactor.AnimationState.SetAnimation(1, detailValue, bLoof);
        charactor.AnimationState.SetEmptyAnimation(0, 0);
        charactor.unscaledTime = false;
    }
}
