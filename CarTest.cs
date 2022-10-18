using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task;

namespace CarTest
{
    [TestClass]
    public class CarTest
    {
        [TestMethod]
        public void Turn_on_car()
        {
            Car car = new Car();

            car.TurnOnEngine();

            Assert.AreEqual(true, car.IsTurnedOn(), "The car won't start");
        }

        [TestMethod]
        public void Cannot_turn_off_the_switched_off_car() // Поменять название +
        {
            Car car = new Car();

            car.TurnOffEngine();

            Assert.AreEqual(false, car.IsTurnedOn(), "The car does not turn off");
        }

        [TestMethod]
        public void Turn_off_the_switched_on_car()
        {
            Car car = new Car();

            car.TurnOnEngine();
            car.TurnOffEngine();

            Assert.AreEqual(false, car.IsTurnedOn(), "The car does not turn off");
        }

        [TestMethod]
        public void Turn_on_twice()
        {
            Car car = new Car();

            car.TurnOnEngine();
            car.TurnOnEngine();

            Assert.AreEqual(true, car.IsTurnedOn(), "The car is turned off");
        }

        [TestMethod]
        public void Turn_off_twice()
        {
            Car car = new Car();

            car.TurnOnEngine();
            car.TurnOffEngine();
            car.TurnOffEngine();

            Assert.AreEqual(false, car.IsTurnedOn(), "The car does not turn off");
        }

        [TestMethod]
        public void Cannot_set_gear_when_car_is_turned_off()
        {
            Car car = new Car();
            Gear expected = Gear.Neutral;

            car.SetGear(Gear.First);
            Gear gear = car.GetGear();

            Assert.AreEqual(expected, gear, "Gear is setted when car is turned off");
        }

        [TestMethod]
        public void Set_gear_when_car_is_turned_on()
        {
            Car car = new Car();
            Gear expected = Gear.Rear;

            car.TurnOnEngine();
            car.SetGear(expected);
            Gear gear = car.GetGear();

            Assert.AreEqual(expected, gear, "Gear not setted correctly");
        }

        [TestMethod]
        public void Set_reverse_gear()
        {
            Car car = new Car();
            Gear expected = Gear.Rear;

            car.TurnOnEngine();
            car.SetGear(Gear.Rear);

            Assert.AreEqual(expected, car.GetGear(), "Gear is not rear");
        }

        [TestMethod]
        public void Cannot_set_speed_when_car_is_turned_off() // Почти в каждом кейсе надо три проверки: вкл. выкл. машины, передача и скорость +
        {
            Car car = new Car();
            float expected = 0;

            car.SetSpeed(10);
            float speed = car.GetSpeed();

            Assert.AreEqual(expected, speed, "Speed is not equal to 0");
        }

        [TestMethod]
        public void Cannot_set_speed_in_neutral_gear()
        {
            Car car = new Car();
            float expected = 0;

            car.TurnOnEngine();
            car.SetSpeed(20);
            float speed = car.GetSpeed();

            Assert.AreEqual(expected, speed, "Speed is not equal to 0");
        }

        [TestMethod]
        public void Set_speed_in_first_gear() // Расположен не в том месте +
        {
            Car car = new Car();
            Gear gear = Gear.First;
            float expected = 20;

            car.TurnOnEngine();
            car.SetGear(gear);
            car.SetSpeed(20);

            float speed = car.GetSpeed();

            Assert.AreEqual(expected, speed, "Speed is incorrect");
        }

        [TestMethod]
        public void Switch_gear_through_gear()
        {
            Car car = new Car();
            Gear expected = Gear.Third;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(30);
            car.SetGear(Gear.Third);

            Assert.AreEqual(expected, car.GetGear(), "Gear is not switched");
        }

        [TestMethod]
        public void Turn_off_car_when_speed_is_not_equal_zero() // Тест должен быть расположен в другом месте, на данный момент передача и скорость не проверены +
        {
            Car car = new Car();
            bool expected = false;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(5);
            car.SetGear(Gear.Neutral);

            car.TurnOffEngine();

            Assert.AreEqual(expected, car.IsTurnedOn(), "Car is not turn on");
        }

        [TestMethod]
        public void Set_gear_when_speed_is_low()
        {
            Car car = new Car();
            Gear gear = Gear.First;
            Gear expected = Gear.First;

            car.TurnOnEngine();
            car.SetGear(gear);
            car.SetSpeed(10);
            
            gear = Gear.Second;
            car.SetGear(gear);

            gear = car.GetGear();

            Assert.AreEqual(expected, gear, "Gear is incorrect");
        }

        [TestMethod]
        public void Set_gear_when_speed_is_high()
        {
            Car car = new Car();
            Gear expected = Gear.Second;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(20);
            car.SetGear(Gear.Second);
            car.SetSpeed(40);
            car.SetGear(Gear.First);

            Gear gear = car.GetGear();

            Assert.AreEqual(expected, gear, "Gear is incorrect");
        }

        [TestMethod]
        public void Cannot_increase_speed_in_neutral()
        {
            Car car = new Car();
            float expected = 20;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(20);
            car.SetGear(Gear.Neutral);
            car.SetSpeed(25);

            Assert.AreEqual(expected, car.GetSpeed(), "Speed increased");
        }

        [TestMethod]
        public void Cannot_set_reverse_gear_when_driving_forward()
        {
            Car car = new Car();
            Gear expected = Gear.First;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(10);
            car.SetGear(Gear.Rear);

            Gear gear = car.GetGear();

            Assert.AreEqual(expected, gear, "Reverse gear is installed");
        }

        [TestMethod]
        public void Cannot_set_forward_gear_when_driving_reverse()
        {
            Car car = new Car();
            Gear expected = Gear.Rear;

            car.TurnOnEngine();
            car.TurnOnEngine();
            car.SetGear(Gear.Rear);
            car.SetSpeed(10);
            car.SetGear(Gear.First);

            Gear gear = car.GetGear();

            Assert.AreEqual(expected, gear, "Forward gear is installed");
        }

        [TestMethod]
        public void Cannot_set_speed_more_than_gear_allows()
        {
            Car car = new Car();
            float expected = 0;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(100);

            float speed = car.GetSpeed();

            Assert.AreEqual(expected, speed, "Speed is set");
        }

        [TestMethod]
        public void Cannot_set_speed_less_than_gear_allows()
        {
            Car car = new Car();
            float expected = 30;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(20);
            car.SetGear(Gear.Second);
            car.SetSpeed(30);
            car.SetSpeed(5);

            float speed = car.GetSpeed();

            Assert.AreEqual(expected, speed, "Speed is set");
        }

        [TestMethod]
        public void When_driving_forward_direction_is_forward() // Это уже интеграционный тест +
        {
            Car car = new Car();
            Direction expected = Direction.Forward;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(20);

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not forward");
        }

        [TestMethod]
        public void Gear_shifting_does_not_affect_the_direction()
        {
            Car car = new Car();
            Direction expected = Direction.Forward;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(20);
            car.SetGear(Gear.Second);

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not forward");
        }

        [TestMethod]
        public void When_car_is_off() // Переделать, это интеграционный тест +
        {
            Car car = new Car();
            Direction expected = Direction.Stop;

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not stop");
        }

        [TestMethod]
        public void When_car_is_turned_on()
        {
            Car car = new Car();
            Direction expected = Direction.Stop;

            car.TurnOnEngine();

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not stop");
        }

        [TestMethod]
        public void When_switched_forward_gear_car_is_stop()
        {
            Car car = new Car();
            Direction expected = Direction.Stop;

            car.TurnOnEngine();
            car.SetGear(Gear.First);

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not stop");
        }

        [TestMethod]
        public void When_car_speed_is_zero()
        {
            Car car = new Car();
            Direction expected = Direction.Stop;

            car.TurnOnEngine();
            car.SetGear(Gear.First);
            car.SetSpeed(10);
            car.SetSpeed(0);

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not stop");
        }

        [TestMethod]
        public void When_switched_reverse_gear()
        {
            Car car = new Car();
            Direction expected = Direction.Stop;

            car.TurnOnEngine();
            car.SetGear(Gear.Rear);

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not stop");
        }

        [TestMethod]
        public void When_drove_back_and_stopped()
        {
            Car car = new Car();
            Direction expected = Direction.Stop;

            car.TurnOnEngine();
            car.SetGear(Gear.Rear);
            car.SetSpeed(15);
            car.SetSpeed(0);

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not stop");
        }

        [TestMethod]
        public void When_go_back_direction_is_back()
        {
            Car car = new Car();
            Direction expected = Direction.Back;

            car.TurnOnEngine();
            car.SetGear(Gear.Rear);
            car.SetSpeed(1);

            Assert.AreEqual(expected, car.GetDirection(), "Direction is not back");
        }
    }
}