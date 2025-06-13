using System;

class Program
{
    static void Main(string[] args)
    {
        Car car = new Car(2006, "Hyundai", "Sonata", 4);
        Console.WriteLine(car.GetVehicleSummary());
        Console.WriteLine(car.GetDoorNumber());
    }

    public class Vehicle
    {
        private int _yearManufactured;
        private string _manufacturer;
        private string _modelName;

        public Vehicle(int yearManufactured, string manufacturer, string modelName)
        {
            _yearManufactured = yearManufactured;
            _manufacturer = manufacturer;
            _modelName = modelName;
        }

        public int GetYearManufactured()
        {
            return _yearManufactured;
        }

        public string GetVehicleSummary()
        {
            return $"{_manufacturer}: {_modelName} - {_yearManufactured}";
        }
    }

    public class Car : Vehicle
    {
        private int _numberOfDoors;
        public Car(int yearManufactured, string manufacturer, string modelName, int numberOfDoors) : base(yearManufactured, manufacturer, modelName)
        {
            _numberOfDoors = numberOfDoors;
        }

        public string GetDoorNumber()
        {
            return $"Number of doors: {_numberOfDoors}";
        }
    }
}
