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
    private bool isTransaction = true;

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

    public void DetailChange(string detailValue, bool bLoof = true)
    {
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
            MotionChange(eMotion, eDetail);
        }
    }
    public void SideAction()
    {
        if(eDetail == eCharactorDetail.ejiji_LoadingCompleted_pout)
        {
            eMotion = eCharactorMotion.ejiji_Default;
            eDetail = eCharactorDetail.ejiji_Default_idle;
            MotionChange(eMotion, eDetail);
        }
    }

    public void SelectedAction()
    {
        switch (eDetail)
        {
            case eCharactorDetail.soo_Selected_cheek_idle:
                eDetail = eCharactorDetail.soo_Selected_cheek_smile;
                break;
            case eCharactorDetail.mark_LoadingComlpeted_snooze:
                eDetail = eCharactorDetail.mark_LoadingComlpeted_wakeUp;
                break;
            case eCharactorDetail.eric_Selected_twoHand:
                eMotion = eCharactorMotion.eric_LoadingCompleted;
                eDetail = eCharactorDetail.eric_LoadingCompleted_jump_smile;
                break;
            case eCharactorDetail.mia_LoadingCompleted_mirror:
                eDetail = eCharactorDetail.mia_LoadingCompleted_mirror_hi;
                break;
            case eCharactorDetail.ecoco_Default_idle:
                eMotion = eCharactorMotion.ecoco_Congrats;
                eDetail = eCharactorDetail.ecoco_Congrats_congrats;
                break;
            case eCharactorDetail.ejiji_LoadingCompleted_pout:
                eMotion = eCharactorMotion.ejiji_Selected;
                eDetail = eCharactorDetail.ejiji_Selected_selected;
                break;
            default:
                break;
        }

        MotionChange(eMotion, eDetail);

    }


    public void OpningScene()
    {
        isTransaction = !isTransaction;
        List<string> defaultList;
        if (eMotion == eCharactorMotion.Daino_Default)
            defaultList = new List<string> { "walk", "run" };
        else if (eMotion == eCharactorMotion.BlackMamba_Default)
            defaultList = new List<string> { "idle", "idle" };
        else
            defaultList = new List<string> { "idle", "hi" };
        Debug.LogFormat("{0}, {1}, {2}", defaultList[0], defaultList[1], isTransaction);
        var value = isTransaction ? defaultList[0] : defaultList[1];

        DetailChange(value);
    }
}
