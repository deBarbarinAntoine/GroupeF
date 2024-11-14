namespace YellowDirectory.Models;

using System;
using System.IO;

/// <summary>
/// DotEnv provides a static method to load environment variables from a .env file.
/// </summary>
public static class DotEnv
{
    /// <summary>
    /// Loads all variables in a .env file into the environment variables of the application.
    /// </summary>
    /// <param name="filePath">the path to the .env file</param>
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}