
var input1 = new int[5] { 1, 2, 3, 4, 5 }; //Output: 11
var input2 = new int[3] { 15, 25, 35 }; //Output: 9
var input3 = new int[2] { 8, 8 };//Output: 12

Console.WriteLine("Total score #1: {0}", FindTotalScore(input1));
Console.WriteLine("Total score #2: {0}", FindTotalScore(input2));
Console.WriteLine("Total score #3: {0}", FindTotalScore(input3));

Console.ReadLine();

int FindTotalScore(int[] numbers)
{
    int totalScore = 0;
    if(numbers != null)
    {
        foreach (int n in numbers)
        {
            if (n % 2 == 0)
            {
                totalScore += 1;
            }
            else if (n % 2 != 0)
            {
                totalScore += 3;
            }
            if (n == 8)
            {
                totalScore += 5;
            }
        }
    }
    return totalScore;
}

