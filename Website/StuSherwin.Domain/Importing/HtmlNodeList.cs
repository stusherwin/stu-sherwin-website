using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Collections;

namespace StuSherwin.Domain.Importing
{
    public class HtmlNodeList : IEnumerable<HtmlNode>
    {
        private HtmlNodeWalkerDirection _direction;
        private List<HtmlNode> _list = new List<HtmlNode>();

        public HtmlNodeList(HtmlNodeWalkerDirection direction)
        {
            _direction = direction;
        }

        public HtmlNodeList(HtmlNodeWalkerDirection direction, IEnumerable<HtmlNode> nodes)
            : this(direction)
        {
            _list = new List<HtmlNode>(nodes);
        }

        public void Add(HtmlNode node)
        {
            if (_direction == HtmlNodeWalkerDirection.Forwards)
            {
                _list.Add(node);
            }
            else
            {
                _list.Insert(0, node);
            }
        }
    
        public IEnumerator<HtmlNode> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public HtmlNode this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public override string ToString()
        {
            return _list.Aggregate("", (a, n) => a += n.OuterHtml);
        }
    }
}
