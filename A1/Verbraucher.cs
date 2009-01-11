using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace A1
{
    class Verbraucher : Worker
    {
        internal event WorkerLogHandler eUse;
        internal event WorkerLogHandler eStart;
        internal event WorkerLogHandler eStop;
        internal event WorkerLogHandler eSleep;

        protected dPop _pop;
        protected StorageCheck _storageCheck;
        protected int _speed = 6;

        public Verbraucher(string name, dPop pop, StorageCheck storageCheck)
        {
            this._name = name;
            this._pop = pop;
            this._storageCheck = storageCheck;
        }

        public void Run()
        {
            this.OnStart("Starting...");
            while (true)
            {
                for (int i = 0; i < this._speed; i++)
                {
                    if (this._storageCheck())
                    {
                        LagerObject obj = this._pop();
                        this.OnUse("Used object " + obj.Id);
                    }
                }

                this.OnSleep("Sleeping for 1000 milliseconds");
                Thread.Sleep(1000);
            }
        }

        protected WorkerEventArgs getEventArgs(string message)
        {
            WorkerEventArgs args = new WorkerEventArgs();
            args.Name = this.Name;
            args.Message = message;
            return args;
        }

        private void OnUse(string message)
        {
            if (eUse != null)
                eUse(this, this.getEventArgs(message));
        }

        private void OnStart(string message)
        {
            if (eStart != null)
                eStart(this, this.getEventArgs(message));
        }

        private void OnStop(string message)
        {
            if (eStop != null)
                eStop(this, this.getEventArgs(message));
        }

        private void OnSleep(string message)
        {
            if (eSleep != null)
                eSleep(this, this.getEventArgs(message));
        }
    }
}
