public class Solution
{
    public int MaxVowels(string s, int k)
    {
        int firstMax = 0;
        for (int i = 0; i < k; i++)
        {
            if (s[i] == 'a' || s[i] == 'e' || s[i] == 'i' || s[i] == 'o' || s[i] == 'u')
            {
                firstMax++;
            }
        }
        int max = firstMax;
        for (int end = k; end < s.Length; end++)
        {
            if (s[end] == 'a' || s[end] == 'e' || s[end] == 'i' || s[end] == 'o' || s[end] == 'u')
            {
                firstMax++;
            }
            if (s[end - k] == 'a' || s[end - k] == 'e' || s[end - k] == 'i' || s[end - k] == 'o' || s[end - k] == 'u')
            {
                firstMax--;
            }
            if (firstMax > max)
            {
                max = firstMax;
            }
        }
        return max;
    }
}