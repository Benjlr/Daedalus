#pragma once

class MonteCarloCalculator
{
private:
	int _iterations = 500;


public:


	void SetIterations(int amount);
	double** Calculate(const double* returns, const int size, double start_amount);


	MonteCarloCalculator(int iterations);

};
