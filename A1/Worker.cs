using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A1
{
    abstract class Worker
    {
        protected int _counter = 0;

        protected bool _active;
        protected string _name;

        public string Name
        {
            set
            {
                this._name = value;
            }
            get
            {
                return this._name;
            }
        }
    }
}
