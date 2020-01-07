using UnityEngine;
using UnityEngine.Events;

namespace Dice{
    [System.Serializable]
    public class OnRollDelegate : UnityEvent<Rollable, int> {}

    public abstract class Rollable{        
        ///Event when the dice is rolled, first Int is its value and seccond is it Max
        [SerializeField]
        public OnRollDelegate OnRoll;

        public static implicit operator int(Rollable o) => o.Roll();

        public abstract int Roll();
    }
}