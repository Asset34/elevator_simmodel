using System;

namespace ElevatorSimulation
{
    /// <summary>
    /// "Singleton" generator of random integer numbers
    /// </summary>
    class RandomGenerator
    {
        public static RandomGenerator Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new RandomGenerator();
                }

                return m_instance;
            }
        }
        /// <summary>
        /// Get next random number between [min; max]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int Next(int min, int max)
        {
            return m_random.Next(min, max);
        }

        private RandomGenerator()
        {
        }

        private static RandomGenerator m_instance;
        private Random m_random = new Random(0);   
    }
}
