

namespace KISA.Model
{
    
    public abstract class AbstractAnimal
    {
        protected int _energy = 5;
        protected const int _viewRadius = 5;
        protected const int _stepWidth = 1;
        protected const int _sprintWidth = 2;
        
        public void AdjustEnergy(int amount)
        {
            _energy += amount;
            if (_energy < 0) _energy = 0;
            else if (_energy > 5) _energy = 5;

        }
        



        /*/// TODO:: Find real world values; Use a distribution; Use constants
        protected void SetupInitialValues(int seed)
        {
            SetupValues(new Random(seed));
        }
        
        protected void SetupInitialValues()
        {
            SetupValues(new Random());
        }

        private void SetupValues(Random rng)
        {
            _energy = rng.Next(10, 100);
            _viewRadius = rng.Next(1, 10);

        }*/


    }
    
    public enum Direction {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    

    
}