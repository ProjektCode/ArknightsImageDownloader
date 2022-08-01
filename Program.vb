Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports HtmlAgilityPack

Module Program
    ReadOnly basePath = AppDomain.CurrentDomain.BaseDirectory
    ReadOnly Downloads = $"{basePath}Downloads\"
    ReadOnly Skins = $"{Downloads}skins\"
    ReadOnly Elite2 = $"{Downloads}elite2\"


    Private eLinks As New List(Of String)
    Private sLinks As New List(Of String)

    Private ReadOnly baseURL = "https://gamepress.gg"
    Private ReadOnly SkinImagesURL = "https://gamepress.gg/arknights/gallery/arknights-skin-art-gallery"
    Private ReadOnly E2ImagesUrl = "https://gamepress.gg/arknights/gallery/arknights-operator-e2-art-gallery"

    Sub Main()
        Console.Title = "Gamepress Arknights Image Downloader"
        If Not Directory.Exists(Downloads) Then
            Directory.CreateDirectory(Skins)
            Directory.CreateDirectory(Elite2)
        End If
        eLinks = GetE2Images()
        sLinks = GetSkinImages()
        Console.WriteLine("Preparing Downloads...")
        DownloadImage(eLinks, "elite2")
        DownloadImage(sLinks, "skins")
        Console.WriteLine("Images have been downloaded...")
        Console.ReadLine()
    End Sub

    Private Function GetSkinImages()
        Try
            Dim web As New HtmlWeb
            Dim doc = web.Load(SkinImagesURL)
            Dim rootNode = doc.DocumentNode
            Dim nodes = rootNode.SelectNodes("//img")
            Dim list As New List(Of String)

            For Each src In nodes
                Dim value = src.Attributes("src").Value
                If value.Contains("/Art") Or value.Contains("char") Then
                    If Not value.Contains("thumbnail") Then
                        Dim link = $"{baseURL}{value}"
                        'Dim name = link.Substring(link.LastIndexOf("/") + 1)
                        list.Add(link)
                        'iNames.Add(name)
                    End If
                End If
            Next

            Return list
        Catch ex As Exception
            Console.WriteLine("Skin images " + ex.Message)
            Return Nothing
        End Try
    End Function

    Private Function GetE2Images()
        Try
            Dim web As New HtmlWeb
            Dim doc = web.Load(E2ImagesUrl)
            Dim rootNode = doc.DocumentNode
            Dim nodes = rootNode.SelectNodes("//img")
            Dim list As New List(Of String)

            For Each src In nodes
                Dim value = src.Attributes("src").Value
                If value.Contains("char") Then
                    If Not value.Contains("thumbnail") Then
                        Dim link = $"{baseURL}{value}"
                        'Dim name = link.Substring(link.LastIndexOf("/") + 1)
                        list.Add(link)
                        'iNames.Add(name)
                    End If
                End If
            Next

            Return list
        Catch ex As Exception
            Console.WriteLine("E2 images " + ex.Message)
            Return Nothing
        End Try
    End Function

    Private Sub DownloadImage(list As List(Of String), folder As String)
        Try
            Dim spinner As New ConsoleSpinner With {
                .Delay = 300
            }

            If Not Directory.Exists($"Downloads/{folder.ToLower}") Then
                Directory.CreateDirectory($"Downloads/{folder.ToLower}")
            End If

            Dim name As New List(Of String)
            For Each l In list
                'Dim name2 = l.Substring(l.LastIndexOf("/") + 1)
                'Dim name3 = Regex.Replace(name2, "[^A-Za-z0-9-\-/]", String.Empty)
                name.Add(l.Substring(l.LastIndexOf("/") + 1))
            Next
            'Download images - check if file exists if it does skip it.
            Using client As New WebClient
                For i = 0 To list.Count - 1
                    Dim n As String = $"Downloads\{folder}\{name(i)}"
                    If Not File.Exists(n) Then
                        client.DownloadFile(list(i), n)
                    End If
                    spinner.Turn("Downloading... ", sequenceCode:=5)
                Next
            End Using
            Console.WriteLine($"Download of {folder} images have completed.")
            Threading.Thread.Sleep(3000)
            Console.Clear()
        Catch ex As Exception
            Console.WriteLine("Download images " + ex.Message)
        End Try
    End Sub

End Module