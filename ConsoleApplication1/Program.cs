//#define COMPACTED
#define LONGER
#define MOREBOTH

using System;
using System.Collections.Generic;
using System.Text;
using SharpNeatLib;
using SharpNeatLib.AppConfig;
using SharpNeatLib.Evolution;
using SharpNeatLib.Evolution.Xml;
using SharpNeatLib.Experiments;
using SharpNeatLib.NeatGenome;
using SharpNeatLib.NeatGenome.Xml;
using SharpNeatLib.NeuralNetwork;
using SharpNeatLib.NeuralNetwork.Xml;
using System.Xml;
using System.IO;

namespace SharpNeat.Experiments
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = "";
            NeatGenome seedGenome = null;
            string filename = null;
            string shape = "triangle";
            bool isMulti = false;

            for (int j = 0; j < args.Length; j++)
            {
                if(j <= args.Length - 2)
                    switch (args[j])
                    {
                        case "-seed": filename = args[++j];
                            Console.WriteLine("Attempting to use seed from file " + filename);
                            break;
                        case "-folder": folder = args[++j];
                            Console.WriteLine("Attempting to output to folder " + folder);
                            break;
                        case "-shape": shape = args[++j];
                            Console.WriteLine("Attempting to do experiment with shape " + shape);
                            break;
                        case "-multi": isMulti = Boolean.Parse(args[++j]);
                            Console.WriteLine("Experiment is heterogeneous? " + isMulti);
                            break;
                    }
            }

            if(filename!=null)
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(filename);
                    seedGenome = XmlNeatGenomeReaderStatic.Read(document);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Problem loading genome. \n" + e.Message);
                }
            }

            double maxFitness = 0;
            int maxGenerations = 1000;
            int populationSize = 150;
            int inputs = 4;
            IExperiment exp = new SkirmishExperiment(inputs, 1, isMulti, shape);
            StreamWriter SW;
            SW = File.CreateText(folder + "logfile.txt");
            XmlDocument doc;
            FileInfo oFileInfo;
            IdGenerator idgen;
            EvolutionAlgorithm ea;
            if (seedGenome == null)
            {
                idgen = new IdGenerator();
                ea = new EvolutionAlgorithm(new Population(idgen, GenomeFactory.CreateGenomeList(exp.DefaultNeatParameters, idgen, exp.InputNeuronCount, exp.OutputNeuronCount, exp.DefaultNeatParameters.pInitialPopulationInterconnections, populationSize)), exp.PopulationEvaluator, exp.DefaultNeatParameters);

            }
            else
            {
                idgen = new IdGeneratorFactory().CreateIdGenerator(seedGenome);
                ea = new EvolutionAlgorithm(new Population(idgen, GenomeFactory.CreateGenomeList(seedGenome, populationSize, exp.DefaultNeatParameters, idgen)), exp.PopulationEvaluator, exp.DefaultNeatParameters);
            }
            for (int j = 0; j < maxGenerations; j++)
            {
                DateTime dt = DateTime.Now;
                ea.PerformOneGeneration();
                if (ea.BestGenome.Fitness > maxFitness)
                {
                    maxFitness = ea.BestGenome.Fitness;
                    doc = new XmlDocument();
                    XmlGenomeWriterStatic.Write(doc, (NeatGenome)ea.BestGenome);
                    oFileInfo = new FileInfo(folder + "bestGenome" + j.ToString() + ".xml");
                    doc.Save(oFileInfo.FullName);

                    // This will output the substrate, uncomment if you want that
                    /* doc = new XmlDocument();
                     XmlGenomeWriterStatic.Write(doc, (NeatGenome) SkirmishNetworkEvaluator.substrate.generateMultiGenomeModulus(ea.BestGenome.Decode(null),5));
                     oFileInfo = new FileInfo(folder + "bestNetwork" + j.ToString() + ".xml");
                     doc.Save(oFileInfo.FullName);
                     */

                   
                }
                Console.WriteLine(ea.Generation.ToString() + " " + ea.BestGenome.Fitness + " " + (DateTime.Now.Subtract(dt)));
                //Do any post-hoc stuff here

                SW.WriteLine(ea.Generation.ToString() + " " + (maxFitness).ToString());

            }
            SW.Close();

            doc = new XmlDocument();
            XmlGenomeWriterStatic.Write(doc, (NeatGenome)ea.BestGenome, ActivationFunctionFactory.GetActivationFunction("NullFn"));
            oFileInfo = new FileInfo(folder + "bestGenome.xml");
            doc.Save(oFileInfo.FullName);

        }
    }
}
