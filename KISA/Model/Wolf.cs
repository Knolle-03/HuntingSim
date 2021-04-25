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
    public class Wolf : AbstractAnimal, IAgent<ForestLayer>, IPositionable
    {
        
        [PropertyDescription]
        public UnregisterAgent UnregisterHandle { get; set; }
        
        [PropertyDescription] 
        private bool ReproducibleSpawning { get; set; }
        
        private ForestLayer _forestLayer;
        
        public void Init(ForestLayer layer)
        {
            _forestLayer = layer;
            Position = _forestLayer.FindRandomPositionWolf(ReproducibleSpawning);
            _forestLayer.WolfEnvironment.Insert(this);
        }

        public string Rule { get; private set; }
        
        public void Tick()
        {
            var deers = GetDeersInSight();
            if (deers.Any())
            {
                Rule = "Hunting";
                HuntMove(deers);  
            }
            else
            {
                Rule = "Idle";
                RandomMove();   
            }
        }
        
        public IEnumerable<Deer> GetDeersInSight()
        {
            return _forestLayer.DeerEnvironment.Explore(Position, _viewRadius);
        }
        public void RandomMove()
        {
            AdjustEnergy(1);
            var bearing = RandomHelper.Random.Next(360);
            Position = _forestLayer.WolfEnvironment.MoveTowards(this, bearing, _stepWidth);
        }

        public void HuntMove(IEnumerable<Deer> deers)
        {
            var bearing = GetDirection(deers);
            if (_energy > 0)
            {
                AdjustEnergy(-1);
                Position = _forestLayer.WolfEnvironment.MoveTowards(this, bearing , _sprintWidth);
            }
            else
            {
                AdjustEnergy(1);
                Position = _forestLayer.WolfEnvironment.MoveTowards(this, bearing , _stepWidth);
            }
        }
        
        public double GetDirection(IEnumerable<Deer> deers)
        {
            return Position.GetBearing(deers.First().Position);
        }




        public Guid ID { get; set; }
        public Position Position { get; set; }
    }
}