Imports System.ComponentModel

Public Class Form1
    Sub ApplyChange(Address As String, Optional ByVal UserName As String = "root", Optional ByVal PassWord As String = "calvin", Optional ByVal Automatic As Boolean = True, Optional ByVal Speed As Integer = 20)
        Dim pr As New Process
        pr.StartInfo.FileName = "ipmitool.exe"
        pr.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        Dim argpred As String = "-I lanplus -H " & Address & " -U " & UserName & " -P " & PassWord & " raw "
        If Automatic Then
            pr.StartInfo.Arguments = argpred & "0x30 0x30 0x01 0x01"
            pr.Start()
        Else
            pr.StartInfo.Arguments = argpred & "0x30 0x30 0x01 0x00"
            pr.Start()
            Try
                pr.WaitForExit()
            Catch
            End Try
            pr.StartInfo.Arguments = argpred & "0x30 0x30 0x02 0xff 0x" & Hex(Speed)
            pr.Start()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ApplyChange(TextBox1.Text, TextBox2.Text, TextBox3.Text, RadioButton1.Checked, NumericUpDown1.Value)
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        NumericUpDown1.Enabled = RadioButton2.Checked
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
    Dim LoadComplete As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.UserName <> "" Then
            TextBox2.Text = My.Settings.UserName
            TextBox3.Text = My.Settings.Password
        End If
        LoadComplete = True
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If LoadComplete And CheckBox1.Checked Then ApplyChange(TextBox1.Text, TextBox2.Text, TextBox3.Text, RadioButton1.Checked, NumericUpDown1.Value)
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        If LoadComplete And CheckBox1.Checked And RadioButton2.Checked Then ApplyChange(TextBox1.Text, TextBox2.Text, TextBox3.Text, RadioButton1.Checked, NumericUpDown1.Value)

    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.UserName = TextBox2.Text
        My.Settings.Password = TextBox3.Text
        My.Settings.IPAddress = TextBox1.Text
        My.Settings.Save()
    End Sub
End Class
