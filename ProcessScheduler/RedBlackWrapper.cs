using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedBlackCS;

namespace ProcessScheduler
{
    class RedBlackWrapper
    {
        RedBlack rb;
        List<IComparable> keys;
        public RedBlackWrapper()
        {
            rb = new RedBlack();
            keys = new List<IComparable>();
        }

        public void Add(IComparable key, object data)
        {
            if (keys.Contains(key))
            {
                ((List<object>)rb.GetData(key)).Add(data);
            }
            else
            {
                keys.Add(key);
                List<object> l = new List<object>();
                l.Add(data);
                rb.Add(key, l);
            }
        }

        public IComparable GetMinKey()
        {
            return rb.GetMinKey();
        }

        public object GetData(IComparable key)
        {
            return rb.GetData(key);
        }

        public bool IsEmpty()
        {
            return (rb.Size() == 0);
        }

        public void RemoveMin()
        {
            if (((List<object>)rb.GetMinValue()).Count == 1)
            {
                keys.Remove(rb.GetMinKey());
                rb.RemoveMin();
            }
            else
            {
                ((List<object>)rb.GetMinValue()).RemoveAt(0);
            }
        }

        public object GetMinValue()
        {
            return rb.GetMinValue();
        }
    }
}
