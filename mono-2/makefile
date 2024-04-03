all: execute

source_three.dll: source_three.cs
	mcs source_three.cs -t:library

source_one.exe: source_one.cs source_three.dll
	mcs source_one.cs

clean:
	rm *.exe *.dll

execute: source_one.exe
	mono source_one.exe
