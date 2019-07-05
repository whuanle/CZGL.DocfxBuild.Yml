using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace CZGL.DocfxBuild.Yml
{
    class Program
    {

        static void Main(string[] args)
        {
            BuildYml build = new BuildYml();
            Console.WriteLine("输入要处理的目录：");
            string a = Console.ReadLine();
            Console.WriteLine("指定每个目录要生成的默认首页规则：\n1.以每个目录第一个.md为首页(输入序号 1) \n" +
                "2. 每个目录以 index.md 为首页(输入序号 2)\n" +
                "3. 其他规则请直接输入文件名(如 home.md)");
            string b = Console.ReadLine();
            switch (b)
            {
                case "1": build.homewhat = 1; break;
                case "2": build.homewhat = 2; build.homepagename = "index.md"; break;
                default: build.homewhat = 3; build.homepagename = b; break;
            }
            Console.WriteLine("处理结果：" + build.Builder(a));
            Console.WriteLine("输入 d 可以删除生成的文件");
            if (Console.ReadLine().ToLower() == "d")
            {
                build.Dedocyml();
            }

            Console.ReadKey();
        }
    }
    public class BuildYml
    {
        string docfx_project_Path;
        public int homewhat;
        public string homepagename;
        string[] redir = { ".git", "image", "images" };
        string[] refile = { "toc.yml", "intro.md" };

        public BuildYml()
        {
            Init();
        }
        async void Init()
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "SetRe.json")))
            {
                var json = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "SetRe.json"));
                var model = JsonConvert.DeserializeObject<SetRe>(json);
                redir = model.Dir_Exclude;
                refile = model.File_Exclude;
            }
        }
        public class SetRe
        {
            public string[] Dir_Exclude { get; set; }
            public string[] File_Exclude { get; set; }
        }
        public bool Builder(string articles)
        {
            docfx_project_Path = articles;
            if (!Directory.Exists(docfx_project_Path))
            {
                Console.WriteLine("目录不存在");
                return false;
            }

            try
            {
                Traverse(docfx_project_Path);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        // 遍历
        public void Traverse(string dirpath)
        {
            string[] dirs = Directory.GetDirectories(dirpath);
            foreach (var item in dirs)
            {
                if (redir.Contains((new DirectoryInfo(item).Name).ToLower()))
                    dirs = dirs.Where(x => x != item).ToArray();
            }
            if (dirs.Length != 0)
            {
                foreach (var item in dirs)
                {
                    Traverse(item);
                }
            }
            BuilderFileYml(dirpath);

        }
        public void Dedocyml()
        {
            DeTocYml(docfx_project_Path);
            Console.WriteLine("删除完成");
        }
        private void DeTocYml(string dirpath)
        {
            string[] dirs = Directory.GetDirectories(dirpath);
            foreach (var item in dirs)
            {
                if (redir.Contains((new DirectoryInfo(item).Name).ToLower()))
                    dirs = dirs.Where(x => x != item).ToArray();
            }

            if (dirs.Length != 0)
            {
                foreach (var item in dirs)
                {
                    DeTocYml(item);
                }
            }
            DeleteYml(dirpath);
        }
        void DeleteYml(string dirpath)
        {
            try
            {
                File.Delete(Path.Combine(dirpath, "toc.yml"));
            }
            catch
            {

            }
        }
        // 对一个目录生成操作
        public async void BuilderFileYml(string dirpath)
        {
            Console.WriteLine("生成：" + dirpath);
            await Task.Run(() =>
            {
                string[] dirs = Directory.GetDirectories(dirpath);
                string[] files = Directory.GetFiles(dirpath);
                if (dirs.Length == 0 && files.Length == 0)
                {
                    FileStream x = new FileStream(Path.Combine(dirpath, "Empty.md"),FileMode.OpenOrCreate);
                    var markdown = Encoding.Default.GetBytes("This an empty directory");
                    x.WriteAsync(markdown, 0, markdown.Length);
                    files = Directory.GetFiles(dirpath);
                }

                foreach (var item in dirs)
                {
                    if (redir.Contains((new DirectoryInfo(item).Name).ToLower()))
                        dirs = dirs.Where(x => x != item).ToArray();
                }
                foreach (var item in files)
                {
                    if (refile.Contains((new DirectoryInfo(item).Name).ToLower()))
                        files = files.Where(x => x != item).ToArray();
                }

                using (FileStream fx = new FileStream(Path.Combine(dirpath, "toc.yml"), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    StreamWriter writer = new StreamWriter(fx);
                    writer.WriteLine("### " + dirpath);
                    foreach (var item in dirs)
                    {
                        writer.WriteLine("- name: " + new DirectoryInfo(item).Name);
                        writer.WriteLine("  href: " + new DirectoryInfo(item).Name + "/" + "toc.yml");
                    }
                    foreach (var item in files)
                    {
                        writer.WriteLine("- name: " + Path.GetFileName(item).Replace(".md", "").Replace(".MD", ""));
                        writer.WriteLine("  href: " + Path.GetFileName(item));
                    }
                    if (files.Length != 0)
                    {
                        if (homewhat == 1)
                        {
                            writer.WriteLine("  homepage: " + Path.GetFileName(files[0]));
                        }
                        else if (homewhat == 2)
                        {
                            writer.WriteLine("  homepage: index.md");
                        }
                        else
                            writer.WriteLine("  homepage: " + homepagename);
                    }
                    writer.Close();
                    fx.Close();
                }
            });

        }
    }
}
