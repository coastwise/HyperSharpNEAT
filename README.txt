HyperSharpNEAT v2.1 C#,
By David D'Ambrosio
http://www.cs.ucf.edu/~ddambro or http://eplex.cs.ucf.edu
ddambro@cs.ucf.edu

Documentation for this package is included in this README file. 

-------------
1. LICENSE
-------------

The majority of this code is from Colin Green's SharpNEAT.  
(http://sharpneat.sourceforge.net/).  All original SharpNEAT code is covered
by the original SharpNEAT license as described by Colin Green:

"The SharpNeat project consists of the core code packaged as SharpNeatLib
and the main application simply called SharpNeat. SharpNeatLib is released
under the Gnu Lesser General Public License (LGPL) which means you can link
to it from your own programs, proprietory or otherwise. 

The SharpNeat application is released under the Gnu General Public License
(GPL).

The additional applications, PreyCaptureSimulator and PoleBalancingSimulator
have no license and are public domain. Modify them at your will."

HyperSharpNEAT modifies original SharpNEAT in the 
following ways to transform it into a HyperNEAT implementation:

-Updated several lists to .NET 2.0 generic versions.  This was done 
simply for efficiency, however Colin Green's original sorting and
manipulation functions were kept as they were found to be more efficient
than built in versions.
-Added the ability have multiple activation functions in a single network.
-Removed the GUI and extra experiments.
-Added HyperNEAT specific classes
    -Substrate.cs: Defines the substrate on which neural networks are 
created.  Should be inherited and implemented for specific experiments.
    -HyperNEATParmeters.cs: Reads parameters from a file that are 
specific to HyperNEAT.  Should be inherited and implemented for specific
experiments

The HyperNEAT additions are covered by the following license:

This program is free software; you can redistribute it and/or modify it
under the terms of the GNU General Public License version 2 as published
by the Free Software Foundation (LGPL may be granted upon request). This 
program is distributed in the hope that it will be useful, but without any 
warranty; without even the implied warranty of merchantability or fitness
for a particular purpose. See the GNU General Public License for more
details.


---------------------
2. USAGE and SUPPORT
---------------------

We hope that this software will be a useful starting point for your own
explorations in interactive evolution with NEAT. The software is 
provided as is,
however, we will do our best to maintain it and accommodate
suggestions. If you want to be notified of future releases of the
software or have questions, comments, bug reports or suggestions, send
email to ddambro@cs.ucf.edu

Alternatively, you may post your questions on the NEAT Users Group at :
http://tech.groups.yahoo.com/group/neat/.


The following explains how to use HyperNEAT.  For information on compiling
HyperNEAT, please see the section on compiling below.

INTRO
-----
HyperNEAT is an extension of NEAT (NeuroEvolution of Augmenting Topologies)
that evolves CPPNs (Compositional Pattern Producing Networks) that
encode large-scale neural network connectivity patterns.  A complete
explanation of HyperNEAT is available here:

@InProceedings{dambrosio:gecco07,
  author       = "David B. D'Ambrosio and Kenneth O. Stanley",
  title        = "A Novel Generative Encoding for Exploiting Neural 
		  Network Sensor and Output Geometry",
  booktitle    = "Proceedings of the Genetic and Evolutionary
                  Computation Conference (GECCO 2007)",
  year         = 2007,
  publisher    = "ACM Press",
  address      = "New York, NY",
  site         = "London",
  url          = "http://eplex.cs.ucf.edu/papers/dambrosio_gecco07.pdf",
}

The version of HyperNEAT distributed in this package executed the 
experiments in the following paper, with a few alterations:

@InProceedings{dambrosio:gecco08,
  author       = "David B. D'Ambrosio and Kenneth O. Stanley",
  title        = "Generative Encoding for Multiagent Learning",
  booktitle    = "Proceedings of the Genetic and Evolutionary
                  Computation Conference (GECCO 2008)",
  year         = 2008,
  publisher    = "ACM Press",
  address      = "New York, NY",
  site         = "Atlanta, GA",
  url          = "http://eplex.cs.ucf.edu/papers/dambrosio_gecco08.pdf",
}


For more information, please visit the EPlex website at:
http://eplex.cs.ucf.edu/

or see more of our publications on HyperNEAT and CPPNs at:
http://eplex.cs.ucf.edu/publications

EXECUTABLE
----------
The executable "CommandLineEvolution.exe" is located in the "release" 
directory of the same project.  It runs the multiagent predator prey experiment
described below with the parameters in the params.txt file and on the command line.  
The project files that are included should also  allow for easy compilation in 
Microsoft's Visual Studio.


GUI
---
The GUI for this experiment is included in the SkirmishVisualization project.  



--------------
3. EXPERIMENTS
--------------

SKIRMISH EXPERIMENT
-------------------------
This is the experiment presented in the GECCO 2008 paper (see above).  The
goal of the experiment is for a group of predators to capture a group of prey.

When run, experiment will output the generation number, the fitness, and 
the amount of time per generation, for each generation.

The CommandLineEvolution.exe can take several command line arguments.

-folder "foldername": "foldername" is the path to a folder where the results are stored
if none if specified, results will be output to the current directory

-multi "true or false":if true, the substrate will be heterogeneous, if false the 
substrate will be homogeneous, the default is homogeneous

-seed "filename":specifies the path to a file containing the seed for evolution.  For
Heterogeneous substrates, you MUST seed with the included seedGenomeModulusBlank.xml or
another seed file with the modulus nodes added.

-shape "shapeType": specifies which shape of prey to train against, valid options are
triangle, diamond, square, and L, the default is triangle.

To run different version of the experiments change the parameters in the 
included params.txt using this guide:

Threshold .2
WeightRange 3
NumberofThreads 2
SubstrateActivationFunction SteepenedSigmoid
StartActivationFunctions
BipolarSigmoid .25
Sine .25
Gaussian .25
Linear .25
EndActivationFunctions


Threshold defines the minimum value a CPPN must output for that 
connection to be expressed, should be 0-1.

WeightRange defines the minimum and maximum values for weights on substrate
connections, they go from -WieghtRange to +WeightRange, can be any integer.

NumberofThreads defines the number of simultaneous evaluations to run.  
This can be any integer greater than 0, however numbers greater than
the number of cores/processors you have available can degrade performance.

SubstrateActivationFunction determines which activation function each node
in the substrate will have.  This can be any of the activation functions in 
SharpNEATLib, you can add your own as well.  The activation function is the name of the 
.cs file containing that function (they are accessed by reflection, so case counts).

Activation Functions that can be in the CPPN start with 
"StartActivationFunctions" and are listed one per line and ended with 
"EndActivationFunctions".  Each activation function is the name of the .cs file
containing that function (they are accessed by reflection, so case counts) followed
by the probability of that function appearing.

The GUI interface to the experiment is included in the SkirmishVisualization project.
To use the GUI, first load a genome by choosing File->Load Genome and selecting a genome
file.  Then change toggle multi to reflect if the genome is homogeneous or heterogeneous.
Next you need to select a group of prey by pressing one of the following keys:
"1,2,3,4,5" These are the 5 single agent, seed training worlds.
"q,w,e" Three variations of the triangle.
"a,s,d" Three variations of the diamond.
"z,x,c" Three variations of the square.
"r,t,y" Three variations of the L.
Clicking on an already displayed scenario adds a prey at that location.

Pressing 'm' toggles the display of the pie sensors.
Pressing '/' advances the simulation by 100 time steps.
Pressing any key not mentioned here advances the simulation 1 time step.


--------------
4. RESULTS
--------------
Unfortunately, changes to the code have rendered the original results incompatible with
the current visualization tools.  Original results are available by request from 
ddambro@cs.ucf.edu.

--------------
5. COMPILING
--------------

DEPENDENCIES
--------------
Everything necessary to compile HyperSharpNEAT is included in this release.

BUILD INSTRUCTIONS:
---------------
UNIX/LINUX/CYGWIN/MACOSX:

To run on these systems, compile and run with the latest version of Mono.

WINDOWS:

The included project files have everything set up to run in Microsoft 
Visual Studio.


--------------
6. FORUM
--------------

We are available to answer questions at the NEAT Users Group:

http://tech.groups.yahoo.com/group/neat/

-------------------
7. Acknowledgements
-------------------

Special thanks to Colin Green for creating SharpNEAT.