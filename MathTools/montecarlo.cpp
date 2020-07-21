// MonteCarloTool.cpp : Defines the exported functions for the DLL application.
//


#include "pch.h"
#include "montecarlo.h"
#include <algorithm>
#include <random>


MonteCarloCalculator::MonteCarloCalculator(int iterations = 500)
{
	_iterations = iterations;
}


void MonteCarloCalculator::SetIterations(const int amount)
{
	_iterations = amount;
}


double** MonteCarloCalculator::Calculate(const double* returns, const int size, const double start_amount)
{
	double** my_iterations = nullptr;
	auto rng = std::default_random_engine{};
	std::vector<int> returnsIndices;

	for (int i = 0; i < size; i++) returnsIndices.push_back(i);

	for (int i = 0; i < _iterations; i++)
	{

		double start_money = start_amount;
		double* iterationResult = &start_money;

		std::shuffle(std::begin(returnsIndices), std::end(returnsIndices), rng);


		for (int y = 1; y <= size; y++)
		{
			iterationResult[y] = returns[returnsIndices[y]] * iterationResult[y - 1];
		}
		my_iterations[i] = iterationResult;

	}

	return  my_iterations;
}