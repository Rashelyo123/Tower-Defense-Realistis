// AliyerEdon@mail.com Christmas 2022
// Use this component to add the animation clip's name to run from other components(AI,Weapon,Health)
// Legacy animation system has been used, use www.Mixamo.com to animate your characters)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationList : MonoBehaviour
{
    [Space(5)]
    [Header("Actor Settings")]

    public Animation actor;
    public string runClip;
    public string seekClip;
    public string fireClip;
    public string deadClip;
}
