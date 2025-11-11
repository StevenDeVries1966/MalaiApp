$filePath = "AiWpf\Views\MainWindow.xaml"
$content = [System.IO.File]::ReadAllText($filePath)

# Find and replace the specific Client Code TextBox line
$oldLine = '   <TextBox Grid.Row="1" Grid.Column="1" Text="{Binding clt_code, Mode=OneWay}"/>'

$newLines = @'
     <ComboBox Grid.Row="1" Grid.Column="1"
          SelectedValuePath="clt_code"
          DisplayMemberPath="clt_code"
      SelectedValue="{Binding clt_code, Mode=TwoWay}"
          ItemsSource="{Binding DataContext.Clients, RelativeSource={RelativeSource AncestorType=Window}}"
          Margin="0,3,0,3"
          Background="WhiteSmoke"
          VerticalAlignment="Center"/>
'@

$content = $content.Replace($oldLine, $newLines)
[System.IO.File]::WriteAllText($filePath, $content)
Write-Host "Successfully replaced Client Code TextBox with ComboBox"
