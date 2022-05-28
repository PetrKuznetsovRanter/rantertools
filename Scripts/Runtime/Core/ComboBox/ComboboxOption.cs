using System;
using System.Collections.Generic;
using System.Linq;

namespace RanterTools.ComboBox
{
    [Serializable]
    public class ComboboxOption<T>
    {
        private static List<ComboboxOption<T>> all = new List<ComboboxOption<T>>();

        public T Meta;
        public string Name;

        public ComboboxOption()
        {
            Name = "";
            Meta = default;
            all.RemoveAll(o => o == null);

            all.Add(this);
            ID = all.Count;
#if UNITY_EDITOR

#endif
        }

        public ComboboxOption(string Name, T Meta = default)
        {
            this.Name = Name;
            this.Meta = Meta;

            all.RemoveAll(o => o == null);
            all.Add(this);
            ID = all.Max(o => o.ID) + 1;
        }

        public int ID { get; }

        public static implicit operator string(ComboboxOption<T> option)
        {
            return option.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(ComboboxOption<T>))
            {
                if ((obj as ComboboxOption<T>).ID == ID)
                    return true;
                return false;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            return Meta.ToString();
        }

        public void Remove()
        {
            all.Remove(this);
        }
    }

    [Serializable]
    public class ComboBoxOptionString : ComboboxOption<string>
    {
        public ComboBoxOptionString()
        {
        }

        public ComboBoxOptionString(string Name, string Data) : base(Name, Data)
        {
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}