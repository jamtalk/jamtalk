using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class AccessoriesElement : MonoBehaviour
{
    public Image image;
    public float targetY;
    public float targetX;
    public string targetBoneName;

    private AnimationCharactor charactor => GetComponentInParent<AnimationCharactor>();
    private Spine.Bone targetBone;

    private Coroutine coroutine;

    private void Awake()
    {
        gameObject.SetActive(true);
        GetBoneData();
        charactor.onChangeAction += GetBoneData;
    }

    private void GetBoneData()
    {
        for (int i = 0; i < charactor.charactor.Skeleton.Bones.Count; i++)
        {
            Spine.Bone bone = charactor.charactor.Skeleton.Bones.Items[i];

            Debug.Log(bone.Data.Name);
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
            foreach (var item in charactor.charactor.Skeleton.Bones.Items)
                targetBone = item.Data.Name == targetBoneName ? item : bone;

            yield return new WaitForEndOfFrame();
            targetBone = bone;

            Vector3 pos = targetBone.GetSkeletonSpacePosition();
            pos *= 100f;
            pos.y += targetY;
            pos.x += targetX;
            transform.localPosition = pos;
            gameObject.SetActive(true);
        }
    }
}
