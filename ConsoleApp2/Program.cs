using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
namespace MyShell
{
    class LitleShell
    {
        // Язык программирования: C#---------------------------------------------------------------------------------------------------------
        // Платформа - .Net Framework 4.6.1--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------Комманды-------------------------------------------------------------------
        // "/up" - Вверх по директории.------------------------------------------------------------------------------------------------------
        // "/down " - Вниз по директории в указанную папку. Пример использования: C:\Users\Uchiha Itachi\Desktop>/down cm-master-------------
        // "/cd " - смена директории. Путь должен быть полным. Пример использования: C:\Users>/cd C:\Users\Uchiha Itachi\Desktop\cm-master---
        // "/subdir" - Выводит в консоль все папки в данной папке, находящиеся в прямой доступности. Внутрь папок не заглядывает в поисках---
        // других папок. Пример использования: C:\Users>/subdir------------------------------------------------------------------------------
        // "/subfiles" - аналогично "/subdir", только кроме папок выводит ещё и файлы.-------------------------------------------------------
        // "/runexe " - запуск приложения (.exe).--------------------------------------------------------------------------------------------
        // Пример использования: C:\Users\Uchiha Itachi\Desktop>/runexe C:\Program Files (x86)\K-Lite Codec Pack\MPC-HC64\mpc-hc64_nvo.exe---
        // или C:\Program Files (x86)>/runexe K-Lite Codec Pack\MPC-HC64\mpc-hc64_nvo.exe----------------------------------------------------
        // "/cftxt " - Создание текстового файла. Для создания файла нужно находиться в директории, где планируется создать файл.------------
        // Пример использования: C:\Users\Uchiha Itachi\Desktop\cm-master/cftxt bongo--------------------------------------------------------
        // "/dftxt " - Удаление текстового файла. Для удаления файла нужно находиться в директории, где планируется создать файл.------------
        // Пример использования: C:\Users\Uchiha Itachi\Desktop\cm-master/вftxt bongo--------------------------------------------------------
        // "/wtxt " - Запись строки в текстовый файл. Для записи строки в файл нужно находиться в директории, где планируется запись---------
        // Пример использования: C:\Users\Uchiha Itachi\Desktop\cm-master/wtxt bongo---------------------------------------------------------
        // "/rtxt " - Чтение строки из текстовыого файла. Для чтения строки из файла нужно находиться в директории, откуда планируется чтение
        // Пример использования: C:\Users\Uchiha Itachi\Desktop\cm-master/rtxt bongo-------------------------------------------------------
        public static string[] commands = {"/up","/down ", "/cd ", "/cftxt ","/dftxt ","/runexe ","/subdir", "/subfiles", "/wtxt ", "/rtxt " };
        public static char[] c = Path.GetInvalidPathChars();
        public static char[] x = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public static char[] r = new[] { '/','*','?','«','<','>','|' };
        public string cpath;
        public string clitle_path;
        public string file_path;
        /*-------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------COMMANDS------------------------------------------------------*/
        /*-------------------------------------------------------------------------------------------------------------------*/
        public string Up(string s)
        {
            int x = 0;
            x = s.LastIndexOf('\\');
            if (x > 0)
            {
                string temp = s.Remove(x);
                return temp;
            }
            return s;
        }
        public string Down(string s, string a)
        {
            string direction = String.Concat(s, "\\", a);
            DirectoryInfo direct = new DirectoryInfo(direction);
            bool e1 = direct.Exists;
            if (e1 == true)
            {
                return direction;
            }
            else
            {
                Console.WriteLine("Wrong direction!");
            }
            return s;
        }
        public string Cd(string s)
        {
            string direction = s;
            int x = Check_direction(direction);
            if (x == -1)
            {
                Console.WriteLine("Wrong direction!");
                return cpath;
            }
            return s;
        }
        public void Sub_directories(string a)
        {
            string direction = a;
            DirectoryInfo direct = new DirectoryInfo(direction);
            DirectoryInfo[] subdir = direct.GetDirectories();
            foreach (DirectoryInfo dri in subdir)
                Console.WriteLine(dri.Name);
        }
        public void Sub_files(string a)
        {
            string direction = a;
            DirectoryInfo direct = new DirectoryInfo(direction);
            FileInfo[] subfiles = direct.GetFiles();
            foreach (FileInfo dri in subfiles)
                Console.WriteLine(dri.Name);
        }
        public void Run_exe()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = file_path;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
        }
        public void Create_text(string a, string b)
        {
            FileInfo newtxt = new FileInfo(String.Concat(a, "\\", b, ".txt"));
            FileStream fs = newtxt.Create();
            fs.Close();
        }
        public void Delete_text(string a, string b)
        {
            FileInfo newtxt = new FileInfo(String.Concat(a, "\\", b, ".txt"));
            newtxt.Delete();
        }
        public void Write_text(string a, string b)
        {

            Console.WriteLine("Write the string");
            string s = Console.ReadLine();
            StreamWriter sw = new StreamWriter(String.Concat(a, "\\", b, ".txt"));
            sw.Write(s);
            sw.Close();
        }
        public string Read_text(string a, string b)
        {
            StreamReader sr = new StreamReader(String.Concat(a, "\\", b, ".txt"));
            string s = sr.ReadLine();
            //Console.WriteLine("Это прочитанная строка = {0}", s);
            sr.Close();
            return s;
        }
        /*-------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------COMMANDS------------------------------------------------------*/
        /*-------------------------------------------------------------------------------------------------------------------*/
        public string Concat_path(string a)
        {
            int s, w, q, z, e = 0;
            if ((a[1] == ':') && (a[2] == '\\'))
            {
                q = Check_path(a, x, r, c);
                z = 1;
            }
            else
            {
                q = Check_clitle_path(a, r, c);
                z = 2;
            }
            if (z == 1)
            {
                w = Check_direction(a);
                if(w == -1)
                {
                    return cpath;
                }
                e = 1;
            }
            if (z == 2)
            {
                w = Check_direction(cpath, a);
                if (w == -1)
                {
                    return cpath;
                }
                e = 2;
            }
            if (e == 1)
            {
                cpath = Change_direction(a);
            }
            if (e == 2)
            {
                cpath = Change_direction(cpath, a);
            }
            return a;
        }
        public int Check_direction(string a, string b)
        {
            string direction = String.Concat(a,"\\",b);
            DirectoryInfo direct = new DirectoryInfo(direction);
            bool e1 = direct.Exists;
            if (e1 == true)
            {
                return 1;
            }
            else
            {
                Console.WriteLine("Wrong direction!");
                return -1;
            }
        }
        public int Check_direction(string a)
        {
            string direction = a;
            DirectoryInfo direct = new DirectoryInfo(direction);
            bool e1 = direct.Exists;
            if (e1 == true)
            {
                return 1;
            }
            else
            {
                Console.WriteLine("Wrong direction!");
                return -1;
            }
        }
        public string Change_direction(string a, string b)
        {
            string direction = String.Concat(a, "\\", b);
            return direction;
        }
        public string Change_direction(string a)
        {
            string direction = a;
            return direction;
        }
        public int Check_path(string s, char[] x, char[] y, char[] z)
        {
            int b1, b2, b3, b4, b5, b6, b7, k;
            b1 = 0; b2 = 0; b3 = 0; b4 = 0; b5 = 0; b6 = 0; b7 = 0; k = 0;
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j] == s[i])
                    {
                        b1 = 1;
                    }
                }
                for (int t = 0; t < r.Length; t++)
                {
                    if (r[t] == s[i])
                    {
                        b1 = 2;
                    }
                }
            }
            for (int j = 0; j < x.Length; j++)
            {
                if (x[j] != s[0])
                {
                    k++;
                }
            }
            if (k == 26) 
            {
                b2 = 1;
            }
            if (s[1] != ':')
            {
                b3 = 1;
            }
            if (s[2] != '\\')
            {
                b4 = 1;
            }
            if (s[s.Length - 1] == '\\')
            {
                b5 = 1;
            }
            bool b = s.Contains("\\\\");
            if (b == true)
            {
                b6 = 1;
            }
            for (int i = 3; i < s.Length; i++)
            {
                if (s[i] == ':')
                {
                    b7 = 1;
                }
            }
            if (b1 + b2 + b3 + b4 + b5 + b6 + b7 > 0)
            {
                Console.WriteLine("Wrong path from check path");
                return -1;
            }
            if (b1 + b2 + b3 + b4 + b5 + b6 + b7 == 0)
            {
                return 1;
            }
            return 1;
        }
        public int Check_clitle_path(string s, char[] x, char[] z)
        {
            int b1, b2, b3, b4;
            b1 = 0; b2 = 0; b3 = 0; b4 = 0;
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j] == s[i])
                    {
                        b1 = 1;
                    }
                }
                for (int t = 0; t < r.Length; t++)
                {
                    if (r[t] == s[i])
                    {
                        b1 = 2;
                    }
                }
            }
            if (s[s.Length - 1] == '\\')
            {
                b2 = 1;
            }
            bool b = s.Contains("\\\\");
            if (b == true)
            {
                b3 = 1;
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ':')
                {
                    b4 = 1;
                }
            }
            if (b1 + b2 + b3 + b4 > 0)
            {
                Console.WriteLine("Wrong path from check path");
                return -1;
            }
            if (b1 + b2 + b3 + b4 == 0)
            {
                return 1;
            }
            return 1;
        }
        public int Check_commands(string s)
        {
            bool b;
            int i;
            for (i = 0; i < commands.Length; i++)
            {
                b = s.StartsWith(commands[i]);
                //Console.WriteLine(commands[i]);
                if (b == true)
                {
                    return i;
                }
            }
            return -1;
        }
        public string Sub_string(string s, int a)
        {
            int length = commands[a].Length;
            string temp = s.Substring(length);
            return temp;
        }
        public static void Main()
        {
            int s;
            int i = 10;
            string command;
            LitleShell first = new LitleShell();
            first.cpath = @"C:\Users\Uchiha Itachi\Desktop\cm-master";
            Console.Write(first.cpath);
            Console.Write(">");
            while (i > 0)
            {
                first.clitle_path = Console.ReadLine();
                s = first.Check_commands(first.clitle_path);
                if (s != -1)
                {
                    command = commands[s];
                    first.clitle_path = first.Sub_string(first.clitle_path, s);
                    bool b = first.clitle_path.EndsWith(".exe");
                    string temp1, temp2;
                    if (b == true)
                    {
                        int xo = 0;
                        xo = first.clitle_path.LastIndexOf('\\');
                        if (xo > 1)
                        {
                            temp1 = (first.clitle_path).Substring(xo);
                            temp2 = first.clitle_path.Remove(xo);
                            first.Concat_path(temp2);
                            first.clitle_path = temp2;
                            int temp3 = first.Check_path(first.cpath, x, c, r);
                            if (temp3 == 1)
                            {
                                first.file_path = String.Concat(first.cpath, temp1);
                            }
                        }
                        else
                        {
                            first.file_path = String.Concat(first.cpath, "\\", first.clitle_path);
                        }

                    }
                    string Read_from_text;
                    switch (command)
                    {
                        case "/up":
                            first.cpath = first.Up(first.cpath);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/down ":
                            first.cpath = first.Down(first.cpath, first.clitle_path);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/cd ":
                            first.cpath = first.Cd(first.clitle_path);
;                           Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/cftxt ":
                            first.Create_text(first.cpath, first.clitle_path);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/dftxt ":
                            first.Delete_text(first.cpath, first.clitle_path);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/runexe ":
                            first.Run_exe();
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/subdir":
                            first.Sub_directories(first.cpath);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/subfiles":
                            first.Sub_directories(first.cpath);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/wtxt ":
                            first.Write_text(first.cpath, first.clitle_path);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        case "/rtxt ":
                            Read_from_text = first.Read_text(first.cpath, first.clitle_path);
                            Console.WriteLine("Our string = {0}", Read_from_text);
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                        default:
                            Console.WriteLine("Error");
                            Console.Write(first.cpath);
                            Console.Write(">");
                            break;
                    }

                }
                else
                {
                    Console.Write(first.cpath);
                    Console.Write(">");
                }
            }
        }
    }

}

