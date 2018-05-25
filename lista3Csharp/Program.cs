using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lista3Csharp
{
    class Creator
    {
        public List<Observer> obsList;
        static Random rnd = new Random();
        private int _counter;
        //public double distance;//the point at which we count the distance 

        //new event and delagate 
        public delegate void NewObserverCreatedDelegate(Observer observer);
        public event NewObserverCreatedDelegate NewObserverCreated;

        //introduce event and delagate 
        public delegate void IntroduceYourselfDelegate();
        public event IntroduceYourselfDelegate IntroduceYourself;

        public Creator()
        {
            _counter = 0;
            obsList = new List<Observer>();
        }

        public void StartCreating()
        {
            //create new observer, setting up the params
            var x = rnd.NextDouble() * (1.0);
            var y = rnd.NextDouble() * (1.0);
            var newObserver = new Observer("Obs" + this._counter, x, y);
            obsList.Add(newObserver);

            _counter++;//we increace the counter, so every new observer has unique name with this value as an id

            this.NewObserverCreated += newObserver.NewObserverCreatedEventHandler;
            this.IntroduceYourself += newObserver.IntroduceYourselfEventHandler;
            NewObserverCreated(newObserver);
            IntroduceYourself();
        }
    }

    //we use sort method while searching closest neighbour, so we need to inherit IComparable class
    class Observer: IComparable<Observer>
    {
        public List<Observer> neighbours;//all neighbours
        public List<Observer> closestNeighbours;
        public string _name;
        public double _x;
        public double _y;
        public double _distance;
     
        public Observer(string name, double x, double y)
        {
            this._name = name;
            this._x = x;
            this._y = y;
            this.neighbours = new List<Observer>();
            this.closestNeighbours = new List<Observer>();
            //IntroduceYourself();//test purposes
        }

        //distance counter method
        public void CountDistance(Observer potentialNeighbourObserver)
        {
            this._distance = Math.Sqrt(Math.Pow(this._x - potentialNeighbourObserver._x, 2) + Math.Pow(this._y - potentialNeighbourObserver._y, 2));
        }

        //sorting based on distance method
        public int CompareTo(Observer potentialNeighbourObserver)
        {
            if (this._distance < potentialNeighbourObserver._distance)
                return -1;
            else if (this._distance > potentialNeighbourObserver._distance)
                return 1;
            else
                return 0;
        }

        public void NewObserverCreatedEventHandler(Observer observer)
        {
            //uncomment line underneath this comment to see data about all neighbourhood
            //Console.WriteLine("Observer {0} has {1} neighbours." ,this._name, neighbours.Count);
            

            //in neighbours we keep data about all neighbourhood (#RODO)
            if (!neighbours.Contains(observer))//we don't want to have redundant, duplicate data
            {
                neighbours.Add(observer);
            }

            //lets add some logic to add just the closest neighbours
            if ((int) Char.GetNumericValue(observer._name[3]) > (int) Char.GetNumericValue(this._name[3]))
            {
                this.closestNeighbours.Add(observer);//adding just these neighbours which moved in later than us
            }

            this.closestNeighbours.Sort();//sorting 'new' neighbours so we can 

            if (this.closestNeighbours.Count > 2)
            {
                while (this.closestNeighbours.Count == 2)
                {
                    this.closestNeighbours.RemoveAt(this.closestNeighbours.Count - 1);//leaving just the closest neighbours
                }
            }
        }

        public void IntroduceYourselfEventHandler()
        {
            Console.WriteLine("I'm: " + _name + " and these are my neighbours: ");
            foreach (var neighbour in this.closestNeighbours)
            {
                var dist = Math.Sqrt(Math.Pow(neighbour._x - this._x, 2) + Math.Pow(neighbour._y - this._y, 2));
                Console.WriteLine("{0}\tx: {1}\ty: {2}\tdistance: {3}", neighbour._name, neighbour._x, neighbour._y, dist);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //create one, main creator
            Creator creator = new Creator();
            //create 10 observers in a loop
            for (int i = 1; i < 5; i++)
            {
                creator.StartCreating();
            }

            Console.ReadKey();
        }
    }
}
