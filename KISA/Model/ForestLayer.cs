using System;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace KISA.Model
    {
        public class ForestLayer : RasterLayer
    {

        [PropertyDescription]
        private int Seed { get; set; }
        public SpatialHashEnvironment<Deer> DeerEnvironment { get; set; }
        public SpatialHashEnvironment<Wolf> WolfEnvironment { get; set; }

        private IAgentManager AgentManager { get; set; }

        


        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle, UnregisterAgent unregisterAgentHandle)
        {
            var initiated = base.InitLayer(layerInitData, registerAgentHandle, unregisterAgentHandle);
            DeerEnvironment = new SpatialHashEnvironment<Deer>(Width - 1, Height - 1) {CheckBoundaries = true};
            WolfEnvironment = new SpatialHashEnvironment<Wolf>(Width - 1, Height - 1) {CheckBoundaries = true};


            AgentManager = layerInitData.Container.Resolve<IAgentManager>();
            AgentManager.Spawn<Deer, ForestLayer>().ToList();


            return initiated;
        }
        
        public Position FindRandomPositionDeer(bool reproducible)
        {
            var rng = reproducible ? new Random(Seed) : new Random();
            return Position.CreatePosition(rng.Next(Width - 1), rng.Next(Height - 1));
        }
        
        public Position FindRandomPositionWolf(bool reproducible)
        {
            Deer element;
            Position pos;
            do
            {
                var rng = reproducible ? new Random(Seed) : new Random();
                pos = Position.CreatePosition(rng.Next(Width - 1), rng.Next(Height - 1));
                element = DeerEnvironment.Explore(pos, 0).FirstOrDefault();
                // TODO:: explore environment further
            } while (element != null);

            return pos;
        }


        public void Tick()
        {
            throw new NotImplementedException();
        }

        public void PreTick()
        {
            throw new NotImplementedException();
        }

        public void PostTick()
        {
            throw new NotImplementedException();
        }
    }
}