using System.Collections.Generic;
using System.Linq;
using Core.Sortables;
using UnityEngine;
using Utils;

namespace Core
{
    public class PrefabStorage : Singleton<PrefabStorage>
    {
        [SerializeField] private List<ColorMaterial> characterMaterials;
        [SerializeField] private List<ColorMaterial> boxMaterials;

        public Material CharacterMatByColor(SortableColor col) => characterMaterials.First(cm => cm.color == col).material;
        public Material BoxMatByColor(SortableColor col) => characterMaterials.First(cm => cm.color == col).material;
    }
}