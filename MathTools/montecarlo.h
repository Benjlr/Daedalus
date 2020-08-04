#pragma once

public class MonteCarloCalculator
{
private:
	int _iterations = 500;


public:

	MonteCarloCalculator(int iterations);

	void SetIterations(int amount);
	double** Calculate(const double* returns, const int size, double start_amount);



};
