Imports System.IO.Ports
Imports System.Threading.Tasks
Imports System.Globalization

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Timer1.Start()

        ToolStripStatusLabel6.Text = PortConnect("COM1")


        Timer2.Interval = 1000
        Timer2.Enabled = True

        Me.MaximizeBox = False



        SETTINGTEXT()


    End Sub
    Public buff_rx As Byte
    Dim datarec As String
    Dim datastr As String

    Public Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        'SerialPort1.ReadTimeout = 20
        Do While SerialPort1.BytesToRead > 0
            Try
                buff_rx = SerialPort1.ReadByte
                Me.Invoke(New EventHandler(AddressOf update_dat))
            Catch ex As Exception

            End Try
            'Application.DoEvents()
        Loop
    End Sub
    Public Sub update_dat(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error GoTo exit1

        Dim RX_DATA As String = Convert.ToChar(buff_rx).ToString()
        'RichTextBox1.AppendText(RX_DATA)
        datarec = datarec + RX_DATA
        If RX_DATA = Chr(13) Then
            datastr = datarec
            datarec = ""
            ToolStripStatusLabel4.Text = datastr

            Dim str() As String = Split(datastr, ",")


            DataGridView1.Rows.Clear()
            DataGridView3.Rows.Clear()


         
            DataGridView3.Rows.Add(str(1)) 'live Time

            DataGridView1.Rows.Add("Shift 1", str(2), str(3), str(8), str(9), str(10))
            DataGridView1.Rows.Add("Shift 2", str(4), str(5), str(11), str(12), str(13))
            DataGridView1.Rows.Add("Shift 3", str(6), str(7), str(14), str(15), str(16))



            DataGridView1.Rows(0).Selected = False
            DataGridView3.Rows(0).Selected = False

        End If

exit1:
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        command_send = "$QE?"
        Senddata(command_send)


        'DataGridView1.Rows.Clear()

        'DataGridView3.Rows.Clear()

        'DataGridView1.Rows.Add("Shift 1", "12:00", "05:00", "", "", "")
        'DataGridView1.Rows.Add("Shift 2", "12:00", "05:00", "", "", "")
        'DataGridView1.Rows.Add("Shift 3", "12:00", "05:00", "", "", "")



        'DataGridView3.Rows.Add("12:00:03")

        'DataGridView1.Rows(0).Selected = False
        'DataGridView3.Rows(0).Selected = False



    End Sub
    Dim Shift As String
    Dim BreakTIme As String
    Dim flag As Integer
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Label19.Text = ""
        'Button2.Enabled = False
        If TextBox1.Text <> "HHMM" And TextBox2.Text <> "HHMM" And TextBox3.Text <> "HHMM" And TextBox4.Text <> "HHMM" And TextBox6.Text <> "HHMM" Then

            If Len(TextBox1.Text) = 4 Then
                If Len(TextBox2.Text) = 4 Then
                    If Len(TextBox4.Text) = 4 Then
                        If Len(TextBox3.Text) = 4 Then
                            If Len(TextBox6.Text) = 4 Then
                                If ComboBox1.SelectedIndex = 0 Then
                                    Shift = "SH1"
                                    BreakTIme = "BR1"
                                ElseIf ComboBox1.SelectedIndex = 1 Then
                                    Shift = "SH2"
                                    BreakTIme = "BR2"
                                ElseIf ComboBox1.SelectedIndex = 2 Then
                                    Shift = "SH3"
                                    BreakTIme = "BR3"
                                End If
                                Button2.Enabled = False
                                command_send = "$" + Shift + TextBox1.Text + TextBox2.Text
                                Senddata(command_send)
                                Label19.ForeColor = Color.Green
                                Label19.Text = "Data send Successfully..(Shift Command)"
                                TextBox1.Clear()
                                TextBox2.Clear()



                                flag = 1

                            Else
                                Label19.Text = "Please Enter Correct Break 3 Time"
                                Label19.ForeColor = Color.Red
                            End If
                        Else
                            Label19.Text = "Please Enter Correct Break 2 Time"
                            Label19.ForeColor = Color.Red
                        End If
                    Else
                        Label19.Text = "Please Enter Correct Break 1 Time"
                        Label19.ForeColor = Color.Red
                    End If
                Else
                    Label19.Text = "Please Enter Correct End Shift Time"
                    Label19.ForeColor = Color.Red
                End If
            Else
                Label19.Text = "Please Enter Correct Start shift time"
                Label19.ForeColor = Color.Red
            End If
        Else
            Label19.Text = "Please Enter Text Box Value..."
            Label19.ForeColor = Color.Red
        End If
    End Sub





    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        If TextBox5.Text <> "HHMMSS" Then
            If Len(TextBox5.Text) = 4 Then
                command_send = "$CLK" + TextBox5.Text
                Senddata(command_send)
                TextBox5.Clear()
                Label21.Text = "Data send Successfully.."
                If TextBox5.Text = "HHMM" Then
                    TextBox5.Text = ""
                    TextBox5.ForeColor = Color.Black
                Else
                    TextBox5.Text = "HHMM"
                    TextBox5.ForeColor = Color.Gray
                End If
            Else
                Label21.Text = "Please Correct Time"
            End If
        Else
            Label21.Text = "Please Time"
        End If

    End Sub

    Public Function Senddata(ByVal str As String)
        On Error Resume Next
        SerialPort1.Write(command_send & vbCr)
        ToolStripStatusLabel2.Text = command_send
        Return Nothing
    End Function


    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub TextBox5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox5.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub TextBox6_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub



    Public Function SETTINGTEXT()
        If TextBox1.Text = "HHMM" Then
            TextBox1.Text = ""
            TextBox1.ForeColor = Color.Black
        Else
            TextBox1.Text = "HHMM"
            TextBox1.ForeColor = Color.Gray
        End If

        If TextBox2.Text = "HHMM" Then
            TextBox2.Text = ""
            TextBox2.ForeColor = Color.Black
        Else
            TextBox2.Text = "HHMM"
            TextBox2.ForeColor = Color.Gray
        End If



        If TextBox4.Text = "HHMM" Then
            TextBox4.Text = ""
            TextBox4.ForeColor = Color.Black
        Else
            TextBox4.Text = "HHMM"
            TextBox4.ForeColor = Color.Gray
        End If


        If TextBox3.Text = "HHMM" Then
            TextBox3.Text = ""
            TextBox3.ForeColor = Color.Black
        Else
            TextBox3.Text = "HHMM"
            TextBox3.ForeColor = Color.Gray
        End If


        If TextBox6.Text = "HHMM" Then
            TextBox6.Text = ""
            TextBox6.ForeColor = Color.Black
        Else
            TextBox6.Text = "HHMM"
            TextBox6.ForeColor = Color.Gray
        End If





        If TextBox5.Text = "HHMM" Then
            TextBox5.Text = ""
            TextBox5.ForeColor = Color.Black
        Else
            TextBox5.Text = "HHMM"
            TextBox5.ForeColor = Color.Gray
        End If



        Return Nothing
    End Function
    Private Sub TextBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.GotFocus
        If TextBox1.Text = "HHMM" Then
            TextBox1.Text = ""
            TextBox1.ForeColor = Color.Black
        End If
    End Sub
    Private Sub TextBox1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Leave
        If TextBox1.Text <> "" Then
            'TextBox1.Text = ""
            TextBox1.ForeColor = Color.Black
        Else
            TextBox1.Text = "HHMM"
            TextBox1.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub TextBox2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.GotFocus
        If TextBox2.Text = "HHMM" Then
            TextBox2.Text = ""
            TextBox2.ForeColor = Color.Black
        End If
    End Sub
    Private Sub TextBox2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.Leave
        If TextBox2.Text <> "" Then
            TextBox2.ForeColor = Color.Black
        Else
            TextBox2.Text = "HHMM"
            TextBox2.ForeColor = Color.Gray
        End If
    End Sub












    Private Sub TextBox5_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.GotFocus
        If TextBox5.Text = "HHMM" Then
            TextBox5.Text = ""
            TextBox5.ForeColor = Color.Black
        End If
    End Sub
    Private Sub TextBox5_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.Leave
        If TextBox5.Text <> "" Then
            TextBox5.ForeColor = Color.Black
        Else
            TextBox5.Text = "HHMM"
            TextBox5.ForeColor = Color.Gray
        End If
    End Sub




    Private Sub TextBox3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.GotFocus
        If TextBox3.Text = "HHMM" Then
            TextBox3.Text = ""
            TextBox3.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBox3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.Leave
        If TextBox3.Text <> "" Then
            TextBox3.ForeColor = Color.Black
        Else
            TextBox3.Text = "HHMM"
            TextBox3.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub TextBox4_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox4.GotFocus
        If TextBox4.Text = "HHMM" Then
            TextBox4.Text = ""
            TextBox4.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBox4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox4.Leave
        If TextBox4.Text <> "" Then
            TextBox4.ForeColor = Color.Black
        Else
            TextBox4.Text = "HHMM"
            TextBox4.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub TextBox6_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox6.GotFocus
        If TextBox6.Text = "HHMM" Then
            TextBox6.Text = ""
            TextBox6.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBox6_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox6.Leave
        If TextBox6.Text <> "" Then
            TextBox6.ForeColor = Color.Black
        Else
            TextBox6.Text = "HHMM"
            TextBox6.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Label6.Text = "Date : " + System.DateTime.Now.ToString("dd/MM/yyyy") & "  " & "Time : " + System.DateTime.Now.ToString("HH:mm:ss")
    End Sub
    Dim delay As Integer
    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer2.Tick

        If flag = 1 Then
            delay = delay + 1

            If delay = 2 Then

                command_send = "$" + BreakTIme + TextBox4.Text + TextBox3.Text + TextBox6.Text
                Senddata(command_send)
                Label19.Text = "Data send Successfully..(Break Command)"

                TextBox4.Clear()
                TextBox3.Clear()
                TextBox6.Clear()

                If TextBox1.Text = "HHMM" Then
                    TextBox1.Text = ""
                    TextBox1.ForeColor = Color.Black
                Else
                    TextBox1.Text = "HHMM"
                    TextBox1.ForeColor = Color.Gray
                End If

                If TextBox2.Text = "HHMM" Then
                    TextBox2.Text = ""
                    TextBox2.ForeColor = Color.Black
                Else
                    TextBox2.Text = "HHMM"
                    TextBox2.ForeColor = Color.Gray
                End If


                If TextBox4.Text = "HHMM" Then
                    TextBox4.Text = ""
                    TextBox4.ForeColor = Color.Black
                Else
                    TextBox4.Text = "HHMM"
                    TextBox4.ForeColor = Color.Gray
                End If


                If TextBox3.Text = "HHMM" Then
                    TextBox3.Text = ""
                    TextBox3.ForeColor = Color.Black
                Else
                    TextBox3.Text = "HHMM"
                    TextBox3.ForeColor = Color.Gray
                End If


                If TextBox6.Text = "HHMM" Then
                    TextBox6.Text = ""
                    TextBox6.ForeColor = Color.Black
                Else
                    TextBox6.Text = "HHMM"
                    TextBox6.ForeColor = Color.Gray
                End If
                flag = 0
                Button2.Enabled = True
                delay = 0
            End If
        End If
    End Sub

    
    
  
End Class
