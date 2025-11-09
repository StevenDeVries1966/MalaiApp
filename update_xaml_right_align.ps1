$filePath = "AiWpf\Views\MainWindow.xaml"
$content = Get-Content $filePath -Raw

# Find and replace - add TextAlignment to LabelStyle
$oldText = @"
    <Grid.Resources>
        <Style TargetType="TextBlock" x:Key="LabelStyle">
   <Setter Property="FontWeight" Value="Bold"/>
        <Setter Property="Margin" Value="0,3,10,3"/>
   <Setter Property="VerticalAlignment" Value="Center"/>
  </Style>
"@

$newText = @"
    <Grid.Resources>
        <Style TargetType="TextBlock" x:Key="LabelStyle">
   <Setter Property="FontWeight" Value="Bold"/>
  <Setter Property="Margin" Value="0,3,10,3"/>
            <Setter Property="VerticalAlignment" Value="Center"/>
   <Setter Property="TextAlignment" Value="Right"/>
       </Style>
"@

$content = $content.Replace($oldText, $newText)
Set-Content $filePath $content -NoNewline
Write-Host "Updated MainWindow.xaml with right-aligned labels"
