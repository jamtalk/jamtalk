using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class AccessoriesElement : MonoBehaviour
{
    public Image image;
    public float targetValue;
    public string targetBoneName;

    private SkeletonGraphic charactor => GetComponentInParent<SkeletonGraphic>();
    private Spine.Bone targetBone;

    private Coroutine coroutine;

    private void Awake()
    {
        gameObject.SetActive(true);
        GetBoneData();
    }

    private void GetBoneData()
    {
        for (int i = 0; i < charactor.Skeleton.Bones.Count; i++)
        {
            Spine.Bone bone = charactor.Skeleton.Bones.Items[i];

            if (bone.Data.Name == targetBoneName)
            {
                coroutine = StartCoroutine(FollowTarget(bone));
                break;
            }
        }
        
    }

    private IEnumerator FollowTarget(Spine.Bone bone)
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            targetBone = bone;

            Vector3 pos = targetBone.GetSkeletonSpacePosition();
            pos *= 100f;
            pos.y += targetValue;
            transform.localPosition = pos;
            gameObject.SetActive(true);
        }
    }
}
