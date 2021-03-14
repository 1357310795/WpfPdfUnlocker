Public Class Window1
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        pass1.Focus()
    End Sub

    Private Sub pass1_KeyDown(sender As Object, e As KeyEventArgs) Handles pass1.KeyDown
        If e.Key = Key.Enter Then
            Me.Close()
        End If
    End Sub
End Class
