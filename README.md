# AoC2024
Advent of Code 2024

Here we go!

## --- Day 1: Historian Hysteria ---  
Starting off easy with some comparisons of arrays  
I could definitely tell I haven't been coding as much in the past year, felt a bit rusty.
Threw together the first thing that came to mind, and it worked.  
Might clean it up later.  

## --- Day 2: Red-Nosed Reports ---  
Checking sequences of integers, if they are increasing or decreasing - and for part 2 allowing for one error and still be reported as valid.  
Part 1 was pretty straightforward, and I thought I had a grip on part 2 as well - but got stuck for a while, had to give it up during the day and do some work *sadface* and returned to it in the evening, checking my input and looking for edge cases.  
I spent a good deal of time before I figured out that I - ONCE AGAIN - had an off by one error. My problem was that I wasn't checking the whole array when checking if it was possible to remove one item and get a passing "report". I started from the index where I discovered the error - which could be a bit further along in the arrays indices.  
When I found the mistake everything worked out as it should.  
Yet again reinforcing that unittests are the shiznitz, but only effective if you're testing for the right thing. :-P  

## --- Day 3: Mull It Over ---  
Regex here we go... :D  
Part 1 was no big deal, just create a decent Regex pattern and then some maths on the results.  
Part 2 had me for a while, I was wrongly using `RegexOptions.MultiLine` - when I should have been using `RegexOptions.Singleline` - Silly me.  
After switching that back to SingleLine the calculation checked out OK.  

## --- Day 4: Ceres Search ---  
Traverse a grid and check for occurences of a word in 8 possible directions. Shouldn't be too hard right?  
Started late - at breakfast, about 1.5 hours later than usual. Then had to leave it and do some work.  
Took it up again at lunchtime-ish, and spent so much time wondering what was wrong - until I realized that I wasn't supposed to allow for words wrapping around the edges of the grid.  
I should have read the instructions a bit more carefully, as per usual I guess..  
Well - stupid mistake. After fixing that it was an easy path forward to sculpting part 2 during a coffebreak.  
Find a centerpoint, and check for matching patterns of "corners".  
Fun exercise, annoyingly stupid me. Lesson learned: Don't read the instructions fast at breakfast, and then try to solve the problem a few hours later without revisiting the instructions first.