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
    private Vector3 targetPos;
    private Spine.Bone targetBone;
    private int targetIndex;

    private void Awake()
    {
        gameObject.SetActive(true);
        GetBoneData();

        //Bone();
    }
    private void Bone()
    {
        for (int i = 0; i < charactor.Skeleton.Bones.Count; i++)
        {
            Spine.Bone bone = charactor.Skeleton.Bones.Items[i];

            //bone.UpdateWorldTransform();
            Vector3 pos = bone.GetSkeletonSpacePosition();
            Debug.LogFormat("{0} : {1}", bone.Data.Name, pos);
            if (bone.Data.Name == targetBoneName)
            {
                var temp = image.rectTransform.rect.height / 5f;
                //Debug.Log(temp);
                pos *= 100f;
                pos.y += targetValue;
                targetPos = pos;
                transform.localPosition = pos;
                gameObject.SetActive(true);
            }
        }
    }

    private void GetBoneData()
    {
        targetIndex = 0;
        for (int i = 0; i < charactor.Skeleton.Bones.Count; i++)
        {
            Spine.Bone bone = charactor.Skeleton.Bones.Items[i];

            if (bone.Data.Name == targetBoneName)
            {
                targetIndex = i;
                StartCoroutine(FollowTarget(bone));
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
            targetPos = pos;
            transform.localPosition = pos;
            gameObject.SetActive(true);
        }
    }
}
