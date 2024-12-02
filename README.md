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