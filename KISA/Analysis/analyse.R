# Title     : TODO
# Objective : TODO
# Created by: 49179
# Created on: 26.04.2021



sheeps <- read.csv(file="C:\\Users\\49179\\RiderProjects\\KI_Softwareagenten\\Wolf Sheep\\model-deployments\\C# Models\\model-wolf-sheep\\WolfSheepPredation\\bin\\Debug\\netcoreapp2.0\\Sheep.csv",head=T, sep=";", dec="." )
wolves <- read.csv(file="C:\\Users\\49179\\RiderProjects\\KI_Softwareagenten\\Wolf Sheep\\model-deployments\\C# Models\\model-wolf-sheep\\WolfSheepPredation\\bin\\Debug\\netcoreapp2.0\\Wolf.csv",head=T, sep=";", dec="." )

attach(sheeps)
attach(wolves)

max_sheep_steps <- max(sheeps$Step)
max_wolf_steps <- max(wolves$Step)

library(rgl)
plot(sheeps$X,sheeps$Y)
#plot(wolves$X,wolves$Y)
