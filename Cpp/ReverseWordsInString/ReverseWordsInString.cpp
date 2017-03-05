// ReverseWordsInString.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string.h>

void reverse(char *s, int start, int end)
{
    for (int i = start, j = end; i < j; i++, j--)
    {
        char temp = s[i];
        s[i] = s[j];
        s[j] = temp;
    }
}

void trimSpacesBetweenWords(char *s)
{
    char prevChar = ' ';
    char *dest = s;
    for (char *src = s; *src != '\0'; ++src)
    {
        if (prevChar == ' ' && *src == ' ')
            continue;
        prevChar = *dest = *src;
        ++dest;
    }

    if (prevChar == ' ' && dest > s) --dest; // Incase last character is space
    *dest = '\0';
}

void reverseWords(char *s) {

    if (s == NULL) return;
    // First reverse the string.

    int length = strlen(s);
    reverse(s, 0, length - 1);

    // Now reverse individual words.
    int start = 0;
    int end = 0;
    char *loc = s;
    while (*loc != '\0')
    {
        if (*loc == ' ')
        {
            end = loc - s - 1;
            reverse(s, start, end);
            start = end + 2;
        }
        ++loc;
    }

    end = length - 1;
    reverse(s, start, end);
    trimSpacesBetweenWords(s);
}

int main()
{
    const int size = 1024;
    char *input = new char[size];
    strcpy_s(input, size, "hello world!");
    strcpy_s(input, size, "  ");
    strcpy_s(input, size, "  hello  world   ");
    strcpy_s(input, size, "  hello  to world   ");
    
    reverseWords(input);

    printf("Result = '%s'\n", input);
    printf("Length of input after reverse = %d\n", strlen(input));
}
