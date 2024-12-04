# Advent of Code 2024 Day03  

Okay, so I revisited this day, and tried to refine my solution for part 2 - it felt like I went down a weird path yesterday.
I realized I was overthinking before. I could have just matched the do() and don't()'s with the same pattern, and then checked for  
- don't() => Active=false  
- do() => Active = true  
- other: if Active => multiply numbers and sum

Instead of removing the data from don't() to the following do() and then running a new regex on that.  

The urge to finish a working solution "quickly" sometimes (often) disables some of my critical and creative thinking, once I find a path I usually go down it full strength at that point.  

So that's why I have two solutions for Part 2.