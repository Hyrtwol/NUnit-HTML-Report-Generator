#region File Header
// <copyright application="NUnit HTML Report Generator" file="Program.cs" company="Jatech Limited">
// Copyright (c) 2014 Jatech Limited. All rights reserved.
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// <author>luke-browning</author>
// <date>26/06/2014</date>
// <summary>
// Console application to convert NUnit XML results file to
// a standalone HTML page based on Bootstrap 3
// </summary>
#endregion

using System;
using System.Collections.Generic;
using System.IO;

namespace Jatech.NUnit
{
    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        #region Private Constants

        /// <summary>
        /// Usage example.
        /// </summary>
        private const string Usage = "Usage: NUnitHTMLReportGenerator.exe [input-path] [output-path]";

        /// <summary>
        /// Switches for displaying the basic help when executed.
        /// </summary>
        private static readonly List<string> HelpParameters = new List<string> { "?", "/?", "help" };

        #endregion

        #region Main

        /// <summary>
        /// Main entry-point for this application.
        /// </summary>
        /// <param name="args">Array of command-line argument strings.</param>
        static void Main(string[] args)
        {
            bool ok = false;
            string input = string.Empty, output = string.Empty;

            if (args.Length == 1)
            {
                input = args[0];

                // Check if the user wants help, otherwise assume its a
                // filename that needs to be processed
                if (HelpParameters.Contains(input))
                {
                    Console.WriteLine(Usage);
                }
                else
                {
                    // Output file with the same name in the same folder
                    // with a html extension
                    output = Path.ChangeExtension(input, "html");

                    // Check input file exists and output file doesn't
                    ok = CheckInputAndOutputFile(input, output);
                }
            }
            else if (args.Length == 2)
            {
                // If two parameters are passed, assume the first is 
                // the input path and the second the output path
                input = args[0];
                output = args[1];

                // Check input file exists and output file doesn't
                ok = CheckInputAndOutputFile(input, output);
            }
            else
            {
                // Display the usage message
                Console.WriteLine(Usage);
            }

            // If input file exists and output doesn't exist
            if (ok)
            {
                var generator = new ReportGenerator(input, output, Console.WriteLine);
                generator.Generate();
            }
        }

        #endregion

        #region Private Methods

        #region File Access

        /// <summary>
        /// Check input and output file existence
        /// Input file should exist, output file should not
        /// </summary>
        /// <param name="input">The input file name</param>
        /// <param name="output">The output name</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        private static bool CheckInputAndOutputFile(string input, string output)
        {
            bool ok = false;

            if (File.Exists(input))
            {
                if (!File.Exists(output))
                {
                    ok = true;
                }
                else
                {
                    Console.WriteLine(string.Format("Output file '{0}' already exists", output));
                }
            }
            else
            {
                Console.WriteLine("File does not exist");
            }

            return ok;
        }

        #endregion

        #endregion
    }
}
