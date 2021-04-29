using System;
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

        private string Rule { get; set; }
        
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

        private IEnumerable<Deer> GetDeerInSight()
        {
            return _forestLayer.DeerEnvironment.Explore(Position, ViewRadius);
        }

        private void RandomMove()
        {
            AdjustEnergy(1);
            var bearing = RandomHelper.Random.Next(360);
            Position = _forestLayer.WolfEnvironment.MoveTowards(this, bearing, StepWidth);
        }

        private void HuntMove(IEnumerable<Deer> deer)
        {
            var bearing = GetDirection(deer);
            if (Energy > 0)
            {
                AdjustEnergy(-1);
                Position = _forestLayer.WolfEnvironment.MoveTowards(this, bearing , SprintWidth);
            }
            else
            {
                AdjustEnergy(1);
                Position = _forestLayer.WolfEnvironment.MoveTowards(this, bearing , StepWidth);
            }
        }

        private double GetDirection(IEnumerable<Deer> deer)
        {
            return Position.GetBearing(deer.First().Position);
        }

        private Deer IsDeerInEatingDistance(IEnumerable<Deer> deerInSight)
        {
            return deerInSight.FirstOrDefault(deer => Distance.Euclidean(Position.PositionArray, deer.Position.PositionArray) <= 1);
        }




        public Guid ID { get; set; }
        public Position Position { get; set; }
    }
}