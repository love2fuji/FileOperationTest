using FileOperationTest.QuartzDemo;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FileOperationTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private void Form1_Load(object sender, EventArgs e)
        {

            HelloJob job = new HelloJob();

            log.Info("Form1_Loading...");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = "test.txt";
            string sourcePath = @"D:\MyCodeTest\sourceData";
            string targetPath = @"D:\MyCodeTest\targetData";


            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }


            if (Directory.Exists(sourcePath))
            {

                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    if (FileIsUsed(s))
                    {
                        //测试使用CloseFile 方法关闭文件
                        //CloseFile(s);
                        //string fileNameTest = @"d:\aaa.txt";

                        //CloseFile(fileNameTest);


                        //使用自己写的关闭方式
                        //System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("EXCEL");

                        //if (process.Length > 0)
                        //{
                        //    foreach (System.Diagnostics.Process p in process)
                        //    {
                        //        p.Kill();
                        //        p.WaitForExit(1000);
                        //        p.Dispose();
                        //        p.Close();
                        //    }
                        //}

                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        if (File.Exists(destFile))
                        {
                            CloseFile(destFile);
                            File.Delete(destFile);
                        }
                        if (File.Exists(s))
                        {
                            File.Move(s, destFile);
                            MessageBox.Show("移动文件成功");
                        }
                        
                    }
                    else
                    {
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        if (File.Exists(destFile))
                        {
                            File.Delete(destFile);
                        }
                        if (File.Exists(s))
                        {
                            File.Move(s, destFile);
                            MessageBox.Show("移动文件成功");
                        }
                       
                    }
                }
            }
            else
            {
                MessageBox.Show("源文件夹不存在！");
            }
        }




        /// <summary>
        /// 返回指示文件是否已被其它程序使用的布尔值
        /// </summary>
        /// <param name="fileFullName">文件的完全限定名，例如：“C:\MyFile.txt”。</param>
        /// <returns>如果文件已被其它程序使用，则为 true；否则为 false。</returns>
        public static Boolean FileIsUsed(String fileFullName)
        {
            Boolean result = false;

            //判断文件是否存在，如果不存在，直接返回 false
            if (!System.IO.File.Exists(fileFullName))
            {
                result = false;

            }//end: 如果文件不存在的处理逻辑
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用

                //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream fileStream = null;
                try
                {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

                    result = false;
                }
                catch (System.IO.IOException ioEx)
                {
                    result = true;
                }
                catch (System.Exception ex)
                {
                    result = true;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }

            }//end: 如果文件存在的处理逻辑

            //返回指示文件是否已被其它程序使用的值
            return result;
        }

        public static void CloseFile(string fileName)
        {

            Process tool = new Process();
            tool.StartInfo.FileName = "handle64.exe";
            tool.StartInfo.Arguments = fileName + " /accepteula";
            tool.StartInfo.UseShellExecute = false;
            tool.StartInfo.RedirectStandardOutput = true;
            tool.Start();
            tool.WaitForExit();
            string outputTool = tool.StandardOutput.ReadToEnd();

            string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
            foreach (Match match in Regex.Matches(outputTool, matchPattern))
            {
                Process.GetProcessById(int.Parse(match.Value)).Kill();
            }
        }

    }


}
