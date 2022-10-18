namespace Task
{
 
    public enum Gear
    {
        Rear = -1,
        Neutral,
        First,
        Second,
        Third,
        Fourth,
        Fifth
    };

    public enum Direction
    {
        Back,
        Stop,
        Forward
    }

    public struct Border
    {
        public float min;
        public float max;
    }

    public class Car
    {
        public Car()
        {
            m_engineOn = false;
            m_gear = Gear.Neutral;
            m_currentSpeed = 0;
            m_currentDirection = Direction.Stop;

            m_speedBorders = new List<Border>();

            Border border = new Border();
            border.min = -20;
            border.max = 0;
            m_speedBorders.Add(border);

            border.min = -20;
            border.max = 150;
            m_speedBorders.Add(border);

            border.min = 0;
            border.max = 30;
            m_speedBorders.Add(border);

            border.min = 20;
            border.max = 50;
            m_speedBorders.Add(border);
            
            border.min = 30;
            border.max = 60;
            m_speedBorders.Add(border);

            border.min = 40;
            border.max = 90;
            m_speedBorders.Add(border);

            border.min = 50;
            border.max = 150;
            m_speedBorders.Add(border);
        }

        public void TurnOnEngine()
        {
            m_engineOn = true;
        }

        public void TurnOffEngine()
        {
            m_engineOn = false;
        }

        public void SetGear(Gear gear)
        {
            if (!IsTurnedOn())
            {
                return;
            }

            if (m_currentSpeed <= m_speedBorders[(int)gear + 1].max && m_currentSpeed >= m_speedBorders[(int)gear + 1].min)
            {
                m_gear = gear;
            }
        }

        public void SetSpeed(float speed)
        {
            if (!IsTurnedOn())
            {
                return;
            }

            if (m_currentSpeed < 0 || m_gear == Gear.Rear)
            {
                speed = -speed;
            }

            int currentGearNumber = (int)m_gear;

            if (speed >= m_speedBorders[currentGearNumber + 1].min 
		        && speed <= m_speedBorders[currentGearNumber + 1].max)
	        {
		        if (currentGearNumber != 0)
		        {
		        	m_currentSpeed = speed;
		        	m_currentDirection = (speed != 0) ? Direction.Forward : Direction.Stop;
        
		        	if (currentGearNumber == -1)
		        	{
		        		m_currentDirection = (speed != 0) ? Direction.Back : Direction.Stop;
		        	}
	        	}
		        else if (m_currentSpeed > 0 && m_currentSpeed >= speed)
	        	{
	        		m_currentSpeed = speed;
                    m_currentDirection = (speed == 0) ? Direction.Stop : m_currentDirection;
	        	}
                else if (m_currentSpeed < 0 && m_currentSpeed <= speed)
	        	{
	        		m_currentSpeed = speed;
	        		m_currentDirection = (speed == 0) ? Direction.Stop : m_currentDirection;
	        	}
	        }
        }

        public bool IsTurnedOn()
        {
            return m_engineOn;
        }

        public Direction GetDirection()
        {
            return m_currentDirection;
        }

        public float GetSpeed()
        {
            return m_currentSpeed;
        }

        public Gear GetGear()
        {
            return m_gear;
        }
        
        private bool m_engineOn = false;
        private Gear m_gear = Gear.Neutral;
        private float m_currentSpeed = 0;
        private Direction m_currentDirection = Direction.Stop;
        private List<Border> m_speedBorders = new List<Border>();
    }
}
