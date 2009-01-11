using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace A1
{
    class Erzeuger : Worker
    {
        internal event WorkerLogHandler eCreate;
        internal event WorkerLogHandler eStart;
        internal event WorkerLogHandler eStop;
        internal event WorkerLogHandler eSleep;

        protected dPush _push;
        protected StorageCheck _storageCheck;
        protected int _speed = 10;

        public Erzeuger(string name, dPush push, StorageCheck storageCheck)
        {
            this._name = name;
            this._push = push;
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
                        LagerObject obj = new LagerObject();
                        this._push(obj);
                        this.OnCreate("Created new object (" + obj.Id + ")");
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
        
        private void OnCreate(string message)
        {
            if (eCreate != null)
                eCreate(this, this.getEventArgs(message));
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
