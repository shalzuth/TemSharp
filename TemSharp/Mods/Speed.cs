using System;
using UnityEngine;
namespace TemSharp
{
    public class Speed : MonoBehaviour
    {
        Single MinValue = 1;
        Single MaxValue = 20;
        Single ScalingValue = 1;
        void OnEnable()
        {
            Menu.ScalingOptions.Add(GetType());
        }
        void Update()
        {
            Time.timeScale = ScalingValue;
        }
        void OnDisable()
        {
            Time.timeScale = MinValue;
            Menu.ScalingOptions.Remove(GetType());
        }
    }
}
