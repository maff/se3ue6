﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A1
{
    delegate void WorkerLogHandler(object o, WorkerEventArgs e);
    delegate void dPush(LagerObject o);
    delegate LagerObject dPop();
    delegate bool StorageCheck();
}
