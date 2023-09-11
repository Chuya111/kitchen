using System;
using System.Collections.Generic;

namespace DD.Service
{
    internal class History
    {

        private static readonly Lazy<History> lazy = new Lazy<History>(() => new History());
        public static History Instance => lazy.Value;

        public void ClearHistory()
        {
            this._steps.Clear();
            this.index = -1;
        }

        public void Undo()
        {
            if (index > -1)
            {
                _steps[index--].Undo();
            }
        }

        public void Redo()
        {
            if (index < _steps.Count - 1)
            {
                _steps[++index].Redo();
            }
        }

        public void Clear()
        {
            _steps.Clear();
            index = -1;
        }

        public void Add(Action undo, Action redo)
        {
            index++;
            if (index < _steps.Count)
                _steps.RemoveRange(index, _steps.Count - index);
            _steps.Add(new Step() { Undo = undo, Redo = redo });
        }

        //操作步骤
        class Step
        {
            public Action Redo { set; get; }
            public Action Undo { set; get; }
        }

        //记录的步骤
        List<Step> _steps = new List<Step>();


        //当前操作的下标
        int index = -1;
    }
}

