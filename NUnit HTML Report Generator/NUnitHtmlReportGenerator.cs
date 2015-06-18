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
//
// Converted into a msbuild task by
// Thomas la Cour
// https://github.com/Hyrtwol
// </summary>
#endregion

using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Jatech.NUnit
{
    /// <summary>
    /// The Task.
    /// </summary>
    public class NUnitHtmlReportGenerator : Task
    {
        #region Properties

        [Required]
        public string InputFileName { get; set; }

        public string OutputFileName { get; set; }

        public bool OverwriteOutput { get; set; }

        #endregion

        #region Task Overrides

        public override bool Execute()
        {
            try
            {
                ExecuteTask();
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, true);
            }
            return !Log.HasLoggedErrors;
        }

        private void ExecuteTask()
        {
            string input = InputFileName, output = OutputFileName;

            if (string.IsNullOrEmpty(input))
            {
                Log.LogError("Missing input file");
                return;
            }

            if (string.IsNullOrEmpty(output))
            {
                output = Path.ChangeExtension(input, "html");
            }

            // Check input file exists and output file doesn't
            bool ok = CheckInputAndOutputFile(input, output);

            // If input file exists and output doesn't exist
            if (ok)
            {
                Log.LogMessage(MessageImportance.High, "Generating {0} from {1}", output, input);
                // Generate the HTML page
                var generator = new ReportGenerator(input, output, LogMessage);
                generator.Generate();
            }
        }

        private void LogMessage(string message, object[] messageArgs)
        {
            Log.LogMessage(MessageImportance.High ,message, messageArgs);
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
        private bool CheckInputAndOutputFile(string input, string output)
        {
            bool ok = false;

            if (File.Exists(input))
            {
                if (OverwriteOutput)
                {
                    ok = true;
                }
                else
                {
                    if (!File.Exists(output))
                    {
                        ok = true;
                    }
                    else
                    {
                        Log.LogError("Output file '{0}' already exists", output);
                    }
                }
            }
            else
            {
                Log.LogError("File does not exist");
            }

            return ok;
        }

        #endregion

        #endregion
    }
}
