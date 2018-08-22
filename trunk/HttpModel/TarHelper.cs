using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Tar;

namespace HttpModel
{
 public class TarHelper
    {
        /// <summary>
        /// 生成 ***.tar 文件
        /// </summary>
        /// <param name="strBasePath">文件基目录（源文件、生成文件所在目录）</param>
        /// <param name="strSourceFolderName">待压缩的源文件夹名</param>
        public bool CreatTarArchive(string strBasePath, string strSourceFolderName)
        {
            if (string.IsNullOrEmpty(strBasePath)
                || string.IsNullOrEmpty(strSourceFolderName)
                || !Directory.Exists(strBasePath)
                || !Directory.Exists(Path.Combine(strBasePath, strSourceFolderName)))
            {
                return false;
            }
            Environment.CurrentDirectory = strBasePath;//要压缩的文件夹名称
            string strSourceFolderAllPath = Path.Combine(strBasePath, strSourceFolderName);
            string strOupFileAllPath = Path.Combine(strBasePath, strSourceFolderName + ".tar");//压缩文件名及路径

            Stream outStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);

            TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            TarEntry entry = TarEntry.CreateEntryFromFile(strSourceFolderAllPath);
            archive.WriteEntry(entry, true);

            if (archive != null)
            {
                archive.Close();
            }
            outStream.Close();
            return true;
        }

        /// <summary>
        /// 打包成Tar包
        /// </summary>
        /// <param name="strBasePath">压缩文件夹路径</param>
        /// <param name="strSourceFolderName">生成tar文件路径</param>
        /// <param name="sTarName">生成tar文件名称</param>
        /// <returns></returns>
        public bool CreatTarArchive(string strBasePath, string strSourceFolderName, string sTarName)
        {
            if (!Directory.Exists(strSourceFolderName))
            {
                Directory.CreateDirectory(strSourceFolderName);//不存在生成Tar文件目录就创建
            }

            if (string.IsNullOrEmpty(strBasePath)
                || string.IsNullOrEmpty(strSourceFolderName)
                || !Directory.Exists(strBasePath))
            {
                return false;
            }
            if (strBasePath.EndsWith("\\"))
            {
                strBasePath = strSourceFolderName.TrimEnd('\\');
            }
            Environment.CurrentDirectory = strBasePath;//要压缩的文件夹名称
            string strSourceFolderAllPath = strBasePath;
            string strOupFileAllPath = strSourceFolderName + "\\" + sTarName + ".tar";//压缩文件名及路径

            Stream outStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);

            TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            TarEntry entry = TarEntry.CreateEntryFromFile(strSourceFolderAllPath);
            archive.WriteEntry(entry, true);

            if (archive != null)
            {
                archive.Close();
            }
            outStream.Close();
            return true;
        }


        public void DeleteFolder(string folderpath)
        {
            try
            {

                foreach (string delFile in Directory.GetFileSystemEntries(folderpath))
                {
                    if (File.Exists(delFile))
                    {
                        FileInfo fi = new FileInfo(delFile);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(delFile);//直接删除其中的文件
                        // SetText("删除文件：" + delFile);
                    }
                    else
                    {
                        DirectoryInfo dInfo = new DirectoryInfo(delFile);
                        if (dInfo.GetFiles().Length != 0)
                        {
                            DeleteFolder(dInfo.FullName);//递归删除子文件夹
                        }
                        Directory.Delete(delFile);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("G1475：" + ex.Message);
   
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBasePath">要压缩文件所在文件夹</param>
        /// <param name="strSourceFolderName">生成TAR包存放路径</param>
        /// <param name="sTarName">TAR包名</param>
        /// <returns></returns>
        public bool CreatTar(string strBasePath, string strSourceFolderName, string sTarName)
        {
            if (!Directory.Exists(strSourceFolderName))
            {
                Directory.CreateDirectory(strSourceFolderName);//不存在生成Tar文件目录就创建
            }
            if (string.IsNullOrEmpty(strBasePath)
                || string.IsNullOrEmpty(strSourceFolderName)
                || !Directory.Exists(strBasePath))
            {
                return false;
            }
            string strOupFileAllPath = strSourceFolderName + "\\EBDT_" + sTarName + ".tar";//压缩文件名及路径
            Stream outStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            SharpCompress.Archive.Tar.TarArchive archive = SharpCompress.Archive.Tar.TarArchive.Create();
            try
            {
                DirectoryInfo theFolder = new DirectoryInfo(strBasePath);
                FileInfo[] xmlfiles = theFolder.GetFiles("*.*");
                if (xmlfiles.Length > 0)
                {
                    Console.WriteLine(strBasePath + "===>Count: " + xmlfiles.Length);
                    for (int i = 0; i < xmlfiles.Length; i++)
                    {
                        archive.AddEntry(xmlfiles[i].Name, xmlfiles[i]);
                    }
                    SharpCompress.Common.CompressionInfo infs = new SharpCompress.Common.CompressionInfo();
                    archive.SaveTo(outStream, infs);
                    outStream.Close();
                    archive.Dispose();
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                outStream.Close();
                return false;
            }
           
            return true;
        }

        /// <summary>
        /// tar包解压
        /// </summary>
        /// <param name="strFilePath">tar包路径</param>
        /// <param name="strUnpackDir">解压到的目录</param>
        /// <returns></returns>
        public bool UnpackTarFiles(string strFilePath, string strUnpackDir)
        {
            try
            {
                if (!File.Exists(strFilePath))
                {
                    return false;
                }
                strUnpackDir = strUnpackDir.Replace("/", "\\");
                if (!strUnpackDir.EndsWith("\\"))
                {
                    strUnpackDir += "\\";
                }
                if (!Directory.Exists(strUnpackDir))
                {
                    Directory.CreateDirectory(strUnpackDir);
                }
                FileStream fr = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                TarInputStream s = new TarInputStream(fr);
                TarEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(strUnpackDir + directoryName);
                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(strUnpackDir + theEntry.Name);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                s.Close();
                fr.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
