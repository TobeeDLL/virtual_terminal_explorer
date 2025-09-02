using System;
using System.Collections.Generic;
using System.Linq;

class File
{
    public string Content = "";
    public string Name = "";
}

class FileEditor
{
    public static void Read(string path)
    {
        string extraPath = "";
        string name = path;

        if (path.Contains("/")) {
            var parts = path.Split("/").ToList();
            parts.RemoveAt(parts.Count - 1);
            extraPath = "/" + string.Join("/", parts);

            string[] lastname = path.Split("/");
            name = lastname[lastname.Length - 1];
        }

        var selectedFile = Manager.Directories[PC.LinerPath() + extraPath].FirstOrDefault(file => file.Name == name);

        Console.WriteLine(selectedFile.Content);
    }

    public static void Write(string path, string data)
    {
        string extraPath = "";
        string name = path;
        
        if (!Manager.Directories[PC.LinerPath() + extraPath].Any(f => f.Name == name)) {
            Console.WriteLine($"Invalid Parameter: File '{name}' doesn't exist!");
            return;
        }

        if (path.Contains("/")) {
            var parts = path.Split("/").ToList();
            parts.RemoveAt(parts.Count - 1);
            extraPath = "/" + string.Join("/", parts);

            string[] lastname = path.Split("/");
            name = lastname[lastname.Length - 1];
        }

        var selectedFile = Manager.Directories[PC.LinerPath() + extraPath].FirstOrDefault(file => file.Name == name);

        selectedFile.Content += data;
    }

    public static void WriteLine(string path, string data)
    {
        string extraPath = "";
        string name = path;

        if (!Manager.Directories[PC.LinerPath() + extraPath].Any(f => f.Name == name)) {
            Console.WriteLine($"Invalid Parameter: File '{name}' doesn't exist!");
            return;
        }

        if (path.Contains("/")) {
            var parts = path.Split("/").ToList();
            parts.RemoveAt(parts.Count - 1);
            extraPath = "/" + string.Join("/", parts);

            string[] lastname = path.Split("/");
            name = lastname[lastname.Length - 1];
        }

        var selectedFile = Manager.Directories[PC.LinerPath() + extraPath].FirstOrDefault(file => file.Name == name);
        string addedValue = "\n";
        
        if (selectedFile.Content.EndsWith("\n")) {addedValue = "";}

        selectedFile.Content = addedValue + selectedFile.Content + data;
    }

    public static void Delete(string path, int amount, bool fromTop = false)
    {
        string extraPath = "";
        string name = path;

        if (path.Contains("/")) {
            var parts = path.Split("/").ToList();
            parts.RemoveAt(parts.Count - 1);
            extraPath = "/" + string.Join("/", parts);

            string[] lastname = path.Split("/");
            name = lastname[lastname.Length - 1];
        }

        var selectedFile = Manager.Directories[PC.LinerPath() + extraPath].FirstOrDefault(file => file.Name == name);

        if (amount > selectedFile.Content.Length) {
            Console.WriteLine($"Invalid Parameter: <amount> larger that file length ({selectedFile.Content.Length})");
            return;
        }

        if (fromTop) {selectedFile.Content = selectedFile.Content.Substring(amount); return;}

        selectedFile.Content = selectedFile.Content.Substring(0, selectedFile.Content.Length - amount);
    }
}

class Manager
{
    public static Dictionary<string, List<File>> Directories = new Dictionary<string, List<File>> {
        {"PC", new List<File>()}
    };

    public static void CreateFolder(string name)
    {
        if (Directories.Keys.Contains(PC.LinerPath() + "/" + name)) {
            Console.WriteLine($"Invalid Parameter: Folder '{name}' already exists!");
            return;
        }
        
        name = string.Join("/", PC.RemoveSpaces(name.Split("/")));

        Directories.Add(PC.LinerPath() + "/" + name, new List<File>());
    }

    public static void CreateFile(string name)
    {
        string extraPath = "";

        if (name.Contains("/")) {
            var parts = name.Split("/").ToList();
            parts.RemoveAt(parts.Count - 1);
            extraPath = "/" + string.Join("/", parts);

            string[] lastname = name.Split("/");
            name = lastname[lastname.Length - 1];
        }

        if (Directories[PC.LinerPath() + extraPath].Any(f => f.Name == name)) {
            Console.WriteLine($"Invalid Parameter: File '{name}' already exists!");
            return;
        }

        Directories[PC.LinerPath() + extraPath].Add(new File{
            Content = "",
            Name = name
        });
    }

    public static void DeleteFolder(string path)
    {
        if (!string.IsNullOrWhiteSpace(path)) {path = PC.LinerPath() + "/" + path;} else {
            if (PC.LinerPath() != "PC")
            {
                path = PC.LinerPath();
            }
            else
            {
                Console.WriteLine("Invalid Parameter: Cannot delete 'PC' directory");
                return;
            }
        }

        foreach (string directory in Directories.Keys)
        {
            if (directory.StartsWith(PC.LinerPath() + "/" + path)) {Directories.Remove(directory);}
        }

        Directories.Remove(path);
    }

    public static void DeleteFile(string name)
    {
        Directories[PC.LinerPath()].RemoveAll(file => file.Name == name);
    }
}

class PC
{
    public static List<string> Paths = new List<string> {"PC"};
    static bool Error_at_CD = false;

    public static string[] RemoveSpaces(string[] array)
    {
        List<string> result = new();
        
        foreach (var element in array)
        {
            if (!string.IsNullOrWhiteSpace(element)) {result.Add(element);}
        }

        return result.ToArray();
    }

    public static void CD(string Destination)
    {
        if (Destination == LinerPath() || string.IsNullOrWhiteSpace(Destination)) {return;}
        if (!Manager.Directories.Keys.Contains(LinerPath() + "/" + Destination))
        {
            Console.WriteLine("Invalid Parameter: Destination doesn't exist");
            Error_at_CD = true;
            return;
        }

        Paths.Add(Destination);
    }

    public static void GoTo(string Destination)
    {
        CD(Destination);
        if (Error_at_CD) {Error_at_CD = false; return;}
        Show(" ");
    }

    public static void GoBack()
    {
        if (Paths.Count == 1) {return;}
        Paths.RemoveAt(Paths.Count - 1);
    }

    public static void Show(string Path)
    {
        if (!Manager.Directories.Keys.Contains(LinerPath() + "/" + Path) && Path != " ")
        {
            Console.WriteLine("Invalid Parameter: Destination doesn't exist");
            return;
        }

        if (Path == " ") {Path = LinerPath();} else {Path = LinerPath() + "/" + Path;}
        
        string CDParam = "";

        if (LinerPath() == "PC") { CDParam = "PC"; } else {CDParam = Path.Remove(0, LinerPath().Length);}
        CD(CDParam);

        string previousFolder = "";
        foreach (var folder in Manager.Directories.Keys)
        {
            if (folder.StartsWith(LinerPath() + "/"))
            {
                var folderName = folder.Remove(0, LinerPath().Length + 1).Split("/")[0];

                if (folderName == previousFolder) {continue;} else {previousFolder = folderName;}
                
                if (!string.IsNullOrWhiteSpace(folderName)){
                    Console.WriteLine("🗀  " + folderName);
                }
            }
        }
        
        foreach (var child in Manager.Directories[Path])
        {
            Console.WriteLine("🗋 " + child.Name);
        }
    }

    public static string LinerPath()
    {
        return string.Join("/", Paths);
    }

    public static void Process(string[] command)
    {
        if (string.IsNullOrWhiteSpace(string.Join("", command))) {return;}
            
        switch (command[0])
        {
            case "cd":
                if (command.Length < 2) {Console.WriteLine("Missing Parameter: <destination>"); break;}
                PC.CD(command[1]);
                break;

            case "goto":
                if (command.Length < 2) {Console.WriteLine("Missing Parameter: <destination>"); break;}
                PC.GoTo(command[1]);
                break;

            case "goback":
                PC.GoBack();
                break;

            case "show":
                if (command.Length < 2) {PC.Show(" "); break;}
                PC.Show(command[1]);
                break;

            case "folder":
                if (command.Length < 2) {Console.WriteLine("Missing Parameter: <name/path+name>"); break;}
                Manager.CreateFolder(command[1]);
                break;

            case "file":
                if (command.Length < 2) {Console.WriteLine("Missing Parameter: <name/path+name>"); break;}
                Manager.CreateFile(command[1]);
                break;

            case "delf":
                if (command.Length < 2) {Manager.DeleteFolder(" "); break;}
                Manager.DeleteFolder(command[1]);
                break;

            case "del":
                if (command.Length < 2) {Manager.DeleteFile(" "); break;}
                Manager.DeleteFile(command[1]);
                break;

            case "read":
                if (command.Length < 2) {Console.WriteLine("Missing Parameter: <file-name>"); break;}
                FileEditor.Read(command[1]);
                break;

            case "write":
                if (command.Length < 3) {
                    Console.WriteLine("Missing Parameter: <file-name>");
                    if (command.Length < 2) {
                        Console.WriteLine("Missing Parameters: <value> <file-name>");
                    }
                    break;
                }
                
                FileEditor.Write(command[1], command[2]);
                break;

            case "writel":
                if (command.Length < 2) {
                    if (command.Length < 3) {Console.WriteLine("Missing Parameter: <file-name>");} else
                    {Console.WriteLine("Missing Parameter: <value>");}
                    break;
                }
                FileEditor.WriteLine(command[1], command[2]);
                break;

            case "delete":
                bool fromTop = false;
                
                if (command.Length < 2) {Console.WriteLine("Missing Parameters: <file-name> <amount> <fromTop (optional)>"); break;}
                if (command.Length == 2) {Console.WriteLine("Missing Parameters: <amount> <fromTop (optional)>"); break;}
                if (!command[2].All(char.IsDigit)) {
                    Console.WriteLine("Invalid Parameter: <amount> must be a number!");
                    break;
                }
                if (command.Length >= 4 && new[] {"true", "false"}.Contains(command[3].ToLower())) {fromTop = bool.Parse(command[3].ToLower());}

                FileEditor.Delete(command[1], int.Parse(command[2]), fromTop);
                
                break;

            case "help":
                Program.Help();
                break;

            case "clear":
                Console.Clear();
                break;

            default:
                if (!(string.IsNullOrWhiteSpace(command[0]) || command[0] == "quit"))
                {
                    Console.WriteLine("Unkown command: " + command[0]);
                }
                break;
        }   
    }     
}

class Program
{
    public static void Help()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("Commands:\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n cd <destination>");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("change current directory");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n goto <destination>");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("show folder contents, and set it as current directory");
    
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n goback");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("goto the last path you visited");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n show <destination>");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("show folder contents");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n folder / file <name / path + name>");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("create a new folder / file");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n delf / del <file-name>");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("delete a folder (delf) / file (del)");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n read / write / writel <file-name> <value>");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("edit file contents");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n delete <file-name> <amount> <fromTop: true/false>");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("delete <amount> chars from the end/top of file");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n clear");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("clears terminal");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n help");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" => ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("shows all commands");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n quit");

        Console.ResetColor();

        Console.Write("\n\n");
    }

    public static void Main()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("Welcome to ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Virtual Box");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("!\n\n");

        Help();

        string input = "";

        while (input != "quit")
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(PC.LinerPath());

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" -> ");

            Console.ResetColor();
            input = Console.ReadLine().Trim();
            string[] command = PC.RemoveSpaces(input.Split(" "));

            PC.Process(command);
        }
    }
}
