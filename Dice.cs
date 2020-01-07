using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dice{
    /// <summary>
    /// Its a group of rollable objects, you can call Roll to roll all objects and trigger their respective events, 
    /// also this container implements IEnumerator and ICollection thus it can be 'Foreached' and 'LinQed',
    /// this was meant to hold a collection of dice but it was kept like this in case a user wants to add their own rollable object to the pile.
    /// </summary>
    public class Group<R> : Rollable, ICollection<R> where R : Rollable{
        [SerializeField]
        private List<R> group = new List<R>();

        public R this[int index]{
            get => group[index];
            set => group[index] = value;
        }

        public static Group<R> operator +(Group<R> dice, R die){
            dice.Add(die);
            return dice;
        }

        public static Group<R> operator -(Group<R> dice, R die){
            dice.Remove(die);
            return dice;
        }

        public override int Roll(){
            int result = 0;
            foreach (R die in group) 
                result += die.Roll();
            OnRoll.Invoke(this, result);
            return result;
        }

        public Group(params R[] dice){
            foreach (R die in dice)
                Add(die);
        }

        public int Count => group.Count;
        public bool IsReadOnly => ((ICollection<R>)group).IsReadOnly;
        public void Add(R item) => group.Add(item);
        public void Clear() => group.Clear();
        public bool Contains(R item) => group.Contains(item);   
        public void CopyTo(R[] array, int arrayIndex) => group.CopyTo(array, arrayIndex); 
        public bool Remove(R item) => group.Remove(item);
        public IEnumerator<R> GetEnumerator() => group.GetEnumerator();        
        IEnumerator IEnumerable.GetEnumerator() => group.GetEnumerator();
    }

    /// <summary>
    /// Its a collection of die
	/// </summary>
    public class Dice : Group<Die>{}
}