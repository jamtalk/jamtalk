using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AnimationCharactor : MonoBehaviour
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
    public void MotionChange(eCharactorMotion eMotion, eCharactorDetail eDetail, bool isLoof = true)
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

        //var containeCheck = eDetail.ToString().Contains(eMotion.ToString());
        var detailValue = eDetail.ToString().Replace(eMotion.ToString() + "_", string.Empty);
        
        charactor.AnimationState.SetAnimation(1, detailValue, bLoof);
        charactor.AnimationState.SetEmptyAnimation(0, 0);
        charactor.unscaledTime = false;
    }

    public void CenterAction()
    {
        if (eDetail == eCharactorDetail.ejiji_Default_idle)
        {
            eMotion = eCharactorMotion.ejiji_LoadingCompleted;
            eDetail = eCharactorDetail.ejiji_LoadingCompleted_pout;
            MotionChange(eMotion, eDetail, true);
        }

    }

    public void SelectedAction()
    {
        if (eDetail == eCharactorDetail.soo_Selected_cheek_idle)
            eDetail = eCharactorDetail.soo_Selected_cheek_smile;
        else if (eDetail == eCharactorDetail.mark_LoadingComlpeted_snooze)
            eDetail = eCharactorDetail.mark_LoadingComlpeted_wakeUp;
        else if (eDetail == eCharactorDetail.eric_Selected_twoHand)
        {
            eMotion = eCharactorMotion.eric_LoadingCompleted;
            eDetail = eCharactorDetail.eric_LoadingCompleted_jump_smile;
        }
        else if (eDetail == eCharactorDetail.mia_LoadingCompleted_mirror)
            eDetail = eCharactorDetail.mia_LoadingCompleted_mirror_hi;
        else if (eDetail == eCharactorDetail.ecoco_Default_idle)
        {
            eMotion = eCharactorMotion.ecoco_Congrats;
            eDetail = eCharactorDetail.ecoco_Congrats_congrats;
        }
        else if (eDetail == eCharactorDetail.ejiji_LoadingCompleted_pout)
        {
            eMotion = eCharactorMotion.ejiji_Selected;
            eDetail = eCharactorDetail.ejiji_Selected_selected;
        }
        else
        {

        }

        MotionChange(eMotion, eDetail, true);

    }
}
