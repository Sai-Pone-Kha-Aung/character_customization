using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Blendshapes
{
    [RequireComponent(typeof(Slider))]
    public class BlendShapeSlider : MonoBehaviour
    {
        public string blendShapeName;
        private Slider _slider;

        public void Start()
        {   
            blendShapeName = blendShapeName.Trim();
            _slider = GetComponent<Slider>();

            _slider.onValueChanged.AddListener(value => CharacterC.Instance.ChangeBlendShapeValue(blendShapeName, value));
        }
    }


}

