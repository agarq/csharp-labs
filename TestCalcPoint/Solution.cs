namespace TestCalcPoint
{
    class Solution
    {
        public int CalPoints(string[] ops)
        {
            int indexOps = 0;
            int indexResult = -1;
            int temp = 0;
            List<int> result = new List<int>();
            bool isNumber;

            foreach(string s in ops)
            {
                isNumber = int.TryParse(s, out temp);

                if(isNumber)
                {
                    Console.Write(temp.ToString() + " is number");
                    Console.Write("\n");
                    result.Add(temp);
                    indexResult++;
                }
                else
                {
                    /*
                    "+" - Record a new score that is the sum of the previous two scores. It is guaranteed there will always be two previous scores.
                    "D" - Record a new score that is double the previous score. It is guaranteed there will always be a previous score.
                    "C" - Invalidate the previous score, removing it from the record. It is guaranteed there will always be a previous score.
                    */
                    switch (s.ToUpper())
                    {
                        case "+":
                            
                            Console.WriteLine("Index of '+': " + indexOps.ToString());
                            Console.WriteLine("Index of new list: " + indexResult.ToString());

                            result.Add(result[indexResult-1] + result[indexResult]);
                            indexResult++;

                            break;

                        case "D":
                            Console.WriteLine("Index of 'D': " + indexOps.ToString());
                            Console.WriteLine("Index of new list: " + indexResult.ToString());

                            result.Add(result[indexResult]*2);
                            indexResult++;
                            break;

                        case "C":
                            Console.WriteLine("Index of 'C': " + indexOps.ToString());
                            Console.WriteLine("Index of new list: " + indexResult.ToString());

                            result.RemoveAt(indexResult);
                            indexResult--;
                            break;

                    }
                }
                
                indexOps++;
                isNumber = false;
            }

            Console.WriteLine(string.Format("Here's the list: ({0}).", string.Join(", ", result)));
            
            return result.Sum();
        }
    }

    class CalPoints
    {
        static void Main(string[] args)
        {
            Solution solution = new Solution();

            Char[] space = new char[] { ' ' };
            
            Console.WriteLine("Type the input:");
            string[] ops = Console.ReadLine().Split(space);

            int output = solution.CalPoints(ops);

            Console.Write("Final Result: " + output.ToString());

        }
    }
}