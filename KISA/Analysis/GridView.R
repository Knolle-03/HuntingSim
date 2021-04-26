# Title     : TODO
# Objective : TODO
# Created by: 49179
# Created on: 26.04.2021

# Create a set of random trajectories.
nodes <- get.nodes(ocean.demo.grid)
set.seed(1)
x <- apply(matrix(rnorm(1000, 0, 0.01), 250), 2, cumsum) - 69.5
y <- apply(matrix(rnorm(1000, 0, 0.01), 250), 2, cumsum) + 42.5
lines(ocean.demo.grid, list(x=x, y=y), lty=1)

