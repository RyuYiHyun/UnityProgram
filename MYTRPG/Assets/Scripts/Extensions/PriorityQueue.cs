using System.Collections.Generic;
namespace PriorityQueue
{
	public class PriorityQueue<Value>
	{
		public class Item
		{
			public Item(int _key, Value _value)
			{
				key = _key;
				value = _value;
			}
			public int key { get; set; }
			public Value value { get; set; }
		}
		private List<Item> _heap = new List<Item>();

		public void Enqueue(int _key, Value _value)
		{
			_heap.Add(new Item(_key, _value));
			int now = _heap.Count - 1;
			while (now > 0)
			{
				int next = (now - 1) / 2;
				if (_heap[now].key > _heap[next].key) // < 置企, > 置社
				{
					break;
				}
				Item temp = _heap[now];
				_heap[now] = _heap[next];
				_heap[next] = temp;
				now = next;
			}
		}
		public void Clear()
        {
			_heap.Clear();

		}
		public int Count => _heap.Count;
		public bool IsEmpty()
        {
			if (_heap.Count <=0)
            {
				return true;
			}
			return false;
        }

		public bool Contains(Value _value)
        {
			foreach(Item item in _heap)
            {
				if(item.value.Equals(_value))
                {
					return true;
                }
            }
			return false;
		}

		public Item Dequeue()
		{
			if(IsEmpty())
            {
				return null;
            }
			Item ret = _heap[0];
			int lastIdx = _heap.Count - 1;
			_heap[0] = _heap[lastIdx];
			_heap.RemoveAt(lastIdx);
			lastIdx--;
			int now = 0;
			while (true)
			{
				int left = 2 * now + 1;
				int right = 2 * now + 2;

				int next = now;

				if (left <= lastIdx && _heap[next].key > _heap[left].key)// < 置企, > 置社
				{
					next = left;
				}
				if (right <= lastIdx && _heap[next].key > _heap[right].key) // < 置企, > 置社
				{
					next = right;
				}

				if (now == next)
				{
					break;
				}

				Item temp = _heap[now];
				_heap[now] = _heap[next];
				_heap[next] = temp;
				now = next;
			}
			return ret;
		}


		public Value DequeueValue()
		{
			if (IsEmpty())
			{
				return default(Value);
			}
			Item ret = _heap[0];
			int lastIdx = _heap.Count - 1;
			_heap[0] = _heap[lastIdx];
			_heap.RemoveAt(lastIdx);
			lastIdx--;
			int now = 0;
			while (true)
			{
				int left = 2 * now + 1;
				int right = 2 * now + 2;

				int next = now;

				if (left <= lastIdx && _heap[next].key > _heap[left].key)// < 置企, > 置社
				{
					next = left;
				}
				if (right <= lastIdx && _heap[next].key > _heap[right].key) // < 置企, > 置社
				{
					next = right;
				}

				if (now == next)
				{
					break;
				}

				Item temp = _heap[now];
				_heap[now] = _heap[next];
				_heap[next] = temp;
				now = next;
			}
			return ret.value;
		}
	}
}


