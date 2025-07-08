using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(fileName = "New MultiObject", menuName = "HexTecGames/Grid/MultiObjectData")]
    public class MultiObjectData : MultiObjectData<MultiObject, MultiObjectData, MultiObjectVisual>
    {
    }
}