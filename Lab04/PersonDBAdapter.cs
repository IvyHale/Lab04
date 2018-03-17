using Lab04.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Lab04
{
    static class PersonDBAdapter
    {
        public static List<Person> Persons { get; }
        static PersonDBAdapter()
        {
            var filepath = Path.Combine(GetAndCreateDataPath(), Person.filename);
            if (File.Exists(filepath))
            {
                try
                {
                    Persons = SerializeHelper.Deserialize<List<Person>>(filepath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to get people from file.{Environment.NewLine}{ex.Message}");
                    throw;
                }
            }
            else
            {
                Persons = new List<Person>();

                Random random = new Random();
                string charsForName = "abcdefghijklmnopqrstuvwxyz";
                string charsForEmail = charsForName + "0123456789";

                for (int i = 0; i < 50; i++)
                {
                    int length = random.Next(3, 21);
                    string name = new string(Enumerable.Repeat(charsForName, length).Select(s => s[random.Next(s.Length)]).ToArray());
                    name = char.ToUpper(name[0]) + name.Substring(1);
                    length = random.Next(3, 21);
                    string surname = new string(Enumerable.Repeat(charsForName, length).Select(s => s[random.Next(s.Length)]).ToArray());
                    surname = char.ToUpper(surname[0]) + surname.Substring(1);
                    length = random.Next(1, 11);
                    string email = new string(Enumerable.Repeat(charsForEmail, length).Select(s => s[random.Next(s.Length)]).ToArray()) + "@"
                        + new string(Enumerable.Repeat(charsForName, random.Next(1, 6)).Select(s => s[random.Next(s.Length)]).ToArray()) + "."
                        + new string(Enumerable.Repeat(charsForName, random.Next(1, 6)).Select(s => s[random.Next(s.Length)]).ToArray());
                    DateTime bd = new DateTime(random.Next(1920, 2018), random.Next(1, 13), random.Next(1, 29));
                    try
                    {
                        CreatePerson(name, surname, email, bd);
                    }
                    catch (Exception e) { --i; }
                }
            }
        }

        internal static Person GetPerson(string email)
        {
            return Persons.FirstOrDefault(person => person.Email == email);
        }
        internal static bool RemovePerson(string email)
        {
            return Persons.Remove(GetPerson(email));
        }
        internal static Person CreatePerson(string name, string surname, string email, DateTime dateOfBirth)
        {
            if (Persons.Any(ps => ps.Email == email))
                throw new Exception($"User with email {email} already exists");
            Person person = new Person(name, surname, email, dateOfBirth);
            Persons.Add(person);
            return person;
        }

        internal static void SaveData()
        {
            SerializeHelper.Serialize(Persons, Path.Combine(GetAndCreateDataPath(), Person.filename));
        }

        private static string GetAndCreateDataPath()
        {
            string dir = StationManager.WorkingDirectory;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
    }
}
