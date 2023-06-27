using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SkeletonGraphic))]
public class AnimationCharactor : MonoBehaviour
{
    [HideInInspector]
    public SkeletonGraphic charactor => GetComponent<SkeletonGraphic>();
    public Image circleImage;
    public eCharactorMotion eMotion;
    public eCharactorDetail eDetail;

    [HideInInspector]
    public eCharactorMotion eIdleMotion;
    [HideInInspector]
    public eCharactorDetail eIdleDetail;
    public eCharactorMotion eCorrectMotion;
    public eCharactorDetail eCorrectDetail;

    public event Action onChangeAction;

    private string skeletonName;
    private string skeletonType;
    private string skeletonPath;
    private bool isTransaction = true;
    public bool isCompleted { get; private set; }

    private void Awake()
    {
        eIdleMotion = eMotion;
        eIdleDetail = eDetail;
        MotionChange(eMotion, eDetail);

        if (eMotion == eCharactorMotion.Daino_Default)
            StartCoroutine(charactorRoutine());
    }
    public void MotionChange(eCharactorMotion eMotion, eCharactorDetail eDetail, bool isLoop = true)
    {
        StartCoroutine(MotionRoutine(eMotion, eDetail, isLoop));
    }

    IEnumerator MotionRoutine(eCharactorMotion eMotion, eCharactorDetail eDetail, bool isLoop)
    {
        if (!isLoop)
            while (!isCompleted)
                yield return null;
        isCompleted = false;

        this.eMotion = eMotion;
        this.eDetail = eDetail;

        var temp = eMotion.ToString().Split('_');

        if (eMotion.ToString().Contains("Bambam"))
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

        SkeletonChange(isLoop);
    }

    /// <summary>
    /// SkeletonGraphic Asset data Change 
    /// </summary>
    void SkeletonChange(bool isLoop)
    {
        var skeletonDataAsset = Resources.Load<SkeletonDataAsset>(skeletonPath);
        charactor.skeletonDataAsset = skeletonDataAsset;
        charactor.Initialize(true);

        DetailChange(eDetail, isLoop);
    }

    /// <summary>
    /// SkeletonGraphic Animation Change
    /// </summary>
    public void DetailChange(eCharactorDetail eDetail, bool isLoop)
    {
        this.eDetail = eDetail;

        //var containeCheck = eDetail.ToString().Contains(eMotion.ToString());
        var detailValue = eDetail.ToString().Replace(eMotion.ToString() + "_", string.Empty);
        
        charactor.AnimationState.SetAnimation(1, detailValue, isLoop);
        charactor.AnimationState.SetEmptyAnimation(0, 0);
        charactor.AnimationState.Complete += OnSpineAnimationComplete;

        charactor.unscaledTime = false;

        onChangeAction?.Invoke();
    }

    private void OnSpineAnimationComplete(TrackEntry trackEntry)
    {
        isCompleted = trackEntry.IsComplete;
    }

    public void DetailChange(string detailValue, bool isLoop = true)
    {
        charactor.AnimationState.SetAnimation(1, detailValue, isLoop);
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
            case eCharactorDetail.soo_Selected_cheekIdle:
                eDetail = eCharactorDetail.soo_Selected_cheekSmile;
                break;
            case eCharactorDetail.mark_LoadingCompleted_snooze:
                eDetail = eCharactorDetail.mark_LoadingCompleted_wakeUp;
                break;
            case eCharactorDetail.eric_Selected_twoHand:
                eMotion = eCharactorMotion.eric_LoadingCompleted;
                eDetail = eCharactorDetail.eric_LoadingCompleted_jumpSmile;
                break;
            case eCharactorDetail.mia_LoadingCompleted_mirror:
                eDetail = eCharactorDetail.mia_LoadingCompleted_mirrorHi;
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

    public void LoadingRoutine()
    {
        StartCoroutine(charactorRoutine());
    }

    private IEnumerator charactorRoutine()
    {
        while (true)
        {
            var randomTime = Random.Range(5, 10);
            yield return new WaitForSecondsRealtime(randomTime);

            LoadingScene();
        }
    }

    private void LoadingScene()
    {
        isTransaction = !isTransaction;

        switch(eMotion)
        {
            case eCharactorMotion.mark_LoadingCompleted:
                eDetail = isTransaction ? eCharactorDetail.mark_LoadingCompleted_snooze : eCharactorDetail.mark_LoadingCompleted_wakeUp;
                break;
            case eCharactorMotion.eric_LoadingCompleted:
                eDetail = isTransaction ? eCharactorDetail.eric_LoadingCompleted_crossArm : eCharactorDetail.eric_LoadingCompleted_exclamation;
                break;
            case eCharactorMotion.mia_LoadingCompleted:
                eDetail = isTransaction ? eCharactorDetail.mia_LoadingCompleted_mirror : eCharactorDetail.mia_LoadingCompleted_mirrorHi;
                break;
            case eCharactorMotion.Daino_Default:
                eDetail = isTransaction ? eCharactorDetail.Daino_Default_walk : eCharactorDetail.Daino_Default_run;
                break;
            case eCharactorMotion.soo_Loading:
                eMotion = eCharactorMotion.soo_LoadingCompleted;
                eDetail = eCharactorDetail.soo_LoadingComplted_sitSmile;
                break;
            case eCharactorMotion.soo_LoadingCompleted:
                eMotion = eCharactorMotion.soo_Loading;
                eDetail = eCharactorDetail.soo_Loading_sitAssemble;
                break;

            default:
                break;
        }

        if (eMotion.ToString().Contains("soo"))
            MotionChange(eMotion, eDetail);
        else
            DetailChange(eDetail, false);
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

        var value = isTransaction ? defaultList[0] : defaultList[1];

        DetailChange(value);
    }
}
