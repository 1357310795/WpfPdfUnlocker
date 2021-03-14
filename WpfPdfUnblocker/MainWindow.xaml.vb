Imports System.ComponentModel
Imports System.IO
Imports Microsoft.Win32

Class MainWindow
    Public Class pdfunlock
        Implements INotifyPropertyChanged
        Public statein As String
        Public Property state As String
            Get
                Return Me.statein
            End Get
            Set(value As String)
                Me.statein = value
                Me.OnPropertyChanged(New PropertyChangedEventArgs("state"))
            End Set
        End Property
        Public Property path As String
        Public Sub New(i As String, j As String)
            Me.statein = i
            Me.path = j
        End Sub

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

        Public Sub OnPropertyChanged(e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("state"))
        End Sub

    End Class

    Dim x1, x2 As List(Of String)
    Dim displayArray As BindingList(Of pdfunlock) = New BindingList(Of pdfunlock)
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        'Class1.removeByITextSharp("H:\文档\！2020猿辅导系列图书pdf\化学\高中化学——猿题库——小猿热搜——化学实验典型题100.pdf",
        '"H:\文档\！2020猿辅导系列图书pdf\化学\cs.pdf")
        List1.ItemsSource = displayArray
        Gridview1.Columns(0).Width = 100
        Gridview1.Columns(0).Width = Double.NaN
    End Sub

    Private Sub Init_Clicked(sender As Object, e As RoutedEventArgs)
        Dim openFileDialog As OpenFileDialog = New OpenFileDialog()
        openFileDialog.AddExtension = True
        openFileDialog.CheckFileExists = True
        openFileDialog.CheckPathExists = True
        openFileDialog.DefaultExt = "*.pdf"
        openFileDialog.Filter = "Acrobat PDF Files (*.pdf)|*.pdf|All files(*.*)|*.*"
        openFileDialog.Multiselect = True
        If openFileDialog.ShowDialog() Then
            For Each i In openFileDialog.FileNames()
                displayArray.Add(New pdfunlock("等待解锁", i))
            Next
        End If
    End Sub

    Private Sub Go_Clicked(sender As Object, e As RoutedEventArgs)
        For Each i In displayArray
            i.state = "等待解锁"
        Next
        x1 = New List(Of String)
        x2 = New List(Of String)
        For Each s As pdfunlock In List1.Items
            Dim t As FileInfo = New FileInfo(s.path)
            Dim n = t.Name.Replace(t.Extension, "")
            Dim o = t.DirectoryName & "\" & t1.Text & n & t2.Text & t.Extension
            Console.WriteLine(o)
            x1.Add(s.path)
            x2.Add(o)
        Next
        gobutton.IsEnabled = False
        progtext.Text = "解锁完成：0/" & x1.Count
        'prog.Visibility = Visibility.Visible
        Dim th As New Threading.Thread(AddressOf doit)
        th.Start()
    End Sub

    Public Delegate Function D2(s As String) As String
    Private Sub doit()
        For i = 0 To x1.Count - 1
            Dim err As String = ""
            Dim f As Boolean = Class1.removeByITextSharp(x1(i), x2(i), err)
            If Not f And err.ToUpper.Contains("PASSWORD") Then
                Dim num As Integer = 0

                Dim delegate2 As D2 = New D2(AddressOf getpassword)
                Dim result As String = Me.Dispatcher.Invoke(delegate2, {x1(i)})

                If Class1.removeByITextSharpWithPass(x1(i), x2(i), result, err) Then
                    f = True
                End If
            End If
            Me.Dispatcher.Invoke(New Action(Of Int32, Boolean, String)(AddressOf AfterEach), {i, f, err})
        Next
        Me.Dispatcher.Invoke(AddressOf AfterAll)
    End Sub


    Public Function getpassword(s As String) As String
        Dim frmPassword As Window1 = New Window1()
        frmPassword.text1.Text = "请输入文件 " & s & " 的密码"
        frmPassword.ShowDialog()
        Return frmPassword.pass1.Text
    End Function

    Private Sub Del_Clicked(sender As Object, e As RoutedEventArgs)
        Dim t As List(Of pdfunlock) = New List(Of pdfunlock)
        For Each i As pdfunlock In List1.SelectedItems
            t.Add(i)
        Next
        For Each i As pdfunlock In t
            displayArray.Remove(i)
        Next
    End Sub

    Private Sub DelAll_Clicked(sender As Object, e As RoutedEventArgs)
        displayArray.Clear()
    End Sub

    Private Sub Open_Clicked(sender As Object, e As RoutedEventArgs)
        If List1.SelectedItems.Count <> 1 Then
            MessageBox.Show("请只选择一个项", "", MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If
        Process.Start(TryCast(List1.SelectedItem, pdfunlock).path)
    End Sub

    Private Sub OpenFolder_Clicked(sender As Object, e As RoutedEventArgs)
        If List1.SelectedItems.Count <> 1 Then
            MessageBox.Show("请只选择一个项", "", MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If
        Dim t As FileInfo = New FileInfo(TryCast(List1.SelectedItem, pdfunlock).path)
        Process.Start("explorer", "/select," & t.FullName)
        'MessageBox.Show("/select," & t.FullName)
    End Sub

    Private Sub AfterEach(i As Int32, f As Boolean, err As String)
        If f Then
            TryCast(displayArray(i), pdfunlock).state = "解锁成功"
            Gridview1.Columns(0).Width = 100
            Gridview1.Columns(0).Width = Double.NaN
        Else
            TryCast(displayArray(i), pdfunlock).state = "解锁失败：" & err
            Gridview1.Columns(0).Width = 100
            Gridview1.Columns(0).Width = Double.NaN
        End If
        progbar.Value = (i + 1) / x1.Count
        progtext.Text = "解锁完成：" & (i + 1) & "/" & x1.Count
    End Sub

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Process.Start("https://github.com/1357310795/WpfPdfUnblocker")
    End Sub

    Private Sub AfterAll()
        gobutton.IsEnabled = True
        progbar.Value = 0
        progtext.Text = "完毕！"
    End Sub
End Class
