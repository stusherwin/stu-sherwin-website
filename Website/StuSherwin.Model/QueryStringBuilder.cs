﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StuSherwin.Model
{
    public class QueryStringBuilder
    {
        private StringBuilder _queryString = new StringBuilder();

        public void Add(string key, string value)
        {
            if (_queryString.Length > 0)
            {
                _queryString.Append("&");
            }
            _queryString.Append(key);
            _queryString.Append("=");
            _queryString.Append(Uri.EscapeDataString(value));
        }

        public override string ToString()
        {
            return _queryString.ToString();
        }
    }
}
