using Task;

Car car = new Car();

car.SetGear(Gear.First);
car.SetSpeed(20);
car.SetGear(Gear.Second);
car.SetSpeed(30);
car.SetSpeed(5);

float speed = car.GetSpeed();