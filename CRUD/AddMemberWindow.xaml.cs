using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfVerein.Core.Entities;
using WpfVerein.Persistence;
using WpfVerein.Utils;

namespace WpfVerein.CRUD
{
	/// <summary>
	/// Interaktionslogik für AddMemberWindow.xaml
	/// </summary>
	public partial class AddMemberWindow : Window
	{
		private CsvWriter _csvWriter;
		private string path = MyString.GetFullNameInApplicationTree("output.csv");
		private Member newCd;
		private static int _indexer = 14;
		private readonly Member _memberToEdit;
		private List<string> _line;
		private UnitOfWork db = new UnitOfWork();

		public AddMemberWindow(Member memberToEdit)
		{
			_memberToEdit = memberToEdit;
			InitializeComponent();
		}
		public AddMemberWindow()
		{
			_indexer++;
			InitializeComponent();
		}
		private void AddMemberWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (_memberToEdit != null)
			{
				//tbIndex.Text = _indexer.ToString();
				tbFirstname.Text = _memberToEdit.Firstname;
				tbLastname.Text = _memberToEdit.Lastname;
				tbEmail.Text = _memberToEdit.Email;
				tbPhone.Text = _memberToEdit.Phone;
				tbBirthday.Text = _memberToEdit.BirthDay.ToString();
			}
			newCd = new Member
			{
				Firstname = tbFirstname.Text,
				Lastname = tbLastname.Text,
				Email = tbEmail.Text,
				Phone = tbPhone.Text,
				ActualDateTime = DateTime.UtcNow,
				BirthDay = DateTime.UtcNow
			};

			grdCdField.DataContext = newCd;
		}

		private void BtnSave_Clicked(object sender, RoutedEventArgs e)
		{
			if (_memberToEdit == null)
			{
				db.PersonRepository.AddMemberAsync(newCd);

				_line = new List<string>
				{
					newCd.Name,
					newCd.Email,
					newCd.Phone
				};
				//CSV file---------------------------------------
				// if member is new one, will be appended into csv file
				_csvWriter = new CsvWriter(path, ";");
				_csvWriter.Write(_line.ElementAt(0), _line.ElementAt(1), _line.ElementAt(2));
				_csvWriter.Flush();

				// Database--------------------------------------
				// Create
				MessageBox.Show("Ein neues Mitglied hinzugefügt");
				db.PersonRepository.AddMember(newCd);
				db.SaveChanges();
			}
			else
			{  // Database-----------------------------------
				// Edit
				_memberToEdit.Firstname = newCd.Firstname;
				_memberToEdit.Lastname = newCd.Lastname;
				_memberToEdit.Email = newCd.Email;
				_memberToEdit.Phone = newCd.Phone;

				db.PersonRepository.UpdateCd(_memberToEdit);

				MessageBox.Show("Mitglied aktualisieren");
				db.SaveChanges();
			}
			Close();
		}

		private void BtnCancel_Clicked(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
