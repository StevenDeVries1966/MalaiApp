$filePath = "AiWpf\Views\MainWindow.xaml"
$content = Get-Content $filePath -Raw

# Add TextAlignment="Right" to LabelStyle
$pattern = '(<Style TargetType="TextBlock" x:Key="LabelStyle">[\s\S]*?<Setter Property="VerticalAlignment" Value="Center"/>)'
$replacement = '$1' + "`r`n  <Setter Property=`"TextAlignment`" Value=`"Right`"/>"
$content = $content -replace [regex]::Escape($pattern.Substring(1, $pattern.Length-2)), $replacement

Set-Content $filePath $content -NoNewline
