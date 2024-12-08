#!/bin/sh

git checkout main
git pull -r
git checkout -b day$1


# Create the project and a unit test
dotnet new console -o $1
cd $1

# Add Build.targets file ref to csproj file
csproj_file="$1.csproj"
import_line='  <Import Project="../Build.targets" />'
sed -i '' "/<\/Project>/i\\
$import_line
" "$csproj_file"
cd ..

cd tests
dotnet new xunit -o $1-test
cd ..
# Add to the solution
dotnet sln add $1/$1.csproj
dotnet sln add tests/$1-test/$1-test.csproj

# Go down into the test project and add shouldly and a reference to the days project
cd tests/$1-test
dotnet add package shouldly
dotnet add reference ../../$1/$1.csproj
cd ..
cd ..

# Get the data for this day, and generate a file for testinput
python3 ./getinput.py $1
touch ./$1/$1test.dat

echo "# Advent of Code 2024 Day$1  " > ./$1/README.md
git add .
git commit -m "Add new day $1"
git push --set-upstream origin day$1