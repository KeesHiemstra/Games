using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationExercise
{
	class Program
	{

		static void Main(string[] args)
		{
			for (int i = 0; i < 5; i++)
			{
				Exercise();
			}

			Console.ReadKey();
		}

		private enum Operations
		{
			Add,
			Sub,
			Mul,
			Dev
		}

		private static void Exercise()
		{
			Random Rnd = new Random();

			const string OperatorString = @"+-*/";
			DateTime StartDate;
			TimeSpan ElapsedTime;
			string AnswerString;
			int Argument1 = Rnd.Next(2, 13);
			int Argument2 = Rnd.Next(2, 13);
			int ArgumentT;
			Operations Operator = Operations.Add;
			int Answer;
			int CorrectAnswer = -1;

			switch (Operator)
			{
				case Operations.Add:
					CorrectAnswer = Argument1 + Argument2;
					break;
				case Operations.Sub:
					ArgumentT = Argument1;
					CorrectAnswer = Argument1 + Argument2;
					Argument1 = CorrectAnswer;
					CorrectAnswer = ArgumentT;
					break;
				case Operations.Mul:
					CorrectAnswer = Argument1 * Argument2;
					break;
				case Operations.Dev:
					ArgumentT = Argument1;
					CorrectAnswer = Argument1 * Argument2;
					Argument1 = CorrectAnswer;
					CorrectAnswer = ArgumentT;
					break;
				default:
					break;
			}
			string Question = $"{Argument1} {OperatorString[(int)Operator]} {Argument2} = ";

			StartDate = DateTime.Now;
			Console.Write(Question);
			AnswerString = Console.ReadLine();
			ElapsedTime = DateTime.Now - StartDate;

			Answer = CheckAnswer(AnswerString);

			if (Answer != CorrectAnswer)
			{
				Console.WriteLine($"Incorrect! should have been {CorrectAnswer} ({ElapsedTime.Milliseconds})");
			}
			else
			{
				Console.WriteLine($"Correct ({ElapsedTime.Milliseconds})");
			}
		}

		private static int CheckAnswer(string answerString)
		{
			int Answer;

			if (!int.TryParse(answerString, out Answer))
			{
				Answer = 0;
			}
			return Answer;
		}
	}
}
