using System;
using System.Collections.Generic;
using KISA.Model;
using Mars.Components.Starter;
using Mars.Interfaces.Model;

namespace KISA
{
    class Program
    {
        static void Main(string[] args)
        {
            var description = new ModelDescription();
            description.AddLayer<ForestLayer>();
            description.AddAgent<Deer, ForestLayer>();
            description.AddAgent<Wolf, ForestLayer>();
            
            
            //use code defined config
             var config = GenerateConfig();

            var starter = SimulationStarter.Start(description, config);
            var handle = starter.Run();
            Console.WriteLine("Successfully executed iterations: " + handle.Iterations);
            starter.Dispose();

        }
        // TODO:: Wolf logging doesn't work
        private static SimulationConfig GenerateConfig()
        {
            return new SimulationConfig
            {
                Globals =
                {
                    Steps = 50,
                    OutputTarget = OutputTargetType.Csv,
                    CsvOptions =
                    {
                        Delimiter = ";",
                        NumberFormat = "en-EN"
                    }
                },
                LayerMappings = new List<LayerMapping>
                {
                    new LayerMapping
                    {
                        Name = nameof(ForestLayer),
                        File = "C:\\Users\\49179\\RiderProjects\\KI_Softwareagenten\\KISA\\KISA\\Resources\\grid.csv"
                        
                    },

                },
                AgentMappings = new List<AgentMapping>
                {
                    new AgentMapping
                    {
                        Name = nameof(Deer),
                        InstanceCount = 1,
                        IndividualMapping = new List<IndividualMapping>
                        {
                            new IndividualMapping {Name = "ReproducibleSpawning", Value = true},
                            new IndividualMapping {Name = "Seed", Value = 42},
                        }
                    },
                    new AgentMapping
                    {
                        Name = nameof(Wolf),
                        InstanceCount = 1,
                        IndividualMapping = new List<IndividualMapping>
                        {
                            new IndividualMapping {Name = "ReproducibleSpawning", Value = true},
                            new IndividualMapping {Name = "Seed", Value = 42},
                        }
                    }
                }
            };
        }
    }
}
    