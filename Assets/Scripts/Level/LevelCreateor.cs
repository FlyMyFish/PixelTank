using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public class LevelCreator
    {
        private readonly List<GameObject> _prefabs;
        private readonly List<int> _prefabIndex;
        private readonly List<Vector3> _prefabPositions;

        public LevelCreator(List<GameObject> prefabs, List<int> prefabIndex, List<Vector3> prefabPositions)
        {
            _prefabs = prefabs;
            _prefabIndex = prefabIndex;
            _prefabPositions = prefabPositions;
        }

        public void CreateMap(MonoBehaviour scene)
        {
            IEnumerator CreateMapTask()
            {
                for (var i = 0; i < _prefabIndex.Count; i++)
                {
                    var wall = _prefabs[_prefabIndex[i]];
                    var position = _prefabPositions[i];
                    var build = Object.Instantiate(wall, position, Quaternion.identity);
                    build.tag = "GameBuild";
                    build.transform.parent = scene.transform;
                }

                yield return null;
            }

            scene.StartCoroutine(CreateMapTask());
        }
    }
}