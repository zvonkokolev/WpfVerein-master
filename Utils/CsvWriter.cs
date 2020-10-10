using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WpfVerein.Utils
{
	public class CsvWriter
	{
		/// <summary>
		/// Gets or sets the outfile.
		/// </summary>
		/// <value>The file to write.</value>
		public string Outfile { get; set; }
		/// <summary>
		/// Gets or sets the delimiter.
		/// </summary>
		/// <value>The delimiter.</value>
		/// <remarks>Standard: ;</remarks>
		public string Delimiter { get; set; }
		/// <summary>
		/// Gets or sets the write lines to file.
		/// </summary>
		/// <value>The write lines to file.</value>
		/// <remarks>Every X files are written to the file.
		///          Standard: 100 lines
		/// </remarks>
		public int WriteLinesToFile { get; set; }
		/// <summary>
		/// Gets or sets the cache.
		/// </summary>
		/// <value>The cache.</value>
		private List<string> Cache { get; set; }
		/// <summary>
		/// Gets or sets the line counter.
		/// </summary>
		/// <value>The line counter.</value>
		/// <remarks>Implemented for performance reason (cheaper to increase a counter than to count all elements of a list)</remarks>
		private int LineCounter { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="CSVWriter"/> class.
		/// </summary>
		/// <param name="Outfile">The outfile.</param>
		public CsvWriter(string Outfile)
		{
			this.Outfile = Outfile;
			this.Delimiter = ";";
			this.WriteLinesToFile = 100;
			Cache = new List<string>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CSVWriter"/> class.
		/// </summary>
		/// <param name="Outfile">The outfile.</param>
		/// <param name="Delimiter">The delimiter.</param>
		public CsvWriter(string Outfile, string Delimiter)
		{
			this.Outfile = Outfile;
			this.Delimiter = Delimiter;
			this.WriteLinesToFile = 100;
			Cache = new List<string>();
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void Write(params object[] value)
		{
			Cache.Add(ConvertToCSVString(value));
		}

		/// <summary>
		/// Writes the line.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteLine(params object[] value)
		{
			Cache.Add(ConvertToCSVString(value));
			LineCounter++;
			if (LineCounter == WriteLinesToFile)
				SaveFile();
		}

		/// <summary>
		/// Saves the file.
		/// </summary>
		private void SaveFile()
		{
			StreamWriter writer = new StreamWriter(Outfile, true);
			for (int i = 0; i < WriteLinesToFile; i++)
			{
				writer.WriteLine(Cache[i]);
			}
			writer.Close();
			Cache.Clear();
		}

		/// <summary>
		/// Converts to CSV string.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private string ConvertToCSVString(params object[] value)
		{
			// StringBuilder implementation
			// faster string concatenation than using "+" etc.
			StringBuilder StringToWrite = new StringBuilder();
			// for-loops are _very_ cheaper (=faster) than foreach loops
			for (int i = 0; i < value.Length; i++)
			{
				StringToWrite.Append(Convert.ToString(value[i]) + Delimiter);
			}
			string result = StringToWrite.ToString();
			// Cut the last added delimiter
			if (result.EndsWith(Delimiter))
				result = result.TrimEnd(Delimiter.ToCharArray());
			return result;
		}

		/// <summary>
		/// Flushes this instance.
		/// </summary>
		/// <remarks>Writes the complete cache to the file</remarks>
		public void Flush()
		{
			int BackupWriteLinesToFile = WriteLinesToFile;
			WriteLinesToFile = Cache.Count;
			SaveFile();
			WriteLinesToFile = BackupWriteLinesToFile;
		}
	}
}
