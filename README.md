# GitLings

Is an exercise platform for learning to solve some of gits more difficult problems. I goes from easy to hard, however, it is expected that you have some knowledge of git, and is willing to search yourself on how to solve your issues.

To begin each exercise, use the included tool `GitLings`.

## Installation 

To begin either use the tool for your operation system in `bin`

or build it yourself from the src folder. Included is some cake scripts, which can be used to build for linux and windows. It should however, be quite easy to add support for other oses, and architectures as well.

```bash
dotnet tool restore

dotnet cake

./bin/linux-x86/GitLings exercises list # Profit
```

## Usage

To start each exercise use the included tool.

```bash
./bin/linux-x86/GitLings exercises start 1
```

To receive hints

```bash
./bin/linux-x86/GitLings exercises hints 1

# If more than one hint
./bin/linux-x86/GitLings exercises hints 1 2
```

For the solution

```bash
./bin/linux-x86/GitLings exercises solution 1
```



