using System;

public class Solution {
    public String openSymbol(string closeSymbol)
    {
        string openSymbol = string.Empty;

        switch(closeSymbol)
        {
            case ")":
                openSymbol = "(";
                break;
            case "]":
                openSymbol = "[";
                break;
            case "}":
                openSymbol = "{";
                break;
        }

        return openSymbol;
    }
    
    public bool IsValid(string s)
    {

        char[] charArr = s.ToCharArray();

        for(int i = 0; i < charArr.Length; i++)
        {
            if(i == 0)
            {
                if (charArr[i].ToString().Equals(")") || charArr[i].ToString().Equals("}") || charArr[i].ToString().Equals("]"))
                {
                    return false;
                }
            }
            else
            {
                // My idea is to remove the valid pairs from charArr. In the end, the length or charArr must be zero for a valid s input.
                if(charArr[i-1].ToString() == openSymbol(charArr[i].ToString()))
                {
                    string newArr = new string(charArr);
                    newArr = newArr.Remove(i-1,2);
                    charArr = newArr.ToCharArray();
                    i = i-2;
                }
            }
        }
        
        if(charArr.Length.Equals(0))
            return true;

        return false;

    }
}

class ValidParentheses
{
    static void Main(string[] args)
    {
        Solution solution = new Solution();
        string? var = string.Empty;

        Console.WriteLine("Type the string: ");

        var = Console.ReadLine();

        if(!String.IsNullOrEmpty(var))
        {
            if(solution.IsValid(var))
                Console.WriteLine("Valid");
            else
                Console.WriteLine("Invalid");
        }
        else
        {
            Console.WriteLine("Empty");
        }

    }
}