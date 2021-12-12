using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text;

namespace Pirexcs
{
    public class UserIC
    {
        private static List<string> color = new List<string>(){
            "d12215","d1af15","0fba48","0f7eba","700fba","ba0f6d"
        };
        private static List<char> symbol = new List<char>(){
            '.','_'
        };
        public static string CreateAvatar(string name){
            Random rand = new Random();
            return $"https://ui-avatars.com/api/?&length=1&background={color[rand.Next(0,6)]}&color=fff&bold=true&size=500&name={name}";
        }
        public static string CreateUsername(string name) {
            Random rand = new Random();
            string nameToUnSign = ConvertToUnSign(name).ToLower();
            var nameSplit = nameToUnSign.Split(" ");
            string username = nameSplit[0] + symbol[rand.Next(0,2)];
            string lastName = nameSplit[nameSplit.Length-1];
            int i = rand.Next(0,lastName.Length);
            for (int j=0; j<i; j++) username += lastName[j];
            username += rand.Next(0,9999).ToString();
            return username;
        }
        public static string ConvertToUnSign(string name){
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = name.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}