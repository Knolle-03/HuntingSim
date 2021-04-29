using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Common;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using Mars.Numerics.Statistics;


namespace KISA.Model
{
    
    public class Deer : AbstractAnimal, IAgent<ForestLayer>, IPositionable
    {
        [PropertyDescription] public UnregisterAgent UnregisterHandle { get; set; } 
        [PropertyDescription] private bool ReproducibleSpawning { get; set; }
        [PropertyDescription] private int Seed { get; set; }
        
        private ForestLayer _forestLayer;

        public void Init(ForestLayer layer)
        {
            
            _forestLayer = layer;
            Position = _forestLayer.FindRandomPositionDeer(ReproducibleSpawning);
            _forestLayer.DeerEnvironment.Insert(this);
        }

        private string Rule { get; set; }
        



        public void Tick()
        {
            var wolfs = GetWolfsInSight();
            if (wolfs.Any())
            {
                Rule = "Fleeing";
                FleeMove(wolfs);  
            }
            else
            {
                Rule = "Idle";
                RandomMove();   
            }
        }

        private void RandomMove()
        {
            AdjustEnergy(1);
            var bearing = RandomHelper.Random.Next(360);
            Position = _forestLayer.DeerEnvironment.MoveTowards(this, bearing, StepWidth);
        }

        private void FleeMove(IEnumerable<Wolf> wolfs)
        {
            var bearing = GetDirection(wolfs);
            if (Energy > 0)
            {
                AdjustEnergy(-1);
                Position = _forestLayer.DeerEnvironment.MoveTowards(this, bearing , SprintWidth);
            }
            else
            {
                AdjustEnergy(1);
                Position = _forestLayer.DeerEnvironment.MoveTowards(this, bearing , StepWidth);
            }
        }

        private IEnumerable<Wolf> GetWolfsInSight()
        {
            return _forestLayer.WolfEnvironment.Explore(Position, ViewRadius);
        }

        private double GetDirection(IEnumerable<Wolf> wolfs)
        {
            
            var avgBearing = wolfs.Sum(wolf => Position.GetBearing(wolf.Position));
            // TODO:: Possible multiple enumeration???
            // average
            avgBearing /= wolfs.Count();
            // invert
            avgBearing += 180;
            // < 360°
            return avgBearing %= 360;
        }


        public Guid ID { get; set; }
        public Position Position { get; set; }


        
        public void Die()
        {
            _forestLayer.DeerEnvironment.Remove(this);
            UnregisterHandle.Invoke(_forestLayer, this);
        }


        
    }
    
}