﻿'Copyright (c) 2018-2020 Analog Devices, Inc. All Rights Reserved.
'This software is proprietary to Analog Devices, Inc. and its licensors.
'
'File:          SelectDUTGUI.vb
'Author:        Alex Nolan (alex.nolan@analog.com)
'Description:   Allows a user to select their DUT (ADcmXL series or IMU).

Public Class SelectDUTGUI
    Inherits FormBase

    Friend isStartup As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

    End Sub

    Public Sub FormSetup() Handles Me.Load

        For Each item In m_TopGUI.DutOptions
            deviceInput.Items.Add(item.DisplayName)
        Next
        If deviceInput.Items.Contains(m_TopGUI.SelectedPersonality) Then
            deviceInput.SelectedItem = m_TopGUI.SelectedPersonality
        Else
            deviceInput.SelectedIndex = 0
        End If

    End Sub

    Private Sub Shutdown() Handles Me.Closing
        're-enable button
        If Not isStartup Then m_TopGUI.btn_SelectDUT.Enabled = True
    End Sub

    Private Sub btn_ApplySetting_Click(sender As Object, e As EventArgs) Handles btn_ApplySetting.Click
        'check for selected item
        Dim selectedDut As Integer = -1
        For i As Integer = 0 To m_TopGUI.DutOptions.Count - 1
            If deviceInput.Text = m_TopGUI.DutOptions(i).DisplayName Then
                selectedDut = i
                Exit For
            End If
        Next

        If selectedDut < 0 Then
            MsgBox("Error: Invalid DUT selected!")
        Else
            If isStartup Then
                m_TopGUI.SelectedPersonality = deviceInput.Text
            Else
                Try
                    If m_TopGUI.ApplyDutPersonality(deviceInput.Text) Then
                        'only proceed if not canceled
                        m_TopGUI.ApplyDutPersonalityRegmap(deviceInput.Text)
                        m_TopGUI.UpdateDutLabel(m_TopGUI.FX3.PartType)
                        m_TopGUI.SaveAppSettings()
                    End If
                Catch ex As Exception
                    MsgBox("Error applying settings! " + ex.Message)
                End Try
            End If
        End If
        Me.Close()
    End Sub

End Class