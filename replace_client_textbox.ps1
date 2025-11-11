$filePath = "AiWpf\Views\MainWindow.xaml"
$content = Get-Content $filePath -Raw

# Find and replace the Client Code TextBox with ComboBox
$oldText = @"
         <TextBlock Grid.Row="1" Grid.Column="0" Text="Client Code:" Style="{StaticResource LabelStyle}"/>
     <TextBox Grid.Row="1" Grid.Column="1" Text="{Binding clt_code, Mode=OneWay}"/>
"@

$newText = @"
    <TextBlock Grid.Row="1" Grid.Column="0" Text="Client Code:" Style="{StaticResource LabelStyle}"/>
   <ComboBox Grid.Row="1" Grid.Column="1" 
         SelectedValuePath="clt_code"
 DisplayMemberPath="clt_code"
     SelectedValue="{Binding clt_code, Mode=TwoWay}"
           ItemsSource="{Binding DataContext.Clients, RelativeSource={RelativeSource AncestorType=Window}}"
           Margin="0,3,0,3"
               Background="WhiteSmoke"
     VerticalAlignment="Center"/>
"@

$content = $content.Replace($oldText, $newText)
Set-Content $filePath $content -NoNewline
Write-Host "Replaced Client Code TextBox with ComboBox"
