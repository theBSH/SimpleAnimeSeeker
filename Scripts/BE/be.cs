using System;
using System.Collections.Generic;
using System.Diagnostics;
using Python.Runtime;
using Godot;
using System.Runtime.InteropServices;

namespace BackEnd 
{
    public class BackEnd
    {
        public List<Med> Search(string inp)
        {
            string projectdir = ProjectSettings.GlobalizePath("res://Scripts/BE/");
            List<Med> mylist = new List<Med>();
            int debug_num = 0;
            if (inp == "debug")
            {
                debug_num = 30;
                for (int i = 0;i < debug_num;i++)
                {
                    Med m1 = new Med();
                    m1.id = 1;
                    m1.is_debug = true;
                    mylist.Add(m1);
                }
            }
            else 
            {
                if (!PythonEngine.IsInitialized)
                {
                   if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                   {
                       Runtime.PythonDLL = ProjectSettings.GlobalizePath("res://python_dep/python311.dll");
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Runtime.PythonDLL = ProjectSettings.GlobalizePath("res://python_dep/libpython3.11.so.1");
                    }
                    PythonEngine.Initialize();
                }
                using (Py.GIL())
                {
                    dynamic sys = Py.Import("sys");
                    sys.path.append(projectdir);
                    var pythonscript = Py.Import("api");
                    var arg = new PyString(inp);
                    var result = pythonscript.InvokeMethod("search",new PyObject[] {arg});
                    PyList res_list = new PyList(result);
                    foreach (PyObject i in res_list)
                    {
                        PyList res_data = new PyList(i);
                        Med m2 = new Med();
                        m2.id = int.Parse(res_data[0].ToString());
                        m2.name = res_data[1].ToString();
                        m2.year = int.Parse(res_data[2].ToString());
                        m2.poster = res_data[3].ToString();
                        m2.is_debug = false;
                        mylist.Add(m2);
                    }
                }
            }
            return mylist;
        }
    }

    public class Med
    {
        public int id;
        public bool is_debug;
        public string name;
        public int year;
        public string poster;
    }
}