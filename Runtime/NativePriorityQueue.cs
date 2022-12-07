using Unity.Burst;
using Unity.Collections;

namespace agray.NativeExtensions
{
    [BurstCompile]
    public struct NativePriorityQueue<T> where T : unmanaged
    {
        struct PriorityItem
        {
            public float priority;
            public T item;
        }
        public T this[int index]
        {
            get {
                if(index < 0 || index >= items.Length || items.Length == 0)
                    return default(T);
                
                return items[index].item;
            }
        }
        NativeList<PriorityItem> items;

        public int Count() => items.Length;
        public bool IsEmpty() => items.IsEmpty;

        public NativePriorityQueue(Allocator allocator)
        {
            items = new NativeList<PriorityItem>(allocator);
        }

        public void Add(T toAdd, float priority)
        {
            if (items.Length == 0)
            {
                items.Add(new PriorityItem { item = toAdd, priority = priority });
                return;
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (priority < items[i].priority)
                {
                    items.InsertRangeWithBeginEnd(i, i + 1);
                    items[i] = new PriorityItem { item = toAdd, priority = priority };
                    break;
                }
            }

            items.Add(new PriorityItem { item = toAdd, priority = priority });
        }
        public T Dequeue()
        {
            if (items.Length > 0)
            {
                var x = items[0].item;
                items.RemoveAt(0);
                return x;
            }

            return default(T);
        }

        public void Dispose()
        {
            items.Dispose();
        }
    }
}
