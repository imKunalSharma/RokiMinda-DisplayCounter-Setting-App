Imports System.IO.Ports

Module Module1

    Public command_send As String
    Public Function PortConnect(ByRef Port1 As String) As String

        Try
            Form1.SerialPort1.PortName = Port1
            Form1.SerialPort1.BaudRate = "9600"
            Form1.SerialPort1.DataBits = 8
            Form1.SerialPort1.StopBits = IO.Ports.StopBits.One
            Form1.SerialPort1.Parity = IO.Ports.Parity.None
            Form1.SerialPort1.Handshake = Handshake.None

            Form1.SerialPort1.Open()
            Form1.ToolStripStatusLabel6.ForeColor = Color.Green
            Return Port1 & " Connected.."

        Catch ex As Exception
            'MsgBox(ErrorToString, MsgBoxStyle.Information, Title:=Port1 & " Commucation Alert")
            Form1.ToolStripStatusLabel6.ForeColor = Color.Red
            Return ErrorToString() & Port1

        End Try

        Return Nothing

    End Function

End Module
