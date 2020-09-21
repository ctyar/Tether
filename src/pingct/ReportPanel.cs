﻿using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Ctyar.Pingct
{
    internal class ReportPanel
    {
        private readonly int _capacity;
        private readonly string _name;
        private readonly Queue<string> _values;
        private string _lastValue;

        public ReportPanel(int capacity, string name)
        {
            _capacity = capacity;
            _name = name;
            _values = new Queue<string>(_capacity);
            _lastValue = string.Empty;
        }

        public void Add()
        {
            if (_values.Count < _capacity)
            {
                _values.Enqueue(_lastValue);
            }
            else
            {
                _values.Dequeue();
                _values.Enqueue(_lastValue);
            }

            _lastValue = string.Empty;
        }

        public void Append(string value)
        {
            _lastValue += value;
        }

        public IRenderable Render()
        {
            return new Panel(Print())
                .SetHeader(_name)
                .RoundedBorder().Expand();
        }

        private string Print()
        {
            var result = new StringBuilder();

            foreach (var item in _values)
            {
                result.AppendLine(item);
            }

            if (_values.Count < _capacity)
            {
                for (var i = 0; i < _capacity - _values.Count; i++)
                {
                    result.AppendLine("                              ");
                }
            }

            return result.ToString();
        }
    }
}