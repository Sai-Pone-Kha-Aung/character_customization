using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CC.Blendshapes
{
    public class CharacterC : Singleton<CharacterC>
    {
        public SkinnedMeshRenderer target;
        public string suffixMax = "Max", suffixMin = "Min";

        private CharacterC() { }

        private SkinnedMeshRenderer skmr;
        private Mesh mesh;

        private Dictionary<string, BlendShape> blendShapesDatabase = new Dictionary<string, BlendShape>();

        private void Start()
        {
            Initialize();
        }

        #region Public functions

        public void ChangeBlendShapeValue(string blendShapeName, float value)
        {
            if (!blendShapesDatabase.ContainsKey(blendShapeName))
            { Debug.LogError("BlendShape" + blendShapeName + "does not exist"); return; }

            value = Mathf.Clamp(value, -100, 100);

            BlendShape blendshape = blendShapesDatabase[blendShapeName];

            if (value >= 0)
            {
                if (blendshape.positiveIndex == -1) return;
                skmr.SetBlendShapeWeight(blendshape.positiveIndex, value);
                if (blendshape.negativeIndex == -1) return;
                skmr.SetBlendShapeWeight(blendshape.negativeIndex, value);
            }
            else
            {
                if (blendshape.negativeIndex == -1) return;
                skmr.SetBlendShapeWeight(blendshape.negativeIndex, -value);
                if (blendshape.positiveIndex == -1) return;
                skmr.SetBlendShapeWeight(blendshape.positiveIndex, 0);
            }

        }

        #endregion

        #region Private Functions

        public void Initialize()
        {
            //if(target == null) print("WEE");

            skmr = target;
            mesh = skmr.sharedMesh;

            ParseBlendShapesToDictionary();

            //print("WEE" + blendshapeDatabase.Count + "entries");
        }

        private SkinnedMeshRenderer GetSkinnedMeshRenderer()
        {
            SkinnedMeshRenderer _skmr = target.GetComponentInChildren<SkinnedMeshRenderer>();

            return _skmr;
        }

        private void ParseBlendShapesToDictionary()
        {
            //Get all blendeshape names

            List<string> blendshapeNames = Enumerable.Range(0, mesh.blendShapeCount).Select(x =>
            mesh.GetBlendShapeName(x)).ToList();

            for (int i = 0; blendshapeNames.Count > 0;)
            {
                string altSuffix, noSuffix;
                //Removes all the max and min suffixes
                noSuffix = blendshapeNames[i].TrimEnd(suffixMax.ToCharArray()).TrimEnd(suffixMin.ToCharArray()).Trim();

                string positiveName = string.Empty, negativeName = string.Empty;
                bool exists = false;

                int positiveIndex = -1, negativeIndex = -1;

                //if Suffix is Positive 
                if (blendshapeNames[i].EndsWith(suffixMax))
                {
                    altSuffix = noSuffix + " " + suffixMin;

                    positiveName = blendshapeNames[i];
                    negativeName = altSuffix;

                    if (blendshapeNames.Contains(altSuffix)) exists = true;

                    positiveIndex = mesh.GetBlendShapeIndex(positiveName);

                    if (exists)
                        negativeIndex = mesh.GetBlendShapeIndex(altSuffix);
                }

                //if Suffix is Negative
                else if (blendshapeNames[i].EndsWith(suffixMin))
                {
                    altSuffix = noSuffix + " " + suffixMax;

                    negativeName = blendshapeNames[i];
                    positiveName = altSuffix;

                    if (blendshapeNames.Contains(altSuffix)) exists = true;

                    negativeIndex = mesh.GetBlendShapeIndex(negativeName);

                    if (exists)
                        positiveIndex = mesh.GetBlendShapeIndex(altSuffix);

                }

                //Doesn't have suffix
                else
                {
                    positiveIndex = mesh.GetBlendShapeIndex(blendshapeNames[i]);
                    positiveName = noSuffix;
                }

                if (blendShapesDatabase.ContainsKey(noSuffix))
                    Debug.LogError(noSuffix + " already exists");
                blendShapesDatabase.Add(noSuffix, new BlendShape(positiveIndex, negativeIndex));

                if (positiveName != string.Empty) blendshapeNames.Remove(positiveName);
                if (negativeName != string.Empty) blendshapeNames.Remove(negativeName);

            }//end loop
        }//end class

        public string[] GetBlendShapeNames()
        {
            return blendShapesDatabase.Keys.ToArray();
        }

        public int GetNumberOfEntries()
        {
            return blendShapesDatabase.Count;
        }

        public BlendShape GetBlendShape(string name)
        {
            return blendShapesDatabase[name];
        }

        public bool DoesTargetMatchSkmr()
        {
            return (target == skmr) ? true : false;
        }

        public void ClearDatabase()
        {
            blendShapesDatabase.Clear();
        }
    }
    #endregion



}
