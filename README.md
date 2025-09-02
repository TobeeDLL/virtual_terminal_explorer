# virtual_terminal_explorer
A program i made for fun to simulate a terminal based file explorer in C#
It's not perfect, if there was any bugs (which will be) feel free to make a pull request!

# How to use
- Create a new C# project using:
```cmd
dotnet new console -n "VirtualExplorer"
```
- Copy contents of 'code.cs' into 'Program.cs"
- Run using:
```bash
cd "VirtualExplorer"
dotnet run
```
||(requires .NET SDK installed)||

# Commands
```bash
 cd <destination> => change current directory
 goto <destination> => show folder contents, and set it as current directory
 goback => goto the last path you visited
 show <destination> => show folder contents
 folder / file <name/path+name> => create a new folder / file
 delf / del <file-name> => delete a folder (delf) / file (del)
 read / write / writel <file-name> <value> => edit file contents
 delete <file-name> <amount> <fromTop: true/false> => delete <amount> chars from the end/top of file
 clear => clears terminal
 help => shows all commands
 quit
```
