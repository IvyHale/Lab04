using Lab04.Exceptions;
using System;
using System.Linq;

namespace Lab04.Model
{
    [Serializable]
    class Person
    {
        internal const string filename = "People.dat";
        private string _name, _surname, _email;
        private DateTime _birthDate;

        internal Person(string name, string surname, string email, DateTime birthDate)
        {
            Name = name;
            Surname = surname;
            Email = email;
            BirthDate = birthDate;
        }

        internal Person(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }

        internal Person(string name, string surname, DateTime birthDate)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                if (value.Any(c => char.IsDigit(c)) || !char.IsUpper(value[0]))
                    throw new WrongNameException($"{value} is a wrong name format (must start with capital letter and must not contain digits).");
                _name = value;
            }
        }
        public string Surname
        {
            get { return _surname; }
            private set
            {
                if (value.Any(c => char.IsDigit(c)) || !char.IsUpper(value[0]))
                    throw new WrongNameException($"{value} is a wrong name format (must start with capital letter and must not contain digits).");
                _surname = value;
            }
        }
        public string Email
        {
            get { return _email; }
            private set
            {
                if (!(value.Contains("@") && value.Contains(".")) || value.Length < 5)
                    throw new EmailException($"{value} is a wrong e-mail format (must contain '@', '.' and at least 5 symbols in general).");
                _email = value;
            }
        }
        public DateTime BirthDate
        {
            get { return _birthDate; }
            private set
            {
                if (DateTime.Now.Year - value.Year > 135)
                    throw new PastException($"{value} is not a valid birth date; the person is too old to be alive.");
                if (value > DateTime.Now)
                    throw new FutureException($"{value} is not a valid birth date; the person hasn't been born yet.");
                _birthDate = value;
            }
        }
        public bool IsAdult
        {
            get
            {
                DateTime now = DateTime.Now;
                int age = now.Year - _birthDate.Year;
                if (now.Month < _birthDate.Month || (now.Month == _birthDate.Month && now.Day < _birthDate.Day))
                    age--;
                return age >= 18;
            }
        }
        public string SunSign
        {
            get
            {
                int month = this._birthDate.Month, day = this._birthDate.Day;
                string sunSign = "unknown";
                switch (month)
                {
                    case 1:
                        if (day <= 20)
                            sunSign = "Capricorn";
                        else
                            sunSign = "Aquarius";
                        break;
                    case 2:
                        if (day <= 19)
                            sunSign = "Aquarius";
                        else
                            sunSign = "Pisces";
                        break;
                    case 3:
                        if (day <= 20)
                            sunSign = "Pisces";
                        else
                            sunSign = "Aries";
                        break;
                    case 4:
                        if (day <= 20)
                            sunSign = "Aries";
                        else
                            sunSign = "Taurus";
                        break;
                    case 5:
                        if (day <= 21)
                            sunSign = "Taurus";
                        else
                            sunSign = "Gemini";
                        break;
                    case 6:
                        if (day <= 22)
                            sunSign = "Gemini";
                        else
                            sunSign = "Cancer";
                        break;
                    case 7:
                        if (day <= 22)
                            sunSign = "Cancer";
                        else
                            sunSign = "Leo";
                        break;
                    case 8:
                        if (day <= 23)
                            sunSign = "Leo";
                        else
                            sunSign = "Virgo";
                        break;
                    case 9:
                        if (day <= 23)
                            sunSign = "Virgo";
                        else
                            sunSign = "Libra";
                        break;
                    case 10:
                        if (day <= 23)
                            sunSign = "Libra";
                        else
                            sunSign = "Scorpio";
                        break;
                    case 11:
                        if (day <= 22)
                            sunSign = "Scorpio";
                        else
                            sunSign = "Sagittarius";
                        break;
                    case 12:
                        if (day <= 21)
                            sunSign = "Sagittarius";
                        else
                            sunSign = "Capricorn";
                        break;
                    default:
                        break;
                }
                return sunSign;
            }
        }

        public string ChineseSign
        {
            get
            {
                var c = new System.Globalization.ChineseLunisolarCalendar();
                var y = c.GetSexagenaryYear(_birthDate);
                var s = c.GetCelestialStem(y) - 1;
                return
                  ",Rat,Ox,Tiger,Rabbit,Dragon,Snake,Horse,Goat,Monkey,Rooster,Dog,Pig".Split(',')[c.GetTerrestrialBranch(y)]
                  + " - "
                  + "Wood,Fire,Earth,Metal,Water".Split(',')[s / 2]
                  + " - Y" + (s % 2 > 0 ? "in" : "ang");
            }
        }

        public bool IsBirthday
        {
            get
            {
                return (DateTime.Now.Day == _birthDate.Day && DateTime.Now.Month == _birthDate.Month);
            }
        }
    }
}
