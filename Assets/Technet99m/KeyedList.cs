
namespace Technet99m
{
    public class KeyedList<TKey, TValue>
    {
        ListMember<TKey, TValue> head;
        int length;
        public int Length { get { return length; } }
        public void Add(TKey key, TValue value)
        {
            var tmp = new ListMember<TKey, TValue>() { Key = key, value = value, next = null };
            if (length == 0)
                head = tmp;
            else
            {
                ListMember<TKey,TValue> pointer = head;
                for (int i = 0; i < length - 1; i++)
                    pointer = pointer.next;
                pointer.next = tmp;
            }
            length++;
        }
        public void RemoveLast()
        {
            ListMember<TKey, TValue> pointer = head;
            for (int i = 0; i < length - 2; i++)
                pointer = pointer.next;
            pointer.next = null;
            length--;
        }
        public TValue this[TKey s]
        {
            get
            {
                ListMember<TKey,TValue> pointer = head;
                for (int i = 0; i < length - 1 && !pointer.Key.Equals(s); i++)
                    pointer = pointer.next;
                if (pointer != null && pointer.Key.Equals(s))
                    return pointer.value;
                else
                    return default;
            }
            set
            {
                ListMember<TKey,TValue> pointer = head;
                for (int i = 0; i < length - 1 && !pointer.Key.Equals(s); i++)
                    pointer = pointer.next;
                if (pointer != null && pointer.Key.Equals(s))
                    pointer.value = value;
                else
                    Add(s, value);
            }
        }
        public KeyedListItem<TKey, TValue> this[int i]
        {
            get
            {
                ListMember<TKey, TValue> pointer = head;
                for (int j = 0; j < length && j!=i; j++)
                {
                    pointer = pointer.next;
                }
                return new KeyedListItem<TKey, TValue>() { key = pointer.Key, value = pointer.value };
            }
            set
            {
                ListMember<TKey, TValue> pointer = head;
                for (int j = 0; j < length && j != i; j++)
                {
                    pointer = pointer.next;
                }
                pointer.Key = value.key;
                pointer.value = value.value;
            }
        }
        public struct KeyedListItem<TKey,TValue>
        {
            public TKey key;
            public TValue value;
        }
        public KeyedList()
        {
            head = null;
            length = 0;
        }

        class ListMember<T1, T2>
        {
            public T1 Key;
            public T2 value;
            public ListMember<T1,T2> next;
        }
    }

}
