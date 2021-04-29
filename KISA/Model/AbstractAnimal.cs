

namespace KISA.Model
{
    
    public abstract class AbstractAnimal
    {
        protected int Energy = 5;
        protected const int ViewRadius = 5;
        protected const int StepWidth = 1;
        protected const int SprintWidth = 2;

        protected void AdjustEnergy(int amount)
        {
            Energy += amount;
            if (Energy < 0) Energy = 0;
            else if (Energy > 5) Energy = 5;

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