using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace A1
{
    delegate void dPush(LagerObject o);
    delegate LagerObject dPop();

    class Lager
    {
        protected Stack<LagerObject> _storage;
        protected int _capacity;
        protected double _storagePercentage = 0.0;

        protected Erzeuger[] _erzeuger;
        protected Verbraucher[] _verbraucher;

        protected Thread[] _erzeugerThread;
        protected Thread[] _verbraucherThread;

        protected Logger _logger = new Logger();

        public Lager(int capacity)
        {
            this.setUp(capacity, 1, 2);
        }

        public Lager(int capacity, int erzeuger, int verbraucher)
        {
            this.setUp(capacity, erzeuger, verbraucher);
        }

        public void setUp(int capacity, int erzeuger, int verbraucher)
        {
            this._capacity = capacity;

            this._storage = new Stack<LagerObject>(capacity);

            this._erzeuger = new Erzeuger[erzeuger];
            this._erzeugerThread = new Thread[erzeuger];
            for (int i = 0; i < this._erzeuger.Length; i++)
                this.createErzeuger(i);
            
            this._verbraucher = new Verbraucher[verbraucher];
            this._verbraucherThread = new Thread[verbraucher];
            for (int j = 0; j < this._verbraucher.Length; j++)
                this.createVerbraucher(j);
        }

        protected void createErzeuger(int index)
        {
            this._erzeuger[index] = new Erzeuger("Erzeuger " + index, this.Push);
            this._erzeuger[index].eCreate += new WorkerLogHandler(this._logger.Log);
            this._erzeuger[index].eStart += new WorkerLogHandler(this._logger.Log);
            this._erzeuger[index].eStop += new WorkerLogHandler(this._logger.Log);
            this._erzeuger[index].eSleep += new WorkerLogHandler(this._logger.Log);

            this._erzeugerThread[index] = new Thread(new ThreadStart(this._erzeuger[index].Run));
        }

        protected void createVerbraucher(int index)
        {
            this._verbraucher[index] = new Verbraucher("Verbraucher " + index, this.Pop);
            this._verbraucher[index].eUse += new WorkerLogHandler(this._logger.Log);
            this._verbraucher[index].eStart += new WorkerLogHandler(this._logger.Log);
            this._verbraucher[index].eStop += new WorkerLogHandler(this._logger.Log);
            this._verbraucher[index].eSleep += new WorkerLogHandler(this._logger.Log);

            this._verbraucherThread[index] = new Thread(new ThreadStart(this._verbraucher[index].Run));
        }

        public void Run()
        {
            this.startErzeuger();
        }

        protected void startErzeuger()
        {
            foreach (Thread e in this._erzeugerThread)
                e.Start();
        }

        protected void stopErzeuger()
        {
            foreach (Thread e in this._erzeugerThread)
                e.Abort();
        }

        protected void startVerbraucher()
        {
            foreach (Thread v in this._verbraucherThread)
                v.Start();
        }

        protected void stopVerbraucher()
        {
            foreach (Thread v in this._verbraucherThread)
                v.Abort();
        }

        protected void calcStoragePercentage()
        {
            this._storagePercentage = ((double) this._storage.Count) / ((double) this._capacity);
        }

        protected void Push(LagerObject o)
        {
            this._storage.Push(o);

            this.calcStoragePercentage();
            if (this._storagePercentage > 0.9)
            {
                this.startVerbraucher();
            }
            if (this._storagePercentage >= 1)
            {
                this.stopErzeuger();
            }
        }

        protected LagerObject Pop()
        {
            bool pop = true;

            this.calcStoragePercentage();
            if (this._storagePercentage < 0.1)
            {
                pop = false;
                this.startErzeuger();
            }
            else if (this._storagePercentage == 0)
            {
                pop = false;
                this.stopVerbraucher();
            }

            if(pop)
                return this._storage.Pop();

            return null;
        }

    }
}
