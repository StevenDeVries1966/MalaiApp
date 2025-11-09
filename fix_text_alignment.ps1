$filePath = "AiWpf\Views\MainWindow.xaml"
$lines = Get-Content $filePath

$newLines = @()
$labelStyleFound = $false

for ($i = 0; $i -lt $lines.Count; $i++) {
    $newLines += $lines[$i]
    
    # If we found LabelStyle and this line has VerticalAlignment Center, add TextAlignment after it
    if ($lines[$i] -match 'x:Key="LabelStyle"') {
   $labelStyleFound = $true
    }
    
    if ($labelStyleFound -and $lines[$i] -match 'Property="VerticalAlignment".*Value="Center"') {
        $newLines += '   <Setter Property="TextAlignment" Value="Right"/>'
        $labelStyleFound = $false
    }
}

$newLines | Set-Content $filePath
Write-Host "Added TextAlignment=Right to LabelStyle"
