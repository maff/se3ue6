using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A1
{
    class LagerObject
    {
        private static int idCounter = 0;

        private int _id;
        public int Id
        {
            get { return this._id; }
            set { }
        }

        public LagerObject()
        {
            this._id = LagerObject.idCounter++;
        }
    }
}
