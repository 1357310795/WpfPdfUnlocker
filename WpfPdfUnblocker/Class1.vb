Imports System.IO
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class Class1
    Public Shared Function removeByITextSharp(s As String, t As String, ByRef err As String) As Boolean
        Dim result As Boolean
        Try
            Dim flag As Boolean = False
            Dim document As Document = Nothing
            Dim pdfWriter As PdfWriter = Nothing
            Dim pdfReader As PdfReader = Nothing
            pdfReader = New PdfReader(s)
            Dim pageSize As Rectangle = pdfReader.GetPageSize(1)
            document = New Document(pageSize)
            pdfWriter = PdfWriter.GetInstance(document, New FileStream(t, FileMode.Create))

            If pdfReader IsNot Nothing Then
                document.Open()
                Dim directContent As PdfContentByte = pdfWriter.DirectContent
                For i As Integer = 1 To pdfReader.NumberOfPages
                    document.SetPageSize(pdfReader.GetPageSizeWithRotation(i))
                    document.NewPage()
                    Dim importedPage As PdfImportedPage = pdfWriter.GetImportedPage(pdfReader, i)
                    Dim pageRotation As Integer = pdfReader.GetPageRotation(i)
                    pdfReader.GetPageSizeWithRotation(i)
                    If pageRotation = 270 Then
                        directContent.AddTemplate(importedPage, 0F, 1.0F, -1.0F, 0F, pdfReader.GetPageSizeWithRotation(i).Width, 0F)
                    ElseIf pageRotation = 90 Then
                        directContent.AddTemplate(importedPage, 0F, -1.0F, 1.0F, 0F, 0F, pdfReader.GetPageSizeWithRotation(i).Height)
                    ElseIf pageRotation = 180 Then
                        directContent.AddTemplate(importedPage, -1.0F, 0F, 0F, -1.0F, pdfReader.GetPageSizeWithRotation(i).Width, pdfReader.GetPageSizeWithRotation(i).Height)
                    Else
                        directContent.AddTemplate(importedPage, 1.0F, 0F, 0F, 1.0F, 0F, 0F)
                    End If
                Next
                document.Close()
                flag = True
            End If
            result = flag
        Catch ex2 As Exception
            err = ex2.Message
            result = False
        End Try
        Return result
    End Function
    Public Shared Function removeByITextSharpWithPass(s As String, t As String, pass As String, ByRef err As String) As Boolean
        Dim result As Boolean
        Try
            Dim flag As Boolean = False
            Dim document As Document = Nothing
            Dim pdfWriter As PdfWriter = Nothing
            Dim pdfReader As PdfReader = Nothing

            Dim bytes As Byte() = Encoding.ASCII.GetBytes(pass)
            pdfReader = New PdfReader(s, bytes)
            Dim pageSize2 As Rectangle = pdfReader.GetPageSize(1)
            document = New Document(pageSize2)
            pdfWriter = PdfWriter.GetInstance(document, New FileStream(t, FileMode.Create))
            Dim permissions As Integer = pdfReader.Permissions

            If pdfReader IsNot Nothing Then
                document.Open()
                Dim directContent As PdfContentByte = pdfWriter.DirectContent
                For i As Integer = 1 To pdfReader.NumberOfPages
                    document.SetPageSize(pdfReader.GetPageSizeWithRotation(i))
                    document.NewPage()
                    Dim importedPage As PdfImportedPage = pdfWriter.GetImportedPage(pdfReader, i)
                    Dim pageRotation As Integer = pdfReader.GetPageRotation(i)
                    pdfReader.GetPageSizeWithRotation(i)
                    If pageRotation = 270 Then
                        directContent.AddTemplate(importedPage, 0F, 1.0F, -1.0F, 0F, pdfReader.GetPageSizeWithRotation(i).Width, 0F)
                    ElseIf pageRotation = 90 Then
                        directContent.AddTemplate(importedPage, 0F, -1.0F, 1.0F, 0F, 0F, pdfReader.GetPageSizeWithRotation(i).Height)
                    ElseIf pageRotation = 180 Then
                        directContent.AddTemplate(importedPage, -1.0F, 0F, 0F, -1.0F, pdfReader.GetPageSizeWithRotation(i).Width, pdfReader.GetPageSizeWithRotation(i).Height)
                    Else
                        directContent.AddTemplate(importedPage, 1.0F, 0F, 0F, 1.0F, 0F, 0F)
                    End If
                Next
                document.Close()
                flag = True
            End If
            result = flag
        Catch ex2 As Exception
            err = ex2.Message
            result = False
        End Try
        Return result
    End Function

End Class
