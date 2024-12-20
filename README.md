# Advent of Code 2024  

This is my attempt at solving the puzzles from [Advent of Code 2024](https://adventofcode.com/2024)  

Status for the Unit tests: [![Advent Of Tests 2024](https://github.com/snailtail/AoC2024/actions/workflows/main.yml/badge.svg)](https://github.com/snailtail/AoC2024/actions/workflows/main.yml)  

## --- Day 1: Historian Hysteria ---  
Starting off easy with some comparisons of arrays  
I could definitely tell I haven't been coding as much in the past year, felt a bit rusty.
Threw together the first thing that came to mind, and it worked.  
Might clean it up later.  

I went back and took a new look at part 2. After looking at the problem I realized it was set up for just summing up all the values from the "Right" list which occured in the "Left" list.  
This could be done in a much simpler LINQ query, and that also means a lot more performant code.  
Not that it actually mattered in this case, but with larger inputs it sure would have.
For my input the difference was going from about 24 000 000 ticks to 400 000 ticks, a factor of 60 times faster for the "smarter" solution. And for larger inputs the factor would be even bigger.

## --- Day 2: Red-Nosed Reports ---  
Checking sequences of integers, if they are increasing or decreasing - and for part 2 allowing for one error and still be reported as valid.  
Part 1 was pretty straightforward, and I thought I had a grip on part 2 as well - but got stuck for a while, had to give it up during the day and do some work *sad face* and returned to it in the evening, checking my input and looking for edge cases.  
I spent a good deal of time before I figured out that I - ONCE AGAIN - had an off by one error. My problem was that I wasn't checking the whole array when checking if it was possible to remove one item and get a passing "report". I started from the index where I discovered the error - which could be a bit further along in the arrays indices.  
When I found the mistake everything worked out as it should.  
Yet again reinforcing that unittests are the shit, but only effective if you're testing for the right thing. :-P  

## --- Day 3: Mull It Over ---  
Regex here we go... :D  
Part 1 was no big deal, just create a decent Regex pattern and then some maths on the results.  
Part 2 had me for a while, I was wrongly using `RegexOptions.MultiLine` - when I should have been using `RegexOptions.Singleline` - Silly me.  
After switching that back to SingleLine the calculation checked out OK.  

## --- Day 4: Ceres Search ---  
Traverse a grid and check for occurrences of a word in 8 possible directions. Shouldn't be too hard right?  
Started late - at breakfast, about 1.5 hours later than usual. Then had to leave it and do some work.  
Took it up again at lunchtime-ish, and spent so much time wondering what was wrong - until I realized that I wasn't supposed to allow for words wrapping around the edges of the grid.  
I should have read the instructions a bit more carefully, as per usual I guess.  
Well - stupid mistake. After fixing that it was an easy path forward to sculpting part 2 during a coffee break.  
Find a center point, and check for matching patterns of "corners".  
Fun exercise, annoyingly stupid me. Lesson learned: Don't read the instructions fast at breakfast, and then try to solve the problem a few hours later without revisiting the instructions first.

## --- Day 5: Print Queue ---  
Check strings containing numbers against a list of rules dictating which numbers need to come before which other numbers.

Part 1 was straightforward enough, I use IndexOf, and completely ignored LINQ and other ways which might have been easier or more efficient.    
But it worked just fine, and the input was not that large - so I didn't think I would need to aim for resource efficiency :D  

Part 2 was a bit trickier, take the non-valid "updates" and move the numbers around in them until they no longer break the rules.
Actually not as tricky as I initially thought when I read the description.  
Then again, this is probably far from the best way to solve this - I'm thinking some sort of sort (he he he) with a custom comparer might have done the trick.  
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
I'm sure there is some cool algorithm out there that can calculate this somehow - bruteforce usually is not the way in AoC. But sometimes it still works, and it runs in about 2 seconds on my puny Windows laptop, so I can live with that. I've done a lot worse brute-forcing in previous years... :D

## --- Day 7: Bridge Repair ---  
Enter Recursion... :D  
I was waiting for it, the classic - "We're going to need recursion for this".  
Trying out different combinations of multiplication and addition of numbers to reach a target value.  
Recursion is always a bit tricky, and it took me a good while to get it working.  

Part 2 introduced a new operator for concatenation. My first instinct was that this would be very hard to do, but then I realized that this could be implemented as a simple method, which would be used in the existing recursive method - and then just add a flag to enable concatenation in the calculations for part 2. 

## --- Day 8: Resonant Collinearity ---  
I had a late start for this one. Sleeping in, eating breakfast, reading the problem, almost giving up as I thought it would be too hard for me to figure out.  
But after a cup of coffee I read through the problem again, and tried to explain why it was so hard to do to my Wife (thank you Wife-rubber-duck! <3), and that made me look at the problem from a different angle.  
Suddenly it was just a matter of parsing, searching, pairing up coordinates, and calculating new coordinates from those pairs.  

Also, today I decided to try this totally TDD - I only made an xUnit test project, and built the tests I knew I would need first. And then built the methods and logic until the tests passed.  
I'm far from good at TDD, but this could be a good way to actually get some practice in.  
I'm also using Rider for the first time, and that's a bit unfamiliar to me - but gives some good practice for that also.  
Fun challenge. Now I haven't even read what Part 2 is asking for yet - I'm a bit afraid to look to be honest... :D  

Well... I was not wrong :D
Part 2 kicked me right in the face.  
I tried all sorts of bad ideas, with the antenna coordinates as starting point. And then for a while using the coordinates for the AntiNodes from Part1 as starting points.  
That only fueled my impostor syndrome really badly. Then I googled around randomly trying to formulate something I wasn't sure how to express. Like "how do I tell if a point is on the same..." - and then I remembered "vectors".  
Ah what a revelation - thank you dear brain for not being completely turned off today. So googling onwards from vectors, I got the idea that one could draw a vector from any point to a known antenna coordinate from one of the antenna pairs, and then check if that vector was parallell to the vector between the coordinates in the pair.  
Now that was a fun trip into forgotten realms, and some new ones as well.  
I haven't studied maths and geometry and such things in \*hrm\* _"a while"_.  

But after googling some more and trying some stuff out, I found that it could be done in such a simple way that I could understand it.  
So the code I implemented for part two does just that. It continues on from the previous solution, traversing each point in the grid and checking against all the pairs of coordinates for antennas. Drawing vectors between the current position in the grid, and the first coordinate in the pair, and one between the coordinates in the pair.
Then checking if these vectors are parallell using the formula a * d == b * c (assuming vector 1 = (a, b) and vector 2 = (c, d)).  
I believe the term I found on Google was _"checking if the vectors are scalar multiples of each other"_.  
Phew, that was certainly a weekend kind of problem.  
But it's always fun when you do give it a go, and you learn something new along the way. I will most likely forget most things about scalar multiples of vectors until next time I need them, but maybe I'll have some tiny fragment of a memory that will help me find the right type of solution faster next time.

## --- Day 9: Disk Fragmenter ---  
FAT tables ahoy!  
A fun day, I spent a while trying to understand the logic for file id's - and then another good while getting a grasp of the wording for how to calculate the checksum.
Oh! How convenient that the test input only had 9 files in it :D  
Trial and error a few runs until I got it right.  

Part 2 was very tricky for me, I almost gave up - but on my coffee break I re-read my solution and realized that I was going through files, moving them to the next available free space - without checking if that free space was farther to the "right" than the file itself...  
So I had a long discussion with myself and the general message was "Why do you rush so dear sweet summer child?". Trying to squeeze these things in during breakfast, lunch, and coffee breaks makes me sloppy due to "stress" - so I don't stop to check that I've figured out all the probable edge cases.  
Lesson learned (again, I've taken that same lesson for the umpteenth time now).  

## --- Day 10: Hoof It ---  
Another recursion day!  
I recognized that BFS or DFS would probably be what I needed.  
I decided on DFS and built up an implementation, and after tweaking around a bit I found one that worked well for my test inputs. That one turned out both to work fine for the larger input, and also to be a good stepping stone for part 2.
For part 1 I only saved the peaks coordinates when I reached them. And it was rather easy to extend this to also saving the entire path as a string, to a hashset.
That gave me a neat list of all possible paths - so today part 2 was just a small tweak for me.  

## --- Day 11: Plutonian Pebbles ---  
Part 1 felt "Easy", just mutate some strings according to three rules. But there was this lurking feeling that this might be one of those days where you do something in part1 that's fine to do in a non optimized manner, and then part 2 has you doing more repetitions of the same thing resulting in needing 40 Petabytes of RAM and/or access to all of Google Clouds GPU's.  
This was the case. 
I'm not sure how long it would take to solve part2 (if it's even possible) with my first "quick-and-dirty" solution for part 1. It's running on the M1 at home since I left for work - probably won't be done when I get home. :D  

Part 2 wanted us to run 75 iterations of the mutations instead of 25.  
Somewhere after iteration 27 my computer started to break a small sweat, and from there on it just got expontially worse.  
I figured I would need to find a smarter solution. So I tried two things:  
1. First just running parallellized - this did roughly nothing to help. 
2. Then I tried storing the values in a smarter way, a Dictionary<string,long> - to not build such a huge list of strings, and instead just keep a count of how many times each string occurs.  
I also slapped on the `Parallel.ForEach` version "just in case" and came down to a runtime of just 216 milliseconds.  

My Dictionary of stones contained 3776 different stones, and the total sum was ridiculous, something like 229 trillion.  
I tested the solution without the Parallellization, and the runtime went up by about 20% - so that did not do much at all in this scenario, what helped 99.9999% was the smarter storage of the numbers/stones.  
I also switched my Part 1 to use the optimized solution, I left the unoptimized solution untouched - just because I'm lazy and don't want to rewrite my unit tests. However they run the same underlying code, so the tests are still valid from my perspective.  
Part 1 took about 160 ms un-optimized, and 8 ms using the optimized version.  


## --- Day 12: Garden Groups ---  

Wow! :D
Got a very late start due to an intense migraine yesterday, so I started a day late with this one.  

For Part 1 I had a feeling that some sort of flood fill could be helpful. So I started researching and implementing one of those.  
That turned out to be something I could use, it took a lot of tweaking to get it right - I'll blame it on my "Migraine hangover" ;)  

For Part 2 it got a lot trickier - fiding "sides" for the shapes/plots in the grid was not as straightforward as I would have wanted.  
But I googled and drew some sketches with pen and paper to visualise - and came to the conclusion that the amount of corners on a shape is equal to the amount of sides.  

That helped a lot :D  
Then it turns out there are some nasty corner cases, for example shapes that are "lines" in the grid. (I solved this by quadrupling the grid - each pixel became a 2x2 blob of pixels instead).    

So I spent the majority of my time rewriting my `IsPlotCorner` method and coming up with new testcases for corner cases (he he he).  

I think the example inputs from the puzzle contains all the tricky corner cases, I just didn't think to implement tests for all of them until I encountered new errors one after the other.  
But after implementing tests for all the example inputs and working out how to determine what is and is not a corner, I had a version that worked well for my real input as well.    


## --- Day 13: Claw Contraption ---  

Good lord...  
This was way out of my league!  

### Part 1  

First I thought this would be another recursive method. But as I couldn't wrap my head around how to calculate this, I didn't even start down that path.
Instead I spent probably around 1.5 hours fiddling around with my own silly ideas, dabbling with "Greatest Common Divisor" and "Least common multiple", multiplying, dividing and trying to find some sort of pattern or relationship.  
Nada. Nothing to be found there apart from headaches.    
So I started trying to Google for  tips on how to solve an equation with two unknown parameters. Or whatever it's called.  
I ended up with a whole bunch of browser tabs, with everything from something like 10 different Youtube videos, this page about the ["Euclidean algorithm"](https://en.wikipedia.org/wiki/Euclidean_algorithm), and a page on Matrix inversions - which is what I think led me on to [Cramer's rule](https://en.wikipedia.org/wiki/Cramer%27s_rule)    

At some point I tried to describe the nature of my challenge to Chat GPT and asked if I could possibly solve such a problem using GCD and/or Cramer's rule. To which the answer was yes.
First it led me down a very dark path for something called [Diophantine euqation](https://en.wikipedia.org/wiki/Diophantine_equation) and I ended up spending at least another fruitless hour there before deleting everyting and closing a lot of browser tabs.  
Long story already a bit too long, but at last I tried to implement Cramer's rule as a method in C# - It went pretty okay I guess, but I resorted to using Chat GPT to debug/rewrite and get a working implementation of that method in the end.  
So anyway that took care of Part 1.  
Now for Part 2 I'm not sure what will come next.

### Part 2  

Step 1: Okay... larger numbers -> Replace int with long.    
Step 2: Pray - +20 trillion in manhattan distance is quite the increase from part 1... :D    
Step 3: Be amazed. It turns out that the equation I used doesn't really care about the size of the inputs - as a recursive method definitely would have if I had gone down that path.      
Luckily I'm too bad at maths to even begin implementing a recursive function, so hence the googling for Part 1.  
Just had to adjust the ints to longs, and add the offset of 10 trillion to the X and Y coordinates for the Prizes.  
Then just re-run and stare in wonder at the large number that came out.  
Where are we ever going to get all those tokens?  
Hopefully we can load them digitally on to some kind of plastic card like we did this summer when we spent some time in the arcade on Santa Monica Pier.  
Much better than lugging around a pile of tokens that would reach something like halfway to the moon if stacked on top of each other... :D  

## --- Day 14: Restroom Redoubt ---  

More grids - sort of.  
Tracking movements of robots in a grid of a certain size.  
Part 1 was very easy, just keep track of the robots positions - made some classes and extensions for it to be easy to run tests on them.  
Part 2 was just freaky. I had to start printing "frames" of the grid to check for a pattern resembling a christmas tree. :D  
I just did that, and then guessed that more than 63% of the robots should be in the lower two quadrants when a tree pattern was displayed in the output grid.  
I was probably lucky, because the tree was smaller than I expected - and more concentrated to the middle of the grid.  
But I just looped the test until I saw a christmas tree in the output, and then recorded how many "seconds" had elapsed.  
Fun day - nice with an easy challenge to switch things up from day 13... :D  

## --- Day 15: Warehouse Woes ---  

Again attempting this puzzle a few days after it's original release because of - well life. :D  

Part 1 was again quite straightforward. Just handle a map and some basic checking if moves can be made, and stuff like that. Again TDD helps a lot with figuring out where to go next. Starting with basic tests and building up to more complete solutions.  

Part 2 looks a bit trickier... I'm going to have to check that out later, after work or during lunch break maybe. The movement stuff is going to be a lot more complex I think.  

Yeah, it's a full day later and I just spent two hours trying to get this to work.    
Lots of tricky bits here, at first I got stuck trying to understand how to calculate the GPS coordinates for part 2. I think the description for that was a bit wonky - I had to reimplement this - which I found after running my solution with some more test inputs from Reddit with more corner cases.    
Turns out I had both misunderstood the GPS calculation, and had a bug in my `MoveRobot()` method. The bug was found when I debugged trying to push an inverted pyramid of boxes upwards.  
I thought I was going to go mad figuring out what I needed to change - it came down to the order for checking tiles in the grid.  
Honestly I was close to giving up on this one, but then I had some epiphany and switched one Stack to a Queue - and Hey Presto!  

I started down the Stack<(int,int)> path for Part 1, which worked fine - and then continued on with that for Part 2 as well.  
Now afterwards I think I might have benefited from doing a recursive method instead, tricky as that is - it might still have been easier to test and debug.  

## --- Day 16: Reindeer Maze ---  

Oh no.. Pathfinding!  
__Part 1__ was not too bad. Pathfinding using a priority queue to sort by cost.  

For __Part 2__ I have done a few futile attempts so far - but I can't seem to wrap my head around how to check all the possible paths and their costs - without risking excluding a path that is similar to one we have already taken, and at the same time avoid an infinite loop.  
I'll have to sleep on this I think.  
Whoo boy. This took a lot of effort. I dabbled with DFS, BFS, Dijkstra's and everything inbetween.  
I had to employ Chat GPT because I was going mad at some point. Turns out I forgot to account for the S and E tiles so this time i had an off-by-two-error instead of the usual off-by-one. Almost gave up again... Feels like I'm getting close to the edge of my capabilities even with "cheating" with Chat GPT and not building every algorithm myself - which I usually aim for.  


## --- Day 17: Chronospatial Computer ---  

**Part 1**  
Yay! I love these kinds of challenges. At least for now when I've only seen Part 1. :D  
_Build a "Three bit computer" and run this program._  
No problem. It's fun implementing these, the TDD helps a lot as always - and this is very well defined as far as expected outcomes.  

