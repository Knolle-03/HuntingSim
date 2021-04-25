
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mars.Common;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using Mars.Numerics.Statistics;
using NetTopologySuite.Algorithm;


namespace KISA.Model
{
    
    public class Deer : AbstractAnimal, IAgent<ForestLayer>, IPositionable
    {
        [PropertyDescription]
        public UnregisterAgent UnregisterHandle { get; set; }
        
        [PropertyDescription] 
        private bool ReproducibleSpawning { get; set; }
        [PropertyDescription]
        private int Seed { get; set; }
        
        private ForestLayer _forestLayer;

        public void Init(ForestLayer layer)
        {
            
            _forestLayer = layer;
            Position = _forestLayer.FindRandomPositionDeer(ReproducibleSpawning);
            _forestLayer.DeerEnvironment.Insert(this);
        }
        
        public string Rule { get; private set; }
        



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

        public void RandomMove()
        {
            AdjustEnergy(1);
            var bearing = RandomHelper.Random.Next(360);
            Position = _forestLayer.DeerEnvironment.MoveTowards(this, bearing, _stepWidth);
        }

        public void FleeMove(IEnumerable<Wolf> wolfs)
        {
            var bearing = GetDirection(wolfs);
            if (_energy > 0)
            {
                AdjustEnergy(-1);
                Position = _forestLayer.DeerEnvironment.MoveTowards(this, bearing , _sprintWidth);
            }
            else
            {
                AdjustEnergy(1);
                Position = _forestLayer.DeerEnvironment.MoveTowards(this, bearing , _stepWidth);
            }
        }

        public IEnumerable<Wolf> GetWolfsInSight()
        {
            return _forestLayer.WolfEnvironment.Explore(Position, _viewRadius);
        }

        public double GetDirection(IEnumerable<Wolf> wolfs)
        {
            
            double avgBearing = 0;
            // TODO:: Possible multiple enumeration???
            foreach (var wolf in wolfs)
            {
                avgBearing += Position.GetBearing(wolf.Position);
            }
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