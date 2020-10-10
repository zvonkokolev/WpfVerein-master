using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfVerein.Core.Entities;
using WpfVerein.CRUD;
using WpfVerein.Model;
using WpfVerein.Persistence;
using WpfVerein.Utils;

namespace WpfVerein
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<Member> _members;
		private UnitOfWork db = new UnitOfWork();
		private CsvWriter _csvWriter;
		private readonly string path = MyString.GetFullNameInApplicationTree("output.csv");

		public MainWindow()
		{
			InitializeComponent();
			Loaded += new RoutedEventHandler(MainWindow_Loaded);
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			{
				if (db == null)
				{
					PersonRepository.LoadCdsFromCsv();
				}

				_members = db.PersonRepository.GetAllMembers();
				lbxCds.ItemsSource = _members;
				// prints members data to csv file
				_csvWriter = new CsvWriter(path, ";");
				for (int i = 0; i < _members.Count; i++)
				{
					_csvWriter.Write
						 (
						 _members.ElementAt(i).Name,
						 _members.ElementAt(i).Email,
						 _members.ElementAt(i).Phone,
						 _members.ElementAt(i).BirthDay.ToShortDateString(),
						 _members.ElementAt(i).ActualDateTime.ToString()
						 );
				}
				// Read and save into database
				db.PersonRepository.AttachMembersRange(_members);
				db.SaveChanges();
				lbxCds.Items.Refresh();
			}
		}

		private void BtnMainWindow_Clicked(object sender, RoutedEventArgs e)
		{  // saves data into csv file
			_csvWriter.Flush();
			Button button = sender as Button;

			Member selectedCd = (Member)lbxCds.SelectedItem;
			// new member
			if (button.Name.Equals("btnNew"))
			{
				AddMemberWindow addMemberWindow = new AddMemberWindow();
				addMemberWindow.ShowDialog();
			}
			else
			{
				// Database---
				if (selectedCd == null)
				{
					MessageBox.Show("Wählen Sie einen Mitglied!");
				}
				else
				{              // delete member
					if (button.Name.Equals("btnDel"))
					{
						// Delete--------------------------
						MessageBox.Show("Lösche Mitglied aus der Datenbank");
						db.PersonRepository.RemoveCd(selectedCd);
						db.SaveChanges();
					}           // edit member
					else if (button.Name.Equals("btnEdit"))
					{
						AddMemberWindow addMemberWindow = new AddMemberWindow(selectedCd);
						addMemberWindow.ShowDialog();
					}
				}
			}
			_members = db.PersonRepository.GetAllMembers();
			lbxCds.ItemsSource = _members;
			lbxCds.Items.Refresh();
		}
	}
}
