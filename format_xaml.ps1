$filePath = "AiWpf\Views\MainWindow.xaml"

# Load the XML
$xml = [xml](Get-Content $filePath)

# Create XML writer settings for proper formatting
$sw = New-Object System.IO.StringWriter
$settings = New-Object System.Xml.XmlWriterSettings
$settings.Indent = $true
$settings.IndentChars = "    " # 4 spaces
$settings.NewLineChars = "`r`n"
$settings.OmitXmlDeclaration = $true

# Write formatted XML
$writer = [System.Xml.XmlWriter]::Create($sw, $settings)
$xml.Save($writer)
$writer.Close()

# Save to file
$sw.ToString() | Set-Content $filePath
Write-Host "Successfully formatted MainWindow.xaml with consistent 4-space indentation"
