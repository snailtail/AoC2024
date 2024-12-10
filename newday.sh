#!/bin/sh

git checkout main
git pull -r
git checkout -b day$1



cd tests
dotnet new xunit -o $1-test
cd $1-test
# Add Build.targets file ref to csproj file
csproj_file="$1-test.csproj"
import_line='  <Import Project="../../Build.targets" />'
sed -i '' "/<\/Project>/i\\
$import_line
" "$csproj_file"
cd ..
cd ..

# Add to the solution
dotnet sln add tests/$1-test/$1-test.csproj


# Get the data for this day, and generate a file for testinput
python3 ./getinput.py $1
touch ./tests/$1-test/$1test.dat

git add .
#git commit -m "Add new day $1"
#git push --set-upstream origin day$1