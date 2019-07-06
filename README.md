[摘要]

这是我编写的一个 Docfx 文档自动生成工具，只要写好 Markdown 文档，使用此工具可为目录、文件快速生成配置，然后直接使用 docfx 运行即可。

https://github.com/whuanle/CZGL.DocfxBuild.Yml/releases/tag/1.0

## 一，安装Docfx

下载地址 https://github.com/dotnet/docfx/releases

下载后解压压缩包，记录好目录路径。

Win 搜索 “环境变量”，会出现 “编辑系统环境变量”。

然后 ↓

![](https://obs1.whuanle.cn/markdown/CZGL.DocfxBuild.Yml/1562330330(1).png)

新建一个目录，用来存放 markdown 文件。打开目录，按住 <kbd>Shift</kbd> + <kbd>鼠标右键</kbd> ，打开 Powershell 。

两个命令

```powershell
docfx init -q
```

```powershell
 docfx docfx_project\docfx.json --serve   // 以实际路径为准
```



## 二， 创建 Docfx 项目

使用此命令在目录下初始一个 docfx 项目

```
docfx init -q
```

-q 这个参数表示忽略询问，直接生成。如果需要自定义配置，可取消 -q 这个参数。

打开 docfx_project 文件夹，目录结构如下：

```
docfx_project 
.
├── api
│   ├── index.md
│   └── toc.yml
├── apidoc
├── articles
│   ├── intro.md
│   └── toc.yml
├── docfx.json
├── images
├── index.md
├── src
└── toc.yml
```

##### 为代码创建 API 文档

src 目录用来存放你需要生成文档的项目，直接把整个项目(解决方案)放进去即可。

![](https://obs1.whuanle.cn/markdown/CZGL.DocfxBuild.Yml/1562332845(1).png)

生成文档

```
 docfx docfx_project\docfx.json --serve
```

预览

![](https://obs1.whuanle.cn/markdown/CZGL.DocfxBuild.Yml/1562333383(1).png)



对于 项目，可以快速生成对象文档、代码文档。

这里有关于 REST API 生成文档的详细方法

https://dotnet.github.io/docfx/tutorial/intro_rest_api_documentation.html

## 三， 文字文档

文字文档使用 markdown 文件编写，存放位置 articles 。 

```
    ├── intro.md
    └── toc.yml
```

这两个文件是默认的， toc.yml 由于默认生成目录结构，intro.md 是打开文档是默认看到的，可以理解为封面内容。例如 Github 仓库的 Readme.md 。

为了生成多级目录，建议每个目录有应该有一个 toc.yml 文件。

最简单的语法

```yaml
- name: xxx
  href: xxxx
```

用于生成目录结构详细、文档文件，href 可以是 目录、 .md 文件、.yml 文件。

但 href 为 .yml 是，会生成目录结构。

使用

```
  items:
    - name: Topic2_1
      href: Topic2_1.md
      ...
      ...
```

也可以生成层次结构。

```yaml
  homepage: index.md

```

用来生成首页文件，对于子目录，用处不大。

用来测试的文件和目录

```
.
└── articles
    ├── a
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   └── d.md
    ├── b
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   └── d.md
    ├── c
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   └── d.md
    ├── d
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   └── d.md
    ├── intro.md
    └── toc.yml

```

使用 CZGL.DocfxBuild.Yml 自动生成后

```
.
└── articles
    ├── a
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   ├── d.md
    │   └── toc.yml
    ├── b
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   ├── d.md
    │   └── toc.yml
    ├── c
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   ├── d.md
    │   └── toc.yml
    ├── d
    │   ├── a
    │   ├── a.md
    │   ├── b
    │   ├── b.md
    │   ├── c
    │   ├── c.md
    │   ├── d
    │   ├── d.md
    │   └── toc.yml
    ├── intro.md
    └── toc.yml


```

生成文档：

```
 docfx docfx_project\docfx.json --serve

```

![](https://obs1.whuanle.cn/markdown/CZGL.DocfxBuild.Yml/1562335013(1).png)

使用 CZGL.DocfxBuild.Yml 可以帮助你快速生成文档目录。

根目录的 toc.yml 文件：

```
- name: a
  href: a/toc.yml
- name: b
  href: b/toc.yml
- name: c
  href: c/toc.yml
- name: d
  href: d/toc.yml
- name: intro
  href: intro.md
  homepage: intro.md


```

name 为目录名称。

href 指向子目录下的 toc.yml 文件

目录 a 的结构

```
.
├── a
│   ├── a.md
│   ├── b
│   ├── b.md
│   ├── c
│   ├── c.md
│   ├── d
│   ├── d.md
│   └── toc.yml
├── a.md
├── b
│   ├── a.md
│   ├── b.md
│   ├── c.md
│   ├── d.md
│   └── toc.yml
├── b.md
├── c
│   ├── a.md
│   ├── b.md
│   ├── c.md
│   ├── d.md
│   └── toc.yml
├── c.md
├── d
│   ├── a.md
│   ├── b.md
│   ├── c.md
│   ├── d.md
│   └── toc.yml
├── d.md
└── toc.yml


```

```
### G:\临时缓存\docfx\docfx_project\articles\a
- name: a
  href: a/toc.yml
- name: b
  href: b/toc.yml
- name: c
  href: c/toc.yml
- name: d
  href: d/toc.yml
- name: a
  href: a.md
- name: b
  href: b.md
- name: c
  href: c.md
- name: d
  href: d.md
  homepage: a.md


```

每个目录一个 .yml 文件

如果这个目录下有目录，则连接子目录的 .yml 文件，如果是子文件，则链接这个 .md 文件。

这样能够快速生成文档目录。

当然也可以尝试 items

官方详细文档地址

https://dotnet.github.io/docfx/tutorial/intro_toc.html

利用空闲时间写了自动生成 docfx 目录的功能，下载地址

https://github.com/whuanle/CZGL.DocfxBuild.Yml/releases/tag/1.0





晚上有事，今天的博客水完了。
