using System.Collections.Generic;
using Python.Runtime;
using System;
using System.IO;
using Godot;
using System.Runtime.InteropServices;

namespace BackEnd 
{
    /// <summary>
    /// Class for the back end
    /// </summary>
    public class BackEnd
    {
        /// <summary>
        /// the function will search using the python libary
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        public List<Med> Search(string inp)
        {
            string projectdir = ProjectSettings.GlobalizePath("res://Scripts/BE/");
            List<Med> mylist = new List<Med>();
            int debug_num = 0;
            // this if block is not important and was used during testing
            if (inp == "debug")
            {
                GD.Print("in debug!");
            }
            else 
            {
                // here we need to point to the python dll in editor we can just use ProjectSettings.GlobalizePath but for release we have to put the python_dep folder manually
                // and use OS.IO to get to it
                if (!PythonEngine.IsInitialized)
                {
                   if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        string executablePath = OS.GetExecutablePath();

                        string projectRoot = Path.GetDirectoryName(executablePath);

                        string projectd = Path.Combine(projectRoot, "python_dep");

                        projectdir = projectd;

                        string absolutePath = Path.Combine(projectRoot, "res://python_dep/python311.dll".Substring("res://".Length));

                        absolutePath = absolutePath.Replace("/", Path.DirectorySeparatorChar.ToString());

                        Runtime.PythonDLL = absolutePath;

                        PythonEngine.Initialize();
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        if (OS.IsDebugBuild())
                        {
                            Runtime.PythonDLL = ProjectSettings.GlobalizePath("res://python_dep/libpython3.11.so.1");
                            PythonEngine.Initialize();
                        }
                        else
                        {
                            string executablePath = OS.GetExecutablePath();

                            string projectRoot = Path.GetDirectoryName(executablePath);

                            string projectd = Path.Combine(projectRoot, "python_dep");

                            projectdir = projectd;

                            string absolutePath = Path.Combine(projectRoot, "res://python_dep/libpython3.11.so.1".Substring("res://".Length));

                            absolutePath = absolutePath.Replace("/", Path.DirectorySeparatorChar.ToString());

                            Runtime.PythonDLL = absolutePath;

                            PythonEngine.Initialize();
                        }
                    }
                }
                using (Py.GIL())
                {
                    // will run the api.py file with the inp variable
                    dynamic sys = Py.Import("sys");
                    sys.path.append(projectdir);
                    var pythonscript = Py.Import("api");
                    var arg = new PyString(inp);
                    var result = pythonscript.InvokeMethod("search",new PyObject[] {arg});
                    PyList res_list = new PyList(result);
                    // we make an object for every search object
                    foreach (PyObject i in res_list)
                    {
                        PyList res_data = new PyList(i);
                        Med m2 = new Med();
                        m2.id = int.Parse(res_data[0].ToString());
                        m2.name = res_data[1].ToString();
                        m2.year = int.Parse(res_data[2].ToString());
                        m2.poster = res_data[3].ToString();
                        m2.poster_large = res_data[4].ToString();
                        m2.synopsis = res_data[5].ToString();
                        m2.agerating = res_data[6].ToString();
                        m2.is_debug = false;
                        mylist.Add(m2);
                    }
                }
            }
            return mylist;
        }
    }
    /// <summary>
    /// Base class for Media objects
    /// </summary>
    public class Med
    {
        public int id;
        public bool is_debug;
        public string name;
        public int year;
        public string poster;
        public string poster_large;
        public string synopsis;
        public string agerating;
    }
}