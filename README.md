# AoC2024
Advent of Code 2024

Here we go!

## --- Day 1: Historian Hysteria ---  
Starting off easy with some comparisons of arrays  
I could definitely tell I haven't been coding as much in the past year, felt a bit rusty.
Threw together the first thing that came to mind, and it worked.  
Might clean it up later.  

I went back and took a new look at part 2. After looking at the problem I realized it was set up for just summing up all the values from the "Right" list which occured in the "Left" list.  
This could be done in a much simpler LINQ query, and that also means a lot more performant code.  
Not that it acually mattered in this case, but with larger inputs it sure would have.
For my input the difference was going from about 24 000 000 ticks to 400 000 ticks, a factor of 60 times faster for the "smarter" solution. And for larger inputs the factor would be even bigger.

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

## --- Day 5: Print Queue ---  
Check strings containing numbers against a list of rules dictating which numbers need to come before which other numbers.

Part 1 was straightforward enough, I use IndexOf, and completely ignored LINQ and other ways which might have been easier or more efficient.    
But it worked just fine, and the input was not that large - so I didn't think I would need to aim for resource efficiency. :D  

Part 2 was a bit trickier, take the non valid "updates" and move the numbers around in them until they no longer break the rules.
Actually not as tricky as I initially thought when I read the description.  
Then again, this is probably far from the best way to solve this - I'm thinking some sort of sort (he he) with a custom comparer might have done the trick.  
However, there was not enough brain- or will power in the tank today to tackle such a thing.  
I'm content with the solution.  

## --- Day 6: Guard Gallivant ---  
For part 1 we were supposed to move a guard around on a map, turn right when encountering obstacles dead ahead. And then count the unique coordinates visited by the guard.    
Not very challenging, most time spent on building some classes and setting up directions/turning and such things.  

For part 2 it gets trickier, find all the places on the map where placing a new obstacle would put the guard in an infinite loop.  
That will have to be solved later because right now it's time for work.    

So now it's later in the day, and I spent some time during my lunch thinking about how to go about this.  
At first it felt almost impossible to grasp - but as I sat looking at the test input I began to think that I could perhaps brute force my way through this.  
If I first find the regular path that the guard would take, with the base map. And then I place an obstacle at each of the open coordinates on that path, and then I just let the guard run through that version of the map - and check for loops.  
Now the hardest part turned out to be deciding on how to detect a loop. I was at first thinking in ways of counting how many times the guard passed the same coordinates - and find some arbitrary number that would probably indicate a loop.  
However, I sketched a bit with a pencil on some paper and found that if a guard hits the same obstacle twice when coming from the same direction, that has to be a sure way to tell that there is a loop.  
So that's what I implemented, and it worked both for my test input, and for the "real input".  
I'm sure there is some cool algorithm out there that can calculate this somehow - bruteforce usually is not the way in AoC. But sometimes it still works, and it runs in about 2 seconds on my puny Windows laptop, so I can live with that. I've done a lot worse bruteforcing in previous years... :D

## --- Day 7: Bridge Repair ---  
Enter Recursion... :D  
I was waiting for it, the classic - "We're gonna need recursion for this".  
Trying out different combinations of multiplication and addition of numbers to reach a target value.  
Recursion is always a bit tricky, and it took me a good while to get it working.  

Part 2 introduced a new operator for concatenation. My first instict was that this would be very hard to do, but then I realized that this could be implemented as a simple method, which would be used in the existing recursive method - and then just add a flag to enable concatenation in the calculations for part 2. 