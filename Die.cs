using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

namespace Dice{



    /// <summary>
    /// This is a die they are rollable object and, they fire specific events for each different side when they roll.
	/// </summary>
    [System.Serializable]
    public class Die : Rollable, IEnumerable<Die.Result> {

        /// <summary>
        /// A result object for Ienumerable of die
	    /// </summary>
        public struct Result{
            readonly int value;
            UnityEvent onResultEvent;
            public Result(int value, UnityEvent onResultEvent){
                this.value = value;
                this.onResultEvent = onResultEvent;
            }
        }

        /// <summary>
        /// The enumerator for die
	    /// </summary>
        public class Enumerator : IEnumerator<Die.Result>{
            private UnityEvent[] onResult;
            int itr = -1;
            public Die.Result Current { get => new Die.Result(itr+1, onResult[itr]); } 
            object IEnumerator.Current { get => Current; }
            public void Dispose() {}
            public bool MoveNext() => itr++ < onResult.Length;
            public void Reset() => itr = -1;
            public Enumerator(UnityEvent[] onResult) => this.onResult = onResult;
        }



        /// <summary>
        /// How many sides this die has
        /// </summary>
        private int sides;
        /// <summary>
        /// Property to how many sides this die has
        /// </summary>
        public int Sides { 
            get => sides; 
            private set {
                if(value >= 0){
                    if(OnResult == null)
                        OnResult = new UnityEvent[value];
                    else if(sides != value)
                        System.Array.Resize(ref OnResult, value);
                    sides = value;
                }
            }
        }
        /// <summary>
        /// returns the event related to when the dice rolls [index]
        /// </summary>
        public UnityEvent this[int index]{
            get => OnResult[index - 1];
            set => OnResult[index - 1] = value;
        }
        /// <summary>
        /// List of individual events for each side when they come up
        /// </summary>
        [SerializeField]
        private UnityEvent[] OnResult;
        /// <summary>
        /// Build a new Die.
        /// </summary>
        public Die(int sides = 6){
            Sides = sides;
            OnResult = new UnityEvent[sides];
        }
        /// <summary>
        /// Roll the dice, call its OnRoll Event, and call the OnResult event associated to the side that went up.
        /// </summary>
        public override int Roll(){
            int result = Random.Range(0, Sides) + 1;
            OnRoll.Invoke(this, result);
            OnResult[result].Invoke();
            return result;
        }
        /// <summary>
        /// Copy this die and its events into a new die
        /// </summary>
        public Die Copy(){
            Die die = new Die(sides);
            OnResult.CopyTo(die.OnResult,0);
            die.OnRoll = OnRoll;
            return die;
        }

        IEnumerator<Die.Result> IEnumerable<Die.Result>.GetEnumerator() => new Die.Enumerator(OnResult);
        
        IEnumerator IEnumerable.GetEnumerator() => new Die.Enumerator(OnResult);
    }
}