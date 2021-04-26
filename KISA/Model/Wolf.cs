using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mars.Common;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using Mars.Numerics;
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
            var deerList = GetDeerInSight();
            var eatableDeer = IsDeerInEatingDistance(deerList);
            if (eatableDeer != null)
            {
                Rule = "Eat Deer";
                EatDeer(eatableDeer);
                return;
            }
            if (deerList.Any())
            {
                Rule = "Hunting";
                HuntMove(deerList);  
            }
            else
            {
                Rule = "Idle";
                RandomMove();   
            }
        }

        private void EatDeer(Deer deer)
        {
            deer.Die();
            // TODO:: fill energy completely when killing deer?
            AdjustEnergy(5);
        }

        public IEnumerable<Deer> GetDeerInSight()
        {
            return _forestLayer.DeerEnvironment.Explore(Position, _viewRadius);
        }
        public void RandomMove()
        {
            AdjustEnergy(1);
            var bearing = RandomHelper.Random.Next(360);
            Position = _forestLayer.WolfEnvironment.MoveTowards(this, bearing, _stepWidth);
        }

        public void HuntMove(IEnumerable<Deer> deer)
        {
            var bearing = GetDirection(deer);
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
        
        public double GetDirection(IEnumerable<Deer> deer)
        {
            return Position.GetBearing(deer.First().Position);
        }

        public Deer IsDeerInEatingDistance(IEnumerable<Deer> deerInSight)
        {
            return deerInSight.FirstOrDefault(deer => Distance.Euclidean(Position.PositionArray, deer.Position.PositionArray) <= 1);
        }




        public Guid ID { get; set; }
        public Position Position { get; set; }
    }
}